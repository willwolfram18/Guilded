using Guilded.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Controllers.Admin
{
    [Area("admin")]
    [ApiRoute("[area]/[controller]")]
    public abstract class AdminControllerBase : ApiControllerBase
    {
    }
}
