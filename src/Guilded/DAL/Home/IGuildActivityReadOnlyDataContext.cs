using Guilded.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Guilded.DAL.Home
{
    public interface IGuildActivityReadOnlyDataContext : IDisposable
    {
        Task<List<GuildActivity>> GetPublicGuildNewsAsync(int pageNumber, int pageSize);
        Task<List<GuildActivity>> GetMembersOnlyNewsAsync(int pageNumber, int pageSize);
    }
}
