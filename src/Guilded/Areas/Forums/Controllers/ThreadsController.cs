using Guilded.Areas.Forums.Constants;
using Guilded.Areas.Forums.DAL;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Constants;
using Guilded.Data.Forums;
using Guilded.Extensions;
using Guilded.Security.Claims;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guilded.Areas.Forums.Controllers
{
    [Route("[area]/[controller]")]
    public class ThreadsController : ForumsController
    {
        public ThreadsController(IForumsDataContext dataContext,
            ILoggerFactory loggerFactory) : base(dataContext, loggerFactory)
        {
        }

        [HttpGet("{id:int}", Name = RouteNames.ViewThreadByIdRoute)]
        public async Task<IActionResult> ThreadById(int id, int page = 1)
        {
            var thread = await DataContext.GetThreadByIdAsync(id);

            if (thread.IsNotFound())
            {
                return RedirectToForumsHome();
            }

            return RedirectToAction(nameof(ThreadBySlug), new {slug = thread.Slug, page});
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> ThreadBySlug(string slug, int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction(nameof(ThreadBySlug), new {slug});
            }

            var thread = await DataContext.GetThreadBySlugAsync(slug);

            if (thread.IsNotFound())
            {
                return RedirectToForumsHome();
            }

            var viewModel = BuildThreadViewModel(thread, page);

            return ThreadView(viewModel, thread.Forum);
        }

        [Authorize(RoleClaimValues.ForumsWriterClaim)]
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

        [Authorize(RoleClaimValues.ForumsWriterClaim)]
        [HttpPost("~/[area]/{forumSlug}/[controller]/new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateThread(CreateThreadViewModel threadToCreate)
        {
            var forum = await DataContext.GetForumBySlugAsync(threadToCreate.ForumSlug);

            if (forum == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Forums" });
            }

            if (ModelState.IsValid &&
                !Regex.IsMatch(threadToCreate.Title.Trim(), CreateThreadViewModel.TitleRegexPattern))
            {
                ModelState.AddModelError(nameof(CreateThreadViewModel.Title), CreateThreadViewModel.TitleRegexErrorMessage);
            }

            if (!ModelState.IsValid)
            {
                return CreateThreadView(threadToCreate, forum.Title);
            }

            threadToCreate.ForumId = forum.Id;

            try
            {
                await DataContext.CreateThreadAsync(threadToCreate.ToThread(User.UserId()));
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
                ModelState.AddModelError("", "An error occurred while creating the thread. Please try again.");
                return CreateThreadView(threadToCreate, forum.Title);
            }

            return RedirectToAction("ForumBySlug", "Home", new { area = "Forums", slug = forum.Slug });
        }

        [Authorize(RoleClaimValues.ForumsWriterClaim)]
        [HttpPost("~/[area]/[controller]/{threadId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateThread(UpdateThreadViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        [Authorize(RoleClaimValues.ForumsWriterClaim)]
        [HttpDelete("~/[area]/[controller]/{threadId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteThread(int threadId)
        {
            var thread = await DataContext.GetThreadByIdAsync(threadId);

            if (thread == null || thread.IsDeleted)
            {
                return NotFound();
            }

            if (thread.AuthorId != User.UserId())
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, "You are not the author of this post.");
            }

            if (thread.IsLocked)
            {
                return BadRequest("The thread is locked, therefore you cannot delete the thread.");
            }

            try
            {
                await DataContext.DeleteThreadAsync(thread);

                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred with your request.");
        }

        [Authorize(RoleClaimValues.ForumsLockingClaim)]
        [HttpPost("~/[area]/[controller]/lock/{threadId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(int threadId)
        {
            var thread = await DataContext.GetThreadByIdAsync(threadId);
            if (thread.IsNotFound())
            {
                return NotFound();
            }

            if (thread.IsLocked)
            {
                return Ok();
            }

            try
            {
                await DataContext.LockThreadAsync(thread);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError("Error occurred locking thread", e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(RoleClaimValues.ForumsLockingClaim)]
        [HttpDelete("~/[area]/[controller]/lock/{threadId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(int threadId)
        {
            var thread = await DataContext.GetThreadByIdAsync(threadId);
            if (thread.IsNotFound())
            {
                return NotFound();
            }

            if (!thread.IsLocked)
            {
                return Ok();
            }

            try
            {
                await DataContext.UnlockThreadAsync(thread);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError("Error occurred locking thread", e);
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(RoleClaimValues.ForumsPinningClaim)]
        [HttpPost("~/[area]/[controller]/pin/{threadId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pin(int threadId)
        {
            var thread = await DataContext.GetThreadByIdAsync(threadId);
            if (thread.IsNotFound())
            {
                return NotFound();
            }

            if (thread.IsPinned)
            {
                return Ok();
            }

            try
            {
                await DataContext.PinThreadAsync(thread);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError("Error occurred pinning thread", e);
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(RoleClaimValues.ForumsPinningClaim)]
        [HttpDelete("~/[area]/[controller]/pin/{threadId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unpin(int threadId)
        {
            var thread = await DataContext.GetThreadByIdAsync(threadId);
            if (thread.IsNotFound())
            {
                return NotFound();
            }

            if (!thread.IsPinned)
            {
                return Ok();
            }

            try
            {
                await DataContext.UnpinThreadAsync(thread);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError("Error occurred unpinning thread", e);
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private RedirectToActionResult RedirectToForumsHome()
        {
            TempData[ViewDataKeys.ErrorMessages] = "That thread does not exist.";
            return RedirectToAction("Index", "Home", new { area = "Forums" });
        }

        private ViewResult ThreadView(ThreadViewModel viewModel, Forum parentForum)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = viewModel.Title
            });
            PushForumBreadcrumb(parentForum.Title, parentForum.Slug);

            return View(viewModel);
        }

        private ViewResult CreateThreadView(CreateThreadViewModel thread, string forumTitle)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = "Create new thread"
            });
            PushForumBreadcrumb(forumTitle, thread.ForumSlug);

            return View(thread);
        }

        private ThreadViewModel BuildThreadViewModel(Thread thread, int page)
        {
            return new ThreadViewModel(thread)
            {
                PagerUrl = Url.Action(nameof(ThreadBySlug), new { slug = thread.Slug }),
                CurrentPage = page,
                LastPage = (int)Math.Ceiling(thread.Replies.Count / (double)PageSize),
                Models = GetRepliesForThreadPage(thread, page),
                ShareLink = Url.RouteUrl(RouteNames.ShareThreadRoute, new { id = thread.Id }, "https")
            };
        }

        private IEnumerable<ReplyViewModel> GetRepliesForThreadPage(Thread thread, int page)
        {
            var isFirstPage = page == 1;

            // Original thread counts as an item on the page, we need to include that in the
            // skip and take counts.
            var replyCountToTake = isFirstPage ? PageSize - 1 : PageSize;
            var replyCountToSkip = PageSize * (page - 1) - 1;

            return thread.Replies.Where(r => !r.IsDeleted)
                .OrderBy(r => r.CreatedAt)
                .Skip(replyCountToSkip)
                .Take(replyCountToTake)
                .Select(r => new ReplyViewModel(r));
        }
    }
}
