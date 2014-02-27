using System.IO;
using System.Web.Mvc;

namespace Scarf.Web
{
    internal sealed class ScarfRazorView : IView
    {
        private WebViewPage viewPage;

        private ControllerContext controllerContext;

        public ScarfRazorView(WebViewPage viewPage, ControllerContext controllerContext)
        {
            this.viewPage = viewPage;
            this.controllerContext = controllerContext;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            var webPageContext = new System.Web.WebPages.WebPageContext(
                controllerContext.HttpContext,
                viewPage,
                viewContext.ViewData.Model);

            viewPage.ExecutePageHierarchy(
                webPageContext,
                writer);
        }
    }
}