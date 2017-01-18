using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Selama.Controllers;
using Selama.Data.DAL.Home;
using Selama.Tests.Common.Mocking;
using Selama.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;


namespace Selama.Tests.Controllers
{
    public class HomeControllerUnitTests : ControllerUnitTestBase<HomeController>
    {
        #region Properties
        #region Private properties
        private const int MEMBERS_NEWS_FEED_PAGE_COUNT = 3;
        private const int MEMBERS_FULL_NEWS_FEED_PAGE_COUNT = MEMBERS_NEWS_FEED_PAGE_COUNT - 1;
        private const int PUBLIC_NEWS_FEED_PAGE_COUNT = 2;
        private const int PUBLIC_FULL_NEWS_FEED_PAGE_COUNT = PUBLIC_NEWS_FEED_PAGE_COUNT - 1;

        private Mock<SignInManager> MockSignInManager { get; set; }

        private readonly Mock<IGuildActivityOnlyDataContext> _mockGuildNewsFeed = new Mock<IGuildActivityOnlyDataContext>();
        private readonly List<GuildActivityViewModel> _membersNewsItems = new List<GuildActivityViewModel>();
        private readonly List<GuildActivityViewModel> _publicNewsItems = new List<GuildActivityViewModel>();
        #endregion
        #endregion

        #region Test setup overrides
        protected override HomeController SetupController()
        {
            InitializeSignInManager();
            return new HomeController(_mockGuildNewsFeed.Object, MockSignInManager.Object);
        }

        protected override void AdditionalSetup()
        {
            _membersNewsItems.AddRange(CreateListOfNewsFeedItems(MEMBERS_FULL_NEWS_FEED_PAGE_COUNT));
            _publicNewsItems.AddRange(CreateListOfNewsFeedItems(PUBLIC_FULL_NEWS_FEED_PAGE_COUNT));
        }
        #endregion

        #region Methods
        #region Test methods
        [Fact]
        public void Index_IsNotNull()
        {
            #region Arrange
            #endregion

            #region Act
            ViewResult result = Controller.Index();
            #endregion

            #region Assert
            Assert.NotNull(result);
            #endregion
        }

        [Fact]
        public void Join_IsNotNull()
        {
            #region Arrange
            #endregion

            #region Act
            ViewResult result = Controller.Join();
            #endregion

            #region Assert
            Assert.NotNull(result);
            #endregion
        }

        [Fact]
        public void About_IsNotNull()
        {
            #region Arrange
            #endregion

            #region Act
            ViewResult result = Controller.About();
            #endregion

            #region Assert
            Assert.NotNull(result);
            #endregion
        }

        [Fact]
        public void MarkdownHelper_IsNotNull()
        {
            #region Arrange
            #endregion

            #region Act
            PartialViewResult result = Controller.MarkdownHelp();
            #endregion

            #region Assert
            Assert.NotNull(result);
            #endregion
        }

        [Fact]
        public void Error_IsNotNul()
        {
            #region Arrange
            #endregion

            #region Act
            ViewResult result = Controller.Error();
            #endregion

            #region Assert
            Assert.NotNull(result);
            #endregion
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task NewsFeed_ForPublicIsCorrect(int requestPageNum)
        {
            #region Arrange
            MockSignInManager.Setup(m => m.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            List<GuildActivityViewModel> expectedNewsFeedItems;
            if (requestPageNum < 1)
            {
                expectedNewsFeedItems = new List<GuildActivityViewModel>();
            }
            else
            {
                expectedNewsFeedItems = _publicNewsItems.Skip(requestPageNum - 1).Take(HomeController.ACTIVITY_PAGE_SIZE).ToList();
            }

            _mockGuildNewsFeed.Setup(f =>
                f.GetPublicGuildNewsAsync(
                    It.Is<int>(i => i == requestPageNum),
                    HomeController.ACTIVITY_PAGE_SIZE
                )
            ).ReturnsAsync(expectedNewsFeedItems);
            #endregion

            #region Act
            PartialViewResult result = await Controller.Activity(requestPageNum);
            #endregion

            #region Assert
            Assert.NotNull(result);
            _mockGuildNewsFeed.Verify(f =>
                f.GetMembersOnlyNewsAsync(
                    It.Is<int>(i => i == requestPageNum),
                    HomeController.ACTIVITY_PAGE_SIZE
                ),
                Times.Never()
            );
            IEnumerable<GuildActivityViewModel> Model = result.ViewData.Model as IEnumerable<GuildActivityViewModel>;
            Assert.NotNull(Model);
            AssertModelMatchesExpected(Model, expectedNewsFeedItems);
            #endregion
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task NewsFeed_ForMembersIsCorrect(int requestPageNum)
        {
            #region Arrange
            MockSignInManager.Setup(m => m.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);
            List<GuildActivityViewModel> expectedNewsFeedItems;
            if (requestPageNum < 1)
            {
                expectedNewsFeedItems = new List<GuildActivityViewModel>();
            }
            else
            {
                expectedNewsFeedItems = _membersNewsItems.Skip(requestPageNum - 1).Take(HomeController.ACTIVITY_PAGE_SIZE).ToList();
            }

            _mockGuildNewsFeed.Setup(f =>
                f.GetMembersOnlyNewsAsync(
                    It.Is<int>(i => i == requestPageNum),
                    HomeController.ACTIVITY_PAGE_SIZE
                )
            ).ReturnsAsync(expectedNewsFeedItems);
            #endregion

            #region Act
            PartialViewResult result = await Controller.Activity(requestPageNum);
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.NotNull(result);
            _mockGuildNewsFeed.Verify(f =>
                f.GetPublicGuildNewsAsync(
                    It.Is<int>(i => i == requestPageNum),
                    HomeController.ACTIVITY_PAGE_SIZE
                ),
                Times.Never()
            );
            IEnumerable<GuildActivityViewModel> Model = result.ViewData.Model as IEnumerable<GuildActivityViewModel>;
            Assert.NotNull(Model);
            AssertModelMatchesExpected(Model, expectedNewsFeedItems);
            #endregion
        }
        #endregion

        #region Private methods
        private void InitializeSignInManager()
        {
            Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.Setup(x => x.HttpContext).Returns(MockHttpContext.Object);
            MockSignInManager = new Mock<SignInManager>(mockContextAccessor.Object);
        }

        private List<GuildActivityViewModel> CreateListOfNewsFeedItems(int numFullPages)
        {
            List<GuildActivityViewModel> newsFeedItems = new List<GuildActivityViewModel>();
            for (int i = 0; i < numFullPages * HomeController.ACTIVITY_PAGE_SIZE; i++)
            {
                newsFeedItems.Add(
                    new GuildActivityViewModel(
                        DateTime.Now.AddMinutes(-i),
                        "Message for news item " + i.ToString()
                    )
                );
            }
            newsFeedItems.Add(
                new GuildActivityViewModel(
                    DateTime.Now.AddMinutes(-newsFeedItems.Count),
                    "Message for news item " + newsFeedItems.Count
                )
            );
            return newsFeedItems;
        }

        private void AssertModelMatchesExpected(IEnumerable<GuildActivityViewModel> Model, List<GuildActivityViewModel> expectedNewsFeedItems)
        {
            List<GuildActivityViewModel> modelAsList = Model.ToList();
            Assert.Equal(expectedNewsFeedItems.Count, modelAsList.Count);
            for (int i = 0; i < expectedNewsFeedItems.Count; i++)
            {
                Assert.Equal(expectedNewsFeedItems[i].Timestamp, modelAsList[i].Timestamp);
                Assert.Equal(expectedNewsFeedItems[i].Content, modelAsList[i].Content);
            }
        }
        #endregion
        #endregion
    }
}
