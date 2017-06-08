using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.Data.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole> store, IEnumerable<IRoleValidator<ApplicationRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationRole>> logger, IHttpContextAccessor contextAccessor) : base(store, roleValidators, keyNormalizer, errors, logger, contextAccessor)
        {
        }

        public override IQueryable<ApplicationRole> Roles
        {
            get
            {
                return base.Roles
                        .Include(r => r.Claims);
            }
        }
    }
}
