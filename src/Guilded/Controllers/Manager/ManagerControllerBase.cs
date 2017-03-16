using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Guilded.Common.Attributes;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Guilded.Controllers.Manager
{
    [Area("manager")]
    [ApiRoute("[area]/[controller]")]
    public abstract class ManagerControllerBase : ApiControllerBase
    {
    }
}
