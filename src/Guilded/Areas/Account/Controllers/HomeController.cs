using Guilded.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Account.Controllers
{
    [Authorize]
    [Area("account")]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [HttpGet("account/sign-in")]
        public ViewResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("account/sign-in")]
        public ViewResult SignIn(SignInUser user)
        {
            return View(user);
        }

        [AllowAnonymous]
        [Route("account/register")]
        public ViewResult Register()
        {
            return View();
        }
    }
}
