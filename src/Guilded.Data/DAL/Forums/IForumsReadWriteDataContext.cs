using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Data.ViewModels.Forums;

namespace Guilded.Data.DAL.Forums
{
    public interface IForumsReadWriteDataContext : IReadWriteDataContext
    {
        IForumSectionReadWriteRepository ForumSections { get; }
    }
}
