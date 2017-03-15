using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SelamaApi.Data.Models.Forums;

namespace SelamaApi.Data.DAL.Forums
{
    interface IForumReadWriteRepository : IReadWriteRepository<Forum>
    {
    }
}
