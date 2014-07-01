using System.Web.Compilation;
using System.Web.Mvc;

namespace Scarf.Web
{
    public interface ScarfViewResultFactory
    {
        ActionResult Create(Controller controller, string viewName, object model = null);
    }

    public class BuildManagerBasedScarfViewResultFactory : ScarfViewResultFactory
    {
        public ActionResult Create(Controller controller, string viewName, object model = null)
        {
            string viewPath = string.Format("~/Views/Scarf/{0}.cshtml", viewName);

            var viewPage = (WebViewPage)BuildManager.CreateInstanceFromVirtualPath(
                viewPath,
                typeof(WebViewPage));

            return new ScarfViewResult(viewPage, model);
        }
    }
}
