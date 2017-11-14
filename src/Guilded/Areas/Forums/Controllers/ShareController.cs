using Guilded.Areas.Forums.Constants;
using Guilded.Areas.Forums.DAL;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Guilded.Extensions;

namespace Guilded.Areas.Forums.Controllers
{
    [AllowAnonymous]
    [Route("[area]/[controller]/[action]")]
    public class ShareController : ForumsController
    {
        public const int ShareDescriptionLength = 100;

        private readonly IConvertMarkdown _markdownConverter;

        public ShareController(IForumsDataContext dataContext,
            ILoggerFactory loggerFactory,
            IConvertMarkdown markdownConverter) : base(dataContext, loggerFactory)
        {
            _markdownConverter = markdownConverter;
        }

        [HttpGet("{id:int}", Name = RouteNames.ShareThreadRoute)]
        public async Task<IActionResult> Thread(int id)
        {
            var thread = await DataContext.GetThreadByIdAsync(id);
            if (thread.IsInactive())
            {
                return NotFound();
            }

            var viewModel = new ThreadPreview
            {
                ShareLink = Url.RouteUrl(RouteNames.ViewThreadByIdRoute, new { id }, "https"),
                Description = _markdownConverter.ConvertAndStripHtml(thread.Content),
                Title = thread.Title
            };

            if (viewModel.Description.Length > ShareDescriptionLength)
            {
                viewModel.Description = viewModel.Description.Substring(0, ShareDescriptionLength);
            }

            return View("ShareContent", viewModel);
        }

        [HttpGet("{id:int}", Name = RouteNames.ShareForumRoute)]
        public async Task<IActionResult> Forum(int id)
        {
            var forum = await DataContext.GetForumByIdAsync(id);
            if (forum == null || !forum.IsActive)
            {
                return NotFound();
            }

            var description = forum.Subtitle;
            if (string.IsNullOrWhiteSpace(description))
            {
                description = $"The '{forum.Title}' forums";
            }

            if (description.Length > ShareDescriptionLength)
            {
                description = description.Substring(0, ShareDescriptionLength);
            }

            var viewModel = new ForumPreview
            {
                Description = description,
                ShareLink = Url.RouteUrl(RouteNames.ViewForumByIdRoute, new { id }, "https"),
                Title = forum.Title
            };

            return View("ShareContent", viewModel);
        }
    }
}
