using Guilded.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleClaimAuthorizationRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
