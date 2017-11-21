using Guilded.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Security.Authorization
{
    public class MultiRoleClaimAuthorizationHandler : AuthorizationHandler<MultiRoleClaimAuthorizationRequirement>
    {
        private readonly IUserRoleClaimsForRequest _claimsForRequest;

        public MultiRoleClaimAuthorizationHandler(IUserRoleClaimsForRequest claimsForRequest)
        {
            _claimsForRequest = claimsForRequest;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MultiRoleClaimAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var roleClaims = (await _claimsForRequest.GetRoleClaimsAsync()).ToArray();
            foreach (var possibleRoleClaim in requirement.PossibleClaims)
            {
                if (roleClaims.Contains(possibleRoleClaim))
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            context.Fail();
        }
    }
}
