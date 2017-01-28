using Selama_SPA.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama_SPA.Data.DAL.Home
{
    public interface IGuildActivityReadOnlyDataContext : IDisposable
    {
        Task<List<GuildActivity>> GetPublicGuildNewsAsync(int pageNumber, int pageSize);
        Task<List<GuildActivity>> GetMembersOnlyNewsAsync(int pageNumber, int pageSize);
    }
}
