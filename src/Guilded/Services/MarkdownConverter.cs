using CommonMark;
using Microsoft.AspNetCore.Html;

namespace Guilded.Services
{
    public class MarkdownConverter : IMarkdownConverter
    {
        public IHtmlContent Convert(string content) => 
            new HtmlString(CommonMarkConverter.Convert(content));
    }
}
