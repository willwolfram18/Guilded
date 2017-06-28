using Guilded.Data.DAL.Abstract;
using Guilded.Data.Home;

namespace Guilded.Data.DAL.Home
{
    public class GuildActivityRepo : ReadOnlyRepositoryBase<GuildActivity>
    {
        public GuildActivityRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}