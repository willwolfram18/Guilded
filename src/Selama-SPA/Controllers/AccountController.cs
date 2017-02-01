using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public IActionResult SignIn([FromBody] UserSignIn signIn)
        {
            throw new NotImplementedException();
        }
    }
}