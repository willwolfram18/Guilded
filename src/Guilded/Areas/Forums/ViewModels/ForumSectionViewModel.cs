using Guilded.Data.Forums;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ForumSectionViewModel
    {
        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public ForumSectionViewModel()
        {
        }

        public ForumSectionViewModel(ForumSection forumSection)
        {
            Title = forumSection.Title;
            DisplayOrder = forumSection.DisplayOrder;
        }
    }
}
