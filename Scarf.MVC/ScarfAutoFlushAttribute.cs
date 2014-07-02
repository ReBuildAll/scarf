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
using System.Web.Mvc;

namespace Scarf.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ScarfAutoFlushAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ScarfLogging.CurrentContext.Commit();
        }
    }
}
