using System.Configuration;

namespace AccessControl.LDAP.Service.Configuration
{
    public class LdapConfig : ConfigurationSection, ILdapConfig
    {
        [ConfigurationProperty("ldapPath", IsRequired = true)]
        public string LdapPath => (string) base["ldapPath"];

        [ConfigurationProperty("userName", IsRequired = false)]
        public string UserName => (string)base["userName"];

        [ConfigurationProperty("password", IsRequired = false)]
        public string Password => (string)base["password"];
    }
}