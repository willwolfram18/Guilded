using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Selama_SPA.Controllers;
using Selama_SPA.Data.Models.Core;
using Selama_SPA.Options;
using Selama_SPA.Services;

namespace Selama.Tests.Controllers
{
    public class AccountControllerUnitTest : ApiControllerUnitTestBase<AccountController>
    {
        #region Properties
        #region Private Properties
        private readonly Mock<IOptions<JwtOptions>> _mockJwtOptions;
        private readonly Mock<ILoggerFactory> _mockLoggerFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<ISmsSender> _mockSmsSender;
        #endregion
        #endregion

        protected override AccountController SetupController()
        {
            return new AccountController(_mockJwtOptions.Object,
                _mockLoggerFactory.Object,
                _userManager,
                _signInManager,
                _mockEmailSender.Object,
                _mockSmsSender.Object
            );
        }
    }
}