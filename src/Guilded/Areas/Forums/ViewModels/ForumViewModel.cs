using Guilded.Data.Forums;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ForumViewModel
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public ForumViewModel()
        {
        }

        public ForumViewModel(Forum forum)
        {
            Title = forum.Title;
            Subtitle = forum.Subtitle;
        }
    }
}
