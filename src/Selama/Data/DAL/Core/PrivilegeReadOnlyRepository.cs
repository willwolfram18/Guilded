using Selama.Data.DAL.Abstract;
using Selama.Data.Models.Core;

namespace Selama.Data.DAL.Core
{
    public class PrivilegeReadOnlyRepository : ReadOnlyRepositoryBase<ResourcePrivilege>, IPrivilegeReadOnlyRepository
    {
        public PrivilegeReadOnlyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

}