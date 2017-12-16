using Guilded.DAL;
using Microsoft.AspNetCore.Authorization;
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

            var roleClaimTypes = await _claimsForRequest.GetRoleClaimsAsync();

            if (roleClaimTypes.Contains(requirement.RequiredRoleClaim))
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
