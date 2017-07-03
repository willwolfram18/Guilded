using System.Collections.Generic;
using System.Linq;
using Guilded.Data.Forums;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ForumSectionViewModel
    {
        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public IEnumerable<ForumOverviewViewModel> Forums { get; set; }

        public ForumSectionViewModel()
        {
        }

        public ForumSectionViewModel(ForumSection forumSection)
        {
            Title = forumSection.Title;
            DisplayOrder = forumSection.DisplayOrder;
            Forums = forumSection.Forums.Where(f => f.IsActive)
                .OrderBy(f => f.Title)
                .ToList() // need to bring into memory before calling constructor.
                .Select(f => new ForumOverviewViewModel(f));
        }
    }
}
