using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Data.Forums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace Guilded.Tests.Areas.Forums.Controllers.ThreadsControllerTests
{
    public class WhenShareThreadIsCalled : ThreadsControllerTest
    {
        private const string DefaultSlug = "my-thread";

        private Thread _defaultThread;

        [SetUp]
        public void SetUp()
        {
            _defaultThread = new Thread
            {
                Slug = DefaultSlug,
            };

            MockMarkdownConverter.Setup(md => md.ConvertAndStripHtml(It.IsAny<string>()))
                .Returns(string.Empty);
            MockDataContext.Setup(d => d.GetThreadBySlugAsync(It.IsAny<string>()))
                .ReturnsAsync(_defaultThread);
        }

        [Test]
        public async Task IfSlugIsNotFoundThenNotFoundResultReturned()
        {
            MockDataContext.Setup(d => d.GetThreadBySlugAsync(It.IsAny<string>()))
                .ReturnsAsync((Thread)null);

            await ThenResultShouldBeNotFound();
        }

        [Test]
        public async Task IfThreadIsDeletedThenNotFoundResultReturned()
        {
            _defaultThread.IsDeleted = true;

            await ThenResultShouldBeNotFound();
        }

        [Test]
        public async Task ThenGetThreadBySlugAsyncIsCalledWithSlugParameter()
        {
            await Controller.ShareThread(DefaultSlug);

            MockDataContext.Verify(d => d.GetThreadBySlugAsync(It.Is<string>(s => s == DefaultSlug)));
        }

        [Test]
        public async Task ThenViewResultIsReturned()
        {
            var result = await Controller.ShareThread(DefaultSlug);

            result.ShouldBeOfType<ViewResult>();
        }

        [Test]
        public async Task ThenViewModelIsAThreadPreview()
        {
            var viewModel = await ShareThreadViewModel();

            viewModel.ShouldNotBeNull();
        }

        [Test]
        public async Task ThenViewModelSlugAndTitleShouldMatchDataModle()
        {
            const string threadTitle = "My thread title";

            _defaultThread.Title = threadTitle;

            var viewModel = await ShareThreadViewModel();

            viewModel.Title.ShouldBe(threadTitle);
            viewModel.Slug.ShouldBe(DefaultSlug);
        }

        [Test]
        public async Task ThenMarkdownConverterShouldUseThreadContent()
        {
            const string threadContent = "I am the content of this thread!";

            _defaultThread.Content = threadContent;

            await Controller.ShareThread(DefaultSlug);

            MockMarkdownConverter.Verify(c => c.ConvertAndStripHtml(It.Is<string>(s => s == threadContent)));
        }

        [Test]
        public async Task ThenViewModelContentIsConvertedMarkdownThatIsStrippedOfHtml()
        {
            const string threadContent = "__Yes__ I _am_ markdown!";
            const string strippedContent = "Yes I am markdown!";

            _defaultThread.Content = threadContent;

            MockMarkdownConverter.Setup(c => c.ConvertAndStripHtml(It.IsAny<string>()))
                .Returns(strippedContent);

            var viewModel = await ShareThreadViewModel();

            viewModel.ContentPreview.ShouldBe(strippedContent);
        }

        [Test]
        public async Task ThenContentPreviewDoesNotExceedDesiredMaximum()
        {
            var threadContent = new string('a', ThreadsController.ThreadPreviewLength + 5);

            MockMarkdownConverter.Setup(c => c.ConvertAndStripHtml(It.IsAny<string>()))
                .Returns(threadContent);

            var viewModel = await ShareThreadViewModel();

            viewModel.ContentPreview.Length.ShouldBe(ThreadsController.ThreadPreviewLength);
        }

        private async Task ThenResultShouldBeNotFound()
        {
            var result = await Controller.ShareThread(DefaultSlug);

            result.ShouldBeOfType<NotFoundResult>();
        }

        private async Task<ThreadPreview> ShareThreadViewModel()
        {
            var result = (ViewResult)await Controller.ShareThread(DefaultSlug);
            return result.Model as ThreadPreview;
        }
    }
}
