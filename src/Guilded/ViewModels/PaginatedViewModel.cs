using System.Collections.Generic;

namespace Guilded.ViewModels
{
    public class PaginatedViewModel<TViewModel> : IPaginatedViewModel<TViewModel>
    {
        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public string PagerUrl { get; set; }

        public IEnumerable<TViewModel> Models { get; set; }

        public PaginatedViewModel()
        {
            Models = new List<TViewModel>();
        }
    }
}
