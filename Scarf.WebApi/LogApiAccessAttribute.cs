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

namespace Scarf.WebApi
{
    public class LogApiAccessAttribute : ScarfApiLoggingAttribute
    {
        public LogApiAccessAttribute(string messageType)
            : base(MessageClass.Access, messageType)
        {
        }

        protected override bool AddQueryStringVariables
        {
            get { return false; }
        }

        protected override bool AddFormVariables
        {
            get { return false; }
        }

        protected override bool AddModelState
        {
            get { return false; }
        }
    }
}