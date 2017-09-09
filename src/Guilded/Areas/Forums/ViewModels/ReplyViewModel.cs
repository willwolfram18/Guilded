using Guilded.Data.Forums;
using Guilded.ViewModels;
using System;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ReplyViewModel : IMarkdownContent
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Author { get; set; }
        
        public bool IsThreadLocked { get; set; }

        public ReplyViewModel()
        {
        }

        public ReplyViewModel(Reply reply)
        {
            Id = reply.Id;
            Content = reply.Content;
            CreatedAt = reply.CreatedAt;
            Author = reply.Author.UserName;
            IsThreadLocked = reply.Thread.IsLocked;
        }
    }
}
