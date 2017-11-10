using Guilded.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.Security.Authorization
{
    public class MultiRoleClaimAuthorizationRequirement : IAuthorizationRequirement
    {
        public IEnumerable<RoleClaim> PossibleClaims { get; }

        public MultiRoleClaimAuthorizationRequirement(IEnumerable<RoleClaim> possibleClaims)
        {
            if (possibleClaims == null)
            {
                throw new ArgumentNullException(nameof(possibleClaims));
            }

            IEnumerable<RoleClaim> roleClaims = possibleClaims as RoleClaim[] ?? possibleClaims.ToArray();
            if (!roleClaims.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(possibleClaims), "Must include one or more role claims");
            }

            PossibleClaims = roleClaims;
        }
    }
}
