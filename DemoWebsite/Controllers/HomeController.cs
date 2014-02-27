using System;
using System.IO;
using System.Resources;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Mvc;
using Scarf;
using Scarf.MVC;

namespace DemoWebsite.Controllers
{
    public class HomeController : Controller
    {
        [LogAccess(MessageType.AccessRead, SaveAdditionalInfo = false)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            ScarfLogging.Debug("You are now in About()!!");

            return View();
        }

        public ActionResult Contact()
        {           
            ViewBag.Message = "Scarf is open source software.";

            return View();
        }

    }
}