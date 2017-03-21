using Guilded.Data.DAL.Abstract;
using Guilded.Data.Models.Core;

namespace Guilded.Data.DAL.Core
{
    public class PermissionsRepository : ReadOnlyRepositoryBase<ResourcePrivilege>, IPermissionsRepository
    {
        public PermissionsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

}