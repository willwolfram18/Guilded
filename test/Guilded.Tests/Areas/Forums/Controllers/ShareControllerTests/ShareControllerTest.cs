using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.DAL;
using Guilded.Services;
using Guilded.Tests.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Guilded.Areas.Forums.ViewModels;

namespace Guilded.Tests.Areas.Forums.Controllers.ShareControllerTests
{
    using ActionExpression = Expression<Func<ShareController, Func<int, Task<IActionResult>>>>;

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

        protected async Task ThenResultShouldBeNotFound(ActionExpression controllerAction)
        {
            await ThenActionResultShouldBeOfType<NotFoundResult>(controllerAction);
        }

        protected async Task ThenViewResultIsReturned(ActionExpression controllerAction)
        {
            await ThenActionResultShouldBeOfType<ViewResult>(controllerAction);
        }

        protected async Task ThenViewModelIsOfType<T>(ActionExpression controllerAction)
            where T : class 
        {
            var viewModel = await GetViewModel<T>(controllerAction);
            viewModel.ShouldBeOfType<T>();
        }

        protected async Task<T> GetViewModel<T>(ActionExpression controllerAction)
            where T : class

        {
            var result = (ViewResult)await InvokeActionExpressionAsync(controllerAction);
            return result.Model as T;
        }

        protected async Task ThenViewModelShareLinkMatchesDefaultShareLink(ActionExpression controllerAction)
        {
            var viewModel = await GetViewModel<IShareForumsContent>(controllerAction);

            viewModel.ShareLink.ShouldBe(DefaultShareLink);
        }

        protected async Task ThenViewModelTitleMatchesExpected<T>(string expectedTitle,
            ActionExpression controllerAction)
            where T : class, IShareForumsContent
        {
            var viewModel = await GetViewModel<T>(controllerAction);

            viewModel.Title.ShouldBe(expectedTitle);
        }

        private async Task<IActionResult> InvokeActionExpressionAsync(ActionExpression controllerAction)
        {
            var actionDelegate = controllerAction.Compile();
            var result = await actionDelegate.Invoke(Controller)(DefaultId);
            return result;
        }

        private async Task ThenActionResultShouldBeOfType<T>(ActionExpression controllerAction)
        {
            var result = await InvokeActionExpressionAsync(controllerAction);

            result.ShouldBeOfType<T>();
        }
    }
}
