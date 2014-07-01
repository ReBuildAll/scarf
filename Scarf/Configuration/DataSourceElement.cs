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
    public class DataSourceElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName", IsRequired=false)]
        public virtual string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public virtual string Type 
        {
            get { return (string)base["type"]; }
        }

        [ConfigurationProperty("path", IsRequired = false)]
        public virtual string Path
        {
            get { return (string)base["path"]; }
        }
    }
}
