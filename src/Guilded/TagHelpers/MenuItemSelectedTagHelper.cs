using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement("a", Attributes = ItemSelectedAttribute)]
    public class MenuItemSelectedTagHelper : TagHelper
    {
        private const string ItemSelectedAttribute = "guilded-route-active";

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private HttpRequest Request => ViewContext.HttpContext.Request;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            var href = output.Attributes["href"];

            if (href?.Value.ToString() == Request.Path)
            {
                var cssClass = output.Attributes["class"] ?? new TagHelperAttribute("class");

                output.Attributes.SetAttribute("class", $"{cssClass.Value} active");
            }
        }
    }
}
