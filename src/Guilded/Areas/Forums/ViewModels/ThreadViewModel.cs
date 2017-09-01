using Guilded.Data.Forums;
using Guilded.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ThreadViewModel : ThreadOverviewViewModel, IMarkdownContent, IPaginatedViewModel<ReplyViewModel>
    {
        public string Content { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public string PagerUrl { get; set; }

        public IEnumerable<ReplyViewModel> Models { get; set; }

        public ThreadViewModel()
        {
        }

        public ThreadViewModel(Thread thread) : base(thread)
        {
            Content = thread.Content;
        }
    }
}
