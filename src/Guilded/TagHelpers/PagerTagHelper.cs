using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
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
        private const string VerticalLocationAttribute = "pager-vertical-location";
        private const string RequiredAttributes = CurrentPageAttribute + "," +
            LastPageAttribute + "," +
            UrlAttribute + "," +
            VerticalLocationAttribute;

        [HtmlAttributeName(CurrentPageAttribute)]
        public int CurrentPage { get; set; }

        [HtmlAttributeName(LastPageAttribute)]
        public int LastPage { get; set; }

        [HtmlAttributeName(UrlAttribute)]
        public string Url { get; set; }

        [HtmlAttributeName(VerticalLocationAttribute)]
        public PagerVerticalLocation VerticalLocation { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var currentCss = output.Attributes["css"];

            output.TagName = OutputTag;
            output.TagMode = TagMode.StartTagAndEndTag;

            if (LastPage == 1)
            {
                output.Content.SetHtmlContent("");
                return base.ProcessAsync(context, output);
            }

            output.Attributes.SetAttribute("class", $"ui buttons pager {VerticalLocation.ToString().ToLower()} {currentCss?.Value}");

            var firstButton = CreateFirstButton();
            var prevCurrentAndNextButtons = CreatePrevCurrentAndNextButtons();
            var lastButton = CreateLastButton();

            output.Content.AppendHtml(firstButton);
            output.Content.AppendHtml(prevCurrentAndNextButtons);
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

            if (CurrentPage == LastPage || LastPage == 0)
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

        private IHtmlContent CreatePrevCurrentAndNextButtons()
        {
            var builder = new HtmlContentBuilder();

            if (CurrentPage - 2 >= 1)
            {
                builder.AppendHtml("<div class='ui button disabled'>...</div>");
            }

            if (CurrentPage != 1)
            {
                builder.AppendHtml(NeighborButton(CurrentPage - 1));
            }

            builder.AppendHtml($"<div class='ui button active'>{CurrentPage}</div>");

            // There are no pages beyond the current, no reason to add a "next" button.
            if (LastPage != 0)
            {
                if (CurrentPage != LastPage)
                {
                    builder.AppendHtml(NeighborButton(CurrentPage + 1));
                }

                if (CurrentPage + 2 <= LastPage)
                {
                    builder.AppendHtml("<div class='ui button disabled'>...</div>");
                }
            }

            return builder;
        }

        private string NeighborButton(int pageNumber)
        {
            return $@"<a class='ui button' href='{Url}?page={pageNumber}'>{pageNumber}</a>";
        }
    }
}
