using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Guilded.Extensions
{
    public static class IHtmlHelperExtensions
    {
        public static Task ValidationScripts(this IHtmlHelper html)
        {
            return html.RenderPartialAsync("ValidationScripts");
        }
    }
}
