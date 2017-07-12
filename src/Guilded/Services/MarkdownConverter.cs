using System.Collections.Generic;
using System.IO;
using System.Linq;
using BracketPipe;
using CommonMark;
using CommonMark.Formatters;
using CommonMark.Syntax;
using Microsoft.AspNetCore.Html;
using HtmlString = Microsoft.AspNetCore.Html.HtmlString;

namespace Guilded.Services
{
    public class MarkdownConverter : IMarkdownConverter
    {
        static MarkdownConverter()
        {
            CommonMarkSettings.Default.OutputDelegate = (doc, output, settings) =>
                new BlockEmphasisAndStrongHtmlFormatter(output, settings).WriteDocument(doc);
        }

        private class BlockEmphasisAndStrongHtmlFormatter : HtmlFormatter
        {
            private readonly char[] _emphasisCharacters = { '*', '_' };
            private readonly Stack<InlineTag> _currentBlockTag = new Stack<InlineTag>();

            public BlockEmphasisAndStrongHtmlFormatter(TextWriter target, CommonMarkSettings settings) : base(target, settings)
            {
            }

            protected override void WriteInline(Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes)
            {
                if (inline.Tag == InlineTag.String && IsEmphasis(inline.LiteralContent))
                {
                    ignoreChildNodes = false;
                    inline.Tag = inline.LiteralContent.Length == 1 ? InlineTag.Emphasis : InlineTag.Strong;

                    if (!_currentBlockTag.Any() || _currentBlockTag.Peek() != inline.Tag)
                    {
                        OpenEmphasisTag(inline.Tag);
                    }
                    else
                    {
                        CloseEmphasisTag(inline.Tag);
                    }
                }
                else
                {
                    base.WriteInline(inline, isOpening, isClosing, out ignoreChildNodes);
                }
            }

            private void OpenEmphasisTag(InlineTag inlineTag)
            {
                switch (inlineTag)
                {
                    case InlineTag.Emphasis:
                        Write("<em>");
                        break;
                    case InlineTag.Strong:
                        Write("<strong>");
                        break;
                }
                _currentBlockTag.Push(inlineTag);
            }

            private void CloseEmphasisTag(InlineTag inlineTag)
            {
                switch (inlineTag)
                {
                    case InlineTag.Emphasis:
                        Write("</em>");
                        break;
                    case InlineTag.Strong:
                        Write("</strong>");
                        break;
                }
                _currentBlockTag.Pop();
            }

            private bool IsEmphasis(string content)
            {
                // Don't evaluate if length is outside the range [1,2]
                if (content.Length == 0 || content.Length > 2)
                {
                    return false;
                }

                for (var i = 0; i < content.Length; i++)
                {
                    if (!_emphasisCharacters.Contains(content[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public IHtmlContent Convert(string content)
        {
            var convertedMarkdown = CommonMarkConverter.Convert(content);
            using (var reader = new HtmlReader(convertedMarkdown))
            {
                 return new HtmlString(reader.Sanitize().Minify().ToHtml());
            }
        }
    }
}
