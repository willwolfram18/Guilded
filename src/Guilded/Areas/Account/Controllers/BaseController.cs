using Guilded.Security.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Account.Controllers
{
    [AuthorizeEnabledUser]
    [Area("account")]
    public class BaseController : Controller
    {
        
    }
}
