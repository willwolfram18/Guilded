using System.Collections.Generic;

namespace Guilded.ViewModels
{
    public interface IPaginatedViewModel
    {
        int CurrentPage { get; }

        int LastPage { get; }

        string PagerUrl { get; }
    }

    public interface IPaginatedViewModel<TViewModel> : IPaginatedViewModel
    {
        IEnumerable<TViewModel> Models { get; }
    }
}
