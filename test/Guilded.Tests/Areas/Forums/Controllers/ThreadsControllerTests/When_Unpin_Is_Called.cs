using Guilded.Areas.Forums.Controllers;
using Guilded.Data.Forums;
using Guilded.Tests.Extensions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public class When_Unpin_Is_Called : ThreadsControllerTest
    {
        protected override Expression<Func<ThreadsController, Func<int, Task<IActionResult>>>> AsyncActionToTest =>
            c => c.Unpin;

        [SetUp]
        public void SetUp()
        {
            ThreadBuilder.IsPinned();
        }

        [Test]
        public async Task If_Thread_Is_Not_Found_Then_NotFoundResult_Returned()
        {
            ThreadBuilder.DoesNotExist();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task If_Forum_Is_Inactive_Then_NotFoundResult_Returned()
        {
            ThreadBuilder.WithInactiveForum();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task If_Thread_Is_Deleted_Then_NotFoundResult_Returned()
        {
            ThreadBuilder.IsDeleted();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task If_Thread_Is_Unpinned_Then_OkayResult_Is_Returned()
        {
            ThreadBuilder.IsUnpinned();

            await ThenOkResultIsReturned();
        }

        [Test]
        public async Task If_Thread_Is_Unpinned_Then_UnpinThreadAsync_Is_Not_Called()
        {
            ThreadBuilder.IsUnpinned();

            await Controller.Unpin(DefaultThreadId);

            MockDataContext.Verify(d => d.UnpinThreadAsync(It.IsAny<Thread>()), Times.Never);
        }

        [Test]
        public async Task Then_OkResult_Is_Returned()
        {
            await ThenOkResultIsReturned();
        }

        [Test]
        public async Task If_Unpin_Throws_Exception_Then_InternalServerError_Is_Returned()
        {
            MockDataContext.Setup(d => d.UnpinThreadAsync(It.IsAny<Thread>())).Throws<Exception>();

            var result = await ThenResultShouldBeOfType<StatusCodeResult>();

            result.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        }
    }
}
