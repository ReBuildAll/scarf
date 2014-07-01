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

using System.Web.Mvc;

namespace Scarf.Web
{
    internal sealed class ScarfViewResult : ActionResult
    {
        private readonly WebViewPage viewPage;

        private readonly object model;

        public ScarfViewResult(WebViewPage viewPage, object model)
        {
            this.viewPage = viewPage;
            this.model = model;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var view = new ScarfRazorView(viewPage, context);

            viewPage.ViewContext = new ViewContext(
                context,
                view,
                context.Controller.ViewData,
                new TempDataDictionary(),
                context.HttpContext.Response.Output);

            viewPage.ViewContext.RouteData = context.RouteData;
            viewPage.ViewData = context.Controller.ViewData;
            viewPage.ViewData.Model = model;
            viewPage.InitHelpers();

            view.Render(viewPage.ViewContext, context.HttpContext.Response.Output);
        }
    }
}