using System;
using System.Web.Mvc;

namespace log3a.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited=true)]
    public abstract class Log3AAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Indicates if the logging will be commited as soon as the action has executed
        /// </summary>
        public bool AutoCommit { get; set; }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}
