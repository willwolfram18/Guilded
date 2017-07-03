using Guilded.Areas.Forums.DAL;
using Guilded.Constants;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Guilded.Areas.Forums.Controllers
{
    [Area("Forums")]
    [Route("[area]")]
    public abstract class BaseController : Guilded.Controllers.BaseController
    {
        public const int PageSize = 25;

        protected readonly Stack<Breadcrumb> Breadcrumbs;
        protected readonly IForumsDataContext DataContext;

        protected BaseController(IForumsDataContext dataContext)
        {
            DataContext = dataContext;
            Breadcrumbs = new Stack<Breadcrumb>();
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
