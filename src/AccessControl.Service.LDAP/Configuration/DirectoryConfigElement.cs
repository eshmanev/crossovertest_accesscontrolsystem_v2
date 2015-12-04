using System.Configuration;

namespace AccessControl.Service.LDAP.Configuration
{
    internal class DirectoryConfigElement : ConfigurationElement, IDirectoryConfig
    {
        [ConfigurationProperty("domain", IsRequired = true)]
        public string DomainName => (string)base["domain"];

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url => (string) base["url"];

        [ConfigurationProperty("userName", IsRequired = false)]
        public string UserName => (string)base["userName"];

        [ConfigurationProperty("password", IsRequired = false)]
        public string Password => (string)base["password"];

        public string CombinePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Url;

            return Url.EndsWith("/") ? Url + path : $"{Url}/{path}";
        }
    }
}