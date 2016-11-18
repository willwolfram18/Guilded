using BattleNetApi.Apis.Interfaces;
using Moq;
using Selama.Data.DAL.Home;
using Selama.ViewModels.Home;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Selama.Tests.Data.DAL.Home
{
    public class GuildNewsUnitOfWorkUnitTest
    {
        #region Properties
        #region Private properties
        private const int PAGE_SIZE = 5;

        private GuildNewsUnitOfWork UnitOfWork { get; set; }
        private Mock<IBattleNetApi> MockBattleNetApi { get; set; }
        private Mock<IWowCommunityApiMethods> MockWowCommunityApi { get; set; }
        #endregion
        #endregion

        #region Constructor
        public GuildNewsUnitOfWorkUnitTest()
        {
            InitializeBattleNetApi();
            UnitOfWork = new GuildNewsUnitOfWork(MockBattleNetApi.Object);
        }
        #endregion

        #region Methods
        #region Test methods
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task PageNumLessThanOneAlwaysGivesEmptyList(bool isEmptyNews, bool isMember)
        {
            #region Arrange
            int pageNum = 0;
            if (!isEmptyNews)
            {
                // TODO: Populate public data
                if (isMember)
                {
                    // TODO: Populate member's only data
                }
            }
            #endregion

            #region Act
            List<GuildNewsFeedViewModel> result = null;
            if (isMember)
            {
                result = await UnitOfWork.GetMembersOnlyNewsAsync(pageNum, PAGE_SIZE);
            }
            else
            {
                result = await UnitOfWork.GetPublicGuildNewsAsync(pageNum, PAGE_SIZE);
            }
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            #endregion
        }

        #region GetPublicNewsAsync
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task EmptyPublicNewsGivesEmptyList(int pageNum)
        {
            #region Arrange
            #endregion

            #region Act
            List<GuildNewsFeedViewModel> result = await UnitOfWork.GetPublicGuildNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            #endregion
        }
        #endregion

        #region GetMembersOnlyNewsAsync
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task EmptyMembersNewsGivesEmptyList(int pageNum)
        {
            #region Arrange
            #endregion

            #region Act
            List<GuildNewsFeedViewModel> result = await UnitOfWork.GetMembersOnlyNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            #endregion
        }
        #endregion
        #endregion

        #region Private methods
        private void InitializeBattleNetApi()
        {
            MockWowCommunityApi = new Mock<IWowCommunityApiMethods>();
            MockBattleNetApi = new Mock<IBattleNetApi>();
            MockBattleNetApi.Setup(b => b.WowCommunityApi).Returns(MockWowCommunityApi.Object);
        }
        #endregion
        #endregion
    }
}
