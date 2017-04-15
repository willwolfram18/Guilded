using Guilded.Controllers.Admin;
using Guilded.Data.DAL.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using ViewModel = Guilded.ViewModels.Core.ApplicationRole;


namespace Guilded.Tests.Controllers.Admin.RolesControllerUnitTests
{
    public abstract class RolesControllerUnitTestBase : ApiControllerUnitTestBase<RolesController>
    {
        protected Mock<IAdminDataContext> _mockAdminContext;

        protected override RolesController SetupController()
        {
            _mockAdminContext = new Mock<IAdminDataContext>();
            return new RolesController(_mockAdminContext.Object);
        }

        protected ViewModel AssertResultIsRoleViewModel(JsonResult result)
        {
            var role = result.Value as ViewModel;
            Assert.NotNull(role);
            return role;
        }
    }
}
