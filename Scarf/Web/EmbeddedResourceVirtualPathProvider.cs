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
using System.Linq;
using System.Web.Hosting;

namespace Scarf.Web
{
    internal sealed class EmbeddedResourceVirtualPathProvider : VirtualPathProvider
    {
        public static void Register()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceVirtualPathProvider());
        }

        private static Dictionary<string, string> MapPathToResourceName;

        static EmbeddedResourceVirtualPathProvider()
        {
            MapPathToResourceName = new Dictionary<string, string>
            {
                {"/Views/Scarf/", "Scarf.Web.Views."},
                {"/scarfresources/", "Scarf.Web.Content."}
            };
        }

        public override bool FileExists(string virtualPath)
        {
            if (IsMyPath(virtualPath))
            {
                string resourceName = GetResourceNameFromPath(virtualPath);

                if (CheckResourceExists(resourceName))
                {
                    return true;
                }
            }
            return base.FileExists(virtualPath);
        }

        private static bool IsMyPath(string virtualPath)
        {
            return MapPathToResourceName.Any(kvp => virtualPath.StartsWith(kvp.Key));
        }

        private string GetResourceNameFromPath(string virtualPath)
        {
            KeyValuePair<string, string> map = 
                MapPathToResourceName.Single(kvp => virtualPath.StartsWith(kvp.Key));

            return map.Value + virtualPath.Substring(map.Key.Length);
        }

        private bool CheckResourceExists(string resourceName)
        {
            var asm = GetType().Assembly;
            var resources = asm.GetManifestResourceNames();
            return resources.Contains(resourceName);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (!IsMyPath(virtualPath)) return base.GetFile(virtualPath);
            var resourceName = GetResourceNameFromPath(virtualPath);
            if (CheckResourceExists(resourceName) == false) return base.GetFile(virtualPath);

            return new EmbeddedResourceVirtualFile(resourceName, virtualPath);
        }

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (!IsMyPath(virtualPath)) return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

            return new System.Web.Caching.CacheDependency(GetType().Assembly.Location);
        }
    }
}