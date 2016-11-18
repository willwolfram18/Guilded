using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace Selama.Controllers
{
    public class HomeController : _ControllerBase
    {
        public ViewResult Index()
        {
            Session.SetString("Sample", "Meep");
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
    }
}
