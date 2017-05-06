using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Account.Controllers
{
    [Authorize]
    [Area("account")]
    public class BaseController : Controller
    {
        
    }
}
