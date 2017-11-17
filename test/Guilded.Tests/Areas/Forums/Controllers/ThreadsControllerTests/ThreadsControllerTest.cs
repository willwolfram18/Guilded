using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.DAL;
using Guilded.Data.Forums;
using Guilded.Tests.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public abstract class ThreadsControllerTest : ControllerTest<ThreadsController>
    {
        protected const int DefaultThreadId = 10;

        protected Thread DefaultThread { get; private set; }

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

        [SetUp]
        public void SharedThreadControllerSetUp()
        {
            DefaultThread = new Thread
            {
                Id = DefaultThreadId,
                Forum = new Forum
                {
                    IsActive = true
                }
            };

            GetThreadByIdReturns(DefaultThread);
        }

        protected Task ThenNotFoundResultIsReturned() => ThenResultShouldBeOfType<NotFoundResult>();

        protected Task ThenOkResultIsReturned() => ThenResultShouldBeOfType<OkResult>();

        protected async Task<TResult> ThenResultShouldBeOfType<TResult>()
        {
            var actionDelegate = AsyncActionToTest.Compile();
            var result = await actionDelegate.Invoke(Controller).Invoke(DefaultThreadId);

            result.ShouldBeOfType<TResult>();
            return (TResult)result;
        }
    }
}
