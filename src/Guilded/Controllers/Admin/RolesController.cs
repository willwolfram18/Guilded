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
using AutoMapper;
using BattleNetApi.Objects.WoW.Enums;

namespace Guilded.Controllers.Admin
{
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
            return Json(Mapper.Map<IQueryable<DataModel>, List<ViewModel>>(_db.GetRoles()));
        }

        [HttpGet("{id}")]
        public Task<JsonResult> Get(string id)
        {
            DataModel role = _db.GetRoleById(id);
            return Task.FromResult(Json(new ViewModel(role)));
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrUpdate(ViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestJson(role);
            }

            DataModel dbRole = _db.GetRoleById(role.Id);
            if (dbRole == null)
            {
                dbRole = await _db.CreateRoleAsync(role.Name, role.Permissions);
            }
            else if (dbRole.ConcurrencyStamp == role.ConcurrencyStamp)
            {
                dbRole.UpdateFromViewModel(role);
                dbRole = await _db.UpdateRoleAsync(dbRole);
            }
            else
            {
                return BadRequestJson(new ViewModel(dbRole));
            }

            return Json(new ViewModel(dbRole));
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
