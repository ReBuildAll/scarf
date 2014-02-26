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

        public bool SaveAdditionalInfo { get; set; }

        public MessageClass MessageClass { get; private set; }

        public string MessageType { get; private set; }
        
        public ScarfLoggingAttribute(MessageClass messageClass, string messageType)
        {
            this.AutoCommit = true;
            this.SaveAdditionalInfo = true;
            this.MessageClass = messageClass;
            this.MessageType = messageType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            LogMessage message = ScarfContext.Current.CreateMessage(this.MessageClass, this.MessageType);
            if (SaveAdditionalInfo == true)
            {
                ScarfContext.Current.AddAdditionalInfo(message, AddFormVariables, AddQueryStringVariables, AddCookies);
            }
            ScarfContext.Current.SetLogMessage(message);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (AutoCommit)
            {
                if (filterContext.Exception != null)
                {
                    ScarfContext.Current.UpdateCurrentMessageWithDetails(filterContext.Exception.ToString());
                }
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
