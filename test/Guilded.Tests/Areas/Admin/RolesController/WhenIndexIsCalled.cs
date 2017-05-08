using System.Collections.Generic;
using System.Linq;
using Guilded.Areas.Admin.ViewModels.Roles;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TController = Guilded.Areas.Admin.Controllers.RolesController;

namespace Guilded.Tests.Areas.Admin.RolesController
{
    public class WhenIndexIsCalled : RolesControllerTestBase
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IfPageIsNotPositiveThenRedirectToPageOne(int page)
        {
            var result = Controller.Index(page) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(Controller.Index), result.ActionName);
            Assert.Equal(1, result.RouteValues["page"]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IfPageIsNotPositiveThenNoCallToDatabase(int page)
        {
            Controller.Index(page);

            MockAdminDataContext.Verify(db => db.GetRoles(), Times.Never);
        }

        [Fact]
        public void IfNoRolesThenViewModelRolesIsEmpty()
        {
            var result = Controller.Index() as ViewResult;

            Assert.NotNull(result);
            var viewModel = result.Model as PaginatedRolesViewModel;
            Assert.NotNull(viewModel);
            Assert.Empty(viewModel.Roles);
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(TController.PageSize + 1, 3, 2)]
        public void IfPageIsGreaterThanLastPageThenRedirectToLastPage(int numRoles, int requestPage, int expectedRedirectPage)
        {
            MockAdminDataContext.Setup(db => db.GetRoles())
                .Returns(CreateRoles(numRoles));

            var result = Controller.Index(requestPage) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(Controller.Index), result.ActionName);
            Assert.Equal(expectedRedirectPage, result.RouteValues["page"]);
        }

        [Fact]
        public void ThenItemsSkippedForCurrentPage()
        {
            int numRoles = TController.PageSize + 1;
            MockAdminDataContext.Setup(db => db.GetRoles())
                .Returns(CreateRoles(numRoles));

            var result = Controller.Index(2) as ViewResult;

            Assert.NotNull(result);
            var viewModel = result.Model as PaginatedRolesViewModel;
            Assert.NotNull(viewModel);
            Assert.Equal(1, viewModel.Roles.Count);
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