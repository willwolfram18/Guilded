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
    public class WhenUnlockIsCalled : ThreadsControllerTest
    {
        protected override Expression<Func<ThreadsController, Func<int, Task<IActionResult>>>> AsyncActionToTest =>
            c => c.Unlock;

        [SetUp]
        public void SetUp()
        {
            DefaultThread.IsLocked = true;
        }

        [Test]
        public async Task IfThreadDoesNotExistThenNotFoundReturned()
        {
            GetThreadByIdReturns(null);

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsDeletedThenNotFoundReturned()
        {
            DefaultThread.IsDeleted = true;

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadsForumIsInactiveThenNotFoundReturned()
        {
            DefaultThread.Forum.IsActive = false;

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsUnlockedThenOkReturned()
        {
            DefaultThread.IsLocked = false;

            await ThenOkResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsUnlockedThenUnlockIsNotCalledOnThread()
        {
            DefaultThread.IsLocked = false;

            await Controller.Unlock(DefaultThreadId);

            MockDataContext.Verify(d => d.UnlockThreadAsync(It.IsAny<Thread>()), Times.Never);
        }

        [Test]
        public async Task ThenUnlockIsCalledOnThread()
        {
            await Controller.Unlock(DefaultThreadId);

            MockDataContext.Verify(d => d.UnlockThreadAsync(It.Is<Thread>(t => t == DefaultThread)));
        }

        [Test]
        public new async Task ThenOkResultIsReturned()
        {
            await base.ThenOkResultIsReturned();
        }

        [Test]
        public async Task IfUnlockThrowsExceptionThenInternalErrorIsReturned()
        {
            var exceptionToThrow = new Exception();
            MockDataContext.Setup(db => db.UnlockThreadAsync(It.IsAny<Thread>()))
                .Throws(exceptionToThrow);

            var result = await ThenResultShouldBeOfType<StatusCodeResult>();

            result.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        }
    }
}
