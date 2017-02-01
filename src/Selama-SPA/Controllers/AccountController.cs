using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Selama_SPA.Common.Attributes;
using Selama_SPA.Data.ViewModels.Account;
using Selama_SPA.Options;

namespace Selama_SPA.Controllers
{
    [ApiRoute("[controller]")]
    public class AccountController : Controller
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger _logger;

        public AccountController(IOptions<JwtOptions> jwtOptions,
            ILoggerFactory loggerFactory)
        {
            _jwtOptions = jwtOptions.Value;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public IActionResult SignIn([FromBody] UserSignIn signIn)
        {
            throw new NotImplementedException();
        }
    }
}