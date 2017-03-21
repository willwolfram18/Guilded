using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Guilded.Data.DAL.Core;
using Guilded.Data.ViewModels.Core;

using DataModel = Guilded.Data.Models.Core.ApplicationRole;
using ViewModel = Guilded.Data.ViewModels.Core.ApplicationRole;
using Guilded.Extensions;

namespace Guilded.Controllers.Manager
{
    // TODO: Remove AllowAnonymous
    [AllowAnonymous]
    public class RolesController : ManagerControllerBase
    {
        #region Properties
        #region Private Properties
        private readonly IManagerDataContext _db;
        #endregion
        #endregion

        public RolesController(IManagerDataContext db)
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
        public async Task<JsonResult> Get(string id)
        {
            DataModel role = await _db.RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return Json(null);
            }

            return Json(new ViewModel(role));
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
