using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Data.DAL.Core;
using Guilded.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Constants;
using Guilded.Extensions;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;

namespace Guilded.Areas.Admin.Controllers
{

    public class RolesController : BaseController
    {
        public const int PageSize = 20;

        private readonly IAdminDataContext _db;
        private readonly ILogger _log;

        public RolesController(IAdminDataContext db,
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

        [HttpGet("[area]/[controller]/edit/{roleId}", Order = 1)]
        [HttpGet("[area]/[controller]/create", Order = 2)]
        public ViewResult EditOrCreate(string roleId = null)
        {
            var dbRole = _db.GetRoleById(roleId) ?? new ApplicationRole();

            return RoleEditorView(dbRole);
        }

        [HttpPost("[area]/[controller]/edit/{roleId}")]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> EditOrCreate(EditOrCreateRoleViewModel role)
        {
            ApplicationRole dbRole = _db.GetRoleById(role.Id);

            if (dbRole == null)
            {
                dbRole = await _db.CreateRoleAsync(role.Name, role.PermissionsAsRoleClaims);
                ViewData[ViewDataKeys.SuccessMessages] = "Role successfully created!";
            }
            else if (dbRole.ConcurrencyStamp == role.ConcurrencyStamp)
            {
                dbRole.UpdateFromViewModel(role);
                dbRole = await _db.UpdateRoleAsync(dbRole);
                ViewData[ViewDataKeys.SuccessMessages] = "Role successfully updated!";
            }
            else
            {
                ViewData[ViewDataKeys.ErrorMessages] = "It appears the role was updated before you saved. The latest version of the role is in the editor below.";
            }

            return RoleEditorView(dbRole);
        }

        [HttpDelete("[area]/[controller]/{roleId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string roleId, int page = 1)
        {
            var role = _db.GetRoleById(roleId);

            if (role == null)
            {
                return NotFound("That role could not be found.");
            }

            var result = await _db.DeleteRole(role);
            if (!result.Succeeded)
            {
                return StatusCode(500, "Failed to delete role.");
            }

            return Ok($"Successfully deleted '{role.Name}'!");
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

        private PaginatedRolesViewModel GetRoles(int page)
        {
            int zeroIndexPage = page - 1;

            var allRoles = _db.GetRoles();
            var rolesForPage = allRoles.Skip(PageSize * zeroIndexPage).Take(PageSize);

            return new PaginatedRolesViewModel
            {
                CurrentPage = page,
                LastPage = (int)Math.Ceiling(allRoles.Count() / (double)PageSize),
                Roles = rolesForPage.ToList().Select(r => new ApplicationRoleViewModel(r)).ToList(),
            };
        }

        private ViewResult RoleEditorView(ApplicationRole dbRole)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = "Edit Role",
                Url = Request.Path
            });

            return View(new EditOrCreateRoleViewModel(dbRole));
        }
    }
}
