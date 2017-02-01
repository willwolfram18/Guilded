using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selama_SPA.Common.Attributes;
using Selama_SPA.Data.ViewModels.Account;

namespace Selama_SPA.Controllers
{
    [ApiRoute("[controller]")]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public IActionResult SignIn([FromBody] UserSignIn signIn)
        {
            throw new NotImplementedException();
        }
    }
}