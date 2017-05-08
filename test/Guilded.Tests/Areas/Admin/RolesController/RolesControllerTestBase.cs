using Guilded.Data.DAL.Core;
using Guilded.Tests.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using RoleControllerImpl = Guilded.Areas.Admin.Controllers.RolesController;

namespace Guilded.Tests.Areas.Admin.RolesController
{

    public class RolesControllerTestBase : ControllerTestBase<RoleControllerImpl>
    {
        protected Mock<IAdminDataContext> MockAdminDataContext;
        protected Mock<ILoggerFactory> MockLoggerFactory;

        protected override RoleControllerImpl SetUpController()
        {
            MockAdminDataContext = new Mock<IAdminDataContext>();
            MockLoggerFactory = new Mock<ILoggerFactory>();

            return new RoleControllerImpl(
                MockAdminDataContext.Object,
                MockLoggerFactory.Object
            );
        }
    }
}