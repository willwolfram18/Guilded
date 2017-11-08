using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "asp-validation-summary")]
    [HtmlTargetElement("span", Attributes = "asp-validation-for")]
    public class ValidationMessageTagHelper : TagHelper
    {
        private const string ValidationSummaryCssClasses = "ui raised warning segment";
        private const string PropertyValidationMessageCssClasses = "ui raised pointing warning label";

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cssClass = output.Attributes["class"];

            if (output.TagName == "div")
            {
                output.Attributes.SetAttribute("class", $"{ValidationSummaryCssClasses} {cssClass.Value}");
            }
            else
            {
                output.Attributes.SetAttribute("class", $"{PropertyValidationMessageCssClasses} {cssClass.Value}");
            }
            return base.ProcessAsync(context, output);
        }
    }
}
