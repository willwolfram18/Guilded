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
    public class WhenLockIsCalled : ThreadsControllerTest
    {
        protected override Expression<Func<ThreadsController, Func<int, Task<IActionResult>>>> AsyncActionToTest =>
            c => c.Lock;

        [Test]
        public async Task IfThreadDoesNotExistThenNotFoundReturned()
        {
            ThreadBuilder.DoesNotExist();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsDeletedThenNotFoundReturned()
        {
            ThreadBuilder.IsDeleted();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadsForumIsInactiveThenNotFoundReturned()
        {
            ThreadBuilder.WithInactiveForum();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfTheadIsLockedThenOkReturned()
        {
            ThreadBuilder.IsLocked();

            await ThenOkResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsLockedThenLockIsNotCalledOnThread()
        {
            ThreadBuilder.IsLocked();

            await Controller.Lock(DefaultThreadId);

            MockDataContext.Verify(db => db.LockThreadAsync(It.IsAny<Thread>()), Times.Never);
        }

        [Test]
        public async Task ThenLockIsCalledOnReturnedThread()
        {
            await Controller.Lock(DefaultThreadId);

            MockDataContext.Verify(db => db.LockThreadAsync(It.Is<Thread>(t => t == ThreadBuilder.Build())));
        }

        [Test]
        public new Task ThenOkResultIsReturned()
        {
            return base.ThenOkResultIsReturned();
        }

        [Test]
        public async Task IfLockThrowsExceptionThenInternalErrorIsReturned()
        {
            var exceptionToThrow = new Exception();
            MockDataContext.Setup(db => db.LockThreadAsync(It.IsAny<Thread>())).Throws(exceptionToThrow);

            var result = await ThenResultShouldBeOfType<StatusCodeResult>();

            result.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        }
    }
}
