using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

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

        [Test]
        public async Task IfAuthorDoesNotMatchCurrentUserThenUnauthorizedStatusResultReturned()
        {
            const int userId = 1;
            const int authorId = userId + 1;

            DataContextReturnsThis(new Reply { AuthorId = authorId.ToString(), Thread = new Thread() });

            MockUser.Setup(u => u.Claims).Returns(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            });

            var result = await Controller.DeleteReply(DefaultReplyId) as ObjectResult;

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe((int)HttpStatusCode.Unauthorized);
            result.Value.ShouldBe("You are not the author of this post.");
        }

        [Test]
        public async Task ThenOkResultReturned()
        {
            UserIsTheAuthor();

            var result = await Controller.DeleteReply(DefaultReplyId);

            result.ShouldBeOfType<OkResult>();
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

        private void UserIsTheAuthor()
        {
            const int authorId = 10;

            DataContextReturnsThis(new Reply
            {
                Thread = new Thread(),
                AuthorId = authorId.ToString()
            });

            MockUser.Setup(u => u.Claims).Returns(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, authorId.ToString()),
            });
        }
    }
}
