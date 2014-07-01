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

        [HttpGet]
        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        [LogAction(MessageType.ActionUiCommand, Message = "Feedback received")]
        public ActionResult Feedback( FormCollection form )
        {
            return RedirectToAction("Index");
        }
    }
}