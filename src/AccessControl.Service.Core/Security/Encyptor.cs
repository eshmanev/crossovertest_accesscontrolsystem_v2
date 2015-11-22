using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AccessControl.Service.Configuration;
using Newtonsoft.Json;

namespace AccessControl.Service.Security
{
    public class Encryptor : IEncryptor
    {
        private readonly string _secret;
        private const string InitVector = "B082669D95BB4AA7";
        private const string Salt = "DA196909-3F94-48FC-983F-55F6779C2655";
        private const int Keysize = 256;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Encryptor" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public Encryptor(IServiceConfig config)
        {
            Contract.Requires(config != null);
            _secret = config.Security.Secret;
        }

        /// <summary>
        ///     Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        public string Decrypt(string cipherText)
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var saltBytes = Encoding.UTF8.GetBytes(Salt);
            var password = new Rfc2898DeriveBytes(_secret, saltBytes);
            var keyBytes = password.GetBytes(Keysize/8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    var plainTextBytes = new byte[cipherTextBytes.Length];
                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                }
            }
        }

        /// <summary>
        ///     Decrypts the specified text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public T Decrypt<T>(string text) where T : class
        {
            var json = Decrypt(text);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        ///     Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var saltBytes = Encoding.UTF8.GetBytes(Salt);
            var password = new Rfc2898DeriveBytes(_secret, saltBytes);
            var keyBytes = password.GetBytes(Keysize/8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    var cipherTextBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(cipherTextBytes);
                }
            }
        }

        /// <summary>
        ///     Encrypts the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public string Encrypt<T>(T obj)
            where T : class
        {
            var json = JsonConvert.SerializeObject(obj);
            return Encrypt(json);
        }
    }
}