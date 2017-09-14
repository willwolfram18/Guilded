using Guilded.Areas.Admin.DAL;
using Guilded.Data.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guilded.DAL;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement(Attributes = "role-claim-protected")]
    public class RoleClaimRequiredTagHelper : TagHelper
    {
        public RoleClaim RequiredClaim { get; set; }

        public RoleClaim[] PossibleClaims { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private ClaimsPrincipal User => ViewContext.HttpContext.User;

        private readonly IUserRoleClaimsForRequest _userRoleClaims;

        public RoleClaimRequiredTagHelper(IUserRoleClaimsForRequest userRoleClaims)
        {
            _userRoleClaims = userRoleClaims;
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

            if (!await MeetsClaimRequirements())
            {
                output.SuppressOutput();
                return;
            }

            await base.ProcessAsync(context, output);
        }

        private async Task<bool> MeetsClaimRequirements()
        {
            var roleClaims = await _userRoleClaims.GetRoleClaimsAsync();

            if (RequiredClaim != null)
            {
                return roleClaims.Any(c => c == RequiredClaim);
            }

            return PossibleClaims.Any(c => roleClaims.Contains(c));
        }
    }
}
