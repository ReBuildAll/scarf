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
    public sealed class DebugLogMessage : LogMessage
    {
        internal override bool CanSave()
        {
            var configuration = ScarfLogging.GetConfiguration();
            if (configuration.Debug != null && configuration.Debug.Enabled == false)
            {
                return false;
            }

            return true;
        }
    }
}