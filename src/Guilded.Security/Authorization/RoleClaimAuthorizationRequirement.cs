using Guilded.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Guilded.Security.Authorization
{
    public class RoleClaimAuthorizationRequirement : IAuthorizationRequirement
    {
        public RoleClaim RequiredRoleClaim { get; }

        public RoleClaimAuthorizationRequirement(RoleClaim requiredRoleClaim)
        {
            RequiredRoleClaim = requiredRoleClaim;
        }
    }
}
