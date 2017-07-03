using Guilded.Data.Forums;
using Guilded.ViewModels;
using System.Collections.Generic;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ForumViewModel : ForumOverviewViewModel, IPaginatedViewModel<ThreadOverviewViewModel>
    {
        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public string PagerUrl { get; set; }

        public IEnumerable<ThreadOverviewViewModel> Models { get; set;  }

        public IEnumerable<ThreadOverviewViewModel> PinnedThreads { get; set; }

        public ForumViewModel()
        {
        }

        public ForumViewModel(Forum forum) : base(forum)
        {
        }
    }
}
