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

        private Mock<IGuildNewsUnitOfWork> MockGuildNewsFeed { get; set; }
        private Mock<SignInManager> MockSignInManager { get; set; }
        private List<GuildNewsFeedViewModel> MembersNewsItems { get; set; }
        public List<GuildNewsFeedViewModel> PublicNewsItems { get; set; }
        #endregion
        #endregion

        #region Test setup overrides
        protected override HomeController SetupController()
        {
            MockGuildNewsFeed = new Mock<IGuildNewsUnitOfWork>();
            InitializeSignInManager();
            return new HomeController(MockGuildNewsFeed.Object, MockSignInManager.Object);
        }

        protected override void AdditionalSetup()
        {
            MembersNewsItems = CreateListOfNewsFeedItems(MEMBERS_FULL_NEWS_FEED_PAGE_COUNT);
            PublicNewsItems = CreateListOfNewsFeedItems(PUBLIC_FULL_NEWS_FEED_PAGE_COUNT);
        }
        #endregion

        #region Methods
        #region Test methods
        [Fact]
        public void IndexIsNotNull()
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
        public void JoinIsNotNull()
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
        public void AboutIsNotNull()
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
        public void MarkdownHelperIsNotNull()
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
        public void ErrorIsNotNul()
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
        public async Task NewsFeedForPublicIsCorrect(int requestPageNum)
        {
            #region Arrange
            MockSignInManager.Setup(m => m.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            List<GuildNewsFeedViewModel> expectedNewsFeedItems;
            if (requestPageNum < 1)
            {
                expectedNewsFeedItems = new List<GuildNewsFeedViewModel>();
            }
            else
            {
                expectedNewsFeedItems = PublicNewsItems.Skip(requestPageNum - 1).Take(HomeController.NEWS_FEED_PAGE_SIZE).ToList();
            }

            MockGuildNewsFeed.Setup(f =>
                f.GetPublicGuildNewsAsync(
                    It.Is<int>(i => i == requestPageNum),
                    HomeController.NEWS_FEED_PAGE_SIZE
                )
            ).ReturnsAsync(expectedNewsFeedItems);
            #endregion

            #region Act
            PartialViewResult result = await Controller.NewsFeed(requestPageNum);
            #endregion

            #region Assert
            Assert.NotNull(result);
            MockGuildNewsFeed.Verify(f =>
                f.GetMembersOnlyNewsAsync(
                    It.Is<int>(i => i == requestPageNum),
                    HomeController.NEWS_FEED_PAGE_SIZE
                ),
                Times.Never()
            );
            IEnumerable<GuildNewsFeedViewModel> Model = result.ViewData.Model as IEnumerable<GuildNewsFeedViewModel>;
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
        public async Task NewsFeedForMembersIsCorrect(int requestPageNum)
        {
            #region Arrange
            MockSignInManager.Setup(m => m.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);
            List<GuildNewsFeedViewModel> expectedNewsFeedItems;
            if (requestPageNum < 1)
            {
                expectedNewsFeedItems = new List<GuildNewsFeedViewModel>();
            }
            else
            {
                expectedNewsFeedItems = MembersNewsItems.Skip(requestPageNum - 1).Take(HomeController.NEWS_FEED_PAGE_SIZE).ToList();
            }

            MockGuildNewsFeed.Setup(f =>
                f.GetMembersOnlyNewsAsync(
                    It.Is<int>(i => i == requestPageNum),
                    HomeController.NEWS_FEED_PAGE_SIZE
                )
            ).ReturnsAsync(expectedNewsFeedItems);
            #endregion

            #region Act
            PartialViewResult result = await Controller.NewsFeed(requestPageNum);
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.NotNull(result);
            MockGuildNewsFeed.Verify(f =>
                f.GetPublicGuildNewsAsync(
                    It.Is<int>(i => i == requestPageNum),
                    HomeController.NEWS_FEED_PAGE_SIZE
                ),
                Times.Never()
            );
            IEnumerable<GuildNewsFeedViewModel> Model = result.ViewData.Model as IEnumerable<GuildNewsFeedViewModel>;
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

        private List<GuildNewsFeedViewModel> CreateListOfNewsFeedItems(int numFullPages)
        {
            List<GuildNewsFeedViewModel> newsFeedItems = new List<GuildNewsFeedViewModel>();
            for (int i = 0; i < numFullPages * HomeController.NEWS_FEED_PAGE_SIZE; i++)
            {
                newsFeedItems.Add(
                    new GuildNewsFeedViewModel(
                        DateTime.Now.AddMinutes(-i),
                        "Message for news item " + i.ToString()
                    )
                );
            }
            newsFeedItems.Add(
                new GuildNewsFeedViewModel(
                    DateTime.Now.AddMinutes(-newsFeedItems.Count),
                    "Message for news item " + newsFeedItems.Count
                )
            );
            return newsFeedItems;
        }

        private void AssertModelMatchesExpected(IEnumerable<GuildNewsFeedViewModel> Model, List<GuildNewsFeedViewModel> expectedNewsFeedItems)
        {
            List<GuildNewsFeedViewModel> modelAsList = Model.ToList();
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
