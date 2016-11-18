using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Selama.Models;
using System.Threading.Tasks;

namespace Selama.Tests.Common.Mocking
{
    public class SignInManager : SignInManager<ApplicationUser>
    {
        public SignInManager(IHttpContextAccessor contextAccessor)
            : base(new UserManager(),
              contextAccessor,
              new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<ILogger<SignInManager<ApplicationUser>>>().Object)
        {
        }

        public override Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null)
        {
            return Task.FromResult(0);
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return Task.FromResult(SignInResult.Success);
        }

        public override Task SignOutAsync()
        {
            return Task.FromResult(0);
        }
    }
}
