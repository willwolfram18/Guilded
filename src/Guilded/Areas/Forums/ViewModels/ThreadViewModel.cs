using Guilded.Data.Forums;
using Guilded.ViewModels;
using System.Collections.Generic;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ThreadViewModel : ThreadOverviewViewModel, IMarkdownContent, IPaginatedViewModel<ReplyViewModel>
    {
        public string Content { get; set; }

        public int CurrentPage { get; }

        public int LastPage { get; }

        public string PagerUrl { get; }

        public IEnumerable<ReplyViewModel> Models { get; }

        public ThreadViewModel()
        {
        }

        public ThreadViewModel(Thread thread) : base(thread)
        {
            Content = thread.Content;
        }
    }
}
