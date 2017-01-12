using Selama.Data.DAL.Abstract;
using Selama.Models.Home;

namespace Selama.Data.DAL.Home
{
    public class GuildNewsFeedRepo : ReadOnlyRepositoryBase<GuildNewsFeedItem>
    {
        public GuildNewsFeedRepo(ApplicationDbContext context) : base(context)
        {

        }
    }
}