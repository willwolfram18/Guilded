using Guilded.Security.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Account.Controllers
{
    [AuthorizeEnabledUser]
    [Area("account")]
    [Route("[area]")]
    public class BaseController : Controller
    {
        
    }
}
