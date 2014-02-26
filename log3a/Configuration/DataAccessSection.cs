using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log3a.Configuration
{
    public class DataAccessSection : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName", IsRequired=true)]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type 
        {
            get { return (string)base["type"]; }
        }
    }
}
