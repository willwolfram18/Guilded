using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelamaApi.Data.DAL.Core;
using SelamaApi.Extensions;

using DataModel = SelamaApi.Data.Models.Core.ResourcePrivilege;
using ViewModel = SelamaApi.Data.ViewModels.Core.ResourcePrivilege;

namespace SelamaApi.Controllers.Manager
{
    // TODO: Remove AllowAnonymous
    [AllowAnonymous]
    public class PrivilegesController : ManagerControllerBase
    {
        #region Properties
        #region Private Properties
        private readonly IPrivilegeReadWriteDataContext _db;
        #endregion
        #endregion

        public PrivilegesController(IPrivilegeReadWriteDataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_db.Privileges.Get().ToListOfDifferentType(p => new ViewModel(p)));
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            DataModel privilege = await _db.Privileges.GetByIdAsync(id);
            if (privilege == null)
            {
                return Json(null);
            }
            return Json(new ViewModel(privilege));
        }
    }
}
