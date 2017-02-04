using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Selama_SPA.Controllers
{
    public class ApiControllerBase : Controller
    {
        #region Properties
        #region Protected properties
        protected int BadRequestStatus
        {
            get
            {
                return (int)HttpStatusCode.BadRequest;
            }
        }
        #endregion
        #endregion

        #region Methods
        #region Protected methods
        protected JsonResult BadRequestJson(object data)
        {
            Response.StatusCode = BadRequestStatus;
            return Json(data);
        }
        #endregion
        #endregion
    }
}
