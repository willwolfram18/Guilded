using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.DAL;
using Guilded.Tests.Controllers;
using Moq;
using System.Security.Principal;
using System.Threading.Tasks;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public abstract class ThreadsControllerTest : ControllerTest<ThreadsController>
    {
        protected const int DefaultThreadId = 10;

        protected Mock<IForumsDataContext> MockDataContext { get; private set; }

        protected override ThreadsController SetUpController()
        {
            MockDataContext = new Mock<IForumsDataContext>();

            return new ThreadsController(
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

        protected void GetThreadByIdReturns(Thread thisThread)
        {
            MockDataContext.Setup(db => db.GetThreadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(thisThread);
        }

        protected Task ThenNotFoundResultIsReturned() => ThenResultShouldBeOfType<NotFoundResult>();

        protected Task ThenOkResultIsReturned() => ThenResultShouldBeOfType<OkResult>();

        protected async Task ThenResultShouldBeOfType<TResult>()
        {
            var actionDelegate = AsyncActionToTest.Compile();
            var result = await actionDelegate.Invoke(Controller).Invoke(DefaultThreadId);

            result.ShouldBeOfType<TResult>();
        }
    }
}
