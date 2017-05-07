using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement(Attributes = ItemSelectedAttribute)]
    public class MenuItemSelectedTagHelper : TagHelper
    {
        private const string ItemSelectedAttribute = "guilded-route-active";

        [HtmlAttributeName(ItemSelectedAttribute)]
        public string GuildRoute { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private HttpRequest Request => ViewContext.HttpContext.Request;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            if (Request.Path.StartsWithSegments(new PathString(GuildRoute)))
            {
                var cssClass = output.Attributes["class"] ?? new TagHelperAttribute("class");

                output.Attributes.SetAttribute("class", $"{cssClass.Value} active");
            }
        }
    }
}
