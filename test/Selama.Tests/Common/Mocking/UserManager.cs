using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Selama.Models;
using System;
using System.Threading.Tasks;

namespace Selama.Tests.Common.Mocking
{
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager()
            : base(new Mock<IUserStore<ApplicationUser>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<ApplicationUser>>().Object,
              new IUserValidator<ApplicationUser>[0],
              new IPasswordValidator<ApplicationUser>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
        {

        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
