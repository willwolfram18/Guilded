using Guilded.Areas.Forums.Constants;
using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Guilded.Tests.Areas.Forums.Controllers.ShareControllerTests
{
    public class WhenForumIsCalled : ShareControllerTest
    {
        private Forum _defaultForum;

        protected override string DefaultShareLink => "https://example.com/forums/share/forum/3";

        protected override Expression<Func<ShareController, Func<int, Task<IActionResult>>>> ActionToTest =>
            c => c.Forum;

        [SetUp]
        public void SetUp()
        {
            _defaultForum = new Forum
            {
                Id = DefaultId,
                IsActive = true
            };

            MockUrlHelper.Setup(u => u.RouteUrl(
                It.IsAny<UrlRouteContext>()
            )).Returns(DefaultShareLink);
            MockDataContext.Setup(d => d.GetForumByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_defaultForum);
        }

        [Test]
        public async Task IfIdIsNotFoundThenNotFoundResultReturned()
        {
            MockDataContext.Setup(d => d.GetForumByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Forum)null);

            await ThenResultShouldBeNotFound();
        }

        [Test]
        public async Task IfForumIsInactiveThenNotFoundResultReturned()
        {
            _defaultForum.IsActive = false;

            await ThenResultShouldBeNotFound();
        }

        [Test]
        public async Task ThenViewModelShouldBeAForumPreview()
        {
            await ThenViewModelIsOfType<ForumPreview>();
        }

        [Test]
        public async Task ThenGetForumByIdAsyncIsCalledWithIdParameter()
        {
            await Controller.Forum(DefaultId);

            MockDataContext.Verify(d => d.GetForumByIdAsync(It.Is<int>(i => i == DefaultId)));
        }

        [Test]
        public async Task ThenViewModelTitleMatchesDataModel()
        {
            const string forumTitle = "My title";

            _defaultForum.Title = forumTitle;

            await ThenViewModelTitleMatchesExpected<ForumPreview>(forumTitle);
        }

        [Test]
        public async Task ThenViewForumRouteIsUsed()
        {
            await Controller.Forum(DefaultId);

            MockUrlHelper.Verify(u => u.RouteUrl(
                It.Is<UrlRouteContext>(c =>
                    c.Protocol == "https" &&
                    c.RouteName == RouteNames.ViewForumByIdRoute &&
                    c.Values.GetType().GetProperty("id") != null
                )
            ));
        }

        [Test]
        public async Task IfSubtitleExistsThenDescriptionIsSubtitle()
        {
            const string subtitle = "My simple subtitle";

            _defaultForum.Subtitle = subtitle;

            var viewModel = await GetViewModel<ForumPreview>();

            viewModel.Description.ShouldBe(subtitle);
        }

        [Test]
        public async Task IfSubtitleIsLongerThanAllowedThenDescriptionUsesShortenedSubtitle()
        {
            var substitle = new string('a', ShareController.ShareDescriptionLength + 5);

            _defaultForum.Subtitle = substitle;

            var viewModel = await GetViewModel<ForumPreview>();

            viewModel.Description.ShouldBe(substitle.Substring(0, ShareController.ShareDescriptionLength));
        }

        [Test]
        public async Task IfSubtitleIsNullOrEmptyThenDescriptionIsGeneric([Values(
            null,
            "",
            "   ")] string subTitle)
        {
            const string title = "Forums 1";
            var genericDescription = $"The '{title}' forums";

            _defaultForum.Subtitle = subTitle;
            _defaultForum.Title = title;

            var viewModel = await GetViewModel<ForumPreview>();

            viewModel.Description.ShouldBe(genericDescription);
        }

        [Test]
        public async Task ThenShareLinkIsResultOfUrlHelper()
        {
            await ThenViewModelShareLinkMatchesDefaultShareLink();
        }
    }
}
