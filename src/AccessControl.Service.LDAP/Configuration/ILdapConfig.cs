

using AccessControl.Service.LDAP.CodeContracts;

namespace AccessControl.Service.LDAP.Configuration
{
    using System.Diagnostics.Contracts;
    using LDAP.CodeContracts;

    [ContractClass(typeof(ILdapConfigContract))]
    public interface ILdapConfig
    {
        string LdapPath { get; } 
        string UserName { get; }
        string Password { get; }
    }
}