using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Selama.Data.DAL.Home;
using Microsoft.AspNetCore.Identity;
using Selama.Models;
using Microsoft.AspNetCore.Html;
using Selama.ViewModels.Home;

namespace Selama.Controllers
{
    public class HomeController : _ControllerBase
    {
        #region Properties
        #region Public properties
        public const int ACTIVITY_PAGE_SIZE = 25;
        #endregion

        #region Private properties
        private readonly IGuildActivityOnlyDataContext _activityDb;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #endregion
        #endregion

        #region Constructor
        public HomeController(IGuildActivityOnlyDataContext db,
            SignInManager<ApplicationUser> signInManager)
        {
            _activityDb = db;
            _signInManager = signInManager;
        }
        #endregion

        #region Methods
        #region Action methods
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult About()
        {
            return View();
        }

        public ViewResult Join()
        {
            return View();
        }

        public PartialViewResult MarkdownHelp()
        {
            return PartialView();
        }

        public ViewResult Error()
        {
            return View();
        }

        [Route("[controller]/activity")]
        public async Task<PartialViewResult> Activity(int page = 1)
        {
            List<GuildActivityViewModel> model = new List<GuildActivityViewModel>();
            if (_signInManager.IsSignedIn(User))
            {
                model = await _activityDb.GetMembersOnlyNewsAsync(page, ACTIVITY_PAGE_SIZE);
            }
            else
            {
                model = await _activityDb.GetPublicGuildNewsAsync(page, ACTIVITY_PAGE_SIZE);
            }

            return PartialView(model);
        }
        #endregion

        #region Protected methods
        protected override void Dispose(bool disposing)
        {
            _activityDb.Dispose();
            base.Dispose(disposing);
        }
        #endregion
        #endregion
    }
}
