using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Selama_SPA.Common.Attributes;
using Selama_SPA.Data.Models.Core;
using Selama_SPA.Data.ViewModels.Account;
using Selama_SPA.Options;
using Selama_SPA.Services;

namespace Selama_SPA.Controllers
{
    [ApiRoute("[controller]")]
    public class AccountController : Controller
    {
        #region Properties
        #region Private Properties
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender; 
        private readonly ISmsSender _smsSender;
        #endregion
        #endregion

        public AccountController(IOptions<JwtOptions> jwtOptions,
            ILoggerFactory loggerFactory,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender)
        {
            _jwtOptions = jwtOptions.Value;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public IActionResult SignIn([FromBody] UserSignIn signIn)
        {
            throw new NotImplementedException();
        }
    }
}