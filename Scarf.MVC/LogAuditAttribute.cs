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
    public class LogAuditAttribute : ScarfLoggingAttribute
    {
        public LogAuditAttribute(string messageType)
            : base ( MessageClass.Audit, messageType )
        {
            
        }

        public override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            if (AutoCommit && ScarfContext.Current.CurrentMessage.Success.HasValue == false )
            {
                if (filterContext.Canceled ||
                    (filterContext.Exception != null &&
                     filterContext.ExceptionHandled == false))
                {
                    ScarfContext.Current.CurrentMessage.Success = false;
                }
                else
                {
                    ScarfContext.Current.CurrentMessage.Success = true;                    
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
