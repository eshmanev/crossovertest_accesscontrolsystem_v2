using System.Configuration;

namespace AccessControl.Data.Configuration
{
    public class DataConfiguration : ConfigurationSection, IDataConfiguration
    {
        [ConfigurationProperty("recreateDatabaseSchema", DefaultValue = false)]
        public bool RecreateDatabaseSchema => (bool) base["recreateDatabaseSchema"];
    }
}