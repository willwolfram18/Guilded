using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Guilded.Data.DAL.Core;
using Guilded.Extensions;

using DataModel = Guilded.Data.Models.Core.ResourcePrivilege;
using ViewModel = Guilded.Data.ViewModels.Core.Permission;

namespace Guilded.Controllers.Manager
{
    // TODO: Remove AllowAnonymous
    [AllowAnonymous]
    public class PrivilegesController : ManagerControllerBase
    {
        #region Properties
        #region Private Properties
        private readonly IManagerDataContext _db;
        #endregion
        #endregion

        public PrivilegesController(IManagerDataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_db.Permissions.Get());
        }
    }
}
