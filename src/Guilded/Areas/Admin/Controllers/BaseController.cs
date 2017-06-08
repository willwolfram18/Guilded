using System.Collections.Generic;
using Guilded.Constants;
using Guilded.Security.Authorization;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Admin.Controllers
{
    [AuthorizeEnabledUser]
    [Area("Admin")]
    public class BaseController : Guilded.Controllers.BaseController
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