using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Guilded.Data.DAL.Core;
using Guilded.ViewModels.Core;

using DataModel = Guilded.Identity.ApplicationRole;
using ViewModel = Guilded.ViewModels.Core.ApplicationRole;
using Guilded.Extensions;

namespace Guilded.Controllers.Admin
{
    // TODO: Remove AllowAnonymous
    [AllowAnonymous]
    public class RolesController : AdminControllerBase
    {
        #region Properties
        #region Private Properties
        private readonly IAdminDataContext _db;
        #endregion
        #endregion

        public RolesController(IAdminDataContext db)
        {
            _db = db;
        }

        #region Methods
        #region Action Methods
        [HttpGet]
        public JsonResult Get()
        {
            // TODO: Perform GET based on current user role
            return Json(_db.RoleManager.Roles.ToListOfDifferentType(r => new ViewModel(r)));
        }

        [HttpGet("{id}")]
        public Task<JsonResult> Get(string id)
        {
            DataModel role = _db.RoleManager.Roles.FirstOrDefault(r => r.Id == id);
            if (role == null)
            {
                return Task.FromResult(Json(null));
            }

            return Task.FromResult(Json(new ViewModel(role)));
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
