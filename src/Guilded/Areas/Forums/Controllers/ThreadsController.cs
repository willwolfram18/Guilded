using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Forums.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Forums.Controllers
{
    [Route("[area]/[controller]")]
    public class ThreadsController : BaseController
    {
        public ThreadsController(IForumsDataContext dataContext) : base(dataContext)
        {
        }

        [Route("{id:int}")]
        public IActionResult ThreadById(int id, int page = 1)
        {
            throw new NotImplementedException();
        }

        [Route("{slug}")]
        public IActionResult ThreadBySlug(string slug, int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}
