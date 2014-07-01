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
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Scarf.Configuration;

namespace Scarf.Web
{
    public class ScarfAuthorizeAttribute : AuthorizeAttribute
    {
        private string _roles;
        private string[] _rolesSplit = new string[0];
        private string _users;
        private string[] _usersSplit = new string[0];

        private bool _requireAuth = true;

        public ScarfAuthorizeAttribute()
        {
            var configuration = ScarfConfiguration.ConfigurationSection;
            if (configuration.Security != null)
            {
                var security = configuration.Security;

                _requireAuth = security.RequireAuthentication;
                _roles = security.AllowRoles;
                _users = security.AllowUsers;

                _rolesSplit = SplitString(_roles);
                _usersSplit = SplitString(_users);
            }
        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            IPrincipal user = httpContext.User;
            if (_requireAuth && !user.Identity.IsAuthenticated)
            {
                return false;
            }

            if (_usersSplit.Length > 0 && !_usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            if (_rolesSplit.Length > 0 && !_rolesSplit.Any(user.IsInRole))
            {
                return false;
            }

            return true;

        }
    }
}
