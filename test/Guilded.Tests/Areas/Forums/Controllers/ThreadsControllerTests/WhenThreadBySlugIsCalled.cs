using Guilded.Areas.Forums.Controllers;
using Guilded.Data.Forums;
using Guilded.Data.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Guilded.Areas.Forums.Constants;
using Guilded.Areas.Forums.ViewModels;
using Microsoft.AspNetCore.Mvc.Routing;

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
            _defaultParentForum = new Forum();

            _defaultReplies = new List<Reply>();
            _defaultAuthor = new ApplicationUser
            {
                UserName = "Default user"
            };
            _defaultThread = new Thread
            {
                Slug = DefaultThreadSlug,
                Replies = _defaultReplies,
                Author = _defaultAuthor,
                Forum = _defaultParentForum
            };

            MockDataContext.Setup(db => db.GetThreadBySlugAsync(It.IsAny<string>()))
                .ReturnsAsync(_defaultThread);
        }

        [Test]
        public async Task IfPageIsLessThanOrEqualToZeroThenRedirectToSelf([Values(
            -2,
            -1,
            0)] int page)
        {
            var result = await Controller.ThreadBySlug(DefaultThreadSlug, page) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(ThreadsController.ThreadBySlug)));
            Assert.That(result.RouteValues.ContainsKey("slug"), Is.True);
            Assert.That(result.RouteValues["slug"], Is.EqualTo(DefaultThreadSlug));
        }

        [Test]
        public async Task IfThreadIsNotFoundThenRedirectToForumsHomePage()
        {
            MockDataContext.Setup(db => db.GetThreadBySlugAsync(It.IsAny<string>()))
                .ReturnsAsync((Thread)null);

            await ThenRedirectToForumsHomePage();
        }

        [Test]
        public async Task IfThreadIsDeletedThenRedirectToForumsHomePage()
        {
            MockDataContext.Setup(db => db.GetThreadBySlugAsync(It.IsAny<string>()))
                .ReturnsAsync(new Thread {IsDeleted = true});

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
                    c.Values.GetType().GetProperty("id") != null
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
