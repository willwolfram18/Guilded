using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SelamaApi.Data.ViewModels.Forums;

namespace SelamaApi.Data.DAL.Forums
{
    public interface IForumsReadWriteDataContext : IReadWriteDataContext
    {
        IForumSectionReadWriteRepository ForumSections { get; }
    }
}
