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
    public static class MessageType
    {
        public const string AuditLogin = "Login";

        public const string AuditLogout = "Logout";

        public const string AuditCreateUser = "Create User";

        public const string AuditDeleteUser = "Delete User";

        public const string AuditDisableUser = "Disable User";

        public const string AuditEnableUser = "Enable User";

        public const string AuditResetPassword = "Reset Password";

        public const string AuditChangePassword = "Change Password";

        public const string AuditChangeLogin = "Change Login";

        public const string AuditExternalLogin = "External login";

        public const string AccessRead = "Read";

        public const string AccessWrite = "Write";

        public const string AccessUpload = "Upload";

        public const string AccessDownload = "Download";

        public const string ActionNotificationReceived = "Notification Received";

        public const string ActionNotificationSent = "Notification Sent";

        public const string ActionCommand = "Command";

        public const string ActionServiceCall = "Service Call";

        public const string ActionServiceCallReceived = "Service Call Received";

        public const string ActionUiCommand = "UI-Command";

        public const string ActionPayment = "Payment";

        public const string ActionEvent = "Event";

        public const string DebugMessage = "Debug Message";

        public static string GetDefaultMessage(string messageSubtype)
        {
            switch (messageSubtype)
            {
                case AuditCreateUser:
                    return "Create user";
                case AccessDownload:
                    return "Download object";
                case AccessRead:
                    return "Read object";
                case AccessUpload:
                    return "Upload object";
                case AccessWrite:
                    return "Write object";
                case ActionCommand:
                    return "Performed command";
                case ActionNotificationReceived:
                    return "Received notification";
                case ActionNotificationSent:
                    return "Sent notification";
                case ActionPayment:
                    return "Payment";
                case ActionServiceCall:
                    return "Service call";
                case ActionServiceCallReceived:
                    return "Service call received";
                case ActionUiCommand:
                    return "UI Command";
                case ActionEvent:
                    return "Event";
                case AuditChangeLogin:
                    return "Change login details";
                case AuditChangePassword:
                    return "Change password";
                case AuditDeleteUser:
                    return "Remove user";
                case AuditDisableUser:
                    return "Disable user";
                case AuditEnableUser:
                    return "Enable user";
                case AuditLogin:
                    return "Login";
                case AuditExternalLogin:
                    return "External login";
                case AuditLogout:
                    return "Logout";
                case AuditResetPassword:
                    return "Password was reset";
                case DebugMessage:
                    return "Debug message";

                default:
                    return messageSubtype;
            }
        }
    }
}
