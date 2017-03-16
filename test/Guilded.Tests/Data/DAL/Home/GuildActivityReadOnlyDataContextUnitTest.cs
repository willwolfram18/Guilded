using BattleNetApi.Apis.Interfaces;
using Moq;
using Guilded.Data.DAL.Home;
using Guilded.Data.ViewModels.Home;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using BattleNetApi.Objects.WoW;
using BattleNetApi.Objects.WoW.Enums;
using System;
using System.Linq;
using Guilded.Extensions;
using Guilded.Data.Models.Home;
using Guilded.Data.DAL;

namespace Guilded.Tests.Data.DAL.Home
{
    public class GuildActivityReadOnlyDataContextUnitTest
    {
        #region Properties
        #region Private properties
        private const int PAGE_SIZE = 5;
        private const int NUM_BATTLE_NET_NEWS_ENTRIES = PAGE_SIZE * 4;
        private const int NUM_WEBSITE_NEWS_ENTRIES = PAGE_SIZE * 3;

        private readonly GuildActivityReadOnlyDataContext _dataContext;
        private readonly Mock<IBattleNetApi> _mockBattleNetNews = new Mock<IBattleNetApi>();
        private readonly Mock<IWowCommunityApiMethods> _mockWowCommunityApi = new Mock<IWowCommunityApiMethods>();
        private readonly Mock<IReadOnlyRepository<Guilded.Data.Models.Home.GuildActivity>> _mockGuildNewsRepo = new Mock<IReadOnlyRepository<Guilded.Data.Models.Home.GuildActivity>>();
        private readonly List<GuildNews> _battleNetNews = new List<GuildNews>();
        private readonly List<Guilded.Data.Models.Home.GuildActivity> _websiteNews = new List<Guilded.Data.Models.Home.GuildActivity>();
        #endregion
        #endregion

        #region Constructor
        public GuildActivityReadOnlyDataContextUnitTest()
        {
            InitializeBattleNetApi();
            InitializeGuildNewsRepo();
            _dataContext = new GuildActivityReadOnlyDataContext(
                _mockBattleNetNews.Object,
                _mockGuildNewsRepo.Object
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
                _battleNetNews.Clear();
            }
            #endregion

            #region Act
            List<Guilded.Data.ViewModels.Home.GuildActivity> result = null;
            if (isMember)
            {
                result = await _dataContext.GetMembersOnlyNewsAsync(pageNum, PAGE_SIZE);
            }
            else
            {
                result = await _dataContext.GetPublicGuildNewsAsync(pageNum, PAGE_SIZE);
            }
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, new List<Guilded.Data.ViewModels.Home.GuildActivity>());
            #endregion
        }

        #region GetPublicNewsAsync
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetPublicGuildNewsAsync_EmptyPublicNewsGivesEmptyList(int pageNum)
        {
            #region Arrange
            _battleNetNews.Clear();
            #endregion

            #region Act
            List<Guilded.Data.ViewModels.Home.GuildActivity> result = await _dataContext.GetPublicGuildNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, new List<Guilded.Data.ViewModels.Home.GuildActivity>());
            #endregion
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetPublicGuildNewsAsync_PublicNewsWithPageInRangeGivesCorrectResult(int pageNum)
        {
            #region Arrange
            List<Guilded.Data.ViewModels.Home.GuildActivity> expectedNews = _battleNetNews
                .ToListOfDifferentType(Guilded.Data.ViewModels.Home.GuildActivity.CreateFromBattleNetNews);
            expectedNews.Sort();
            expectedNews = expectedNews
                .Skip((pageNum - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();
            #endregion

            #region Act
            List<Guilded.Data.ViewModels.Home.GuildActivity> result = await _dataContext.GetPublicGuildNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, expectedNews);
            #endregion
        }

        [Fact]
        public async Task GetPublicGuildNewsAsync_PublicNewsWithPageOutsideRangeGivesEmptyList()
        {
            #region Arrange
            int pageNum = (NUM_BATTLE_NET_NEWS_ENTRIES / PAGE_SIZE) + 1;
            #endregion

            #region Act
            List<Guilded.Data.ViewModels.Home.GuildActivity> result = await _dataContext.GetPublicGuildNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, new List<Guilded.Data.ViewModels.Home.GuildActivity>());
            #endregion
        }
        #endregion

        #region GetMembersOnlyNewsAsync
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetMembersOnlyNewsAsync_EmptyMembersNewsGivesEmptyList(int pageNum)
        {
            #region Arrange
            _battleNetNews.Clear();
            _websiteNews.Clear();
            #endregion

            #region Act
            List<Guilded.Data.ViewModels.Home.GuildActivity> result = await _dataContext.GetMembersOnlyNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, new List<Guilded.Data.ViewModels.Home.GuildActivity>());
            #endregion
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetMembersOnlyNewsAsync_MembersNewsWithPageInRangeGivesCorrectResult(int pageNum)
        {
            #region Arrange
            List<Guilded.Data.ViewModels.Home.GuildActivity> expectedNews = _battleNetNews
                .ToListOfDifferentType(Guilded.Data.ViewModels.Home.GuildActivity.CreateFromBattleNetNews)
                .Concat(
                    _websiteNews.ToListOfDifferentType(n =>
                        new Guilded.Data.ViewModels.Home.GuildActivity(n.Timestamp, n.Content)
                    )
                )
                .ToList();
            expectedNews.Sort();
            expectedNews = expectedNews.Skip((pageNum - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();
            #endregion

            #region Act
            List<Guilded.Data.ViewModels.Home.GuildActivity> result = await _dataContext.GetMembersOnlyNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, expectedNews);
            #endregion
        }

        [Fact]
        public async Task GetMembersOnlyNewsAsync_MembersNewsWithPageOutsideRangeGivesEmptyList()
        {
            #region Arrange
            int pageNum = ((NUM_BATTLE_NET_NEWS_ENTRIES + NUM_WEBSITE_NEWS_ENTRIES) / PAGE_SIZE) + 1;
            #endregion

            #region Act
            List<Guilded.Data.ViewModels.Home.GuildActivity> result = await _dataContext.GetMembersOnlyNewsAsync(pageNum, PAGE_SIZE);
            #endregion

            #region Assert
            AssertResultMatchesExpected(result, new List<Guilded.Data.ViewModels.Home.GuildActivity>());
            #endregion
        }
        #endregion
        #endregion

        #region Private methods
        private void InitializeBattleNetApi()
        {
            _mockBattleNetNews.Setup(b => b.WowCommunityApi).Returns(_mockWowCommunityApi.Object);
            CreateBattleNetNews();

            _mockWowCommunityApi.Setup(c =>
                c.GetGuildProfileAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    "news"
                )
            ).ReturnsAsync(new Guild { News = _battleNetNews });
        }
        private void CreateBattleNetNews()
        {
            for (int i = NUM_BATTLE_NET_NEWS_ENTRIES - 1; i >= 0; i--)
            {
                _battleNetNews.Add(new GuildNewsPlayerItem
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

            _mockGuildNewsRepo.Setup(r =>
                r.Get(
                    It.IsAny<Func<IQueryable<Guilded.Data.Models.Home.GuildActivity>, IOrderedQueryable<Guilded.Data.Models.Home.GuildActivity>>>()
                )
            ).Returns(
                _websiteNews.OrderByDescending(t => t.Timestamp).AsQueryable()
            );
        }
        private void CreateWebsiteNews()
        {
            for (int i = NUM_WEBSITE_NEWS_ENTRIES - 1; i >= 0; i--)
            {
                int adjustedIndex = NUM_WEBSITE_NEWS_ENTRIES - i;
                _websiteNews.Add(new Guilded.Data.Models.Home.GuildActivity
                {
                    Id = adjustedIndex + 1,
                    Content = "Sample string for news entry " + i.ToString(),
                    Timestamp = DateTime.Now.AddHours(-i),
                });
            }
        }

        private void AssertResultMatchesExpected(List<Guilded.Data.ViewModels.Home.GuildActivity> result, List<Guilded.Data.ViewModels.Home.GuildActivity> expected)
        {
            Assert.NotNull(result);
            Assert.Equal(result.Count, expected.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Timestamp, result[i].Timestamp);
                Assert.Equal(expected[i].Content, result[i].Content);
            }
        }
        #endregion
        #endregion
    }
}
