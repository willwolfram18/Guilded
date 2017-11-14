using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public class WhenLockIsCalled : ThreadsControllerTest
    {
        private Thread _defaultThread;

        [SetUp]
        public void SetUp()
        {
            _defaultThread = new Thread
            {
                Id = DefaultThreadId
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

            MockDataContext.Verify(db => db.LockThread(It.IsAny<Thread>()), Times.Never);
        }
    }
}
