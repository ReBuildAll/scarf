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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited=true, AllowMultiple=false)]
    public abstract class ScarfLoggingAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Indicates if the logging will be commited as soon as the action has executed
        /// </summary>
        public bool AutoCommit { get; set; }

        public LogMessageType MessageType { get; private set; }

        public string MessageSubtype { get; private set; }
        
        public ScarfLoggingAttribute(LogMessageType messageType, string messageSubtype)
        {
            this.AutoCommit = true;
            this.MessageType = messageType;
            this.MessageSubtype = messageSubtype;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            LogMessage message = ScarfContext.Current.CreateMessage(this.MessageType, this.MessageSubtype);
            ScarfContext.Current.AddAdditionalInfo(message, AddFormVariables, AddQueryStringVariables, AddCookies);
            ScarfContext.Current.QueueLogMessage(message);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (AutoCommit)
            {
                ScarfContext.Current.Commit();
            }
        }

        protected virtual bool AddFormVariables
        {
            get
            {
                return true;
            }
        }

        protected virtual bool AddCookies
        {
            get
            {
                return true;
            }
        }

        protected virtual bool AddQueryStringVariables
        {
            get
            {
                return true;
            }
        }
    }
}
