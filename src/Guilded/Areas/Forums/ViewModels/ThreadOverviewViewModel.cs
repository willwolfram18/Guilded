using Guilded.Data.Forums;
using System;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ThreadOverviewViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsLocked { get; set; }

        public bool IsPinned { get; set; }

        public ThreadOverviewViewModel()
        {
        }

        public ThreadOverviewViewModel(Thread thread)
        {
            Id = thread.Id;
            Title = thread.Title;
            Slug = thread.Slug;
            Author = thread.Author.UserName;
            CreatedAt = thread.CreatedAt;
            IsLocked = thread.IsLocked;
            IsPinned = thread.IsPinned;
        }
    }
}
