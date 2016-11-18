using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Selama.Mvc.TagHelpers
{
    [HtmlTargetElement("dropdown", Attributes = TITLE)]
    [RestrictChildren("dropdown-item")]
    public class DropdownTagHelper : TagHelper
    {
        #region Properties
        #region Public Properties
        public const string TITLE = "bs-title";
        public const string ROOT_TAG_OUTPUT = "bs-render-tag";

        [HtmlAttributeName(TITLE)]
        public string Title { get; set; }

        [HtmlAttributeName(ROOT_TAG_OUTPUT)]
        public string RootTagOutput { get; set; }
        #endregion

        #region Private properties
        private DropdownContext DropdownContext { get; set; }
        #endregion
        #endregion

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            DropdownContext = new DropdownContext();
            context.Items.Add(typeof(DropdownContext), DropdownContext);

            await output.GetChildContentAsync();

            output.TagName = RootTagOutput;

            ForceDropdownCssClassOnTag(output);

            AppendHtmlToOutput(context, output);
        }

        private void ForceDropdownCssClassOnTag(TagHelperOutput output)
        {
            string classNames = "dropdown";
            if (output.Attributes.ContainsName("class"))
            {
                classNames = string.Format("{0} {1}", classNames, output.Attributes["class"].Value.ToString());
            }
            output.Attributes.SetAttribute("class", classNames);
        }

        private void AppendHtmlToOutput(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.AppendHtml($@"<a href='#' class='dropdown-toggle' data-toggle='dropdown'>{Title} <span class='caret'></span></a>
    <ul class='dropdown-menu'>");
            foreach (var item in DropdownContext.Items)
            {
                output.Content.AppendHtml("<li>");
                output.Content.AppendHtml(item);
                output.Content.AppendHtml("</li>");
            }
            output.Content.AppendHtml($@"</ul>");
        }
    }

    public class DropdownContext
    {
        #region Properties
        #region Public properties
        public TagHelperContent Title { get; set; }

        public List<IHtmlContent> Items { get; set; }
        #endregion
        #endregion

        #region Constructors
        public DropdownContext()
        {
            Items = new List<IHtmlContent>();
        }
        #endregion
    }

    [HtmlTargetElement("dropdown-title", ParentTag = "dropdown")]
    public class DropdownItemTitleTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var titleContent = output.GetChildContentAsync();
            DropdownContext dropdownContext = context.Items[typeof(DropdownContext)] as DropdownContext;
            dropdownContext.Title = await titleContent;
            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("dropdown-item", ParentTag = "dropdown")]
    public class DropdownItemTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContent = output.GetChildContentAsync();
            DropdownContext dropdownContext = context.Items[typeof(DropdownContext)] as DropdownContext;
            dropdownContext.Items.Add(await itemContent);
            output.SuppressOutput();
        }
    }
}