using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.DAL;
using Guilded.Tests.Controllers;
using Moq;
using System.Security.Principal;

namespace Guilded.Tests.Areas.Forums.Controllers.RepliesControllerTests
{
    public abstract class RepliesControllerTest : ControllerTest<RepliesController>
    {
        protected Mock<IForumsDataContext> MockDataContext { get; private set; }

        protected override RepliesController SetUpController()
        {
            MockDataContext = new Mock<IForumsDataContext>();

            return new RepliesController(
                MockDataContext.Object,
                MockLoggerFactory.Object
            );
        }

        protected override Mock<IIdentity> SetUpUserIdentity()
        {
            var identity = new Mock<IIdentity>();

            identity.Setup(i => i.IsAuthenticated).Returns(true);

            return identity;
        }
    }
}
