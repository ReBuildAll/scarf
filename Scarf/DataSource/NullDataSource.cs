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

        public void SaveLogMessage(ScarfLogMessage message)
        {
        }

        public int GetMessages(string application, int pageIndex, int pageSize, ICollection<ScarfLogMessage> messageList)
        {
            return 0;
        }

        public ScarfLogMessage GetMessageById(Guid messageId)
        {
            return null;
        }
    }
}
