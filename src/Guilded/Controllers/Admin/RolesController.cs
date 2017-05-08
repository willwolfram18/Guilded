using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Guilded.Data.DAL.Core;
using Guilded.Areas.Admin.ViewModels.Roles;

using DataModel = Guilded.Identity.ApplicationRole;
using ViewModel = Guilded.Areas.Admin.ViewModels.Roles.ApplicationRole;
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
        //[HttpGet]
        //public JsonResult Get()
        //{
        //    return Json(Mapper.Map<IQueryable<DataModel>, List<ViewModel>>(_db.GetRoles()));
        //}

        [HttpGet("{id}")]
        public Task<JsonResult> Get(string id)
        {
            DataModel role = _db.GetRoleById(id);
            return Task.FromResult(Json(new ViewModel(role)));
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrUpdate([FromBody] ViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestJson(ModelErrorsAsJson());
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
        public async Task<IActionResult> Delete(string id)
        {
            var dbRole = _db.GetRoleById(id);
            if (dbRole == null)
            {
                return NotFound();
            }

            var deleteResult = await _db.DeleteRole(dbRole);
            if (!deleteResult.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }
        #endregion
        #endregion
    }
}
