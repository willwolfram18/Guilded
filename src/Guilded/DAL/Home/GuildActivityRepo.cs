using Guilded.DAL.Abstract;
using Guilded.Data;
using Guilded.Data.Home;

namespace Guilded.DAL.Home
{
    public class GuildActivityRepo : ReadOnlyRepositoryBase<GuildActivity>
    {
        public GuildActivityRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}