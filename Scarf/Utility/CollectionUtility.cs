#region Copyright and license
//
// SCARF - Security Audit, Access and Action Logging
// Copyright (c) 2014 ReBuildAll Solutions Ltd
//
// Author:
//    Lenard Gunda 
//
// Licensed under MIT license, see included LICENSE file for details
// ---------------------------------------------------------------------
// These methods are based on the open source ELMAH project and licensed
// under the Apache-2 license. https://code.google.com/p/elmah/
// ---------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Scarf.Utility
{
    internal static class CollectionUtility
    {
        internal static Dictionary<string,string> CopyCollection(NameValueCollection collection)
        {
            if (collection == null || collection.Count == 0)
                return null;

            return collection.AllKeys.ToDictionary(key => key, key => collection[key]);
        }

        internal static Dictionary<string,string> CopyCollection(HttpCookieCollection cookies)
        {
            if (cookies == null || cookies.Count == 0)
                return null;

            var copy = new Dictionary<string,string>();

            for (var i = 0; i < cookies.Count; i++)
            {
                var cookie = cookies[i];

                //
                // NOTE: We drop the Path and Domain properties of the 
                // cookie for sake of simplicity.
                //

                if (cookie != null)
                {
                    copy.Add(cookie.Name, cookie.Value);
                }
            }

            return copy;
        }
    }
}
