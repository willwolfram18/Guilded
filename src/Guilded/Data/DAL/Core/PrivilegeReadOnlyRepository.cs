using Guilded.Data.DAL.Abstract;
using Guilded.Data.Models.Core;

namespace Guilded.Data.DAL.Core
{
    public class PrivilegeReadOnlyRepository : ReadOnlyRepositoryBase<ResourcePrivilege>, IPrivilegeReadOnlyRepository
    {
        public PrivilegeReadOnlyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

}