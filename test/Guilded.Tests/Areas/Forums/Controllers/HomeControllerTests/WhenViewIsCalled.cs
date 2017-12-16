using Guilded.Constants;
using Guilded.ViewModels;
using NUnit.Framework;
using System.Collections.Generic;

namespace Guilded.Tests.Areas.Forums.Controllers.HomeControllerTests
{
    public class WhenViewIsCalled : HomeControllerTest
    {
        [Test]
        public void ThenViewDataContainsBreadcumbsStack()
        {
            var result = Controller.View();

            var breadcrumbs = result.ViewData[ViewDataKeys.Breadcrumbs];
            Assert.That(breadcrumbs, Is.InstanceOf<Stack<Breadcrumb>>());
        }

        [Test]
        public void ThenTopOfStackIsForums()
        {
            var result = Controller.View();

            var breadcrumbs = result.ViewData[ViewDataKeys.Breadcrumbs] as Stack<Breadcrumb>;

            Assume.That(breadcrumbs, Is.Not.Null);
            Assume.That(breadcrumbs.Count, Is.GreaterThan(0));
            Assert.That(breadcrumbs.Peek().Title, Is.EqualTo("Forums"));
        }
    }
}
