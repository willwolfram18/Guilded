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
        private const int DefaultThreadId = 10;
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

            var result = await Controller.Lock(DefaultThreadId);

            result.ShouldBeOfType<NotFoundResult>();
        }

        [Test]
        public async Task IfThreadIsDeletedThenNotFoundReturned()
        {

        }
    }
}
