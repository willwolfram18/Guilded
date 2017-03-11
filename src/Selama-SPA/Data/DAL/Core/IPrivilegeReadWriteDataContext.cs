using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama_SPA.Data.Models.Core;

namespace Selama_SPA.Data.DAL.Core
{
    public interface IPrivilegeReadWriteDataContext : IReadWriteDataContext
    {
        ApplicationRole CreateRole(string roleName);

        IEnumerable<ResourcePrivilege> AddPrivilegeToRole(ApplicationRole role, ResourcePrivilege privilege);
    }
}
