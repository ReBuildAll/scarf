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

namespace Scarf
{
    public static class ScarfLogging
    {
        public static void Debug(string message, string details = null)
        {
            ScarfLogMessage logMessage = ScarfContext.Current.CreateMessage(MessageClass.Debug, MessageType.DebugMessage);
            if (logMessage.CanSave() == false) return;

            ScarfContext.Current.AddAdditionalInfo(logMessage, true, true, true);
            logMessage.Message = message;
            logMessage.Details = details;

            ScarfContext.Current.SaveMessage(logMessage);
        }

        public static ScarfLogMessage CreateEmptyMessageInstanceFromClass(MessageClass messageClass)
        {
            switch (messageClass)
            {
                case MessageClass.Debug:
                    return new DebugLogMessage();
                case MessageClass.Audit:
                    return new AuditLogMessage();
                case MessageClass.Action:
                    return new ActionLogMessage();
                case MessageClass.Access:
                    return new AccessLogMessage();
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Type MapMessageClassToClrType(MessageClass messageClass)
        {
            switch (messageClass)
            {
                case MessageClass.Debug:
                    return typeof(DebugLogMessage);
                case MessageClass.Audit:
                    return typeof(AuditLogMessage);
                case MessageClass.Action:
                    return typeof(ActionLogMessage);
                case MessageClass.Access:
                    return typeof(AccessLogMessage);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
