using Microsoft.AspNetCore.Html;

namespace Guilded.Services
{
    public interface IConvertMarkdown
    {
        IHtmlContent Convert(string content);

        string ConvertAndStripHtml(string content);
    }
}
