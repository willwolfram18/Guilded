using Selama_SPA.Data.DAL.Abstract;
using Selama_SPA.Data.Models.Home;

namespace Selama_SPA.Data.DAL.Home
{
    public class GuildActivityRepo : ReadOnlyRepositoryBase<GuildActivity>
    {
        public GuildActivityRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}