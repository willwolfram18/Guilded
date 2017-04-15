using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Guilded.Controllers
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

        protected Dictionary<string, List<string>> ModelErrorsAsJson()
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            foreach (var modelError in ModelState)
            {
                result.Add(modelError.Key, new List<string>());
                foreach (var error in modelError.Value.Errors)
                {
                    result[modelError.Key].Add(error.ErrorMessage);
                }
            }
            return result;
        }
        #endregion
        #endregion
    }
}
