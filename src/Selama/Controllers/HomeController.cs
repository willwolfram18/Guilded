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

namespace Selama.Controllers
{
    public class HomeController : _ControllerBase
    {
        #region Properties
        #region Private properties
        private readonly IGuildNewsUnitOfWork _newsFeedDb;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #endregion
        #endregion

        #region Constructor
        public HomeController()
        {

        }

        public HomeController(IGuildNewsUnitOfWork db,
            SignInManager<ApplicationUser> signInManager)
        {
            _newsFeedDb = db;
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

        [Route("[controller]/news-feed")]
        public async Task<PartialViewResult> NewsFeed(int page = 1)
        {
            if (_signInManager.IsSignedIn(User))
            {

            }
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
