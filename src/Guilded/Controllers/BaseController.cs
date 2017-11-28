using Guilded.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        protected StatusCodeResult StatusCode(HttpStatusCode statusCode) => StatusCode((int)statusCode);

        protected ObjectResult StatusCode(HttpStatusCode statusCode, object value) =>
            new ObjectResult(value)
            {
                StatusCode = (int)statusCode
            };
    }
}
