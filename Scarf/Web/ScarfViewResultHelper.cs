using System.Web.Compilation;
using System.Web.Mvc;

namespace Scarf.Web
{
    public static class ScarfViewResultHelper
    {
        public static ActionResult ScarfView(this Controller controller, string viewName, object model = null)
        {
            string viewPath = string.Format("~/Views/Scarf/{0}.cshtml", viewName);

            var viewPage = (WebViewPage)BuildManager.CreateInstanceFromVirtualPath(
                viewPath, 
                typeof(WebViewPage));

            return new ScarfViewResult(viewPage, model);
        }

    }
}