using System.Diagnostics.Contracts;
using AccessControl.Service.LDAP.CodeContracts;

namespace AccessControl.Service.LDAP.Configuration
{
    [ContractClass(typeof(ILdapConfigContract))]
    public interface ILdapConfig
    {
        string LdapPath { get; }
        string Password { get; }
        string UserName { get; }
        string CombinePath(string path);
    }
}