using Guilded.Constants;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement("guilded-breadcumbs", TagStructure = TagStructure.WithoutEndTag)]
    public class BreadcrumbsTagHelper : TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Stack<Breadcrumb> breadcrumbs = ViewContext.ViewData[ViewDataKeys.Breadcrumbs] as Stack<Breadcrumb>;

            if (breadcrumbs == null)
            {
                throw new KeyNotFoundException("ViewData[ViewDataKeys.Breadcrumbs]");
            }

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            var cssClass = output.Attributes["class"];
            output.Attributes.SetAttribute("class", $"ui breadcrumb {cssClass?.Value}");

            while (breadcrumbs.Any())
            {
                Breadcrumb currentCrumb = breadcrumbs.Pop();
                var isLastCrumb = !breadcrumbs.Any();

                if (isLastCrumb)
                {
                    output.Content.AppendHtml($"<div class='active section'>{currentCrumb.Title}</div>");
                }
                else
                {
                    output.Content.AppendHtml($"<a class='section' href='{currentCrumb.Url}'>{currentCrumb.Title}</a>");
                    output.Content.AppendHtml("<i class='right chevron icon divider'></i>");
                }
            }

            return base.ProcessAsync(context, output);
        }
    }
}
