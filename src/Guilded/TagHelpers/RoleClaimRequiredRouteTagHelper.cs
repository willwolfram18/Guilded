using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Admin.Data.DAL;
using Guilded.Extensions;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement("a", Attributes = RequiredClaimAttribute)]
    public class RoleClaimRequiredRouteTagHelper : TagHelper
    {
        private const string RequiredClaimAttribute = "required-claim";

        public RoleClaim RequiredClaim { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private ClaimsPrincipal User => ViewContext.HttpContext.User;

        private readonly IUsersDataContext _usersDataContext;

        public RoleClaimRequiredRouteTagHelper(IUsersDataContext usersDataContext)
        {
            _usersDataContext = usersDataContext;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!User.Identity.IsAuthenticated)
            {
                output.SuppressOutput();
                return;
            }

            var appUser = await _usersDataContext.GetUserFromClaimsPrincipalAsync(User);
            var currentRole = await _usersDataContext.GetRoleForUserAsync(appUser);

            if (!currentRole.HasRoleClaim(RequiredClaim))
            {
                output.SuppressOutput();
                return;
            }

            await base.ProcessAsync(context, output);
        }
    }
}
