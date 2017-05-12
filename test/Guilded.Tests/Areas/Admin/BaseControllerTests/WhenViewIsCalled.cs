using Guilded.Areas.Admin.Controllers;
using Guilded.Constants;
using Guilded.Tests.Controllers;
using Guilded.ViewModels;
using NUnit.Framework;
using System.Collections.Generic;

namespace Guilded.Tests.Areas.Admin.BaseControllerTests
{
    public class WhenViewIsCalled : ControllerTestBase<BaseController>
    {
        protected override BaseController SetUpController()
        {
            return new BaseController();
        }

        [Test]
        public void ThenViewDataContainsBreadcrumbStack()
        {
            var result = Controller.View();

            Assert.That(result.ViewData[ViewDataKeys.Breadcrumbs], Is.InstanceOf<Stack<Breadcrumb>>());
        }

        [Test]
        public void ThenBreadcrumbStackStartsWithHome()
        {
            var result = Controller.View();

            var breadcrumbs = result.ViewData[ViewDataKeys.Breadcrumbs] as Stack<Breadcrumb>;

            Assert.That(breadcrumbs, Is.Not.Empty);
        }
    }
}
