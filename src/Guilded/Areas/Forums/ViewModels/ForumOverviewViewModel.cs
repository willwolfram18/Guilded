using Guilded.Data.Forums;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ForumOverviewViewModel
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public ForumOverviewViewModel()
        {
        }

        public ForumOverviewViewModel(Forum forum)
        {
            Id = forum.Id;
            Slug = forum.Slug;
            Title = forum.Title;
            Subtitle = forum.Subtitle;
        }
    }
}
