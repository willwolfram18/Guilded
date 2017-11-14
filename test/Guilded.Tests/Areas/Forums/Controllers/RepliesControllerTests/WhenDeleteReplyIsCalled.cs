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

        private Reply _defaultReply;

        [SetUp]
        public void SetUp()
        {
            _defaultReply = new Reply
            {
                Id = DefaultReplyId,
                Thread = new Thread
                {
                    Forum = new Forum
                    {
                        IsActive = true
                    }
                }
            };

            DataContextReturnsThis(_defaultReply);
        }

        [Test]
        public async Task IfReplyReturnedIsNullThenNotFoundResultReturned()
        {
            DataContextReturnsThis(null);

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfReplyIsDeletedThenNotFoundResultReturned()
        {
            _defaultReply.IsDeleted = true;

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsDeletedThenNotFoundResultReturned()
        {
            _defaultReply.Thread.IsDeleted = true;

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsLockedThenBadRequestIsReturned()
        {
            _defaultReply.Thread.IsLocked = true;

            var result = await Controller.DeleteReply(DefaultReplyId);

            result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task IfThreadIsLockedThenBadRequestMessageTellsUser()
        {
            _defaultReply.Thread.IsLocked = true;

            var result = await Controller.DeleteReply(DefaultReplyId) as BadRequestObjectResult;

            Assume.That(result, Is.Not.Null);

            result.Value.ShouldBe("The thread is locked, therefore you cannot delete the reply.");
        }

        [Test]
        public async Task IfThreadsForumIsInactiveThenNotFoundIsReturned()
        {
            _defaultReply.Thread.Forum.IsActive = false;

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfAuthorDoesNotMatchCurrentUserThenUnauthorizedStatusResultReturned()
        {
            const int userId = 1;
            const int authorId = userId + 1;

            _defaultReply.AuthorId = authorId.ToString();

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

            _defaultReply.AuthorId = authorId.ToString();

            MockUser.Setup(u => u.Claims).Returns(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, authorId.ToString()),
            });
        }
    }
}
