using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Data.Models.Core;

namespace Guilded.Data.DAL.Core
{
    public interface IPermissionsRepository : IReadOnlyRepository<ResourcePrivilege>
    {
    }
}
