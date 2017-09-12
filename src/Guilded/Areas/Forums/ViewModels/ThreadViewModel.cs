using Guilded.Data.Forums;
using Guilded.Extensions;
using Guilded.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ThreadViewModel : ThreadOverviewViewModel, IMarkdownContent, IPaginatedViewModel<ReplyViewModel>, IForumPost
    {
        private readonly string _authorId;

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
            _authorId = thread.AuthorId;
        }

        public bool IsUserTheAuthor(ClaimsPrincipal user)
        {
            return user.UserId() == _authorId;
        }
    }
}
