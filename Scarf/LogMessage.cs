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

namespace Scarf
{
    public sealed class LogMessage
    {
        public const string AdditionalInfo_Form = "Form";
        public const string AdditionalInfo_Cookies = "Cookies";
        public const string AdditionalInfo_QueryString = "QueryString";
        public const string AdditionalInfo_ServerVariables = "ServerVariables";


        public Guid EntryId { get; set; }

        public DateTime LoggedAt { get; set; }

        public string User { get; set; }

        public string Application { get; set; }

        public string Computer { get; set; }
        
        public LogMessageType Type { get; set; }

        public string SubType { get; set; }

        public bool? Success { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public Dictionary<string,Dictionary<string,string>> AdditionalInfo { get; set; }
    }
}
