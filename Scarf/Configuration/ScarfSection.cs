#region Copyright and license
//
// SCARF - Security Audit, Access and Action Logging
// Copyright (c) 2014 ReBuildAll Solutions Ltd
//
// Author:
//    Lenard Gunda 
//
// Licensed under MIT license, see included LICENSE file for details
#endregion

using System.Configuration;

namespace Scarf.Configuration
{
    public class ScarfSection : ConfigurationSection
    {
        [ConfigurationProperty("dataSource", IsRequired=false)]
        public virtual DataSourceElement DataSource 
        {
            get { return (DataSourceElement)base["dataSource"]; }
        }

        [ConfigurationProperty("audit", IsRequired = false)]
        public virtual AuditElement Audit
        {
            get { return (AuditElement)base["audit"]; }
        }

        [ConfigurationProperty("action", IsRequired = false)]
        public virtual ActionElement Action
        {
            get { return (ActionElement)base["action"]; }
        }

        [ConfigurationProperty("access", IsRequired = false)]
        public virtual AccessElement Access
        {
            get { return (AccessElement)base["access"]; }
        }

        [ConfigurationProperty("debug", IsRequired = false)]
        public virtual DebugElement Debug
        {
            get { return (DebugElement)base["debug"]; }
        }

        [ConfigurationProperty("security", IsRequired = false)]
        public virtual SecurityElement Security
        {
            get { return (SecurityElement)base["security"]; }
        }

        [ConfigurationProperty("applicationName")]
        public virtual string ApplicationName
        {
            get { return (string) base["applicationName"]; }
        }
    }
}
