using Guilded.Areas.Forums.DAL;
using Guilded.Constants;
using Guilded.Security.Claims;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Guilded.Areas.Forums.Controllers
{
    [Area("Forums")]
    [Route("[area]")]
    [Authorize(RoleClaimValues.ForumsReaderClaim)]
    public abstract class ForumsController : Guilded.Controllers.BaseController
    {
        public const int PageSize = 25;

        protected readonly Stack<Breadcrumb> Breadcrumbs;
        protected readonly IForumsDataContext DataContext;
        protected readonly ILogger Logger;

        protected ForumsController(IForumsDataContext dataContext, ILoggerFactory loggerFactory)
        {
            DataContext = dataContext;
            Breadcrumbs = new Stack<Breadcrumb>();
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected void PushForumBreadcrumb(string forumName, string forumSlug)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = forumName,
                Url = Url.Action("ForumBySlug", "Home", new { area = "Forums", slug = forumSlug })
            });
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
