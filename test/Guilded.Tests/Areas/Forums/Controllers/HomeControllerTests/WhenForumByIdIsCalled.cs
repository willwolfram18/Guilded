using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Forums.Controllers;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Guilded.Tests.Areas.Forums.Controllers.HomeControllerTests
{
    public class WhenForumByIdIsCalled : HomeControllerTest
    {
        private const int DefaultForumId = 5;

        [Test]
        public async Task IfInvalidIdThenNotFoundResultReturned()
        {
            MockDataContext.Setup(db => db.GetForumByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Forum)null);

            var result = await Controller.ForumById(DefaultForumId);

            MockDataContext.Verify(db => db.GetForumByIdAsync(It.Is<int>(i => i == DefaultForumId)));

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task ThenRedirectToForumBySlug()
        {
            var forum = new Forum
            {
                Slug = "example slug"
            };

            MockDataContext.Setup(db => db.GetForumByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(forum);

            var result = await Controller.ForumById(DefaultForumId) as RedirectToActionResult;

            MockDataContext.Verify(db => db.GetForumByIdAsync(It.Is<int>(i => i == DefaultForumId)));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(HomeController.ForumBySlug)));
            Assert.That(result.RouteValues.ContainsKey("slug"), Is.True);
            Assert.That(result.RouteValues["slug"], Is.EqualTo(forum.Slug));
        }
    }
}
