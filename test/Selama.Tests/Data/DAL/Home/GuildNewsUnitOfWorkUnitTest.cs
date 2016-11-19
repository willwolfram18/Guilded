using BattleNetApi.Apis.Interfaces;
using Moq;
using Selama.Data.DAL.Home;
using Selama.ViewModels.Home;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using BattleNetApi.Objects.WoW;
using BattleNetApi.Objects.WoW.Enums;
using System;
using System.Linq;
using Selama.Common.Extensions;
using Selama.Models.Home;
using Selama.Data.DAL;

namespace Selama.Tests.Data.DAL.Home
{
    public class GuildNewsUnitOfWorkUnitTest
    {
        #region Properties
        #region Private properties
        private const int PAGE_SIZE = 5;
        private const int NUM_BATTLE_NET_NEWS_ENTRIES = PAGE_SIZE * 4;
        private const int NUM_WEBSITE_NEWS_ENTRIES = PAGE_SIZE * 3;

        private GuildNewsUnitOfWork UnitOfWork { get; set; }
        private Mock<IBattleNetApi> MockBattleNetApi { get; set; }
        private Mock<IWowCommunityApiMethods> MockWowCommunityApi { get; set; }
        private Mock<IEntityRepo<GuildNewsFeedItem>> MockWebsiteRepo { get; set; }
        private List<GuildNews> BattleNetNews { get; set; }
        private List<GuildNewsFeedItem> WebsiteNews { get; set; }
        #endregion
        #endregion

        #region Constructor
        public GuildNewsUnitOfWorkUnitTest()
        {
            InitializeBattleNetApi();
            InitializeGuildNewsRepo();
            UnitOfWork = new GuildNewsUnitOfWork(
                MockBattleNetApi.Object,
                MockWebsiteRepo.Object
            );
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
            if (isEmptyNews)
            {
                BattleNetNews.Clear();
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
            AssertResultMatchesExpected(result, new List<GuildNewsFeedViewModel>());
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
            BattleNetNews.Clear();
            #endregion

            #region Act
            List<GuildNewsFeedViewModel> result = await UnitOfWork.GetPublicGuildNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, new List<GuildNewsFeedViewModel>());
            #endregion
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task PublicNewsWithPageInRangeGivesCorrectResult(int pageNum)
        {
            #region Arrange
            List<GuildNewsFeedViewModel> expectedNews = BattleNetNews
                .ToListOfDifferentType(GuildNewsFeedViewModel.CreateFromBattleNetNews);
            expectedNews.Sort();
            expectedNews = expectedNews
                .Skip((pageNum - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();
            #endregion

            #region Act
            List<GuildNewsFeedViewModel> result = await UnitOfWork.GetPublicGuildNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, expectedNews);
            #endregion
        }

        [Fact]
        public async Task PublicNewsWithPageOutsideRangeGivesEmptyList()
        {
            #region Arrange
            int pageNum = (NUM_BATTLE_NET_NEWS_ENTRIES / PAGE_SIZE) + 1;
            #endregion

            #region Act
            List<GuildNewsFeedViewModel> result = await UnitOfWork.GetPublicGuildNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, new List<GuildNewsFeedViewModel>());
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
            BattleNetNews.Clear();
            WebsiteNews.Clear();
            #endregion

            #region Act
            List<GuildNewsFeedViewModel> result = await UnitOfWork.GetMembersOnlyNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, new List<GuildNewsFeedViewModel>());
            #endregion
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task MembersNewsWithPageInRangeGivesCorrectResult(int pageNum)
        {
            #region Arrange
            List<GuildNewsFeedViewModel> expectedNews = BattleNetNews
                .ToListOfDifferentType(GuildNewsFeedViewModel.CreateFromBattleNetNews)
                .Concat(
                    WebsiteNews.ToListOfDifferentType(n =>
                        new GuildNewsFeedViewModel(n.Timestamp, n.Content)
                    )
                )
                .ToList();
            expectedNews.Sort();
            expectedNews = expectedNews.Skip((pageNum - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();
            #endregion

            #region Act
            List<GuildNewsFeedViewModel> result = await UnitOfWork.GetMembersOnlyNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, expectedNews);
            #endregion
        }

        [Fact]
        public async Task MembersNewsWithPageOutsideRangeGivesEmptyList()
        {
            #region Arrange
            int pageNum = ((NUM_BATTLE_NET_NEWS_ENTRIES + NUM_WEBSITE_NEWS_ENTRIES) / PAGE_SIZE) + 1;
            #endregion

            #region Act
            List<GuildNewsFeedViewModel> result = await UnitOfWork.GetMembersOnlyNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, new List<GuildNewsFeedViewModel>());
            #endregion
        }
        #endregion
        #endregion

        #region Private methods
        private void InitializeBattleNetApi()
        {
            SetupMockApiObjects();
            CreateBattleNetNews();

            MockWowCommunityApi.Setup(c =>
                c.GetGuildProfileAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    "news"
                )
            ).ReturnsAsync(new Guild { News = BattleNetNews });
        }
        private void SetupMockApiObjects()
        {
            MockWowCommunityApi = new Mock<IWowCommunityApiMethods>();
            MockBattleNetApi = new Mock<IBattleNetApi>();
            MockBattleNetApi.Setup(b => b.WowCommunityApi).Returns(MockWowCommunityApi.Object);
        }
        private void CreateBattleNetNews()
        {
            BattleNetNews = new List<GuildNews>();
            for (int i = NUM_BATTLE_NET_NEWS_ENTRIES - 1; i >= 0; i--)
            {
                BattleNetNews.Add(new GuildNewsPlayerItem
                {
                    Type = GuildNewsType.ItemLoot,
                    CharacterName = "Ickthid",
                    DateTimeTimestamp = DateTime.Now.AddHours(-i),
                    ItemId = 18803
                });
            }
        }

        private void InitializeGuildNewsRepo()
        {
            CreateWebsiteNews();
            MockWebsiteRepo = new Mock<IEntityRepo<GuildNewsFeedItem>>();
            MockWebsiteRepo.Setup(r => 
                r.Get(
                    It.IsAny<Func<IQueryable<GuildNewsFeedItem>, IOrderedQueryable<GuildNewsFeedItem>>>()
                )
            ).Returns(
                WebsiteNews.OrderByDescending(t => t.Timestamp).AsQueryable()
            );
        }
        private void CreateWebsiteNews()
        {
            WebsiteNews = new List<GuildNewsFeedItem>();
            for (int i = NUM_WEBSITE_NEWS_ENTRIES - 1; i >= 0; i--)
            {
                int adjustedIndex = NUM_WEBSITE_NEWS_ENTRIES - i;
                WebsiteNews.Add(new GuildNewsFeedItem
                {
                    Id = adjustedIndex + 1,
                    Content = "Sample string for news entry " + i.ToString(),
                    Timestamp = DateTime.Now.AddHours(-i),
                });
            }
        }

        private void AssertResultMatchesExpected(List<GuildNewsFeedViewModel> result, List<GuildNewsFeedViewModel> expected)
        {
            Assert.NotNull(result);
            Assert.Equal(result.Count, expected.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Timestamp, result[i].Timestamp);
                Assert.Equal(expected[i].Content.Value, result[i].Content.Value);
            }
        }
        #endregion
        #endregion
    }
}
