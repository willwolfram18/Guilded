using Guilded.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Controllers
{
    public class BaseController : Controller
    {
        public override ViewResult View(string viewName, object model)
        {
            ViewData[ViewDataKeys.SuccessMessages] = TempData[ViewDataKeys.SuccessMessages] ??
                ViewData[ViewDataKeys.SuccessMessages];
            ViewData[ViewDataKeys.ErrorMessages] = TempData[ViewDataKeys.ErrorMessages] ??
                ViewData[ViewDataKeys.ErrorMessages];

            return base.View(viewName, model);
        }
    }
}
