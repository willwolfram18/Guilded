using Guilded.Areas.Forums.DAL;
using Guilded.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Constants;
using Guilded.Data.Forums;
using Guilded.ViewModels;

namespace Guilded.Areas.Forums.Controllers
{
    [Route("[area]/[controller]")]
    public class ThreadsController : BaseController
    {
        private readonly IMarkdownConverter _markdownConverter;

        public ThreadsController(IForumsDataContext dataContext, IMarkdownConverter markdownConverter) : base(dataContext)
        {
            _markdownConverter = markdownConverter;
        }

        [Route("{id:int}")]
        public async Task<IActionResult> ThreadById(int id, int page = 1)
        {
            var thread = await DataContext.GetThreadByIdAsync(id);

            if (thread == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(ThreadBySlug), new {slug = thread.Slug, page});
        }

        [Route("{slug}")]
        public async Task<IActionResult> ThreadBySlug(string slug, int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction(nameof(ThreadBySlug), new {slug});
            }

            var thread = await DataContext.GetThreadBySlugAsync(slug);

            if (thread == null)
            {
                return NotFound();
            }

            var viewModel = CreateThreadViewModel(thread, page);

            throw new NotImplementedException();
        }

        [HttpGet("~/[area]/{forumSlug}/[controller]/new")]
        public async Task<IActionResult> CreateThread(string forumSlug)
        {
            var forum = await DataContext.GetForumBySlugAsync(forumSlug);

            if (forum == null)
            {
                return RedirectToAction("Index", "Home", new {area = "Forums"});
            }

            return CreateThreadView(new CreateThreadViewModel
            {
                ForumId = forum.Id,
                ForumSlug = forumSlug
            }, forum.Title);
        }

        [HttpPost("~/[area]/{forumSlug}/[controller]/new")]
        public async Task<IActionResult> CreateThread(CreateThreadViewModel threadToCreate)
        {
            var forum = await DataContext.GetForumBySlugAsync(threadToCreate.ForumSlug);

            if (forum == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Forums" });
            }

            return CreateThreadView(threadToCreate, forum.Title);
        }

        private IActionResult CreateThreadView(CreateThreadViewModel thread, string forumTitle)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = "Create new thread"
            });
            PushForumBreadcrumb(forumTitle, thread.ForumSlug);

            return View(thread);
        }

        private ThreadViewModel CreateThreadViewModel(Thread thread, int page)
        {
            throw new NotImplementedException();
        }
    }
}
