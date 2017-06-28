using Guilded.Attributes;
using Guilded.Data.DAL.Forums;
using Guilded.Extensions;
using Guilded.ViewModels.Forums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using DataModel = Guilded.Data.Forums.ForumSection;

namespace Guilded.Controllers.Forums
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
