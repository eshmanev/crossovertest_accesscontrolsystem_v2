using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AccessControl.Service.LDAP.Configuration
{
    internal class LdapConfigElementCollection : ConfigurationElementCollection, IDirectoryConfigCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DirectoryConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DirectoryConfigElement) element).DomainName;
        }

        IDirectoryConfig IDirectoryConfigCollection.this[string domain]
        {
            get { return this.Single(x => string.Equals(x.DomainName, domain, StringComparison.InvariantCultureIgnoreCase)); }
        }

        IEnumerator<IDirectoryConfig> IEnumerable<IDirectoryConfig>.GetEnumerator()
        {
            var enumerator = base.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return (IDirectoryConfig)enumerator.Current;
            }
        }
    }
}