using Guilded.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Guilded.DAL
{
    public interface IUserRoleClaimsForRequest
    {
        Task<IEnumerable<RoleClaim>> GetRoleClaimsAsync();
    }
}
