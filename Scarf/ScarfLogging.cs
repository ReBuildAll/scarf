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
            ScarfLogMessage msg = ScarfContext.CurrentInternal.AddSecondaryMessage(
                MessageClass.Debug, 
                MessageType.DebugMessage,
                new MessageOptions
                {
                    AddCookies = true,
                    AddFormVariables = true,
                    AddQueryStringVariables = true,
                    SaveAdditionalInfo = true,
                });
            if (msg.CanSave() == false) return;

            msg.Message = message;
            msg.Details = details;
        }

        public static void Flush()
        {
            CurrentContext.Commit();
        }

        public static IScarfContext CurrentContext
        {
            get
            {
                return ScarfContext.CurrentInternal;
            }
        }

        public static IScarfContext BeginInlineContext(HttpContextBase httpContext = null )
        {
            if (httpContext != null)
            {
                return ScarfContext.GetCurrent(httpContext);
            }
            if (ScarfContext.HasThreadContext)
            {
                throw new InvalidOperationException("Cannot have multiple inline contexts on the same thread!");
            }
            return ScarfContext.GetThreadContext();
        }

        public static void AddCustom(string key, string value)
        {
            ScarfContext.CurrentInternal.PrimaryMessage.AddCustomInfo(key, value);
        }
    }
}
