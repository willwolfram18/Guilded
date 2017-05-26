using Guilded.Areas.Admin.Data.DAL;
using Guilded.Areas.Admin.ViewModels.Users;
using Guilded.Constants;
using Guilded.Identity;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Guilded.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        public const int PageSize = 20;

        private readonly IUsersDataContext _usersDataContext;
        private readonly ILogger _logger;

        public UsersController(IUsersDataContext usersDataContext, ILoggerFactory loggerFactory)
        {
            _usersDataContext = usersDataContext;
            _logger = loggerFactory.CreateLogger<UsersController>();
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = await GetUsers(page);

            if (viewModel.LastPage == 0 && page != 1)
            {
                return RedirectToAction(nameof(Index));
            }
            if (viewModel.LastPage != 0 && page > viewModel.LastPage)
            {
                return RedirectToAction(nameof(Index), new { page = viewModel.LastPage });
            }

            return View(viewModel);
        }

        [HttpGet("[area]/[controller]/edit/{userId}")]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _usersDataContext.GetUserByIdAsync(userId);

            if (user == null)
            {
                TempData[ViewDataKeys.ErrorMessages] = "That user doesn't appear to exist.";
                return RedirectToAction(nameof(Index));
            }

            return await UserEditorView(user);
        }

        [HttpDelete("[area]/[controller]/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableUser(DisableUserViewModel user)
        {
            var dbUser = await _usersDataContext.GetUserByIdAsync(user.Id);

            if (dbUser == null)
            {
                return NotFound($"Unable to find user with Id '{user.Id}'");
            }

            dbUser.IsEnabled = false;
            dbUser.EnabledAfter = user.EnableAfter;

            try
            {
                
            }
            catch (Exception e)
            {
                _logger.LogError(EventIdRangeStart + 10, e.Message, e);
                return StatusCode(500, $"There was an error disabling {dbUser.UserName}.");
            }

            return Ok(new
            {
                userId = user.Id,
                message = $"{dbUser.UserName} successfully disabled." +
                          (
                            user.EnableAfter.HasValue ?
                            $" {dbUser.UserName} will regain access on {user.EnableAfter.Value.Date.AddDays(1):d}." :
                            string.Empty
                          )
            });
        }


        public override ViewResult View(string viewName, object model)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = "Users",
                Url = Url.Action(nameof(Index))
            });

            return base.View(viewName, model);
        }

        private async Task<ViewResult> UserEditorView(ApplicationUser user)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = "Edit User",
                Url = Url.Action(nameof(Edit), new { userId = user.Id})
            });

            return View("Edit", new ApplicationUserViewModel(user, await _usersDataContext.GetRoleForUserAsync(user)));
        }

        private async Task<PaginatedViewModel<ApplicationUserViewModel>> GetUsers(int page)
        {
            var zeroIndexPage = page - 1;
            var allUsers = _usersDataContext.GetUsers().OrderBy(u => u.UserName);
            var usersForPage = allUsers.Skip(PageSize * zeroIndexPage).Take(PageSize);
            var viewModel = new PaginatedViewModel<ApplicationUserViewModel>
            {
                CurrentPage = page,
                LastPage = (int)Math.Ceiling(allUsers.Count() / (double)PageSize),
            };

            foreach (var user in usersForPage)
            {
                viewModel.Models.Add(new ApplicationUserViewModel(
                    user,
                    await _usersDataContext.GetRoleForUserAsync(user)
                ));
            }

            return viewModel;
        }
    }
}
