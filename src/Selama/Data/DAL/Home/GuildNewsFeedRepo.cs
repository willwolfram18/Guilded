using Selama.Data.DAL.Abstract;
using Selama.Models.Home;

namespace Selama.Data.DAL.Home
{
    public class GuildNewsFeedRepo : EntityRepoBase<GuildNewsFeedItem>
    {
        public GuildNewsFeedRepo(ApplicationDbContext context) : base(context)
        {

        }
    }
}