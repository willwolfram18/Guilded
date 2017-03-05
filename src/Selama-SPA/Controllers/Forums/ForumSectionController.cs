using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Selama_SPA.Common.Attributes;
using Selama_SPA.Data.DAL.Forums;
using Selama_SPA.Data.ViewModels.Forums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Selama_SPA.Controllers.Forums
{
    [ApiRoute("forum-sections/[id?:number]")]
    public class ForumSectionController : ApiControllerBase
    {
        #region Properties
        #region Private Properties
        private readonly IForumsReadWriteDataContext _db;
        #endregion
        #endregion

        public ForumSectionController(IForumsReadWriteDataContext db)
        {
            _db = db;
        }

        #region Methods
        #region Action methods        
        [HttpGet]
        public JsonResult Get(bool activeOnly = true)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult Post(EditForumSection sectionToUpdate)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
