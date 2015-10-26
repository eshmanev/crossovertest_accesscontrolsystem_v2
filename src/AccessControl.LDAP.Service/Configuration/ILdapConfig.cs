

namespace AccessControl.LDAP.Service.Configuration
{
    using System.Diagnostics.Contracts;
    using AccessControl.LDAP.Service.CodeContracts;

    [ContractClass(typeof(ILdapConfigContract))]
    public interface ILdapConfig
    {
        string LdapPath { get; } 
    }
}