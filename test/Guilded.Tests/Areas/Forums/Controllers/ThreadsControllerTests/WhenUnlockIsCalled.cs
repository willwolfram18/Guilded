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
            ThreadBuilder.IsLocked();
        }

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
        public async Task IfThreadIsUnlockedThenOkReturned()
        {
            ThreadBuilder.IsUnlocked();

            await ThenOkResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsUnlockedThenUnlockIsNotCalledOnThread()
        {
            ThreadBuilder.IsUnlocked();

            await Controller.Unlock(DefaultThreadId);

            MockDataContext.Verify(d => d.UnlockThreadAsync(It.IsAny<Thread>()), Times.Never);
        }

        [Test]
        public async Task ThenUnlockIsCalledOnThread()
        {
            await Controller.Unlock(DefaultThreadId);

            MockDataContext.Verify(d => d.UnlockThreadAsync(It.Is<Thread>(t => t == ThreadBuilder.Build())));
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
