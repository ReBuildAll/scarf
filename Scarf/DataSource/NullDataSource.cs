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
    internal sealed class NullDataSource : ScarfDataSource
    {
        public void Initialize(DataSourceElement configuration)
        {            
        }

        public void SaveLogMessage(LogMessage message)
        {
        }

        public int GetMessages(string application, int pageIndex, int pageSize, ICollection<LogMessage> messageList)
        {
            return 0;
        }

        public LogMessage GetMessageById(Guid messageId)
        {
            return null;
        }
    }
}
