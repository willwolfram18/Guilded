using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement(Attributes = RouteActiveAttribute)]
    public class RouteActiveTagHelper : TagHelper
    {
        private const string RouteActiveAttribute = "route-active";

        [HtmlAttributeName(RouteActiveAttribute)]
        public bool RouteActiveCheck { get; set; } = true;

        [HtmlAttributeName("route-active-area")]
        public string AreaName { get; set; } = "";

        [HtmlAttributeName("route-active-controller")]
        public string ControllerName { get; set; } = "Home";

        [HtmlAttributeName("route-active-action")]
        public string ActionName { get; set; } = "Index";

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private HttpRequest Request => ViewContext.HttpContext.Request;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            if (RouteActiveCheck)
            {
                var urlHelper = new UrlHelper(ViewContext);
                var activeRoute = urlHelper.Action(ActionName, ControllerName, new { area = AreaName });

                if (Request.Path.StartsWithSegments(new PathString(activeRoute)))
                {
                    var cssClass = output.Attributes["class"] ?? new TagHelperAttribute("class", "item");

                    output.Attributes.SetAttribute("class", $"{cssClass.Value} active");
                }
            }
        }
    }
}
