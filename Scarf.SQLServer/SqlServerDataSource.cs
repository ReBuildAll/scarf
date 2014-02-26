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

using System.Collections.Generic;
using Scarf.Configuration;

namespace Scarf.DataSource.SQLServer
{
    public class SqlServerDataSource : ScarfDataSource
    {
        public void Initialize(DataSourceSection configuration)
        {
            throw new System.NotImplementedException();
        }

        public void SaveLogMessage(LogMessage message)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<LogMessage> GetMessages(string application)
        {
            throw new System.NotImplementedException();
        }
    }
}
