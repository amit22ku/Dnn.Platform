﻿#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2014
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System.Threading;
using System.Web;
using System.Web.WebPages;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Security;
using DotNetNuke.Security.Permissions;

namespace DotNetNuke.Entities.Tabs.TabVersions
{
    internal static class TabVersionUtils
    {
        internal static bool TryGetUrlVersion(out int versionInt)
        {
            var version = HttpContext.Current.Request.QueryString["version"];
            if (version.IsEmpty())
            {
                versionInt = Null.NullInteger;
                return false;
            }
            return int.TryParse(version, out versionInt);
        }

        internal static bool CanSeeVersionedPages()
        {
            return CanSeeVersionedPages(TabController.CurrentPage);
        }

        internal static bool CanSeeVersionedPages(TabInfo tab)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return false;
            }

            var currentPortal = PortalController.Instance.GetCurrentPortalSettings();
            var isAdminUser = currentPortal.UserInfo.IsSuperUser || PortalSecurity.IsInRole(currentPortal.AdministratorRoleName);
            if (isAdminUser)
            {
                return true;
            }

            return TabPermissionController.HasTabPermission(tab.TabPermissions, "EDIT,CONTENT,MANAGE");
        }
    }
}
