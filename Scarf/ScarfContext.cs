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
using System.Diagnostics.Contracts;
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
                    return GetThreadContext();
                }

                return GetCurrent(new HttpContextWrapper(System.Web.HttpContext.Current));
            }
        }

        private static ScarfContext GetThreadContext()
        {
            if (threadContext == null)
            {
                threadContext = new ScarfContext(null);
            }
            return threadContext;
        }

        private static ScarfContext GetCurrent(HttpContextBase httpContext)
        {
            if (httpContext.Items["ScarfContext"] == null)
            {
                httpContext.Items["ScarfContext"] = new ScarfContext(httpContext);
            }

            return httpContext.Items["ScarfContext"] as ScarfContext;
        }

        private readonly HttpContextBase _httpContext;

        private ScarfLogMessage currentMessage;
        
        private ScarfContext(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public void SetLogMessage(ScarfLogMessage message)
        {
            if (currentMessage != null)
            {
                throw new InvalidOperationException();
            }

            currentMessage = message;
        }
        
        internal ScarfLogMessage CurrentMessage
        {
            get { return currentMessage; }
        }

        public void UpdateCurrentMessageWithDetails(string details)
        {
            CurrentMessage.Details = details;
        }

        public void UpdateCurrentMessageWithAdditionalInfo(string infoKey, Dictionary<string, string> info)
        {
            Contract.Assert(CurrentMessage != null);

            CurrentMessage.AdditionalInfo.Add(infoKey, info);
        }

        public void Commit()
        {
            SaveMessage(currentMessage);
            currentMessage = null;
        }

        public void Rollback()
        {
            currentMessage = null;
        }

        public void SaveMessage(ScarfLogMessage message)
        {
            if (message.CanSave())
            {
                ScarfDataSource dataSource = ScarfConfiguration.DataSourceFactory.CreateDataSourceInstance();
                dataSource.SaveLogMessage(message);
            }
        }

        public ScarfLogMessage CreateMessage(
            MessageClass messageClass, 
            string messageType)
        {
            ScarfLogMessage message = ScarfLogging.CreateEmptyMessageInstanceFromClass(messageClass);

            FillMessageValues(messageClass, messageType, message);

            return message;
        }

        private void FillMessageValues(MessageClass messageClass, string messageType, ScarfLogMessage message)
        {
            message.EntryId = Guid.NewGuid();
            message.User = FindUser();
            message.ResourceURI = FindResourceUri();
            message.Application = FindApplication();
            message.MessageClass = messageClass;
            message.MessageType = messageType;
            message.Computer = FindComputer();
            message.LoggedAt = DateTime.UtcNow;
            message.Success = null;
            message.Message = MessageType.GetDefaultMessage(messageType);
            message.Source = FindSource();
        }

        public void AddAdditionalInfo(ScarfLogMessage message, bool addForm, bool addQueryString, bool addCookies)
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

                message.AdditionalInfo = new Dictionary<string, Dictionary<string, string>>();

                message.AdditionalInfo.Add(ScarfLogMessage.AdditionalInfo_ServerVariables,
                    CollectionUtility.CopyCollection(_httpContext.Request.ServerVariables));

                if (addForm)
                {
                    message.AdditionalInfo.Add(ScarfLogMessage.AdditionalInfo_Form,
                        CollectionUtility.CopyCollection(unvalidatedCollections.Form));
                }
                if (addQueryString)
                {
                    message.AdditionalInfo.Add(ScarfLogMessage.AdditionalInfo_QueryString,
                        CollectionUtility.CopyCollection(unvalidatedCollections.QueryString));
                }
                if (addCookies)
                {
                    message.AdditionalInfo.Add(ScarfLogMessage.AdditionalInfo_Cookies,
                        CollectionUtility.CopyCollection(unvalidatedCollections.Cookie));
                }
            }
        }

        #region Find information methods

        private string FindResourceUri()
        {
            if (_httpContext.Request != null)
            {
                return _httpContext.Request.Path;
            }
            
            return null;
        }

        private string FindApplication()
        {
            ScarfSection configuration = ScarfConfiguration.ConfigurationSection;
            if (string.IsNullOrWhiteSpace(configuration.ApplicationName) == false)
            {
                return configuration.ApplicationName;
            }

            string appName = null;
            if (_httpContext.Request != null)
            {
                appName = _httpContext.Request.ServerVariables["APPL_MD_PATH"];
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

            if (_httpContext == null) return user;

            var webUser = _httpContext.User;
            if (webUser != null
                && (webUser.Identity.Name ?? string.Empty).Length > 0)
            {
                user = webUser.Identity.Name;
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

        #endregion
    }
}
