using System;
using System.Threading.Tasks;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement("pager", TagStructure = TagStructure.WithoutEndTag, Attributes = RequiredAttributes)]
    public class PagerTagHelper : TagHelper
    {
        private const string OutputTag = "div";
        private const string DefaultCssClass = "ui small pager buttons";
        private const string ModelAttribute = "model";
        private const string VerticalLocationAttribute = "pager-vertical-location";
        private const string RequiredAttributes = VerticalLocationAttribute;

        private IPaginatedViewModel _model;

        [HtmlAttributeName(ModelAttribute)]
        public IPaginatedViewModel Model {
            get
            {
                _model = _model ?? ViewContext.ViewData.Model as IPaginatedViewModel;
                if (_model == null)
                {
                    throw new ArgumentException($"The model for the pager does not implement {nameof(IPaginatedViewModel)}");
                }

                return _model;
            }
            set => _model = value;
        }

        [HtmlAttributeName(VerticalLocationAttribute)]
        public PagerVerticalLocation VerticalLocation { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var currentCss = output.Attributes["css"];

            output.TagName = OutputTag;
            output.TagMode = TagMode.StartTagAndEndTag;

            // Nothing to output, since there are is only one page.
            if (Model.LastPage == 0 || Model.LastPage == 1)
            {
                output.Content.SetHtmlContent("");
                return base.ProcessAsync(context, output);
            }

            output.Attributes.SetAttribute("class", $"{DefaultCssClass} {VerticalLocation.ToString().ToLower()} {currentCss?.Value}");

            output.Content.AppendHtml(CreateFirstButton());
            output.Content.AppendHtml(CreatePrevCurrentAndNextButtons());
            output.Content.AppendHtml(CreateLastButton());

            return base.ProcessAsync(context, output);
        }

        private IHtmlContent CreateFirstButton()
        {
            var firstButton = new HtmlContentBuilder();

            if (Model.CurrentPage != 1)
            {
                firstButton.SetHtmlContent(FirstOrLastPageButtonHtml(1));
            }

            return firstButton;
        }

        private IHtmlContent CreateLastButton()
        {
            var lastButton = new HtmlContentBuilder();

            if (Model.CurrentPage != Model.LastPage && Model.LastPage != 0)
            {
                lastButton.SetHtmlContent(FirstOrLastPageButtonHtml(Model.LastPage));
            }

            return lastButton;
        }

        private string FirstOrLastPageButtonHtml(int pageNumber)
        {
            var textAndIcon = pageNumber == 1 ? 
                "<i class='chevron left icon'></i> First" :
                "Last <i class='chevron right icon'></i>";

            return $@"<a class='ui icon button' href='{Model.PagerUrl}?page={pageNumber}'>
    {textAndIcon}
</a>";
        }

        private IHtmlContent CreatePrevCurrentAndNextButtons()
        {
            var builder = new HtmlContentBuilder();

            if (Model.CurrentPage - 2 >= 1)
            {
                builder.AppendHtml("<div class='ui button disabled'>...</div>");
            }

            if (Model.CurrentPage != 1)
            {
                builder.AppendHtml(NeighborButton(Model.CurrentPage - 1));
            }

            builder.AppendHtml($"<div class='ui button active'>{Model.CurrentPage}</div>");

            if (Model.CurrentPage != Model.LastPage)
            {
                builder.AppendHtml(NeighborButton(Model.CurrentPage + 1));
            }

            if (Model.CurrentPage + 2 <= Model.LastPage)
            {
                builder.AppendHtml("<div class='ui button disabled'>...</div>");
            }

            return builder;
        }

        private string NeighborButton(int pageNumber)
        {
            return $@"<a class='ui button' href='{Model.PagerUrl}?page={pageNumber}'>{pageNumber}</a>";
        }
    }
}
