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
using System.Web.Routing;
using Scarf.Web;

[assembly: System.Web.PreApplicationStartMethod(typeof(MvcIntegration), "Start")]

namespace Scarf.Web
{
    public static class MvcIntegration
    {
        public static void Start()
        {
            RegisterRoutes(RouteTable.Routes);

            RegisterVirtualPathProvider();
        }

        private static void RegisterVirtualPathProvider()
        {
            EmbeddedResourceVirtualPathProvider.Register();
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            string[] namespaces = {"Scarf.Web.Controllers"};

            routes.MapRoute(
                "scarf-route",
                "scarf/{action}/{id}",
                new
                {
                    controller = "Scarf",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                null,
                namespaces);
        }
    }
}
