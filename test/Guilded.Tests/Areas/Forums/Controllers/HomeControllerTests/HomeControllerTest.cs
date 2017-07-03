using Guilded.Areas.Forums.Controllers;
using Guilded.Areas.Forums.DAL;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Data.Forums;
using Guilded.Tests.Controllers;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace Guilded.Tests.Areas.Forums.Controllers.HomeControllerTests
{
    public class HomeControllerTest : ControllerTest<HomeController>
    {
        protected Mock<IForumsDataContext> MockDataContext { get; private set; }

        protected override HomeController SetUpController()
        {
            MockDataContext = new Mock<IForumsDataContext>();

            return new HomeController(MockDataContext.Object);
        }

        protected void AssertForumSectionsMatch(ForumSectionViewModel actualSection, ForumSection expectedSection)
        {
            Assert.That(actualSection.Title, Is.EqualTo(expectedSection.Title));
            Assert.That(actualSection.DisplayOrder, Is.EqualTo(expectedSection.DisplayOrder));
            Assert.That(actualSection.Forums.Count(), Is.EqualTo(expectedSection.Forums.Count(f => f.IsActive)));

            var expectedForums = expectedSection.Forums.Where(f => f.IsActive)
                .OrderBy(f => f.Title)
                .ToList();
            var actualForums = actualSection.Forums.ToList();

            for (int i = 0; i < expectedForums.Count; i++)
            {
                AssertThatForumsMatch(actualForums[i], expectedForums[i]);
            }
        }

        protected void AssertThatForumsMatch(ForumOverviewViewModel actualForumOverview, Forum expectedForum)
        {
            Assert.That(actualForumOverview.Id, Is.EqualTo(expectedForum.Id));
            Assert.That(actualForumOverview.Slug, Is.EqualTo(expectedForum.Slug));
            Assert.That(actualForumOverview.Title, Is.EqualTo(expectedForum.Title));
            Assert.That(actualForumOverview.Subtitle, Is.EqualTo(expectedForum.Subtitle));
        }
    }
}
