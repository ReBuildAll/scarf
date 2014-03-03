using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarf.Configuration
{
    public class SecurityElement : ConfigurationElement
    {
        [ConfigurationProperty("requireAuthentication", IsRequired = false, DefaultValue = true)]
        public bool RequireAuthentication
        {
            get { return (bool)base["requireAuthentication"]; }
        }

        [ConfigurationProperty("allowRoles", IsRequired = false, DefaultValue = "")]
        public string AllowRoles
        {
            get { return (string)base["allowRoles"]; }
        }

        [ConfigurationProperty("allowUsers", IsRequired = false, DefaultValue = "")]
        public string AllowUsers
        {
            get { return (string)base["allowUsers"]; }
        }
    }
}
