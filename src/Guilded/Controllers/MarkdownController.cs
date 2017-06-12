using Guilded.Services;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Controllers
{
    public class MarkdownController : Controller
    {
        private readonly IMarkdownConverter _markdown;

        public MarkdownController(IMarkdownConverter markdown)
        {
            _markdown = markdown;
        }

        [HttpPost]
        public IActionResult Index(string content)
        {
            return Ok(_markdown.Convert(content).ToString());
        }
    }
}
