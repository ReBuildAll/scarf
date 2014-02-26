using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace log3a
{
    public sealed class ScarfContext
    {
        [ThreadStatic] private static ScarfContext threadContext;

        public static ScarfContext Current
        {
            get
            {
                if (System.Web.HttpContext.Current == null)
                {
                    if (threadContext == null)
                    {
                        threadContext = new ScarfContext(null);
                    }
                    return threadContext;
                }
                else
                {
                    return GetCurrent(new HttpContextWrapper(System.Web.HttpContext.Current));
                }
            }
        }

        private static ScarfContext GetCurrent(HttpContextBase httpContext)
        {
            if (httpContext.Items["LOG3AContext"] == null)
            {
                httpContext.Items["LOG3AContext"] = new ScarfContext(httpContext);
            }

            return httpContext.Items["LOG3AContext"] as ScarfContext;
        }

        private readonly HttpContextBase HttpContext;

        private readonly System.Collections.Concurrent.ConcurrentQueue<LogMessage> queuedMessages;
        
        private ScarfContext(HttpContextBase httpContext)
        {
            HttpContext = httpContext;
            queuedMessages = new System.Collections.Concurrent.ConcurrentQueue<LogMessage>();
        }

        public void QueueLogMessage(LogMessage message)
        {
            
        }
        
    }
}
