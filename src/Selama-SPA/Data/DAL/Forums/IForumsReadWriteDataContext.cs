using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama.Data.ViewModels.Forums;

namespace Selama.Data.DAL.Forums
{
    public interface IForumsReadWriteDataContext : IReadWriteDataContext
    {
        IForumSectionReadWriteRepository ForumSections { get; }
    }
}
