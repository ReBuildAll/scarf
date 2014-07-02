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
            if (httpContext != null)
            {
                return GetCurrent(httpContext);
            }
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

        public void CreatePrimaryMessage(
            MessageClass messageClass, 
            string messageType,
            MessageOptions messageOptions = null )
        {
            if (_primaryMessage != null)
            {
                throw new InvalidOperationException("Ambient message already created in this context!");
            }
            if (IsDisposed)
            {
                throw new InvalidOperationException("Disposed context cannot be used!");
            }

            _primaryMessage = ScarfLogging.CreateEmptyMessageInstanceFromClass(messageClass, _httpContext);

            FillMessageDefaultValues(messageClass, messageType, _primaryMessage);

            FillMessageAdditionalInfo(messageOptions);
        }

        private void FillMessageAdditionalInfo(MessageOptions messageOptions)
        {
            if (messageOptions != null && messageOptions.SaveAdditionalInfo)
            {
                _primaryMessage.AddAdditionalInfo(
                    messageOptions.AddFormVariables,
                    messageOptions.AddQueryStringVariables,
                    messageOptions.AddCookies);
            }
        }

        private void FillMessageDefaultValues(MessageClass messageClass, string messageType, ScarfLogMessage message)
        {
            message.EntryId = Guid.NewGuid();
            message.User = FindUser();
            message.ResourceUri = FindResourceUri();
            message.Application = FindApplication();
            message.MessageClass = messageClass;
            message.MessageType = messageType;
            message.Computer = FindComputer();
            message.LoggedAt = DateTime.UtcNow;
            message.Success = null;
            message.Message = MessageType.GetDefaultMessage(messageType);
            message.Source = FindSource();
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
            threadContext = null;
        }

        public bool IsDisposed { get; private set; }

        #endregion
    }

    public class MessageOptions
    {
        public bool SaveAdditionalInfo { get; set; }
        public bool AddFormVariables { get; set; }
        public bool AddQueryStringVariables { get; set; }
        public bool AddCookies { get; set; }
    }
}
