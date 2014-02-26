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
    public static class LogMessageSubtype
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

        public const string AccessRead = "Read";

        public const string AccessWrite = "Write";

        public const string AccessUpload = "Upload";

        public const string AccessDownload = "Download";

        public const string ActionNotificationReceived = "Notification Received";

        public const string ActionNotificationSent = "Notification Sent";

        public const string ActionCommand = "Command";

        public const string ActionServiceCall = "Service Call";

        public const string ActionUiCommand = "UI-Command";

        public const string ActionPayment = "Payment";

        public const string DebugMessage = "Debug Message";
    }
}
