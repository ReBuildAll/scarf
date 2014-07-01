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
    public interface ScarfDataSource
    {
        void Initialize(DataSourceElement configuration);

        void SaveLogMessage(ScarfLogMessage message);

        int GetMessages(string application, int pageIndex, int pageSize, ICollection<ScarfLogMessage> messageList );

        ScarfLogMessage GetMessageById(Guid messageId);
    }
}
