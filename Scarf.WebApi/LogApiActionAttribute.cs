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
using System.Web.Http.Filters;

namespace Scarf.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class LogApiActionAttribute : ScarfApiLoggingAttribute
    {
        public string Message { get; set; }

        public LogApiActionAttribute(string messageType) :
            base(MessageClass.Action, messageType)
        {
        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            if (string.IsNullOrWhiteSpace(Message) == false)
            {
                ScarfAction.SetMessage(Message);
            }
            base.OnActionExecuted(filterContext);
        }
    }
}