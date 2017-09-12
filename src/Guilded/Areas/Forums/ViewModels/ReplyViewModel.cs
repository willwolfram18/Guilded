using Guilded.Data.Forums;
using Guilded.Extensions;
using Guilded.ViewModels;
using System;
using System.Security.Claims;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ReplyViewModel : IMarkdownContent, IForumPost
    {
        private readonly string _authorId;

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Author { get; set; }

        public bool IsLocked { get; set; }

        public ReplyViewModel()
        {
        }

        public ReplyViewModel(Reply reply)
        {
            Id = reply.Id;
            Content = reply.Content;
            CreatedAt = reply.CreatedAt;
            Author = reply.Author.UserName;
            IsLocked = reply.Thread.IsLocked;
            _authorId = reply.AuthorId;
        }

        public bool IsUserTheAuthor(ClaimsPrincipal user)
        {
            return user.UserId() == _authorId;
        }
    }
}
