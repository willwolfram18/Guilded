using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Forums.Controllers;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public class When_Pin_Is_Called : ThreadsControllerTest
    {
        protected override Expression<Func<ThreadsController, Func<int, Task<IActionResult>>>> AsyncActionToTest =>
            c => c.Pin;

        [Test]
        public async Task If_Thread_Is_Not_Found_Then_NotFoundResult_Is_Returned()
        {
            ThreadBuilder.DoesNotExist();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task If_Threads_Forum_Is_Not_Active_Then_NotFoundResult_Is_Returned()
        {
            ThreadBuilder.WithInactiveForum();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task If_Thread_Is_Deleted_Then_NotFoundResult_Is_Returned()
        {
            ThreadBuilder.IsDeleted();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task If_Thread_Is_Pinned_Then_OkResult_Is_Returned()
        {
            ThreadBuilder.IsPinned();

            await ThenOkResultIsReturned();
        }

        [Test]
        public async Task If_Thread_Is_Pinned_Then_PinAsync_Is_Not_Called()
        {
            ThreadBuilder.IsPinned();

            await Controller.Pin(DefaultThreadId);

            MockDataContext.Verify(d => d.PinThreadAsync(It.IsAny<Thread>()), Times.Never);
        }

        [Test]
        public async Task Then_OkResult_Is_Returned()
        {
            await ThenOkResultIsReturned();
        }

        [Test]
        public async Task Then_PinThreadAsync_Is_Called()
        {
            await Controller.Pin(DefaultThreadId);
        }
    }
}
