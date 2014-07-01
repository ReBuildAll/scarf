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
using System.Web;

namespace Scarf
{
    public static class ScarfLogging
    {
        public static void Debug(string message, string details = null)
        {
            ScarfContext.Current.CreatePrimaryMessage(MessageClass.Debug, MessageType.DebugMessage);
            if (ScarfContext.Current.PrimaryMessage.CanSave() == false) return;

            ScarfContext.Current.PrimaryMessage.AddAdditionalInfo(true, true, true);
            ScarfContext.Current.PrimaryMessage.Message = message;
            ScarfContext.Current.PrimaryMessage.Details = details;
        }

        internal static ScarfLogMessage CreateEmptyMessageInstanceFromClass(MessageClass messageClass, HttpContextBase httpContext)
        {
            switch (messageClass)
            {
                case MessageClass.Debug:
                    return new DebugLogMessage(httpContext);
                case MessageClass.Audit:
                    return new AuditLogMessage(httpContext);
                case MessageClass.Action:
                    return new ActionLogMessage(httpContext);
                case MessageClass.Access:
                    return new AccessLogMessage(httpContext);
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
