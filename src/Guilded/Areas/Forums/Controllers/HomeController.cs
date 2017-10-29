using Guilded.Areas.Forums.DAL;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Constants;

namespace Guilded.Areas.Forums.Controllers
{
    public class HomeController : ForumsController
    {
        public HomeController(IForumsDataContext dataContext, ILoggerFactory loggerFactory) : base(dataContext, loggerFactory)
        {
        }

        public ViewResult Index()
        {
            var forumSections = DataContext.GetActiveForumSections();

            return View(forumSections.OrderBy(f => f.DisplayOrder)
                .ToList()
                .Select(f => new ForumSectionViewModel(f))
            );
        }

        [Route("{slug}")]
        public async Task<IActionResult> ForumBySlug(string slug, int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction(nameof(ForumBySlug), new { slug });
            }

            var forum = await DataContext.GetForumBySlugAsync(slug);

            if (forum == null)
            {
                return RedirectToForumsHome();
            }

            var viewModel = CreatePaginatedForumViewModel(forum, page);

            if (viewModel.LastPage == 0 && page != 1)
            {
                return RedirectToAction(nameof(ForumBySlug), new {slug});
            }
            if (viewModel.LastPage != 0 && page > viewModel.LastPage)
            {
                return RedirectToAction(nameof(ForumBySlug), new {slug, page = viewModel.LastPage});
            }

            return ForumView(viewModel);
        }

        [Route("{id:int}")]
        public async Task<IActionResult> ForumById(int id)
        {
            var forum = await DataContext.GetForumByIdAsync(id);

            if (forum == null)
            {
                return RedirectToForumsHome();
            }

            return RedirectToAction(nameof(ForumBySlug), new {slug = forum.Slug});
        }

        private RedirectToActionResult RedirectToForumsHome()
        {
            TempData[ViewDataKeys.ErrorMessages] = "That forum does not exist.";
            return RedirectToAction(nameof(Index), "Home", new { area = "Forums" });
        }

        private ForumViewModel CreatePaginatedForumViewModel(Forum forum, int page)
        {
            var zeroIndexedPage = page - 1;
            var pinnedThreads = forum.Threads.Where(t => !t.IsDeleted && t.IsPinned).OrderByDescending(t => t.CreatedAt);
            var threads = forum.Threads.Where(t => !t.IsDeleted && !t.IsPinned).OrderByDescending(t => t.CreatedAt);

            return new ForumViewModel(forum)
            {
                CurrentPage = page,
                PagerUrl = Url.Action(nameof(ForumBySlug), new { slug = forum.Slug}),
                LastPage = (int)Math.Ceiling(forum.Threads.Count / (double)PageSize),
                PinnedThreads = pinnedThreads.ToList().Select(t => new ThreadOverviewViewModel(t)),
                Models = threads.Skip(zeroIndexedPage * PageSize)
                    .Take(PageSize)
                    .ToList()
                    .Select(t => new ThreadOverviewViewModel(t)),
            };
        }

        private ViewResult ForumView(ForumViewModel viewModel)
        {
            PushForumBreadcrumb(viewModel.Title, viewModel.Slug);

            return View(viewModel);
        }
    }
}
