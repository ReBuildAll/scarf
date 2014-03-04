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

using System.Web.Http.Filters;
using Scarf.MVC;

namespace Scarf.WebApi
{
    public class LogApiAuditAttribute : ScarfApiLoggingAttribute
    {
        public LogApiAuditAttribute(string messageType)
            : base ( MessageClass.Audit, messageType )
        {
            
        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            if (ScarfAudit.HasResult == false )
            {
                if ( filterContext.Exception != null )
                {
                    ScarfAudit.Failed();
                }
                else
                {
                    ScarfAudit.Succeeded();
                }
            }
            base.OnActionExecuted(filterContext);
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
