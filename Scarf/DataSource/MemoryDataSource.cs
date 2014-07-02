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
        private static readonly HashSet<ScarfLogMessage> Messages = new HashSet<ScarfLogMessage>();

        public void Initialize(DataSourceElement configuration)
        {
        }

        public void SaveLogMessages(IEnumerable<ScarfLogMessage> messages)
        {
            foreach (var scarfLogMessage in messages)
            {
                Messages.Add(scarfLogMessage);
            }
        }

        public int GetMessages(string application, int pageIndex, int pageSize, ICollection<ScarfLogMessage> messageList)
        {
            IOrderedEnumerable<ScarfLogMessage> appMessages =
                from m in Messages
                where m.Application == application
                orderby m.LoggedAt descending
                select m;

            IEnumerable<ScarfLogMessage> viewableMessages = appMessages.Skip(pageIndex*pageSize).Take(pageSize);

            foreach (var logMessage in viewableMessages)
            {
                messageList.Add(logMessage);
            }

            return appMessages.Count();
        }

        public ScarfLogMessage GetMessageById(Guid messageId)
        {
            return Messages.SingleOrDefault(m => m.EntryId == messageId);
        }
    }
}
