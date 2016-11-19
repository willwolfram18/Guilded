using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama.ViewModels.Home;
using BattleNetApi.Apis.Interfaces;
using BattleNetApi.Apis;
using BattleNetApi.Objects.WoW;
using Selama.Common.Extensions;
using Selama.Models.Home;

namespace Selama.Data.DAL.Home
{
    public class GuildNewsUnitOfWork : IGuildNewsUnitOfWork
    {
        #region Properties
        #region Private properties
        private readonly IBattleNetApi _battleNetClient;
        private readonly IEntityRepo<GuildNewsFeedItem> _websiteNews;
        #endregion
        #endregion

        #region Constructor
        public GuildNewsUnitOfWork()
        {
            // _battleNetClient = new BattleNetApiClient();
            // _websiteNews = new GuildNewsFeedRepo();
        }

        public GuildNewsUnitOfWork(IBattleNetApi bnetClient, 
            IEntityRepo<GuildNewsFeedItem> websiteNews)
        {
            _battleNetClient = bnetClient;
            _websiteNews = websiteNews;
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
            Guild guildProfile = await _battleNetClient.WowCommunityApi.GetGuildProfileAsync("", "", "news");

            var result = guildProfile.News.ToListOfDifferentType(GuildNewsFeedViewModel.CreateFromBattleNetNews);
            result.Sort();
            return result;
        }

        public void Dispose()
        {

        }
    }
}
