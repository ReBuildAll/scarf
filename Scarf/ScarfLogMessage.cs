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
using System.Web;
using Scarf.Utility;

namespace Scarf
{
    public class ScarfLogMessage
    {
        public const string AdditionalInfo_Form = "Form";
        public const string AdditionalInfo_Cookies = "Cookies";
        public const string AdditionalInfo_QueryString = "QueryString";
        public const string AdditionalInfo_ServerVariables = "ServerVariables";
        public const string AdditionalInfo_ModelState = "ModelState";
        public const string AdditionalInfo_Custom = "Custom";

        private HttpContextBase _httpContext;

        public ScarfLogMessage(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public Guid EntryId { get; set; }

        public DateTime LoggedAt { get; set; }

        public string ResourceUri { get; set; }

        public string User { get; set; }

        public string Application { get; set; }

        public string Computer { get; set; }
        
        public MessageClass MessageClass { get; set; }

        public string MessageType { get; set; }

        public bool? Success { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        private Dictionary<string, Dictionary<string, string>> additionalInfo;

        public Dictionary<string, Dictionary<string, string>> AdditionalInfo
        {
            get
            {
                EnsureAdditionalInfo();
                return additionalInfo;
            }
        }

        internal virtual bool CanSave() { return true; }

        internal void AddAdditionalInfo(bool addForm, bool addQueryString, bool addCookies)
        {
            if (_httpContext != null)
            {
                var unvalidatedCollections =
                    _httpContext.Request.TryGetUnvalidatedCollections((form, queryString, cookie) => new
                    {
                        Form = form,
                        QueryString = queryString,
                        Cookie = cookie
                    });

                EnsureAdditionalInfo();

                AdditionalInfo.Add(AdditionalInfo_ServerVariables,
                    CollectionUtility.CopyCollection(_httpContext.Request.ServerVariables));

                if (addForm)
                {
                    AdditionalInfo.Add(AdditionalInfo_Form,
                        CollectionUtility.CopyCollection(unvalidatedCollections.Form));
                }
                if (addQueryString)
                {
                    AdditionalInfo.Add(AdditionalInfo_QueryString,
                        CollectionUtility.CopyCollection(unvalidatedCollections.QueryString));
                }
                if (addCookies)
                {
                    AdditionalInfo.Add(AdditionalInfo_Cookies,
                        CollectionUtility.CopyCollection(unvalidatedCollections.Cookie));
                }
            }
        }

        private void EnsureAdditionalInfo()
        {
            if (additionalInfo == null)
            {
                additionalInfo = new Dictionary<string, Dictionary<string, string>>();
            }
        }

        internal static ScarfLogMessage CreateInstanceFromMessageClass(MessageClass messageClass, HttpContextBase httpContext)
        {
            switch (messageClass)
            {
                case MessageClass.Debug:
                    return new DebugLogMessage(httpContext);
                case MessageClass.Audit:
                    return new AuditLogMessage(httpContext);
                case MessageClass.Action:
                    return new ActionLogMessage(httpContext);
                case MessageClass.Access:
                    return new AccessLogMessage(httpContext);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Type GetMessageClassClrType(MessageClass messageClass)
        {
            switch (messageClass)
            {
                case MessageClass.Debug:
                    return typeof(DebugLogMessage);
                case MessageClass.Audit:
                    return typeof(AuditLogMessage);
                case MessageClass.Action:
                    return typeof(ActionLogMessage);
                case MessageClass.Access:
                    return typeof(AccessLogMessage);
                default:
                    throw new InvalidOperationException();
            }
        }

        public void AddCustomInfo(string key, string value)
        {
            if (AdditionalInfo.ContainsKey(AdditionalInfo_Custom) == false)
            {
                AdditionalInfo.Add(AdditionalInfo_Custom, new Dictionary<string, string>());
            }

            AdditionalInfo[AdditionalInfo_Custom].Add(key, value);
        }
    }
}
