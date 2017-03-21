using System;
using System.Collections.Generic;
using System.Text;

namespace Guilded.Security.Claims
{
    public static class RoleClaimTypes
    {
        public const string RoleManagement = "Guilded:Role Management";

        public const string ForumsPinning = "Guilded:Forums Pinning";
        public const string ForumsLocking = "Guilded:Forums Locking";
        public const string ForumsReaded = "Guilded:Forums Reader";
        public const string ForumsWriter = "Guilded:Forums Writer";
    }
}
