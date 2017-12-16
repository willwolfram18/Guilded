using Guilded.Areas.Forums.Constants;
using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Data.Forums;
using Guilded.Data.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public class WhenThreadBySlugIsCalled : ThreadsControllerTest
    {
        private const string DefaultThreadSlug = "example-slug";

        private Forum _defaultParentForum;

        private Thread _defaultThread;
        private List<Reply> _defaultReplies;
        private ApplicationUser _defaultAuthor;

        [SetUp]
        public void SetUp()
        {
            ThreadBuilder.WithActiveForum()
                .WithSlug(DefaultThreadSlug)
                .WithAuthorName("Default user")
                .WithNoReplies();
        }

        [Test]
        public async Task IfPageIsLessThanOrEqualToZeroThenRedirectToSelf([Values(
            -2,
            -1,
            0)] int page)
        {
            var result = await Controller.ThreadBySlug(DefaultThreadSlug, page) as RedirectToActionResult;

            result.ShouldNotBeNull();
            result.ActionName.ShouldBe(nameof(ThreadsController.ThreadBySlug));
            result.RouteValues.ShouldContainKey("slug");
            result.RouteValues["slug"].ShouldBe(DefaultThreadSlug);
        }

        [Test]
        public async Task IfThreadIsNotFoundThenRedirectToForumsHomePage()
        {
            ThreadBuilder.DoesNotExist();

            await ThenRedirectToForumsHomePage();
        }

        [Test]
        public async Task IfThreadIsDeletedThenRedirectToForumsHomePage()
        {
            ThreadBuilder.IsDeleted();

            await ThenRedirectToForumsHomePage();
        }

        [Test]
        public async Task IfThreadsForumIsInactiveThenRedirectToForumsHomePage()
        {
            ThreadBuilder.WithInactiveForum();

            await ThenRedirectToForumsHomePage();
        }

        [Test]
        public async Task ThenGetThreadBySlugAsyncIsCalled()
        {
            await Controller.ThreadBySlug(DefaultThreadSlug);

            MockDataContext.Verify(db => db.GetThreadBySlugAsync(It.Is<string>(s => s == DefaultThreadSlug)));
        }

        [Test]
        public async Task ThenThreadSharingRouteIsUsed()
        {
            await Controller.ThreadBySlug(DefaultThreadSlug);

            MockUrlHelper.Verify(u => u.RouteUrl(
                It.Is<UrlRouteContext>(c => 
                    c.RouteName == RouteNames.ShareThreadRoute &&
                    c.Values.GetType().GetProperty("id") != null &&
                    c.Protocol == "https"
            )));
        }

        [Test]
        public async Task ThenThreadShareLinkIsTheResultOfTheUrlHelper()
        {
            const string expectedShareUrl = "http://example.com/forums/share/thread/1";

            MockUrlHelper.Setup(u => u.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(expectedShareUrl);

            var result = (ViewResult)await Controller.ThreadBySlug(DefaultThreadSlug);
            var viewModel = (ThreadViewModel)result.Model;

            viewModel.ShareLink.ShouldBe(expectedShareUrl);
        }

        private async Task ThenRedirectToForumsHomePage()
        {
            var result = await Controller.ThreadBySlug(DefaultThreadSlug) as RedirectToActionResult;

            result.ShouldNotBeNull();
            result.ActionName.ShouldBe(nameof(HomeController.Index));
            result.ControllerName.ShouldBe("Home");
            result.RouteValues.Keys.ShouldContain("area");
            result.RouteValues["area"].ShouldBe("Forums");
        }
    }
}
