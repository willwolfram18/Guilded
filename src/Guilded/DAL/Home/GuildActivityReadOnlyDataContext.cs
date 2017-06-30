using BattleNetApi.Apis.Interfaces;
using BattleNetApi.Objects.WoW;
using Guilded.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.DAL.Home
{
    public class GuildActivityReadOnlyDataContext : IGuildActivityReadOnlyDataContext
    {
        #region Properties
        #region Private properties
        private readonly IBattleNetApi _battleNetClient;
        private readonly IReadOnlyRepository<Data.Home.GuildActivity> _websiteNews;
        #endregion
        #endregion

        #region Constructor
        public GuildActivityReadOnlyDataContext(IBattleNetApi bnetClient,
            IReadOnlyRepository<Data.Home.GuildActivity> websiteNews)
        {
            _battleNetClient = bnetClient;
            _websiteNews = websiteNews;
        }
        #endregion

        #region Methods
        #region Public Methods
        public async Task<List<ViewModels.Home.GuildActivity>> GetMembersOnlyNewsAsync(int pageNumber, int pageSize)
        {
            var battleNetNews = GetBattleNetGuildNews();
            var websiteNews = GetWebsiteNews();
            return GetPageItems(pageNumber, pageSize, websiteNews, await battleNetNews);
        }

        public async Task<List<ViewModels.Home.GuildActivity>> GetPublicGuildNewsAsync(int pageNumber, int pageSize)
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
        private async Task<List<ViewModels.Home.GuildActivity>> GetBattleNetGuildNews()
        {
            // TODO: Remove hard coding of Guild server and name
            try
            {
                Guild guildProfile = await _battleNetClient.WowCommunityApi.GetGuildProfileAsync("Wyrmrest Accord", "Guilded Ashalanore", "news");

                var result = guildProfile.News.ToListOfDifferentType(ViewModels.Home.GuildActivity.CreateFromBattleNetNews);
                result.Sort();
                return result;
            }
            catch
            {
                return new List<ViewModels.Home.GuildActivity>();
            }
            
        }

        private List<ViewModels.Home.GuildActivity> GetWebsiteNews()
        {
            IQueryable<Data.Home.GuildActivity> websiteNews = _websiteNews.Get(orderBy: n => n.OrderByDescending(i => i.Timestamp));
            return websiteNews.ToListOfDifferentType(n =>
                new ViewModels.Home.GuildActivity(n.Timestamp, n.Content)
            );
        }

        private List<ViewModels.Home.GuildActivity> GetPageItems(int pageNumber, int pageSize, params List<ViewModels.Home.GuildActivity>[] sources)
        {
            List<ViewModels.Home.GuildActivity> results = new List<ViewModels.Home.GuildActivity>();
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
