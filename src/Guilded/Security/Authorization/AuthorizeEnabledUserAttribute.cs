using Microsoft.AspNetCore.Authorization;

namespace Guilded.Security.Authorization
{
    public class AuthorizeEnabledUserAttribute : AuthorizeAttribute
    {
        public const string PolicyName = CustomAuthorizationPolicies.RequireEnabledUser;

        public AuthorizeEnabledUserAttribute() : base(PolicyName)
        {
        }
    }
}
