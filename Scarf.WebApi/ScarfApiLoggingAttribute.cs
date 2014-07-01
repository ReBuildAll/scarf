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
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace Scarf.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited=true, AllowMultiple=false)]
    public abstract class ScarfApiLoggingAttribute : ActionFilterAttribute
    {
        public bool SaveAdditionalInfo { get; set; }

        public MessageClass MessageClass { get; private set; }

        public string MessageType { get; private set; }
        
        public ScarfApiLoggingAttribute(MessageClass messageClass, string messageType)
        {
            this.SaveAdditionalInfo = true;
            this.MessageClass = messageClass;
            this.MessageType = messageType;
        }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ScarfLogMessage message = ScarfContext.Current.CreatePrimaryMessage(this.MessageClass, this.MessageType);
            if (SaveAdditionalInfo == true)
            {
                ScarfContext.Current.AddAdditionalInfo(message, AddFormVariables, AddQueryStringVariables, AddCookies);
            }
            ScarfContext.Current.SetLogMessage(message);
        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (SaveAdditionalInfo && AddModelState )
            {
                UpdateModelState(filterContext);
            }

            if (filterContext.Exception != null)
            {
                ScarfContext.Current.UpdateCurrentMessageWithDetails(filterContext.Exception.ToString());
            }
            ScarfContext.Current.Commit();
        }

        private void UpdateModelState(HttpActionExecutedContext filterContext)
        {
            if (filterContext.ActionContext.ModelState != null)
            {
                var modelStateInfo = new Dictionary<string, string>();
                foreach (var modelState in filterContext.ActionContext.ModelState)
                {
                    var modelStateErrorMessages = modelState.Value.Errors.Select(FormatModelStateErrorMessage);

                    int index = 0;
                    foreach (var stateError in modelStateErrorMessages)
                    {
                        string stateKey = modelState.Key + string.Format ( "[{0}]", index++);

                        modelStateInfo.Add(
                            stateKey,
                            stateError);
                    }
                }

                if (modelStateInfo.Count > 0)
                {
                    ScarfContext.Current.UpdateCurrentMessageWithAdditionalInfo(ScarfLogMessage.AdditionalInfo_ModelState,
                        modelStateInfo);
                }
                else
                {
                    ScarfContext.Current.UpdateCurrentMessageWithAdditionalInfo(ScarfLogMessage.AdditionalInfo_ModelState,
                        null);
                }
            }
        }

        private static string FormatModelStateErrorMessage(ModelError e)
        {
            return string.Format("{0}\r\n{1}", e.ErrorMessage,
                e.Exception != null ? e.Exception.ToString() : "");
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

        protected virtual bool AddModelState
        {
            get
            {
                return true;
            }
        }
    }
}
