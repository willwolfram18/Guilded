using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement("guilded-pager", TagStructure = TagStructure.WithoutEndTag, Attributes = RequiredAttributes)]
    public class PagerTagHelper : TagHelper
    {
        private const string OutputTag = "div";
        private const string CurrentPageAttribute = "pager-page";
        private const string LastPageAttribute = "pager-last-page";
        private const string UrlAttribute = "pager-url";
        private const string RequiredAttributes = CurrentPageAttribute + "," +
            LastPageAttribute + "," +
            UrlAttribute;

        [HtmlAttributeName(CurrentPageAttribute)]
        public int CurrentPage { get; set; }

        [HtmlAttributeName(LastPageAttribute)]
        public int LastPage { get; set; }

        [HtmlAttributeName(UrlAttribute)]
        public string Url { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var currentCss = output.Attributes["css"];

            output.TagName = OutputTag;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", $"ui buttons {currentCss?.Value}");

            var firstButton = CreateFirstButton();
            var lastButton = CreateLastButton();

            output.Content.AppendHtml(firstButton);
            output.Content.AppendHtml(lastButton);

            return base.ProcessAsync(context, output);
        }

        private IHtmlContent CreateFirstButton()
        {
            var firstButton = new HtmlContentBuilder();

            if (CurrentPage == 1)
            {
                return firstButton;
            }

            firstButton.AppendHtml(FirstOrLastPageButtonHtml(1));

            return firstButton;
        }

        private IHtmlContent CreateLastButton()
        {
            var lastButton = new HtmlContentBuilder();

            if (CurrentPage == LastPage)
            {
                return lastButton;
            }

            lastButton.AppendHtml(FirstOrLastPageButtonHtml(LastPage));

            return lastButton;
        }

        private string FirstOrLastPageButtonHtml(int pageNumber)
        {
            string textAndIcon = pageNumber == 1 ? 
                "<i class='chevron left icon'></i> First" :
                "Last <i class='chevron right icon'></i>";

            return $@"<a class='ui icon button' href='{Url}?page={pageNumber}'>
    {textAndIcon}
</a>";
        }
    }
}
