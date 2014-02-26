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

using System.Configuration;
using Scarf.Configuration;

namespace Scarf
{
    public static class ScarfLogging
    {
        public static void Debug(string message, string details = null)
        {
            LogMessage logMessage = ScarfContext.Current.CreateMessage(MessageClass.Debug, MessageType.DebugMessage);
            if (logMessage.CanSave() == false) return;

            ScarfContext.Current.AddAdditionalInfo(logMessage, true, true, true);
            logMessage.Message = message;
            logMessage.Details = details;

            ScarfContext.Current.SaveMessage(logMessage);
        }

        internal static ScarfSection GetConfiguration()
        {
            return ConfigurationManager.GetSection("scarf") as ScarfSection;
        }
    }
}
