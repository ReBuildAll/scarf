﻿#region Copyright and license
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
        public static void LoggedInAs(string username)
        {
            ScarfContext.Current.CurrentMessage.Message = string.Format("User '{0}' logged in", username);
            Succeeded();
        }
        
        public static void CreatedUser(string username)
        {
            ScarfContext.Current.CurrentMessage.Message = string.Format("Created new user '{0}'", username);
            Succeeded();
        }

        public static void PasswordChanged()
        {
            ScarfContext.Current.CurrentMessage.Message = string.Format("Changed password");
            Succeeded();
        }

        public static void Failed()
        {
            ScarfContext.Current.CurrentMessage.Message += " failed.";
            ScarfContext.Current.CurrentMessage.Success = false;            
        }

        public static void Succeeded()
        {
            ScarfContext.Current.CurrentMessage.Success = true;
        }

        public static bool HasResult
        {
            get { return ScarfContext.Current.CurrentMessage.Success.HasValue; }
        }
    }
}
