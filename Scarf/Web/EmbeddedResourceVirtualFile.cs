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