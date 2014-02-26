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

namespace Scarf.MVC
{
    public class LogAuditAttribute : ScarfLoggingAttribute
    {
        public LogAuditAttribute(string messageSubType)
            : base ( LogMessageType.Audit, messageSubType )
        {
            
        }
    }
}
