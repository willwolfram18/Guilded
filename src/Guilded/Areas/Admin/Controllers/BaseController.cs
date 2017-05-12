using System.Collections.Generic;
using Guilded.Constants;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class BaseController : Controller
    {
        protected readonly Stack<Breadcrumb> Breadcrumbs;

        public BaseController()
        {
            Breadcrumbs = new Stack<Breadcrumb>();
        }

        public override ViewResult View(string viewName, object model)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = "Admin",
                Url = Url.Action(nameof(HomeController.Index), "Home", new { area = "Admin" }),
            });

            ViewData[ViewDataKeys.Breadcrumbs] = Breadcrumbs;

            return base.View(viewName, model);
        }
    }
}