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

namespace Scarf.DataSource.SQLServer
{
    public class SqlServerDataSource : ScarfDataSource
    {
        public void Initialize(DataSourceElement configuration)
        {
            throw new System.NotImplementedException();
        }

        public void SaveLogMessage(LogMessage message)
        {
            throw new System.NotImplementedException();
        }

        public int GetMessages(string application, int pageIndex, int pageSize, ICollection<LogMessage> messageList)
        {
            throw new System.NotImplementedException();
        }

        public LogMessage GetMessageById(Guid messageId)
        {
            throw new NotImplementedException();
        }
    }
}
