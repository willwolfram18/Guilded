using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Attributes;
using Guilded.Data.DAL.Core;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Admin.Controllers
{
    using DataModel = Identity.ApplicationRole;
    using ViewModel = ApplicationRole;

    public class RolesController : BaseController
    {
        private const int PageSize = 2;

        private readonly IAdminDataContext _db;

        public RolesController(IAdminDataContext db)
        {
            _db = db;
        }

        public ViewResult Index(int page = 1)
        {
            var viewModel = GetRoles(page);

            return View(viewModel);
        }

        private PaginatedRolesViewModel GetRoles(int page)
        {
            int zeroIndexPage = page - 1;
            if (zeroIndexPage < 0)
            {
                zeroIndexPage = 0;
            }

            var allRoles = _db.GetRoles();
            var rolesForPage = allRoles.Skip(PageSize * zeroIndexPage).Take(PageSize);
            return new PaginatedRolesViewModel
            {
                CurrentPage = page,
                LastPage = allRoles.Count() / PageSize,
                Roles = Mapper.Map<IQueryable<DataModel>, List<ViewModel>>(rolesForPage)
            };
        }
    }
}
