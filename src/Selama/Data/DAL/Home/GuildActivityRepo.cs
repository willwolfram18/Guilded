using Selama.Data.DAL.Abstract;
using Selama.Data.Models.Home;

namespace Selama.Data.DAL.Home
{
    public class GuildActivityRepo : ReadOnlyRepositoryBase<GuildActivity>
    {
        public GuildActivityRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}