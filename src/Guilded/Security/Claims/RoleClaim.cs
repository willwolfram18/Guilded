namespace Guilded.Security.Claims
{
    public class RoleClaim
    {
        public string ClaimValue { get; private set; }

        public string Description { get; private set; }

        public RoleClaim(string claimValue, string description)
        {
            ClaimValue = claimValue;
            Description = description;
        }
    }
}
