using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Constants;
using Guilded.Controllers;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Forums.Controllers
{
    [Area("Forums")]
    [Route("[area]")]
    public class HomeController : BaseController
    {
        protected readonly Stack<Breadcrumb> Breadcrumbs;

        public HomeController()
        {
            Breadcrumbs = new Stack<Breadcrumb>();
        }

        public ViewResult Index()
        {
            return View();
        }

        public override ViewResult View(string viewName, object model)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = "Forums",
                Url = Url.Action("Index", "Home", new { area = "Forums" })
            });

            ViewData[ViewDataKeys.Breadcrumbs] = Breadcrumbs;

            return base.View(viewName, model);
        }
    }
}
