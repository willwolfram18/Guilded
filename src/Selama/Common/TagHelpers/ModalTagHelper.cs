using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Common.TagHelpers
{
    [HtmlTargetElement("modal", Attributes = MODAL_TITLE_ATTRIBUTE)]
    [RestrictChildren("modal-body", "modal-footer")]
    public class ModalTagHelper : TagHelper
    {
        public const string MODAL_TITLE_ATTRIBUTE = "bs-title";

        [HtmlAttributeName(MODAL_TITLE_ATTRIBUTE)]
        public string Title { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ModalContext modalContext = new ModalContext();
            context.Items.Add(typeof(ModalTagHelper), modalContext);

            await output.GetChildContentAsync();

            var template =
                $@"<div class='modal-dialog' role='document'>
    <div class='modal-content'>
        <div class='modal-header'>
            <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                <span aria-hidden='true'>&times;</span>
            </button>
            <h4 class='modal-title' id='{context.UniqueId}Title'>{Title}</h4>
        </div>
        <div class='modal-body'>";

            output.TagName = "div";
            output.Attributes.SetAttribute("role", "dialog");

            string classNames = "modal fade";
            if (output.Attributes.ContainsName("class"))
            {
                classNames = string.Format("{0} {1}", classNames, output.Attributes["class"].Value.ToString());
            }

            output.Attributes.SetAttribute("class", classNames);
            output.Content.AppendHtml(template);
            if (modalContext.Body != null)
            {
                output.Content.AppendHtml(modalContext.Body);
            }
            output.Content.AppendHtml("</div>");
            if (modalContext.Footer != null)
            {
                output.Content.AppendHtml("<div class='modal-footer'>");
                output.Content.AppendHtml(modalContext.Footer);
                output.Content.AppendHtml("</div>");
            }

            output.Content.AppendHtml("</div></div>");
        }
    }

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
}
