using System;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Guilded.Tests.Areas.Forums.Controllers.RepliesControllerTests
{
    public class WhenDeleteReplyIsCalled : RepliesControllerTest
    {
        private const int DefaultReplyId = 3;

        [Test]
        public async Task IfReplyReturnedIsNullThenNotFoundResultReturned()
        {
            DataContextReturnsThis(null);

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfReplyIsDeletedThenNotFoundResultReturned()
        {
            DataContextReturnsThis(new Reply { IsDeleted = true });

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsDeletedThenNotFoundResultReturned()
        {
            DataContextReturnsThis(new Reply
            {
                Thread = new Thread { IsDeleted = true }
            });

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsLockedThenBadRequestIsReturned()
        {
            DataContextReturnsThis(new Reply
            {
                Thread = new Thread
                {
                    IsLocked = true
                }
            });

            var result = await Controller.DeleteReply(DefaultReplyId);

            result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task IfThreadIsLockedThenBadRequestMessageTellsUser()
        {
            DataContextReturnsThis(new Reply
            {
                Thread = new Thread
                {
                    IsLocked = true
                }
            });

            var result = await Controller.DeleteReply(DefaultReplyId) as BadRequestObjectResult;

            Assume.That(result, Is.Not.Null);

            result.Value.ShouldBe("The thread is locked, therefore you cannot delete the reply.");
        }

        private void DataContextReturnsThis(Reply reply)
        {
            MockDataContext.Setup(d => d.GetReplyByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(reply);
        }

        private async Task ThenNotFoundResultIsReturned()
        {
            var result = await Controller.DeleteReply(DefaultReplyId);

            result.ShouldBeOfType<NotFoundResult>();
        }
    }
}
