using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Data.DAL.Core;
using Guilded.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DataModel = Guilded.Identity.ApplicationRole;

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
            return Task.FromResult(Json(new ApplicationRoleViewModel(role)));
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrUpdate([FromBody] ApplicationRoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestJson(ModelErrorsAsJson());
            }

            DataModel dbRole = _db.GetRoleById(roleViewModel.Id);
            //if (dbRole == null)
            //{
            //    dbRole = await _db.CreateRoleAsync(roleViewModel.Name, roleViewModel.Permissions);
            //}
            //else if (dbRole.ConcurrencyStamp == roleViewModel.ConcurrencyStamp)
            //{
            //    dbRole.UpdateFromViewModel(roleViewModel);
            //    dbRole = await _db.UpdateRoleAsync(dbRole);
            //}
            //else
            //{
            //    return BadRequestJson(new ApplicationRoleViewModel(dbRole));
            //}

            return Json(new ApplicationRoleViewModel(dbRole));
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
