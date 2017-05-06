using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Account.Controllers
{
    [Authorize]
    [Area("account")]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [Route("account/sign-in")]
        public ViewResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("account/register")]
        public ViewResult Register()
        {
            return View();
        }
    }
}
