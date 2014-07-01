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
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Scarf.DataSource;

namespace Scarf.Web.Controllers
{
    [ScarfAuthorize]
    public class ScarfController : Controller
    {
        public const int PAGE_SIZE = 50;

        public ActionResult Index( int? page )
        {
            ScarfDataSource dataSource = ScarfConfiguration.DataSourceFactory.CreateDataSourceInstance();

            var messages = new List<ScarfLogMessage>();
            var pageIndex = page.HasValue ? page.Value - 1 : 0;
            if (pageIndex < 0) pageIndex = 0;

            int totalMessages = dataSource.GetMessages(
                ScarfConfiguration.ConfigurationSection.ApplicationName,
                pageIndex, 
                PAGE_SIZE, 
                messages);

            ViewBag.TotalMessages = totalMessages;
            ViewBag.CurrentPage = pageIndex + 1;
            ViewBag.TotalPages = totalMessages/PAGE_SIZE + 1;
            
            return ScarfConfiguration.ViewResultFactory.Create(this, "Index", messages);
        }

        [HandleError(View="Index", ExceptionType=typeof(ArgumentException))]
        public ActionResult Details(Guid? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index");
            }

            ScarfDataSource dataSource = ScarfConfiguration.DataSourceFactory.CreateDataSourceInstance();

            ScarfLogMessage message = dataSource.GetMessageById(id.Value);
            if (message == null) return RedirectToAction("Index");

            return ScarfConfiguration.ViewResultFactory.Create(this, "Details", message);
        }

        public ActionResult Resource(string fileid)
        {
            var provider = new EmbeddedResourceVirtualPathProvider();

            VirtualFile file = provider.GetFile("/scarfresources/" + fileid);
            using (Stream stream = file.Open())
            {
                var contents = new byte[stream.Length];
                stream.Read(contents, 0, contents.Length);
                return new FileContentResult(contents, GetContentType(fileid));
            }
        }

        private string GetContentType(string id)
        {
            var extension = Path.GetExtension(id);
            if (extension != null && extension.ToLower() == ".css")
            {
                return "text/css";
            }
            if (extension != null && extension.ToLower() == ".js")
            {
                return "text/javascript";
            }

            return "application/octet-stream";
        }
    }
}
