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
    public sealed class ScarfContext : IDisposable
    {
        #region Static

        [ThreadStatic] private static ScarfContext threadContext;

        public static ScarfContext Current
        {
            get
            {
                // inline context
                if (threadContext != null)
                {
                    return threadContext;
                }
                // thread static context
                if (System.Web.HttpContext.Current == null)
                {
                    return GetThreadContext();
                }
                // httpcontext based context
                return GetCurrent(new HttpContextWrapper(System.Web.HttpContext.Current));
            }
        }

        public static ScarfContext CreateInlineContext(HttpContextBase httpContext = null )
        {
            if (threadContext != null)
            {
                throw new InvalidOperationException("Cannot have multiple inline contexts on the same thread!");
            }
            return GetThreadContext();
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

        #endregion

        private readonly HttpContextBase _httpContext;

        private ScarfLogMessage _primaryMessage;

        private List<ScarfLogMessage> _secondaryMessages = new List<ScarfLogMessage>();
        
        private ScarfContext(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public void SetLogMessage(ScarfLogMessage message)
        {
            if (_primaryMessage != null)
            {
                throw new InvalidOperationException();
            }

            _primaryMessage = message;
        }
        
        internal ScarfLogMessage PrimaryMessage
        {
            get { return _primaryMessage; }
        }

        public void UpdateCurrentMessageWithDetails(string details)
        {
            PrimaryMessage.Details = details;
        }

        public void UpdateCurrentMessageWithAdditionalInfo(string infoKey, Dictionary<string, string> info)
        {
            Contract.Assert(PrimaryMessage != null);

            PrimaryMessage.AdditionalInfo.Add(infoKey, info);
        }

        public void Commit()
        {
            SaveMessage(_primaryMessage);
            _primaryMessage = null;
        }

        public void Rollback()
        {
            _primaryMessage = null;
            _secondaryMessages.Clear();
        }

        public void SaveMessage(ScarfLogMessage message)
        {
            if (message.CanSave())
            {
                ScarfDataSource dataSource = ScarfConfiguration.DataSourceFactory.CreateDataSourceInstance();
                dataSource.SaveLogMessage(message);
            }
        }

        public ScarfLogMessage CreatePrimaryMessage(
            MessageClass messageClass, 
            string messageType)
        {
            if (_primaryMessage != null)
            {
                throw new InvalidOperationException("Ambient message already created in this context!");
            }
            if (IsDisposed)
            {
                throw new InvalidOperationException("Disposed context cannot be used!");
            }

            _primaryMessage = ScarfLogging.CreateEmptyMessageInstanceFromClass(messageClass);

            FillMessageDefaultValues(messageClass, messageType, _primaryMessage);

            return _primaryMessage;
        }

        private void FillMessageDefaultValues(MessageClass messageClass, string messageType, ScarfLogMessage message)
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
            if (_httpContext != null && _httpContext.Request != null)
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
            if (_httpContext != null)
            {
                var webUser = _httpContext.User;
                if (webUser != null
                    && (webUser.Identity.Name ?? string.Empty).Length > 0)
                {
                    return webUser.Identity.Name;
                }
            }

            return Thread.CurrentPrincipal.Identity.Name ?? string.Empty;
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

        #region IDisposable 

        public void Dispose()
        {
            Dispose(true);
        }

        ~ScarfContext()
        {            
            Dispose(false);
        }

        private void Dispose(bool byUser)
        {
            if (byUser)
            {
                GC.SuppressFinalize(this);
            }

            Rollback();
            IsDisposed = true;
            if (threadContext != null)
            {
                threadContext = null;
            }
        }

        public bool IsDisposed { get; private set; }

        #endregion
    }
}
