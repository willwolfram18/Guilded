using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SelamaApi.Data.Models.Core;

namespace SelamaApi.Data.DAL.Core
{
    public interface IPrivilegeReadOnlyRepository : IReadOnlyRepository<ResourcePrivilege>
    {
    }
}
