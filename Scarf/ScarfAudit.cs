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
            ScarfLogging.CurrentContext.CreateMessage(MessageClass.Audit, messageType);
        }

        public static void LoggedInAs(string username)
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message = string.Format("User '{0}' logged in", username);
            Succeeded();
        }

        public static void LoggedOut(string username)
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message = string.Format("User '{0}' logged out", username);
            Succeeded();
        }
        
        public static void UserCreated(string username)
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message = string.Format("Created new user '{0}'", username);
            Succeeded();
        }

        public static void UserDeleted(string username)
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message = string.Format("Removed user '{0}'", username);
            Succeeded();
        }

        public static void PasswordChanged()
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message = string.Format("User changed password");
            Succeeded();
        }

        public static void PasswordChanged(string forUsername)
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message = string.Format("User {0} changed password", forUsername);
            Succeeded();
        }

        public static void LoginChanged(string oldUsername, string newUsername)
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message = string.Format("User {0} changed to {1}", oldUsername, newUsername );
            Succeeded();
        }

        public static void PasswordReset(string forUsername)
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message = string.Format("User {0} reset her password", forUsername);
            Succeeded();
        }

        public static void Failed()
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message += " failed.";
            ScarfContext.CurrentInternal.PrimaryMessage.Success = false;            
        }

        public static void Succeeded()
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Success = true;
        }

        public static bool HasResult
        {
            get { return ScarfContext.CurrentInternal.PrimaryMessage.Success.HasValue; }
        }
    }
}
