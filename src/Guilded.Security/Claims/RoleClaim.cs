using System;
using System.Collections.Generic;
using System.Text;

namespace Guilded.Security.Claims
{
    public class RoleClaim
    {
        #region Properties
        #region Public properties
        public string ClaimType { get; private set; }

        public string Description { get; private set; }
        #endregion
        #endregion

        public RoleClaim(string claimType, string description)
        {
            ClaimType = claimType;
            Description = description;
        }
    }
}
