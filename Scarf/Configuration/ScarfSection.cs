using System.Configuration;

namespace Scarf.Configuration
{
    public sealed class ScarfSection : ConfigurationSection
    {
        [ConfigurationProperty("dataSource", IsRequired=false)]
        public DataSourceSection DataSource 
        {
            get { return (DataSourceSection)base["dataSource"]; }
        }

        [ConfigurationProperty("applicationName")]
        public string ApplicationName
        {
            get { return (string) base["applicationName"]; }
        }
    }
}
