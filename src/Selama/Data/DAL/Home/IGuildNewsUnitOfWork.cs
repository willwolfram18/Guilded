using BattleNetApi.Apis.Interfaces;
using Selama.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Data.DAL.Home
{
    public interface IGuildNewsUnitOfWork : IDisposable
    {
        IBattleNetApiClient BattleNetClient { get; }

        Task<List<GuildNewsFeedViewModel>> GetPublicGuildNewsAsync(int pageNumber, int pageSize);
        Task<List<GuildNewsFeedViewModel>> GetMembersOnlyNewsAsync(int pageNumber, int pageSize);
    }
}
