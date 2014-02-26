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
using System.Security;
using System.Threading;
using System.Web;
using Scarf.Configuration;
using Scarf.DataSource;
using Scarf.Utility;

namespace Scarf
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

        private LogMessage currentMessage;
        
        private ScarfContext(HttpContextBase httpContext)
        {
            HttpContext = httpContext;
        }

        public void SetLogMessage(LogMessage message)
        {
            if (currentMessage != null)
            {
                throw new InvalidOperationException();
            }

            currentMessage = message;
        }
        
        public LogMessage CurrentMessage
        {
            get { return currentMessage; }
        }

        public void Commit()
        {
            SaveCurrentMessages(currentMessage);
            currentMessage = null;
        }

        private void SaveCurrentMessages(LogMessage message)
        {
            ScarfDataSource dataSource = DataSourceFactory.CreateDataSourceInstance();
            dataSource.SaveLogMessage(message);
        }

        public LogMessage CreateMessage(
            MessageClass messageClass, 
            string messageType)
        {
            var message = new LogMessage()
            {
                EntryId = Guid.NewGuid(),
                User = FindUser(),
                ResourceURI = FindResourceUri(),
                Application = FindApplication(),
                MessageClass = messageClass,
                MessageType = messageType,
                Computer = FindComputer(),
                LoggedAt = DateTime.UtcNow,
                Success = null,
                Message = MessageType.GetDefaultMessage ( messageType ),
                Source = FindSource(),
            };

            return message;
        }

        private string FindResourceUri()
        {
            if (HttpContext.Request != null)
            {
                return HttpContext.Request.Path;
            }
            else
            {
                return null;
            }
        }

        public void AddAdditionalInfo(LogMessage message, bool addForm, bool addQueryString, bool addCookies)
        {
            if (HttpContext != null)
            {
                var unvalidatedCollections =
                    HttpContext.Request.TryGetUnvalidatedCollections((form, queryString, cookie) => new
                    {
                        Form = form,
                        QueryString = queryString,
                        Cookie = cookie
                    });

                message.AdditionalInfo = new Dictionary<string, Dictionary<string, string>>();

                message.AdditionalInfo.Add(LogMessage.AdditionalInfo_ServerVariables,
                    CollectionUtility.CopyCollection(HttpContext.Request.ServerVariables));

                if (addForm)
                {
                    message.AdditionalInfo.Add(LogMessage.AdditionalInfo_Form,
                        CollectionUtility.CopyCollection(unvalidatedCollections.Form));
                }
                if (addQueryString)
                {
                    message.AdditionalInfo.Add(LogMessage.AdditionalInfo_QueryString,
                        CollectionUtility.CopyCollection(unvalidatedCollections.QueryString));
                }
                if (addCookies)
                {
                    message.AdditionalInfo.Add(LogMessage.AdditionalInfo_Cookies,
                        CollectionUtility.CopyCollection(unvalidatedCollections.Cookie));
                }
            }
        }

        private string FindApplication()
        {
            ScarfSection configuration = ScarfLogging.GetConfiguration();
            if (string.IsNullOrWhiteSpace(configuration.ApplicationName) == false)
            {
                return configuration.ApplicationName;
            }

            string appName = null;
            if (HttpContext.Request != null)
            {
                appName = HttpContext.Request.ServerVariables["APPL_MD_PATH"];
            }

            if (string.IsNullOrEmpty(appName))
            {
                appName = HttpRuntime.AppDomainAppVirtualPath;
            }

            return string.IsNullOrEmpty(appName) ? "/" : appName;

        }

        internal string FindUser()
        {
            string user = Thread.CurrentPrincipal.Identity.Name ?? string.Empty;

            if (HttpContext != null)
            {
                var webUser = HttpContext.User;
                if (webUser != null
                    && (webUser.Identity.Name ?? string.Empty).Length > 0)
                {
                    user = webUser.Identity.Name;
                }
            }

            return user;
        }

        private static string FindComputer()
        {
            string hostName;
            try
            {
                hostName = Environment.MachineName;
            }
            catch (SecurityException)
            {
                // A SecurityException may occur in certain, possibly 
                // user-modified, Medium trust environments.
                hostName = string.Empty;
            }
            return hostName;
        }

        private string FindSource()
        {
            return null;
        }
    }
}
