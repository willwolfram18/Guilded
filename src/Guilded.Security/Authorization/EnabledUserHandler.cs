using Guilded.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Guilded.Security.Authorization
{
    public class EnabledUserHandler : AuthorizationHandler<EnabledUserRequirement>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public EnabledUserHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EnabledUserRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var currentUser = await _userManager.GetUserAsync(context.User);

            if (currentUser.IsTemporarilyDisabled)
            {
                if (!currentUser.IsTemporaryDisableOver)
                {
                    await FailRequirement(context);
                    return;
                }

                currentUser.IsEnabled = true;
                currentUser.EnabledAfter = null;
                await _userManager.UpdateAsync(currentUser);

                context.Succeed(requirement);
                return;
            }

            if (!currentUser.IsEnabled)
            {
                await FailRequirement(context);
                return;
            }

            context.Succeed(requirement);
        }

        private async Task FailRequirement(AuthorizationHandlerContext context)
        {
            context.Fail();
            await _signInManager.SignOutAsync();
        }
    }
}
