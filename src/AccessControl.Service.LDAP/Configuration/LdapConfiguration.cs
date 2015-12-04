using System.Configuration;

namespace AccessControl.Service.LDAP.Configuration
{
    internal class LdapConfiguration : ConfigurationSection, ILdapConfig
    {
        [ConfigurationProperty("directories", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DirectoryConfigElement), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public LdapConfigElementCollection Directories => (LdapConfigElementCollection) base["directories"];

        IDirectoryConfigCollection ILdapConfig.Directories => Directories;
    }
}