using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Forums.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Forums.Controllers
{
    public class ThreadsController : BaseController
    {
        public ThreadsController(IForumsDataContext dataContext) : base(dataContext)
        {
        }

        public IActionResult ThreadById(int id, int page = 1)
        {
            throw new NotImplementedException();
        }

        public IActionResult ThreadBySlug(string slug, int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}
