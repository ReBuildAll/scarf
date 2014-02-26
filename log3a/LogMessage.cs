using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log3a
{
    public sealed class LogMessage
    {
        public Guid EntryId { get; set; }

        public DateTime LoggedAt { get; set; }

        public string User { get; set; }

        public string Application { get; set; }

        public string Computer { get; set; }
        
        public LogMessageType Type { get; set; }

        public string SubType { get; set; }

        public bool Success { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public Dictionary<string,Dictionary<string,string>> AdditionalInfo { get; set; }
    }
}
