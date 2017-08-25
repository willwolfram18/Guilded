using Guilded.Areas.Admin.DAL;
using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Constants;
using Guilded.Data.Identity;
using Guilded.Extensions;
using Guilded.Security.Claims;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Areas.Admin.Controllers
{
    [Authorize(Policy = RoleClaimTypes.RoleManagementClaim)]
    public class RolesController : BaseController
    {
        public const int PageSize = 20;

        private readonly IRolesDataContext _db;
        private readonly ILogger _log;

        public RolesController(IRolesDataContext db,
            ILoggerFactory loggerFactory)
        {
            _db = db;
            _log = loggerFactory.CreateLogger<RolesController>();
        }

        public IActionResult Index(int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction(nameof(Index), new { page = 1 });
            }

            var viewModel = GetRoles(page);

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

        [HttpGet("{roleId}", Order = 1)]
        [HttpGet("create", Order = 2)]
        public async Task<ViewResult> EditOrCreate(string roleId = null)
        {
            var dbRole = await _db.GetRoleByIdAsync(roleId) ?? new ApplicationRole();

            return RoleEditorView(dbRole);
        }

        [HttpPost("{roleId}")]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> EditOrCreate(EditOrCreateRoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return RoleEditorView(role.ToApplicationRole());
            }

            ApplicationRole dbRole = await _db.GetRoleByIdAsync(role.Id);

            if (dbRole == null)
            {
                dbRole = await _db.CreateRoleAsync(role.ToApplicationRole());
                ViewData[ViewDataKeys.SuccessMessages] = "Role successfully created!";

                _log.LogInformation($"{User.Identity.Name} created role {role.Name} ({role.Id}).");
            }
            else
            {
                try
                {
                    dbRole.UpdateFromViewModel(role);
                    dbRole = await _db.UpdateRoleAsync(dbRole);
                    ViewData[ViewDataKeys.SuccessMessages] = "Role successfully updated!";

                    _log.LogInformation($"{User.Identity.Name} updated role {role.Name} ({role.Id}).");

                }
                catch (Exception e)
                {
                    ViewData[ViewDataKeys.ErrorMessages] = "An error occurred while attempting to save the role.";
                    _log.LogError(e.Message, e);
                }
            }

            return RoleEditorView(dbRole);
        }

        [HttpDelete("{roleId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string roleId, int page = 1)
        {
            var role = await _db.GetRoleByIdAsync(roleId);

            if (role == null)
            {
                return NotFound("That role could not be found.");
            }

            var result = await _db.DeleteRoleAsync(role);
            if (!result.Succeeded)
            {
                _log.LogError($"{User.Identity.Name} failed to delete role {role.Name}: " +
                    $"{string.Join(",", result.Errors.Select(e => e.Description))}");

                return StatusCode(500, "Failed to delete role.");
            }

            _log.LogInformation($"{User.Identity.Name} deleted role {role.Name} ({role.Id}).");
            return Ok(new { message = $"Successfully deleted '{role.Name}'!", roleId });
        }

        public override ViewResult View(string viewName, object model)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Url = Url.Action(nameof(Index), "Roles", new { area = "Admin" }),
                Title = "Roles",
            });

            return base.View(viewName, model);
        }

        private PaginatedViewModel<ApplicationRoleViewModel> GetRoles(int page)
        {
            int zeroIndexPage = page - 1;

            var allRoles = _db.GetRoles().OrderBy(r => r.Name);
            var rolesForPage = allRoles.Skip(PageSize * zeroIndexPage).Take(PageSize);

            return new PaginatedViewModel<ApplicationRoleViewModel>
            {
                CurrentPage = page,
                LastPage = (int)Math.Ceiling(allRoles.Count() / (double)PageSize),
                Models = rolesForPage.ToList()
                    .Select(r => new ApplicationRoleViewModel(r, _db.GetClaimsForRole(r))),
                PagerUrl = Url.Action(nameof(Index))
            };
        }

        private ViewResult RoleEditorView(ApplicationRole dbRole)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = "Edit Role",
                Url = Request.Path
            });

            return View(new EditOrCreateRoleViewModel(dbRole, _db.GetClaimsForRole(dbRole)));
        }
    }
}
