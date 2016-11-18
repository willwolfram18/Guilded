using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Selama.Controllers;
using Selama.Data.DAL.Home;
using Selama.Tests.Common.Mocking;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;


namespace Selama.Tests.Controllers
{
    public class HomeControllerUnitTests : ControllerUnitTestBase<HomeController>
    {
        #region Properties
        #region Private properties
        private const int NUM_NEWS_FEED_PAGES = 3;
        private const int NUM_FULL_NEWS_FEED_PAGES = NUM_NEWS_FEED_PAGES - 1;

        private Mock<IGuildNewsUnitOfWork> MockGuildNewsFeed { get; set; }
        private Mock<SignInManager> MockSignInManager { get; set; }
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
            base.AdditionalSetup();
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
            #endregion

            #region Act
            PartialViewResult result = await Controller.NewsFeed(requestPageNum);
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
        public async Task NewsFeedForMembersIsCorrect(int requestPageNum)
        {
            #region Arrange
            MockSignInManager.Setup(m => m.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);
            #endregion

            #region Act
            PartialViewResult result = await Controller.NewsFeed(requestPageNum);
            #endregion

            #region Assert
            Assert.NotNull(result);
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
        #endregion
        #endregion
    }
}
