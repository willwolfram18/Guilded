using Guilded.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Security.Authorization
{
    public class RoleClaimAuthorizationHandler : AuthorizationHandler<RoleClaimAuthorizationRequirement>
    {
        private readonly IUserRoleClaimsForRequest _claimsForRequest;

        public RoleClaimAuthorizationHandler(IUserRoleClaimsForRequest claimsForRequest)
        {
            _claimsForRequest = claimsForRequest;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleClaimAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var roleClaimTypes = new List<string>(
                (await _claimsForRequest.GetRoleClaimsAsync())
                .Select(c => c.ClaimValue)
            );

            if (roleClaimTypes.Contains(requirement.RequiredRoleClaim.ClaimValue))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
