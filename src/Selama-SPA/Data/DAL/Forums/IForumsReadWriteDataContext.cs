using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama_SPA.Data.ViewModels.Forums;

namespace Selama_SPA.Data.DAL.Forums
{
    public interface IForumsReadWriteDataContext : IReadWriteDataContext
    {
        IForumSectionReadWriteRepository ForumSections { get; }
    }
}
