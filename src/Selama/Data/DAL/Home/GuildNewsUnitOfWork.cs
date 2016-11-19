﻿using System;
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

        #region Methods
        #region Public Methods
        public async Task<List<GuildNewsFeedViewModel>> GetMembersOnlyNewsAsync(int pageNumber, int pageSize)
        {
            var battleNetNews = GetBattleNetGuildNews();
            var websiteNews = GetWebsiteNews();
            return GetPageItems(pageNumber, pageSize, websiteNews, await battleNetNews);
        }

        public async Task<List<GuildNewsFeedViewModel>> GetPublicGuildNewsAsync(int pageNumber, int pageSize)
        {
            var battleNetNews = await GetBattleNetGuildNews();
            return GetPageItems(pageNumber, pageSize, battleNetNews);
        }
        public void Dispose()
        {
            _websiteNews.Dispose();
        }
        #endregion

        #region Private methods
        private async Task<List<GuildNewsFeedViewModel>> GetBattleNetGuildNews()
        {
            Guild guildProfile = await _battleNetClient.WowCommunityApi.GetGuildProfileAsync("", "", "news");

            var result = guildProfile.News.ToListOfDifferentType(GuildNewsFeedViewModel.CreateFromBattleNetNews);
            result.Sort();
            return result;
        }

        private List<GuildNewsFeedViewModel> GetWebsiteNews()
        {
            IQueryable<GuildNewsFeedItem> websiteNews = _websiteNews.Get(orderBy: n => n.OrderByDescending(i => i.Timestamp));
            return websiteNews.ToListOfDifferentType(n =>
                new GuildNewsFeedViewModel(n.Timestamp, n.Content)
            );
        }

        private List<GuildNewsFeedViewModel> GetPageItems(int pageNumber, int pageSize, params List<GuildNewsFeedViewModel>[] sources)
        {
            List<GuildNewsFeedViewModel> results = new List<GuildNewsFeedViewModel>();
            if (pageNumber < 1 || sources == null || sources.Length == 0)
            {
                return results;
            }

            foreach (var newsSource in sources)
            {
                results.AddRange(newsSource);
            }
            results.Sort();
            return results
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        #endregion
        #endregion
    }
}
