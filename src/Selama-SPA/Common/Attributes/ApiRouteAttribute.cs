using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama_SPA.Common.Attributes
{
    public class ApiRouteAttribute : RouteAttribute
    {
        public ApiRouteAttribute(string template) : base("~/api/" + template)
        {
        }
    }
}
