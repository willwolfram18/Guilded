using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama.Data.Models.Core;

namespace Selama.Data.DAL.Core
{
    public interface IPrivilegeReadOnlyRepository : IReadOnlyRepository<ResourcePrivilege>
    {
    }
}
