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
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scarf.WebApi;

namespace Scarf.Tests.Infrastructure
{
    [TestClass]
    public abstract class ScarfApiLoggingAttributeTestBase<TAttribute> : InlineScarfContextTestBase
        where TAttribute : ScarfApiLoggingAttribute
    {
        private Exception lastException;

        protected void BeforeAction(TAttribute attribute)
        {
            var context = new HttpActionContext();
            attribute.OnActionExecuting(context);

            lastException = null;
        }

        protected void ActionThrewException(Exception x)
        {
            lastException = x;
        }

        protected void AfterAction(TAttribute attribute, bool addModelStateErrors = false )
        {
            var context = new HttpActionExecutedContext();
            context.ActionContext = new HttpActionContext();
            if (addModelStateErrors)
            {
                context.ActionContext.ModelState.AddModelError("test", new ArgumentException());
            }
            if (lastException != null)
            {
                context.Exception = lastException;
            }
            attribute.OnActionExecuted(context);
        }
    }
}
