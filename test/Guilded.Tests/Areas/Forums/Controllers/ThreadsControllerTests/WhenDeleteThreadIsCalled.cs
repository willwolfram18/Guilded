using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public class WhenDeleteThreadIsCalled : ThreadsControllerTest
    {
        private const int DefaultThreadId = 5;

        [Test]
        public async Task IfThreadCannotBeFoundThenNotFoundResultIsReturned()
        {
            GetThreadByIdReturnsThisThread(null);

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsDeletedThenNotFoundResultIsReturned()
        {
            GetThreadByIdReturnsThisThread(new Thread { IsDeleted = true });

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfUserIsNotAuthorThenUnauthorizedStatusCodeIsReturned()
        {
            const int authorId = 1;
            const int userId = authorId + 1;

            GetThreadByIdReturnsThisThread(new Thread { AuthorId = authorId.ToString() });
            CurrentUsersIdIsThis(userId);

            var result = await Controller.DeleteThread(DefaultThreadId) as ObjectResult;

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe((int)HttpStatusCode.Unauthorized);
            result.Value.ShouldBe("You are not the author of this post.");
        }

        [Test]
        public async Task IfThreadIsLockedThenBadRequestIsReturned()
        {
            var thread = CurrentUserIsAuthor();

            thread.IsLocked = true;

            var result = await Controller.DeleteThread(DefaultThreadId) as BadRequestObjectResult;

            result.ShouldNotBeNull();
            result.Value.ShouldBe("The thread is locked, therefore you cannot delete the thread.");
        }

        [Test]
        public async Task ThenOkResultIsReturned()
        {
            CurrentUserIsAuthor();

            var result = await Controller.DeleteThread(DefaultThreadId);

            result.ShouldBeOfType<OkResult>();
        }

        [Test]
        public async Task ThenThreadIsSavedToDataContext()
        {
            var threadToDelete = CurrentUserIsAuthor();

            await Controller.DeleteThread(DefaultThreadId);

            MockDataContext.Verify(d => d.DeleteThreadAsync(
                It.Is<Thread>(t => t == threadToDelete)
            ));
        }

        private void GetThreadByIdReturnsThisThread(Thread thread)
        {
            MockDataContext.Setup(d => d.GetThreadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(thread);
        }

        private async Task ThenNotFoundResultIsReturned()
        {
            var result = await Controller.DeleteThread(DefaultThreadId);

            result.ShouldBeOfType<NotFoundResult>();
        }

        private void CurrentUsersIdIsThis(int userId)
        {
            MockUser.Setup(u => u.Claims)
                .Returns(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                });
        }

        private Thread CurrentUserIsAuthor()
        {
            const int authorId = 1;

            var threadFromDataContext = new Thread
            {
                Id = DefaultThreadId,
                AuthorId = authorId.ToString()
            };

            CurrentUsersIdIsThis(authorId);
            GetThreadByIdReturnsThisThread(threadFromDataContext);

            return threadFromDataContext;
        }
    }
}
