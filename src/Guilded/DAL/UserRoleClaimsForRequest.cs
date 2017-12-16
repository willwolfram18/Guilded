using Guilded.Areas.Admin.DAL;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.DAL
{
    public class UserRoleClaimsForRequest : IUserRoleClaimsForRequest
    {
        private readonly IUsersDataContext _usersDataContext;
        private readonly IRolesDataContext _rolesDataContext;
        private readonly IHttpContextAccessor _httpContext;

        private List<RoleClaim> _roleClaims;

        public UserRoleClaimsForRequest(
            IUsersDataContext usersDataContext,
            IRolesDataContext rolesDataContext,
            IHttpContextAccessor httpContext)
        {
            _usersDataContext = usersDataContext;
            _rolesDataContext = rolesDataContext;
            _httpContext = httpContext;
        }

        public async Task<IEnumerable<RoleClaim>> GetRoleClaimsAsync()
        {
            if (_roleClaims != null)
            {
                return _roleClaims.AsReadOnly();
            }

            if (!_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _roleClaims = new List<RoleClaim>();
            }
            else
            {
                var appUser = await _usersDataContext.GetUserFromClaimsPrincipalAsync(_httpContext.HttpContext.User);
                var userRole = await _usersDataContext.GetRoleForUserAsync(appUser);

                _roleClaims = _rolesDataContext.GetClaimsForRole(userRole)
                    .Select(c => RoleClaimValues.LookUpGuildedRoleClaim(c.Value))
                    .ToList();
            }

            return _roleClaims;
        }
    }
}
