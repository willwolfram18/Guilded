using Guilded.Data.DAL.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Controllers.Admin
{
    // TODO: Remove AllowAnonymous
    [AllowAnonymous]
    public class PermissionsController : AdminControllerBase
    {
        #region Properties
        #region Private Properties
        private readonly IAdminDataContext _db;
        #endregion
        #endregion

        public PermissionsController(IAdminDataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_db.GetPermissions());
        }
    }
}
