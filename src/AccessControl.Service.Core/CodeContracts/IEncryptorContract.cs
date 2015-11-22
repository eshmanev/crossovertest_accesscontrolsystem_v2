using System.Diagnostics.Contracts;
using AccessControl.Service.Security;

namespace AccessControl.Service.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IEncryptor" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IEncryptor))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IEncryptorContract : IEncryptor
    {
        public string Encrypt(string plainText)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(plainText));
            return null;
        }

        public string Decrypt(string cipherText)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(cipherText));
            return null;
        }

        public string Encrypt<T>(T obj) where T : class
        {
            Contract.Requires(obj != null);
            return null;
        }

        public T Decrypt<T>(string cipherText) where T : class
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(cipherText));
            return null;
        }
    }
}