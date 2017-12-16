using System.Linq;
using System.Security.Claims;

namespace Guilded.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string UserId(this ClaimsPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            return user.Claims.Single(u => u.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
