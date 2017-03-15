using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selama_SPA.Data.DAL.Core;
using Selama_SPA.Data.ViewModels.Core;

using ViewModel = Selama_SPA.Data.ViewModels.Core.ApplicationRole;
using Selama_SPA.Extensions;

namespace Selama_SPA.Controllers.Manager
{
    // TODO: Remove AllowAnonymous
    [AllowAnonymous]
    public class RolesController : ManagerControllerBase
    {
        #region Properties
        #region Private Properties
        private readonly IPrivilegeReadWriteDataContext _db;
        #endregion
        #endregion

        public RolesController(IPrivilegeReadWriteDataContext db)
        {
            _db = db;
        }

        #region Methods
        #region Action Methods
        [HttpGet]
        public JsonResult Get()
        {
            return Json(
                _db.RoleManager.Roles.ToListOfDifferentType(r => new ViewModel(r))
                    .OrderBy(r => r.Name)
            );
        }

        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult CreateOrUpdate(ViewModel role)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
