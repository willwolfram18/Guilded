using System;
using Guilded.Data.Forums;
using Guilded.Tests.Extensions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Guilded.Areas.Forums.Controllers;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public class WhenDeleteThreadIsCalled : ThreadsControllerTest
    {
        protected override Expression<Func<ThreadsController, Func<int, Task<IActionResult>>>> AsyncActionToTest =>
            c => c.DeleteThread;

        [Test]
        public async Task IfThreadCannotBeFoundThenNotFoundResultIsReturned()
        {
            ThreadBuilder.DoesNotExist();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfThreadIsDeletedThenNotFoundResultIsReturned()
        {
            ThreadBuilder.IsDeleted();

            await ThenNotFoundResultIsReturned();
        }

        [Test]
        public async Task IfUserIsNotAuthorThenUnauthorizedStatusCodeIsReturned()
        {
            const int authorId = 1;
            const int userId = authorId + 1;

            ThreadBuilder.WithAuthorId(authorId.ToString());

            MockUserIdIsThis(userId);

            var result = await Controller.DeleteThread(DefaultThreadId) as ObjectResult;

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            result.Value.ShouldBe("You are not the author of this post.");
        }

        [Test]
        public async Task IfThreadIsLockedThenBadRequestIsReturned()
        {
            CurrentUserIsAuthor();

            ThreadBuilder.IsLocked();

            var result = await Controller.DeleteThread(DefaultThreadId) as BadRequestObjectResult;

            result.ShouldNotBeNull();
            result.Value.ShouldBe("The thread is locked, therefore you cannot delete the thread.");
        }

        [Test]
        public new async Task ThenOkResultIsReturned()
        {
            CurrentUserIsAuthor();

            await base.ThenOkResultIsReturned();
        }

        [Test]
        public async Task ThenThreadIsSavedToDataContext()
        {
            CurrentUserIsAuthor();

            await Controller.DeleteThread(DefaultThreadId);

            MockDataContext.Verify(d => d.DeleteThreadAsync(
                It.Is<Thread>(t => t == ThreadBuilder.Build())
            ));
        }

        private void MockUserIdIsThis(int userId)
        {
            MockUser.Setup(u => u.Claims)
                .Returns(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                });
        }

        private void CurrentUserIsAuthor()
        {
            const int authorId = 1;

            ThreadBuilder.WithAuthorId(authorId.ToString());

            MockUserIdIsThis(authorId);
        }
    }
}
