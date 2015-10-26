using System.Configuration;

namespace AccessControl.LDAP.Service.Configuration
{
    public class LdapConfig : ConfigurationSection, ILdapConfig
    {
        [ConfigurationProperty("ldapPath", IsRequired = true)]
        public string LdapPath => (string) base["ldapPath"];
    }
}