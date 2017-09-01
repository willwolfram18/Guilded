using Guilded.Data.Forums;
using Guilded.ViewModels;
using System;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ReplyViewModel : IMarkdownContent
    {
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Author { get; set; }

        public ReplyViewModel()
        {
        }

        public ReplyViewModel(Reply reply)
        {
            Content = reply.Content;
            CreatedAt = reply.CreatedAt;
            Author = reply.Author.UserName;
        }
    }
}
