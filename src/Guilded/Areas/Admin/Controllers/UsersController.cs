using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Admin.Data.DAL;
using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Identity;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersDataContext _userManager;

        public UsersController(IUsersDataContext userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index(int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction(nameof(Index), new { page = 1 });
            }

            return View();
        }


    }
}
