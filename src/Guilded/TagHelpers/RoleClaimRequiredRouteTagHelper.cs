using Guilded.Areas.Admin.DAL;
using Guilded.Data.Identity;
using Guilded.Extensions;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement("a")]
    public class RoleClaimRequiredRouteTagHelper : TagHelper
    {
        private const string RequiredClaimAttribute = "required-claim";

        public RoleClaim RequiredClaim { get; set; }

        public RoleClaim[] PossibleClaims { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private ClaimsPrincipal User => ViewContext.HttpContext.User;

        private readonly IUsersDataContext _usersDataContext;
        private readonly IRolesDataContext _rolesDataContext;

        public RoleClaimRequiredRouteTagHelper(IUsersDataContext usersDataContext, IRolesDataContext rolesDataContext)
        {
            _usersDataContext = usersDataContext;
            _rolesDataContext = rolesDataContext;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (RequiredClaim == null && (PossibleClaims == null || PossibleClaims.Length == 0))
            {
                await base.ProcessAsync(context, output);
                return;
            }

            if (!User.Identity.IsAuthenticated)
            {
                output.SuppressOutput();
                return;
            }

            var appUser = await _usersDataContext.GetUserFromClaimsPrincipalAsync(User);
            var currentRole = await _usersDataContext.GetRoleForUserAsync(appUser);

            if (!MeetsClaimRequirements(currentRole))
            {
                output.SuppressOutput();
                return;
            }

            await base.ProcessAsync(context, output);
        }

        private bool MeetsClaimRequirements(ApplicationRole role)
        {
            var roleClaims = _rolesDataContext.GetClaimsForRole(role);

            if (RequiredClaim != null)
            {
                return roleClaims.Any(c => c.Value == RequiredClaim.ClaimValue);
            }

            return PossibleClaims.Any(c => roleClaims.Any(r => r.Value == c.ClaimValue));
        }
    }
}
