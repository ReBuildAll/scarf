using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Mvc;
using Scarf.DataSource;
using Scarf.Web;

namespace Scarf.Web.Controllers
{
    public class ScarfController : Controller
    {
        public const int PAGE_SIZE = 50;

        public ActionResult Index( int? page )
        {
            ScarfDataSource dataSource = DataSourceFactory.CreateDataSourceInstance();

            var messages = new List<LogMessage>();
            var pageIndex = page.HasValue ? page.Value - 1 : 0;
            if (pageIndex < 0) pageIndex = 0;

            int totalMessages = dataSource.GetMessages(
                ScarfLogging.GetConfiguration().ApplicationName,
                pageIndex, 
                PAGE_SIZE, 
                messages);
            
            ViewBag.CurrentPage = pageIndex + 1;
            ViewBag.TotalPages = totalMessages/PAGE_SIZE + 1;
            
            return this.ScarfView("Index", messages);
        }

        public ActionResult Details(Guid id)
        {
            ScarfDataSource dataSource = DataSourceFactory.CreateDataSourceInstance();

            LogMessage message = dataSource.GetMessageById(id);

            return this.ScarfView("Details", message);
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
            if (Path.GetExtension(id).ToLower() == ".css")
            {
                return "text/css";
            }
            if (Path.GetExtension(id).ToLower() == ".js")
            {
                return "text/javascript";
            }

            return "application/octet-stream";
        }
    }
}
