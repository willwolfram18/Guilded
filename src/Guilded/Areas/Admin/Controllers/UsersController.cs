using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Admin.Data.DAL;
using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Areas.Admin.ViewModels.Users;
using Guilded.Identity;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        public const int PageSize = 20;

        private readonly IUsersDataContext _db;

        public UsersController(IUsersDataContext db)
        {
            _db = db;
        }

        public IActionResult Index(int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = GetUsers(page);

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

        public override ViewResult View(string viewName, object model)
        {
            Breadcrumbs.Push(new Breadcrumb
            {
                Title = "Users",
                Url = Url.Action(nameof(Index))
            });

            return base.View(viewName, model);
        }

        private PaginatedViewModel<ApplicationUserViewModel> GetUsers(int page)
        {
            var zeroIndexPage = page - 1;
            var allUsers = _db.GetUsers().OrderBy(u => u.UserName);
            var usersForPage = allUsers.Skip(PageSize * zeroIndexPage).Take(PageSize);

            return new PaginatedViewModel<ApplicationUserViewModel>
            {
                CurrentPage = page,
                LastPage = (int)Math.Ceiling(allUsers.Count() / (double)PageSize),
                Models = usersForPage.ToList()
                    .Select(u => new ApplicationUserViewModel(u))
                    .ToList()
            };
        }
    }
}
