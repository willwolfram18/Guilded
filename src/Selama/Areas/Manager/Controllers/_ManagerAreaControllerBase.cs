using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Selama.Controllers;

namespace Selama.Areas.Manager.Controllers
{
    [Area("manager")]
    public class _ManagerAreaControllerBase : _AuthorizeControllerBase
    {
    }
}
