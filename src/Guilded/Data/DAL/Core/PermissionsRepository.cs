using Guilded.Data.DAL.Abstract;
using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Extensions;
using Guilded.Security.Claims;
using System.Collections;
using System.Collections.Generic;

namespace Guilded.Data.DAL.Core
{
    public class PermissionsRepository : IPermissionsRepository
    {
        public IEnumerable<Permission> Get()
        {
            return RoleClaimTypes.RoleClaims.ToListOfDifferentType(rc => new Permission(rc));
        }
    }

}