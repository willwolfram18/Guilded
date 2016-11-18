using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama.ViewModels.Home;
using BattleNetApi.Apis.Interfaces;
using BattleNetApi.Apis;
using BattleNetApi.Objects.WoW;
using Selama.Common.Extensions;

namespace Selama.Data.DAL.Home
{
    public class GuildNewsUnitOfWork : IGuildNewsUnitOfWork
    {
        #region Properties
        #region Public properties
        public IBattleNetApiClient BattleNetClient
        {
            get
            {
                return _battleNetClient;
            }
        }
        #endregion

        #region Private properties
        private readonly IBattleNetApiClient _battleNetClient;
        #endregion
        #endregion

        #region Constructor
        public GuildNewsUnitOfWork()
        {
            // _battleNetClient = new BattleNetApiClient();
        }

        public GuildNewsUnitOfWork(IBattleNetApiClient bnetClient)
        {
            _battleNetClient = bnetClient;
        }
        #endregion

        public Task<List<GuildNewsFeedViewModel>> GetMembersOnlyNewsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<List<GuildNewsFeedViewModel>> GetPublicGuildNewsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        private async Task<List<GuildNewsFeedViewModel>> GetBattleNetGuildNews()
        {
            Guild guildProfile = await BattleNetClient.WowCommunityApi.GetGuildProfileAsync("", "", "news");

            var result = guildProfile.News.ToListOfDifferentType(GuildNewsFeedViewModel.CreateFromBattleNetNews);
            result.Sort();
            return result;
        }

        public void Dispose()
        {

        }
    }
}
