using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace AccessControl.Contracts
{
    public class Encryptor
    {
        private const string InitVector = "B082669D95BB4AA7";
        private const string Salt = "DA196909-3F94-48FC-983F-55F6779C2655";
        private const int Keysize = 256;

        public static string Encrypt(string plainText, string passPhrase)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(plainText));
            Contract.Requires(!string.IsNullOrWhiteSpace(passPhrase));

            var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var saltBytes = Encoding.UTF8.GetBytes(Salt);
            var password = new Rfc2898DeriveBytes(passPhrase, saltBytes);
            var keyBytes = password.GetBytes(Keysize / 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                var cipherTextBytes = memoryStream.ToArray();
                return Convert.ToBase64String(cipherTextBytes);
            }   
        }
        public static string Decrypt(string cipherText, string passPhrase)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(cipherText));
            Contract.Requires(!string.IsNullOrWhiteSpace(passPhrase));

            var initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var saltBytes = Encoding.UTF8.GetBytes(Salt);
            var password = new Rfc2898DeriveBytes(passPhrase, saltBytes);
            var keyBytes = password.GetBytes(Keysize / 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            using (var memoryStream = new MemoryStream(cipherTextBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                var plainTextBytes = new byte[cipherTextBytes.Length];
                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            } 
        }

        public static string Encrypt<T>(T obj, string passPhrase)
            where T : class
        {
            Contract.Requires(obj != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(passPhrase));

            var json = JsonConvert.SerializeObject(obj);
            return Encrypt(json, passPhrase);
        }

        public static T Decrypt<T>(string text, string passPhrase)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(passPhrase));

            var json = Decrypt(text, passPhrase);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}