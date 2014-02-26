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
    public class AuditElement : ConfigurationElement
    {
        [ConfigurationProperty("onlyFailures", IsRequired = false, DefaultValue = false)]
        public bool LogOnlyFailures {
            get { return (bool) base["onlyFailures"]; }
        }

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue=true)]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
        }
    }
}
