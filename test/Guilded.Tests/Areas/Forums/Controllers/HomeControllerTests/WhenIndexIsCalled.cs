using Guilded.Areas.Forums.ViewModels;
using Guilded.Data.Forums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.Tests.Areas.Forums.Controllers.HomeControllerTests
{
    public class WhenIndexIsCalled : HomeControllerTest
    {
        [SetUp]
        public void SetUp()
        {
            MockDataContext.Setup(d => d.GetActiveForumSections())
                .Returns(new List<ForumSection>().AsQueryable());
        }

        [Test]
        public void ThenResultIsNotNull()
        {
            var result = Controller.Index();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ThenViewModelIsIEnumerableViewModel()
        {
            var result = Controller.Index();

            var viewModel = result.Model;

            Assert.That(viewModel, Is.InstanceOf<IEnumerable<ForumSectionViewModel>>());
        }

        [Test]
        public void ThenGetActiveForumSectionsIsCalled()
        {
            Controller.Index();

            MockDataContext.Verify(d => d.GetActiveForumSections());
        }

        [Test]
        public void ThenForumSectionsMatchDataContext()
        {
            const int numSections = 5;
            const int numForums = 4;
            var forumSections = CreateForumSections(numSections, numForums);

            MockDataContext.Setup(d => d.GetActiveForumSections())
                .Returns(forumSections.AsQueryable());

            var result = Controller.Index();

            var viewModel = result.Model as IEnumerable<ForumSectionViewModel>;
            Assume.That(viewModel, Is.Not.Null);

            var viewModelList = viewModel.ToList();
            Assert.That(viewModelList.Count, Is.EqualTo(numSections));

            forumSections = forumSections.OrderBy(f => f.DisplayOrder).ToList();

            for (var i = 0; i < viewModelList.Count; i++)
            {
                AssertForumSectionsMatch(viewModelList[i], forumSections[i]);
            }
        }

        private List<ForumSection> CreateForumSections(int numSections, int numForumsPerSection)
        {
            var sections = new List<ForumSection>();

            for (var i = 1; i <= numSections; i++)
            {
                sections.Add(new ForumSection
                {
                    DisplayOrder = numSections - i,
                    Forums = CreateForums(i + ((i - 1) * numForumsPerSection), numForumsPerSection),
                    Id = i,
                    Title = $"Forum Section {i}",
                    IsActive = i % 2 == 0,
                });
            }

            return sections;
        }

        private List<Forum> CreateForums(int startingId, int numForums)
        {
            var forums = new List<Forum>();

            for (int i = startingId; i < startingId + numForums; i++)
            {
                forums.Add(new Forum
                {
                    Title = $"Forum {i}",
                    Id = i,
                    IsActive = i % 2 == 1,
                    Slug = $"forum-{i}-slug",
                    Subtitle = $"Subtitle for forum {i}",
                });
            }

            return forums;
        }
    }
}
