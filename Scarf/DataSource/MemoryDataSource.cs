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

using System;
using System.Collections.Generic;
using Scarf.Configuration;

namespace Scarf.DataSource
{
    internal sealed class MemoryDataSource : ScarfDataSource
    {
        public void Initialize(DataSourceElement configuration)
        {
            throw new NotImplementedException();
        }

        public void SaveLogMessage(LogMessage message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LogMessage> GetMessages(string application)
        {
            throw new NotImplementedException();
        }
    }
}
