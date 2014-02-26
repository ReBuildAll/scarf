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

using System;

namespace Scarf.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class LogActionAttribute : ScarfLoggingAttribute
    {
        public LogActionAttribute(string messageType) :
            base(MessageClass.Action, messageType)
        {
        }
    }
}