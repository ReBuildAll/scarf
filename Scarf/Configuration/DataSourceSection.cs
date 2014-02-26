using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarf.Configuration
{
    public class DataSourceSection : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName", IsRequired=false)]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type 
        {
            get { return (string)base["type"]; }
        }

        [ConfigurationProperty("path", IsRequired = false)]
        public string Path
        {
            get { return (string)base["path"]; }
        }
    }
}
