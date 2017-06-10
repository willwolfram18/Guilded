using Guilded.Areas.Admin.Data.DAL;
using Guilded.Areas.Admin.ViewModels.Users;
using Guilded.Constants;
using Guilded.Data.Identity;
using Guilded.Security.Claims;
using Guilded.Services;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Areas.Admin.Controllers
{
    [Authorize(RoleClaimTypes.UserManagementClaim)]
    public class UsersController : BaseController
    {
        public const int PageSize = 20;

        private readonly IUsersDataContext _usersDataContext;
        private readonly IRolesDataContext _rolesDataContext;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public UsersController(IUsersDataContext usersDataContext,
            IRolesDataContext rolesDataContext,
            IEmailSender emailSender,
            ILoggerFactory loggerFactory)
        {
            _usersDataContext = usersDataContext;
            _rolesDataContext = rolesDataContext;
            _emailSender = emailSender;
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

            ViewData[ViewDataKeys.UserRolesSelectList] = _rolesDataContext.GetRoles()
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id,
                });

            return View(viewModel);
        }

        [HttpPost("{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableUser(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _usersDataContext.GetUserByIdAsync(userId);

            if (user == null)
            {
                return UserNotFound(userId);
            }

            user.IsEnabled = true;
            user.EnabledAfter = null;

            try
            {
                await _usersDataContext.UpdateUserAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return ServerError(user);
            }

            return Ok(new
            {
                message = $"{user.UserName} successfully enabled.",
                userId
            });
        }

        [HttpDelete("{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableUser(DisableUserViewModel userToDisable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUser = await _usersDataContext.GetUserByIdAsync(userToDisable.Id);

            if (dbUser == null)
            {
                return UserNotFound(userToDisable.Id);
            }

            dbUser.IsEnabled = false;
            dbUser.EnabledAfter = userToDisable.EnableAfter;

            try
            {
                await _usersDataContext.UpdateUserAsync(dbUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return ServerError(dbUser);
            }

            return Ok(new
            {
                userId = userToDisable.Id,
                message = $"{dbUser.UserName} successfully disabled." +
                          (
                            userToDisable.EnableAfter.HasValue ?
                            $" {dbUser.UserName} will regain access on {userToDisable.EnableAfter.Value.Date.AddDays(1):d}." :
                            string.Empty
                          )
            });
        }

        [HttpPost("{userId}/role")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(ChangeRoleViewModel userToChangeRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUser = await _usersDataContext.GetUserByIdAsync(userToChangeRole.UserId);

            if (dbUser == null)
            {
                return UserNotFound(userToChangeRole.UserId);
            }

            var newRole = await _rolesDataContext.GetRoleByIdAsync(userToChangeRole.NewRoleId);

            if (newRole == null)
            {
                return NotFound(new
                {
                    userId = userToChangeRole.UserId,
                    message = $"Unable to find role with Id '{userToChangeRole.NewRoleId}'.",
                    roleInfo = new
                    {
                        roleId = userToChangeRole.NewRoleId
                    }
                });
            }

            try
            {
                dbUser = await _usersDataContext.ChangeUserRoleAsync(dbUser, newRole);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return ServerError(dbUser);
            }

            return Ok(new
            {
                userId = userToChangeRole.UserId,
                message = $"Successfully changed user '{dbUser.UserName}' to role '{newRole.Name}'.",
                roleInfo = new
                {
                    roleId = newRole.Id,
                    roleName = newRole.Name
                }
            });
        }

        [HttpPost("{userId}/email")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel userToChangeEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUser = await _usersDataContext.GetUserByIdAsync(userToChangeEmail.UserId);
            if (dbUser == null)
            {
                return UserNotFound(userToChangeEmail.UserId);
            }

            if (dbUser.EmailConfirmed)
            {
                return BadRequest(new
                {
                    userId = dbUser.Id,
                    message = $"{dbUser.UserName}'s email has already been confirmed; you cannot update it."
                });
            }

            dbUser.Email = userToChangeEmail.NewEmailAddress;

            try
            {
                dbUser = await _usersDataContext.UpdateUserAsync(dbUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return ServerError(dbUser);
            }

            return Ok(new
            {
                userId = dbUser.Id,
                message = $"Successfully updated {dbUser.UserName}'s email to {dbUser.Email}.",
                email = dbUser.Email
            });
        }

        [HttpPost("{userId}/confirmation-email")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(string userId)
        {
            var dbUser = await _usersDataContext.GetUserByIdAsync(userId);

            if (dbUser == null)
            {
                return UserNotFound(userId);
            }

            if (dbUser.EmailConfirmed)
            {
                return BadRequest(new
                {
                    message = $"{dbUser.UserName}'s email has already been confirmed."
                });
            }

            var code = await _usersDataContext.GenerateEmailConfirmationTokenAsync(dbUser);
            var callbackUrl = Url.Action(
                nameof(Account.Controllers.HomeController.ConfirmEmail),
                "Home",
                new { area = "Account", userId, code },
                protocol: HttpContext.Request.Scheme
            );
            await _emailSender.SendEmailAsync(dbUser.Email, "Confirm your account",
                $"Please confirm your account by clicking <a href='{callbackUrl}'>this link</a>.");

            return Ok();
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

        private NotFoundObjectResult UserNotFound(string userId)
        {
            return NotFound(new
            {
                userId,
                message = $"Unable to find user with Id '{userId}'."
            });
        }

        private async Task<PaginatedViewModel<ApplicationUserViewModel>> GetUsers(int page)
        {
            var zeroIndexPage = page - 1;
            var allUsers = _usersDataContext.GetUsers().OrderBy(u => u.UserName);
            var usersForPage = allUsers.Skip(PageSize * zeroIndexPage).Take(PageSize);
            var viewModelItems = new List<ApplicationUserViewModel>();

            foreach (var user in usersForPage)
            {
                viewModelItems.Add(new ApplicationUserViewModel(
                    user,
                    await _usersDataContext.GetRoleForUserAsync(user)
                ));
            }

            return new PaginatedViewModel<ApplicationUserViewModel>
            {
                CurrentPage = page,
                LastPage = (int)Math.Ceiling(allUsers.Count() / (double)PageSize),
                Models = viewModelItems,
                PagerUrl = Url.Action(nameof(Index))
            };
        }

        private ObjectResult ServerError(ApplicationUser dbUser)
        {
            return StatusCode(500, new
            {
                message = $"There was an error performing the update on {dbUser.UserName}.",
                userId = dbUser.Id
            });
        }
    }
}
