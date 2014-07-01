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

using Scarf.Configuration;

namespace Scarf
{
    public sealed class AuditLogMessage : ScarfLogMessage
    {
        internal override bool CanSave()
        {
            var configuration = ScarfConfiguration.ConfigurationSection;
            if (configuration.Audit != null)
            {
                if (configuration.Audit.Enabled == false)
                {
                    return false;
                }

                if (configuration.Audit.LogOnlyFailures && Success.HasValue && Success.Value == true)
                {
                    return false;
                }
            }

            return true;
        }
    }
}