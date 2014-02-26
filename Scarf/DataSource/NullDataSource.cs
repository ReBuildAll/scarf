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

namespace Scarf.DataSource
{
    internal sealed class NullDataSource : ScarfDataSource
    {
        public void Initialize(DataSourceSection configuration)
        {            
        }

        public void SaveLogMessage(LogMessage message)
        {
        }

        public IEnumerable<LogMessage> GetMessages(string application)
        {
            return new LogMessage[0];
        }
    }
}
