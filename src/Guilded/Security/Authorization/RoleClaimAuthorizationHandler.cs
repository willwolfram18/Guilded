using Guilded.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Security.Authorization
{
    public class RoleClaimAuthorizationHandler : AuthorizationHandler<RoleClaimAuthorizationRequirement>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleClaimAuthorizationHandler(ILoggerFactory loggerFactory, 
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _logger = loggerFactory.CreateLogger<RoleClaimAuthorizationHandler>();
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleClaimAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var currentUser = await _userManager.GetUserAsync(context.User);
            var userRoleNames = await _userManager.GetRolesAsync(currentUser);

            var userRoles = _roleManager.Roles.Where(r => userRoleNames.Contains(r.Name));
            var roleClaims = userRoles.SelectMany(r => r.Claims.Select(c => c.ClaimType));

            if (roleClaims.Contains(requirement.RequiredRoleClaim.ClaimType))
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
