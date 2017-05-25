using Guilded.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Guilded.Security.Authorization
{
    public class EnabledUserHandler : AuthorizationHandler<EnabledUserRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EnabledUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EnabledUserRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var currentUser = await _userManager.GetUserAsync(context.User);

            if (currentUser.IsTemporarilyDisabled && currentUser.IsTemporaryDisableOver)
            {
                currentUser.IsEnabled = true;
                currentUser.EnabledAfter = null;
                await _userManager.UpdateAsync(currentUser);

                context.Succeed(requirement);
                return;
            }

            if (!currentUser.IsEnabled)
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
    }
}
