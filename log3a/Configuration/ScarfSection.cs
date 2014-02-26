using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log3a.Configuration
{
    public sealed class ScarfSection : ConfigurationSection
    {
        [ConfigurationProperty("dataAccess", IsRequired=false)]
        public DataAccessSection DataAccess 
        {
            get { return (DataAccessSection) base["dataAccess"]; }
        }
    }
}
