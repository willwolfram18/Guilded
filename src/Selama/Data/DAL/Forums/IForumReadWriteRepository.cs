using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama.Data.Models.Forums;

namespace Selama.Data.DAL.Forums
{
    interface IForumReadWriteRepository : IReadWriteRepository<Forum>
    {
    }
}
