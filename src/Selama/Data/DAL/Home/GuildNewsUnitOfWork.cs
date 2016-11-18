using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama.ViewModels.Home;
using BattleNetApi.Apis.Interfaces;
using BattleNetApi.Apis;

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

        public void Dispose()
        {

        }
    }
}
