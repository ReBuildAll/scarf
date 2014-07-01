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
using Scarf.DataSource;

namespace Scarf.Tests.Infrastructure
{
    internal sealed class TestDataSource : ScarfDataSource
    {
        private readonly HashSet<ScarfLogMessage> _messages = new HashSet<ScarfLogMessage>();

        public void Initialize(DataSourceElement configuration)
        {
        }

        public void SaveLogMessage(ScarfLogMessage message)
        {
            _messages.Add(message);
        }

        public int GetMessages(string application, int pageIndex, int pageSize, ICollection<ScarfLogMessage> messageList)
        {
            IOrderedEnumerable<ScarfLogMessage> appMessages =
                from m in _messages
                where m.Application == application
                orderby m.LoggedAt descending
                select m;

            IEnumerable<ScarfLogMessage> viewableMessages = appMessages.Skip(pageIndex * pageSize).Take(pageSize);

            foreach (var logMessage in viewableMessages)
            {
                messageList.Add(logMessage);
            }

            return appMessages.Count();
        }

        public ScarfLogMessage GetMessageById(Guid messageId)
        {
            return _messages.SingleOrDefault(m => m.EntryId == messageId);
        }
    }
}
