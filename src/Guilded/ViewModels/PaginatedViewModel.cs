using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guilded.ViewModels
{
    public class PaginatedViewModel<TViewModel>
    {
        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public List<TViewModel> Models { get; set; }
    }
}
