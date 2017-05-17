using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}