using Guilded.Areas.Forums.Controllers;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public class WhenLockIsCalled : ThreadsControllerTest
    {
        private Thread _defaultThread;

        protected override Expression<Func<ThreadsController, Func<int, Task<IActionResult>>>> AsyncActionToTest =>
            c => c.Lock;

        [SetUp]
        public void SetUp()
        {
            _defaultThread = new Thread
            {
                Id = DefaultThreadId,
                Forum = new Forum
                {
                    IsActive = true
                }
            };

            GetThreadByIdReturns(_defaultThread);
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
            _defaultThread.IsDeleted = true;

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadsForumIsInactiveThenNotFoundReturned()
        {
            _defaultThread.Forum.IsActive = false;

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfTheadIsLockedThenOkReturned()
        {
            _defaultThread.IsLocked = true;

            await ThenOkResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsLockedThenLockIsNotCalledOnThread()
        {
            _defaultThread.IsLocked = true;

            await Controller.Lock(DefaultThreadId);

            MockDataContext.Verify(db => db.LockThreadAsync(It.IsAny<Thread>()), Times.Never);
        }

        [Test]
        public async Task ThenLockIsCalledOnReturnedThread()
        {
            await Controller.Lock(DefaultThreadId);

            MockDataContext.Verify(db => db.LockThreadAsync(It.Is<Thread>(t => t == _defaultThread)));
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

            result.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        }
    }
}
