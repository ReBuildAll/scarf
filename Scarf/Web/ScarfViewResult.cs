using System;
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