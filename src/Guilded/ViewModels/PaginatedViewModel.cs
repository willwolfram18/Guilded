using System.Collections.Generic;

namespace Guilded.ViewModels
{
    public class PaginatedViewModel<TViewModel>
    {
        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public List<TViewModel> Models { get; set; }

        public PaginatedViewModel()
        {
            Models = new List<TViewModel>();
        }
    }
}
