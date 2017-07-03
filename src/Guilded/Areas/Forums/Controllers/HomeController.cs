using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Forums.DAL;
using Guilded.Areas.Forums.ViewModels;
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
        private readonly Stack<Breadcrumb> Breadcrumbs;
        private readonly IForumsDataContext _dataContext;

        public HomeController(IForumsDataContext dataContext)
        {
            _dataContext = dataContext;
            Breadcrumbs = new Stack<Breadcrumb>();
        }

        public ViewResult Index()
        {
            var forumSections = _dataContext.GetActiveForumSections();

            return View(forumSections.OrderBy(f => f.DisplayOrder)
                .ToList()
                .Select(f => new ForumSectionViewModel(f))
            );
        }

        [Route("{slug}")]
        public ViewResult ForumBySlug(string slug)
        {
            throw new NotImplementedException();
        }

        [Route("{id:int}")]
        public ViewResult ForumById(int id)
        {
            throw new NotImplementedException();
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
