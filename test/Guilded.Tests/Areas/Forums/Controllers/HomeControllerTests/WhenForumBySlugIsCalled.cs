using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Forums.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Tests.Areas.Forums.Controllers.HomeControllerTests
{
    public class WhenForumBySlugIsCalled : HomeControllerTest
    {
        private const string DefaultSlug = "example-slug";

        [Test]
        public async Task IfPageIsLessThanOrEqualToZeroThenRedirectToSlug([Values(-2, -1, 0)] int pageNum)
        {
            var result = await Controller.ForumBySlug(DefaultSlug, pageNum) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(HomeController.ForumBySlug)));
            Assert.That(result.RouteValues.ContainsKey("slug"), Is.True);
            Assert.That(result.RouteValues["slug"], Is.EqualTo(DefaultSlug));
        }

        [Test]
        public async Task IfPageIsLessThanOrEqualToZeroThenRedirectContainsNoPage([Values(-2, -1, 0)] int pageNum)
        {
            var result = await Controller.ForumBySlug(DefaultSlug, pageNum) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(HomeController.ForumBySlug)));
            Assert.That(result.RouteValues.ContainsKey("page"), Is.False);
        }
    }
}
