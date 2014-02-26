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
        public static void DebugMessage(string message, string details = null)
        {
            
        }

        public static ScarfSection GetConfiguration()
        {
            return ConfigurationManager.GetSection("scarf") as ScarfSection;
        }
    }
}
