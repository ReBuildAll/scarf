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
using System.Linq;
using Scarf.Configuration;

namespace Scarf.DataSource
{
    internal sealed class MemoryDataSource : ScarfDataSource
    {
        private static readonly HashSet<LogMessage> Messages = new HashSet<LogMessage>();

        public void Initialize(DataSourceElement configuration)
        {
        }

        public void SaveLogMessage(LogMessage message)
        {
            Messages.Add(message);
        }

        public int GetMessages(string application, int pageIndex, int pageSize, ICollection<LogMessage> messageList)
        {
            IOrderedEnumerable<LogMessage> appMessages =
                from m in Messages
                where m.Application == application
                orderby m.LoggedAt descending
                select m;

            IEnumerable<LogMessage> viewableMessages = appMessages.Skip(pageIndex*pageSize).Take(pageSize);

            foreach (var logMessage in viewableMessages)
            {
                messageList.Add(logMessage);
            }

            return appMessages.Count();
        }

        public LogMessage GetMessageById(Guid messageId)
        {
            return Messages.SingleOrDefault(m => m.EntryId == messageId);
        }
    }
}
