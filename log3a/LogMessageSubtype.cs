using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log3a
{
    public sealed class LogMessageSubtype
    {
        public static readonly string AuditLogin = "Login";

        public static readonly string AuditLogout = "Logout";

        public static readonly string AuditCreateUser = "Create User";

        public static readonly string AuditDeleteUser = "Delete User";

        public static readonly string AuditDisableUser = "Disable User";

        public static readonly string AuditEnableUser = "Enable User";

        public static readonly string AuditResetPassword = "Reset Password";

        public static readonly string AuditChangePassword = "Change Password";

        public static readonly string AuditChangeLogin = "Change Login";

        public static readonly string AccessRead = "Read";

        public static readonly string AccessWrite = "Write";

        public static readonly string AccessUpload = "Upload";

        public static readonly string AccessDownload = "Download";

        public static readonly string ActionNotificationReceived = "Notification Received";

        public static readonly string ActionNotificationSent = "Notification Sent";

        public static readonly string ActionCommand = "Command";

        public static readonly string ActionServiceCall = "Service Call";

        public static readonly string ActionUiCommand = "UI-Command";

        public static readonly string ActionPayment = "Payment";

        public static readonly string DebugMessage = "Debug Message";
    }
}
