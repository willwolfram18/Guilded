using Selama_SPA.Data.DAL.Abstract;
using Selama_SPA.Data.Models.Core;

namespace Selama_SPA.Data.DAL.Core
{
    public class PrivilegeReadOnlyRepository : ReadOnlyRepositoryBase<ResourcePrivilege>, IPrivilegeReadOnlyRepository
    {
        public PrivilegeReadOnlyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

}