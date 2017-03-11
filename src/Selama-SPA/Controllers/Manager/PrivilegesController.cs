using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Selama_SPA.Data.DAL.Core;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Selama_SPA.Controllers.Manager
{
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
            throw new NotImplementedException();
        }

        [HttpGet("[id:number]")]
        public JsonResult Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
