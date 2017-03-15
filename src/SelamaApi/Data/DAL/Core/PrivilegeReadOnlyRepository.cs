using SelamaApi.Data.DAL.Abstract;
using SelamaApi.Data.Models.Core;

namespace SelamaApi.Data.DAL.Core
{
    public class PrivilegeReadOnlyRepository : ReadOnlyRepositoryBase<ResourcePrivilege>, IPrivilegeReadOnlyRepository
    {
        public PrivilegeReadOnlyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

}