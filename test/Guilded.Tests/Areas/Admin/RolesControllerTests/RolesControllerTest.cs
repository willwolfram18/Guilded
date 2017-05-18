using Guilded.Areas.Admin.Controllers;
using Guilded.Data.DAL.Core;
using Guilded.Tests.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Guilded.Tests.Areas.Admin.RolesControllerTests
{
    public class RolesControllerTest : ControllerTest<RolesController>
    {
        protected Mock<IAdminDataContext> MockAdminDataContext;
        protected Mock<ILoggerFactory> MockLoggerFactory;

        protected override RolesController SetUpController()
        {
            MockAdminDataContext = new Mock<IAdminDataContext>();
            MockLoggerFactory = new Mock<ILoggerFactory>();

            return new RolesController(
                MockAdminDataContext.Object,
                MockLoggerFactory.Object
            );
        }
    }
}