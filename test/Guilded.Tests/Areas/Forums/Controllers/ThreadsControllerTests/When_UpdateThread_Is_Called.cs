using Guilded.Tests.Extensions;
using Guilded.Tests.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Data.Forums;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public class When_UpdateThread_Is_Called : ThreadsControllerTest
    {
        private UpdateThreadViewModelBuilder _viewModelBuilder;

        [SetUp]
        public void SetUp()
        {
            _viewModelBuilder = new UpdateThreadViewModelBuilder()
                .WithThreadId(DefaultThreadId);

            const string authorId = "author";
            ThreadBuilder.WithAuthorId(authorId);
            MockUserIdIsThis(authorId);

            MockDataContext.Setup(d => d.UpdateThreadContentByIdAsync(
                It.IsAny<int>(),
                It.IsAny<string>()
            )).ReturnsAsync((Func<int, string, Thread>)((id, content) => new ThreadBuilder()
                .WithId(id)
                .WithContent(content)
                .WithActiveForum()
                .Build()
            ));
        }

        [Test]
        public async Task If_Thread_Does_Not_Exist_Then_NotFoundObjectResult_Is_Returned()
        {
            ThreadBuilder.DoesNotExist();

            await ThenResultIsNotFoundWithMessage();
        }

        [Test]
        public async Task If_Thread_Is_Deleted_Then_NotFoundObjectResult_Is_Returned()
        {
            ThreadBuilder.IsDeleted();

            await ThenResultIsNotFoundWithMessage();
        }

        [Test]
        public async Task If_Forum_Is_Inactive_Then_NotFoundObjectResult_Is_Returned()
        {
            ThreadBuilder.WithInactiveForum();

            await ThenResultIsNotFoundWithMessage();
        }

        [Test]
        public async Task If_Forum_Is_Locked_Then_BadRequestObjectResult_Is_Returned()
        {
            ThreadBuilder.IsLocked();

            var result = await ThenResultShouldBeOfType<BadRequestObjectResult>();

            result.Value.ShouldBe("This post is locked and cannot be edited.");
        }

        [Test]
        public async Task If_User_Is_Not_Author_Then_UnathorizedStatusCode_Is_Returned()
        {
            ThreadBuilder.WithAuthorId("author");
            MockUserIdIsThis("user");

            var result = await ThenResultShouldBeOfType<ObjectResult>();

            result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            result.Value.ShouldBe("You are not the author of this post.");
        }

        [Test]
        public async Task Then_UpdateThreadContentByIdAsync_Is_Called_With_ViewModel_Content()
        {
            const string updatedContent = "Yup yup yup! I'm the new content";

            _viewModelBuilder.WithContent(updatedContent);

            await Controller.UpdateThread(_viewModelBuilder.Build());

            MockDataContext.Verify(d => d.UpdateThreadContentByIdAsync(
                It.Is<int>(i => i == DefaultThreadId),
                It.Is<string>(s => s == updatedContent)
            ));
        }

        [Test]
        public async Task If_UpdateThreadContentByIdAsync_Throws_Error_Then_InternalServer_Status_Code_Returned()
        {
            MockDataContext.Setup(d => d.UpdateThreadContentByIdAsync(
                It.IsAny<int>(),
                It.IsAny<string>()
            )).Throws<Exception>();

            var result = await ThenResultShouldBeOfType<StatusCodeResult>();

            result.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task Then_PartialViewResult_Is_Returned()
        {
            const string threadContent = @"## Aw yiss
I updated content.";

            _viewModelBuilder.WithContent(threadContent);

            var result = await ThenResultShouldBeOfType<PartialViewResult>();
            var viewModel = result.Model as ThreadViewModel;

            viewModel.ShouldNotBeNull();
            viewModel.Id.ShouldBe(DefaultThreadId);
            viewModel.Content.ShouldBe(threadContent);
        }

        private new async Task<TResult> ThenResultShouldBeOfType<TResult>()
        {
            var result = await Controller.UpdateThread(_viewModelBuilder.Build());

            result.ShouldBeOfType<TResult>();
            return (TResult)result;
        }

        private async Task ThenResultIsNotFoundWithMessage()
        {
            var result = await ThenResultShouldBeOfType<NotFoundObjectResult>();
            result.Value.ShouldBe("That thread does not exist.");
        }
    }
}
