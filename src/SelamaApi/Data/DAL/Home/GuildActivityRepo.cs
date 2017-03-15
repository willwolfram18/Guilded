using SelamaApi.Data.DAL.Abstract;
using SelamaApi.Data.Models.Home;

namespace SelamaApi.Data.DAL.Home
{
    public class GuildActivityRepo : ReadOnlyRepositoryBase<GuildActivity>
    {
        public GuildActivityRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}