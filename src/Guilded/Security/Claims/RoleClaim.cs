namespace Guilded.Security.Claims
{
    public class RoleClaim
    {
        public string ClaimValue { get; }

        public string Description { get; }

        public RoleClaim(string claimValue, string description)
        {
            ClaimValue = claimValue;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            var otherRoleClaim = obj as RoleClaim;
            if (otherRoleClaim == null)
            {
                return false;
            }


            return ClaimValue.Equals(otherRoleClaim?.ClaimValue);
        }

        public override int GetHashCode()
        {
            return  ClaimValue?.GetHashCode() ?? 0;
        }
        
        public static bool operator==(RoleClaim leftHand, RoleClaim rightHand)
        {
            if ((object)leftHand == null)
            {
                return (object)rightHand == null;
            }

            return leftHand.Equals(rightHand);
        }

        public static bool operator !=(RoleClaim leftHand, RoleClaim rightHand)
        {
            return !(leftHand == rightHand);
        }
    }
}
