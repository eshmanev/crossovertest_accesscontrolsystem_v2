using System.Diagnostics.Contracts;
using System.DirectoryServices;

namespace AccessControl.Service.LDAP.Consumers
{
    internal static class SearchResultExtensions
    {
        public static string GetProperty(this SearchResult result, string propertyName)
        {
            Contract.Requires(result != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));
            return result.Properties[propertyName].Count > 0 ? (string) result.Properties[propertyName][0] : null;
        }

        public static string GetProperty(this DirectoryEntry entry, string propertyName)
        {
            Contract.Requires(entry != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));
            return entry.Properties[propertyName].Count > 0 ? (string)entry.Properties[propertyName][0] : null;
        }
    }
}