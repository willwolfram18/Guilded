using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Data.Models.Forums;

namespace Guilded.Data.DAL.Forums
{
    interface IForumReadWriteRepository : IReadWriteRepository<Forum>
    {
    }
}
