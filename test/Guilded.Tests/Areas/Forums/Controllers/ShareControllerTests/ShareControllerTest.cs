using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.DAL;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Services;
using Guilded.Tests.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Guilded.Tests.Areas.Forums.Controllers.ShareControllerTests
{
    public abstract class ShareControllerTest : ControllerTest<ShareController>
    {
        protected const int DefaultId = 3;

        protected abstract string DefaultShareLink { get; }

        protected Mock<IForumsDataContext> MockDataContext { get; private set; }

        protected Mock<IConvertMarkdown> MockMarkdownConverter { get; private set; }

        protected override ShareController SetUpController()
        {
            MockDataContext = new Mock<IForumsDataContext>();
            MockMarkdownConverter = new Mock<IConvertMarkdown>();

            return new ShareController(
                MockDataContext.Object,
                MockLoggerFactory.Object,
                MockMarkdownConverter.Object
            );
        }

        protected async Task ThenResultShouldBeNotFound()
        {
            await ThenActionResultShouldBeOfType<NotFoundResult>();
        }

        [Test]
        protected async Task ThenViewResultIsReturned()
        {
            await ThenActionResultShouldBeOfType<ViewResult>();
        }

        protected async Task ThenViewModelIsOfType<T>()
            where T : class 
        {
            var viewModel = await GetViewModel<T>();
            viewModel.ShouldBeOfType<T>();
        }

        protected async Task<T> GetViewModel<T>()
            where T : class

        {
            var result = (ViewResult)await InvokeActionExpressionAsync();
            return result.Model as T;
        }

        protected async Task ThenViewModelShareLinkMatchesDefaultShareLink()
        {
            var viewModel = await GetViewModel<IShareForumsContent>();

            viewModel.ShareLink.ShouldBe(DefaultShareLink);
        }

        protected async Task ThenViewModelTitleMatchesExpected<T>(string expectedTitle)
            where T : class, IShareForumsContent
        {
            var viewModel = await GetViewModel<T>();

            viewModel.Title.ShouldBe(expectedTitle);
        }

        private async Task<IActionResult> InvokeActionExpressionAsync()
        {
            var actionDelegate = AsyncActionToTest.Compile();
            var result = await actionDelegate.Invoke(Controller)(DefaultId);
            return result;
        }

        private async Task ThenActionResultShouldBeOfType<T>()
        {
            var result = await InvokeActionExpressionAsync();

            result.ShouldBeOfType<T>();
        }
    }
}
