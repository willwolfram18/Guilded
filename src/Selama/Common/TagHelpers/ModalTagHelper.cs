using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Selama.Common.TagHelpers
{
    [HtmlTargetElement("modal", Attributes = MODAL_TITLE_ATTRIBUTE)]
    [RestrictChildren("modal-body", "modal-footer")]
    public class ModalTagHelper : TagHelper
    {
        #region Properties
        #region Public properties
        public const string MODAL_TITLE_ATTRIBUTE = "bs-title";

        [HtmlAttributeName(MODAL_TITLE_ATTRIBUTE)]
        public string Title { get; set; }
        #endregion

        #region Private properties
        private ModalContext ModalContext { get; set; }
        #endregion
        #endregion

        #region Methods
        #region Public methods
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ModalContext = new ModalContext();
            context.Items.Add(typeof(ModalTagHelper), ModalContext);

            await output.GetChildContentAsync();

            output.TagName = "div";
            output.Attributes.SetAttribute("role", "dialog");

            ForceModalAndFadeCssClassesOnTag(output);

            AppendHtmlToOutput(context, output);
        }
        #endregion

        #region Private methods
        private void ForceModalAndFadeCssClassesOnTag(TagHelperOutput output)
        {
            string classNames = "modal fade";
            if (output.Attributes.ContainsName("class"))
            {
                // Preserve the current set of classes, but guarantee modal and fade are included
                classNames = string.Format("{0} {1}", classNames, output.Attributes["class"].Value.ToString());
            }
            output.Attributes.SetAttribute("class", classNames);
        }

        private void AppendHtmlToOutput(TagHelperContext context, TagHelperOutput output)
        {
            var template =
                $@"<div class='modal-dialog' role='document'>
    <div class='modal-content'>
        <div class='modal-header'>
            <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                <span aria-hidden='true'>&times;</span>
            </button>
            <h4 class='modal-title' id='{context.UniqueId}Title'>{Title}</h4>
        </div>";

            output.Content.AppendHtml(template);
            AppendModalBody(output);
            AppendModalFooter(output);

            // closes the template's modal-content and modal-dialog tags
            output.Content.AppendHtml("</div></div>");
        }
        private void AppendModalBody(TagHelperOutput output)
        {
            output.Content.AppendHtml("<div class='modal-body'>");
            if (ModalContext.Body != null)
            {
                output.Content.AppendHtml(ModalContext.Body);
            }
            output.Content.AppendHtml("</div>");
        }
        private void AppendModalFooter(TagHelperOutput output)
        {
            if (ModalContext.Footer != null)
            {
                output.Content.AppendHtml("<div class='modal-footer'>");
                output.Content.AppendHtml(ModalContext.Footer);
                output.Content.AppendHtml("</div>");
            }
        }
        #endregion
        #endregion
    }

    #region Helper classes
    public class ModalContext
    {
        public IHtmlContent Body { get; set; }
        public IHtmlContent Footer { get; set; }
    }

    [HtmlTargetElement("modal-body", ParentTag = "modal")]
    public class ModalBodyTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var bodyContent = output.GetChildContentAsync();
            ModalContext modalContext = context.Items[typeof(ModalTagHelper)] as ModalContext;
            modalContext.Body = await bodyContent;
            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("modal-footer", ParentTag = "modal")]
    public class ModalFooterTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var footerContent = output.GetChildContentAsync();
            ModalContext modalContext = context.Items[typeof(ModalTagHelper)] as ModalContext;
            modalContext.Footer = await footerContent;
            output.SuppressOutput();
        }
    }
    #endregion
}
