using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.DAL;
using Guilded.Services;
using Guilded.Tests.Controllers;
using Moq;

namespace Guilded.Tests.Areas.Forums.Controllers.ShareControllerTests
{
    public class ShareControllerTest : ControllerTest<ShareController>
    {
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
    }
}
