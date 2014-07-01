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

using System.IO;
using System.Web.Hosting;

namespace Scarf.Web
{
    internal sealed class EmbeddedResourceVirtualFile : VirtualFile
    {
        private readonly string resourceName;
        private readonly string _virtualPath;

        public EmbeddedResourceVirtualFile(string resourceName, string virtualPath)
            : base(virtualPath)
        {
            this.resourceName = resourceName;
            this._virtualPath = virtualPath;
        }

        public override Stream Open()
        {
            var asm = GetType().Assembly;
            // ReSharper disable once AssignNullToNotNullAttribute
            return asm.GetManifestResourceStream(resourceName);
        }

        public override bool IsDirectory
        {
            get
            {
                return false;
            }
        }

        public override string Name
        {
            get
            {
                return _virtualPath;
            }
        }
    }
}