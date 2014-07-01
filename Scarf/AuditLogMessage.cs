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

using System.Web;

namespace Scarf
{
    public sealed class AuditLogMessage : ScarfLogMessage
    {
        public AuditLogMessage(HttpContextBase httpContext) : base(httpContext)
        {
        }

        internal override bool CanSave()
        {
            if (ScarfConfiguration.IsAuditLoggingEnabled)
            {
                if (!ScarfConfiguration.IsAuditLoggingOnlyForFailures || (Success.HasValue && Success.Value == false))
                {
                    return true;
                }
            }

            return false;
        }
    }
}