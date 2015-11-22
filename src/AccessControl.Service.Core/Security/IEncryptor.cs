using System.Diagnostics.Contracts;
using AccessControl.Service.CodeContracts;

namespace AccessControl.Service.Security
{
    [ContractClass(typeof(IEncryptorContract))]
    public interface IEncryptor
    {
        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        string Decrypt(string cipherText);

        /// <summary>
        /// Encrypts the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        string Encrypt<T>(T obj) where T : class;

        /// <summary>
        /// Decrypts the specified text and deserializes the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        T Decrypt<T>(string cipherText) where T : class;
    }
}