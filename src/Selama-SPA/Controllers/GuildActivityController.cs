using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selama_SPA.Common.Attributes;
using Selama_SPA.Data.DAL.Home;

namespace Selama_SPA.Controllers
{
    [ApiRoute("guild-activity")]
    public class GuildActivityController : ApiControllerBase
    {
        public const int PAGE_SIZE = 25;

        private readonly IGuildActivityReadOnlyDataContext _db;

        public GuildActivityController(IGuildActivityReadOnlyDataContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<JsonResult> Get(int page = 1)
        {
            return Json(await _db.GetPublicGuildNewsAsync(page, PAGE_SIZE));
        }

        [HttpGet("test")]
        public string Test()
        {
            return "Hello, world";
        }
    }
}