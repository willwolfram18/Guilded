using BattleNetApi.Apis.Interfaces;
using Selama.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Data.DAL.Home
{
    public interface IGuildActivityOnlyDataContext : IDisposable
    {
        Task<List<GuildActivityViewModel>> GetPublicGuildNewsAsync(int pageNumber, int pageSize);
        Task<List<GuildActivityViewModel>> GetMembersOnlyNewsAsync(int pageNumber, int pageSize);
    }
}
