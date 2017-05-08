using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}
