using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log3a
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited=true)]
    public abstract class Log3AAttribute : Attribute
    {
    }
}
