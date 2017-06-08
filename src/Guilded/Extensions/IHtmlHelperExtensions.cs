using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Guilded.Extensions
{
    public static class IHtmlHelperExtensions
    {
        public static Task ValidationScripts(this IHtmlHelper html)
        {
            return html.RenderPartialAsync("ValidationScripts");
        }

        public static Task SuccessAndErrorMessages(this IHtmlHelper html)
        {
            return html.RenderPartialAsync("SuccessAndErrorMessages");
        }
    }
}
