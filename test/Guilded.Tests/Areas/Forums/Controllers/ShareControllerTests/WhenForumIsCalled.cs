using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;

namespace Guilded.Tests.Areas.Forums.Controllers.ShareControllerTests
{
    public class WhenForumIsCalled : ShareControllerTest
    {
        private const int DefaultId = 3;
        private const string DefaultShareLink = "https://example.com/forums/share/forum/3";

        private Forum _defaultForum;

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

            await ThenResultShouldBeNotFound(c => c.Forum);
        }

        [Test]
        public async Task IfForumIsInactiveThenNotFoundResultReturned()
        {
            _defaultForum.IsActive = false;

            await ThenResultShouldBeNotFound(c => c.Forum);
        }

        [Test]
        public async Task ThenViewResultIsReturned()
        {
            await ThenViewResultIsReturned(c => c.Forum);
        }

        [Test]
        public async Task ThenViewModelShouldBeAForumPreview()
        {
            await ThenViewModelIsOfType<ForumPreview>(c => c.Forum);
        }
    }
}
