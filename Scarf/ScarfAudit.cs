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

namespace Scarf
{
    public static class ScarfAudit
    {
        public static void Start(string messageType)
        {
            ScarfContext.Current.CreatePrimaryMessage(MessageClass.Audit, messageType);
        }

        public static void LoggedInAs(string username)
        {
            ScarfContext.Current.PrimaryMessage.Message = string.Format("User '{0}' logged in", username);
            Succeeded();
        }
        
        public static void CreatedUser(string username)
        {
            ScarfContext.Current.PrimaryMessage.Message = string.Format("Created new user '{0}'", username);
            Succeeded();
        }

        public static void PasswordChanged()
        {
            ScarfContext.Current.PrimaryMessage.Message = string.Format("Changed password");
            Succeeded();
        }

        public static void Failed()
        {
            ScarfContext.Current.PrimaryMessage.Message += " failed.";
            ScarfContext.Current.PrimaryMessage.Success = false;            
        }

        public static void Succeeded()
        {
            ScarfContext.Current.PrimaryMessage.Success = true;
        }

        public static bool HasResult
        {
            get { return ScarfContext.Current.PrimaryMessage.Success.HasValue; }
        }
    }
}
