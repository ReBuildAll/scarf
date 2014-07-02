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
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scarf.MVC;

namespace Scarf.Tests.Infrastructure
{
    [TestClass]
    public abstract class ScarfLoggingAttributeTestBase<TAttribute> : InlineScarfContextTestBase
        where TAttribute : ScarfLoggingAttribute
    {
        private Exception lastException;
        private bool lastExceptionHandled;
        private bool isCanceled;

        protected void BeforeAction(TAttribute attribute)
        {
            var context = new ActionExecutingContext();
            attribute.OnActionExecuting(context);

            isCanceled = false;
            lastException = null;
            lastExceptionHandled = false;
        }

        protected void CancelAction()
        {
            isCanceled = true;
        }

        protected void ActionThrewException(Exception x, bool handled)
        {
            lastException = x;
            lastExceptionHandled = handled;
        }

        protected void AfterAction(TAttribute attribute, bool addModelStateErrors = false )
        {
            var context = new ActionExecutedContext();
            context.Controller = CreateMockController(addModelStateErrors);
            if (isCanceled)
            {
                context.Canceled = true;
            }
            if (lastException != null)
            {
                context.Exception = lastException;
                context.ExceptionHandled = lastExceptionHandled;
            }
            attribute.OnActionExecuted(context);
        }

        private Controller CreateMockController(bool addModelStateErrors)
        {
            var mockController = new MockController();

            if (addModelStateErrors)
            {
                var state = new ModelState();
                state.Errors.Add(new FormatException());
                state.Value = new ValueProviderResult("1,44", "1.44", new System.Globalization.CultureInfo("fi-FI"));
                mockController.ViewData.ModelState.Add("test", state);
            }

            return mockController;
        }

        private class MockController : Controller { }
    }
}
