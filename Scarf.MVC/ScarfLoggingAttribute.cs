﻿#region Copyright and license
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
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Scarf.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited=true, AllowMultiple=false)]
    public abstract class ScarfLoggingAttribute : ActionFilterAttribute
    {
        public bool SaveAdditionalInfo { get; set; }

        public MessageClass MessageClass { get; private set; }

        public string MessageType { get; private set; }
        
        public ScarfLoggingAttribute(MessageClass messageClass, string messageType)
        {
            this.SaveAdditionalInfo = true;
            this.MessageClass = messageClass;
            this.MessageType = messageType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var options = new MessageOptions()
            {
                SaveAdditionalInfo = SaveAdditionalInfo,
                AddFormVariables = AddFormVariables,
                AddQueryStringVariables = AddQueryStringVariables,
                AddCookies = AddCookies
            };

            ScarfLogging.CurrentContext.CreateMessage(this.MessageClass, this.MessageType, options );
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (SaveAdditionalInfo && AddModelState )
            {
                UpdateModelState(filterContext);
            }

            if (filterContext.Exception != null)
            {
                ScarfLogging.CurrentContext.UpdateMessageDetails(filterContext.Exception.ToString());
            }

            ScarfLogging.CurrentContext.Commit();
        }

        private void UpdateModelState(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState != null)
            {
                var modelStateInfo = new Dictionary<string, string>();
                foreach (var modelState in filterContext.Controller.ViewData.ModelState)
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
                    ScarfLogging.CurrentContext.UpdateMessageAdditionalInfo(ScarfLogMessage.AdditionalInfo_ModelState,
                        modelStateInfo);
                }
                else
                {
                    ScarfLogging.CurrentContext.UpdateMessageAdditionalInfo(ScarfLogMessage.AdditionalInfo_ModelState,
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
