using Microsoft.AspNetCore.Html;

namespace Guilded.Services
{
    public interface IMarkdownConverter
    {
        IHtmlContent Convert(string content);
    }
}
