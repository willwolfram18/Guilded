using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Services;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Guilded.TagHelpers
{
    [HtmlTargetElement("markdown-content", TagStructure = TagStructure.WithoutEndTag)]
    public class MarkdownContentTagHelper : TagHelper
    {
        private readonly IMarkdownConverter _converter;

        private string _content;
        public string Content
        {
            get => _content ??
                   (_content = (Context.ViewData.Model as IMarkdownContent).Content);
            set => _content = value;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext Context { get; set; }

        public MarkdownContentTagHelper(IMarkdownConverter converter)
        {
            _converter = converter;
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";

            var currentCss = output.Attributes["class"];
            output.Attributes.SetAttribute("class", $"markdown-content {currentCss?.Value}");

            output.Content.SetHtmlContent(_converter.Convert(Content));

            return base.ProcessAsync(context, output);
        }
    }
}
