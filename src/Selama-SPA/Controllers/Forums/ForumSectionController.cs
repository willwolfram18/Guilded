using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Selama_SPA.Common.Attributes;
using Selama_SPA.Data.DAL.Forums;
using Selama_SPA.Data.ViewModels.Forums;
using Selama_SPA.Extensions;
using DataModel = Selama_SPA.Data.Models.Forums.ForumSection;

namespace Selama_SPA.Controllers.Forums
{
    [ApiRoute("forum-sections/{id?}")]
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
            IQueryable<DataModel> result;
            Func<IQueryable<DataModel>, IOrderedQueryable<DataModel>> sectionOrdering = (sections => sections.OrderBy(s => s.DisplayOrder));
            if (activeOnly)
            {
                result = _db.ForumSections.Get(
                    s => s.IsActive,
                    sectionOrdering
                );
            }
            else
            {
                result = _db.ForumSections.Get(sectionOrdering);
            }

            return Json(result.ToListOfDifferentType(s => new ForumSection(s)));
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
