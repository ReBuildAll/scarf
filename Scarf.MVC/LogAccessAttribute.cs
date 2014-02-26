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

namespace Scarf.MVC
{
    public class LogAccessAttribute : ScarfLoggingAttribute
    {
        public LogAccessAttribute(string messageSubtype)
            : base(LogMessageType.Access, messageSubtype)
        {
        }

        protected override bool AddQueryStringVariables
        {
            get
            {
                return false;
            }
        }

        protected override bool AddFormVariables
        {
            get
            {
                return false;
            }
        }
    }
}