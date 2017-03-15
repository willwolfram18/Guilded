using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Selama.Common.Attributes;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Selama.Controllers.Manager
{
    [Area("manager")]
    [ApiRoute("[area]/[controller]")]
    public abstract class ManagerControllerBase : ApiControllerBase
    {
    }
}
