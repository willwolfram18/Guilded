using Guilded.Areas.Admin.Controllers;
using Guilded.Areas.Admin.ViewModels.Roles;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Guilded.Constants;
using Guilded.ViewModels;

namespace Guilded.Tests.Areas.Admin.RolesControllerTests
{
    public class WhenIndexIsCalled : RolesControllerTestBase
    {
        [Test]
        public void IfPageIsNotPositiveThenRedirectToPageOne([Values(-1, 0)] int page)
        {
            var result = Controller.Index(page) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(Controller.Index)));
            Assert.That(result.RouteValues["page"], Is.EqualTo(1));
        }

        [Test]
        public void IfPageIsNotPositiveThenNoCallToDatabase([Values(-1, 0)] int page)
        {
            Controller.Index(page);

            MockAdminDataContext.Verify(db => db.GetRoles(), Times.Never);
        }

        [Test]
        public void IfNoRolesThenViewModelRolesIsEmpty()
        {
            var result = Controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);

            var viewModel = result.Model as PaginatedRolesViewModel;

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Roles, Is.Empty);
        }

        [Test]
        [TestCase(1, 2, 1)]
        [TestCase(RolesController.PageSize + 1, 3, 2)]
        public void IfPageIsGreaterThanLastPageThenRedirectToLastPage(int numRoles, int requestPage, int expectedRedirectPage)
        {
            MockAdminDataContext.Setup(db => db.GetRoles())
                .Returns(CreateRoles(numRoles));

            var result = Controller.Index(requestPage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(Controller.Index)));
            Assert.That(result.RouteValues["page"], Is.EqualTo(expectedRedirectPage));
        }

        [Test]
        public void ThenItemsSkippedForCurrentPage()
        {
            var numRoles = RolesController.PageSize + 1;
            MockAdminDataContext.Setup(db => db.GetRoles())
                .Returns(CreateRoles(numRoles));

            var result = Controller.Index(2) as ViewResult;

            Assert.That(result, Is.Not.Null);

            var viewModel = result.Model as PaginatedRolesViewModel;

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Roles.Count, Is.EqualTo(1));
        }

        [Test]
        public void ThenBreadcrumbStackContainsTwoItems()
        {
            var result = Controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);

            var breadcrumbs = result.ViewData[ViewDataKeys.Breadcrumbs] as Stack<Breadcrumb>;
            
            Assert.That(breadcrumbs, Is.Not.Null);
            Assert.That(breadcrumbs.Count, Is.EqualTo(2));
        }

        private IQueryable<Identity.ApplicationRole> CreateRoles(int numRoles)
        {
            var roles = new List<Identity.ApplicationRole>();

            for (int i = 0; i < numRoles; i++)
            {
                roles.Add(new Identity.ApplicationRole());
            }

            return roles.AsQueryable();
        }
    }
}