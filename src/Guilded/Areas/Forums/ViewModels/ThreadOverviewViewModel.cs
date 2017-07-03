using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Data.Forums;

namespace Guilded.Areas.Forums.ViewModels
{
    public class ThreadOverviewViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string Author { get; set; }

        public ThreadOverviewViewModel()
        {
        }

        public ThreadOverviewViewModel(Thread thread)
        {
            Id = thread.Id;
            Title = thread.Title;
            Slug = thread.Slug;
            Author = thread.Author.UserName;
        }
    }
}
