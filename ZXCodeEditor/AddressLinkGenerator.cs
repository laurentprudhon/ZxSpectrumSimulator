using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace ZXCodeEditor
{
    /*
    public class AddressLinkGenerator : VisualLineElementGenerator
    {        
        AsmCodeEditor codeEditor;

        public AddressLinkGenerator(AsmCodeEditor codeEditor)
        {
            this.codeEditor = codeEditor;
        }

        ISegment GetAddressSegment(int startOffset)
        {
            CurrentContext.
            int endOffset = CurrentContext.VisualLine.FirstDocumentLine.EndOffset;
            StringSegment relevantText = CurrentContext.GetText(startOffset, endOffset - startOffset);
            Match m = linkRegex.Match(relevantText.Text, relevantText.Offset, relevantText.Count);
            matchOffset = m.Success ? m.Index - relevantText.Offset + startOffset : -1;
            return m;
        }

        /// <inheritdoc/>
        public override int GetFirstInterestedOffset(int startOffset)
        {
            int matchOffset;
            GetMatch(startOffset, out matchOffset);
            return matchOffset;
        }

        /// <inheritdoc/>
        public override VisualLineElement ConstructElement(int offset)
        {
            int matchOffset;
            Match m = GetMatch(offset, out matchOffset);
            if (m.Success && matchOffset == offset)
            {
                return ConstructElementFromMatch(m);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Constructs a VisualLineElement that replaces the matched text.
        /// The default implementation will create a <see cref="VisualLineLinkText"/>
        /// based on the URI provided by <see cref="GetUriFromMatch"/>.
        /// </summary>
        protected virtual VisualLineElement ConstructElementFromMatch(Match m)
        {
            Uri uri = GetUriFromMatch(m);
            if (uri == null)
                return null;
            VisualLineLinkText linkText = new VisualLineLinkText(CurrentContext.VisualLine, m.Length);
            linkText.NavigateUri = uri;
            linkText.RequireControlModifierForClick = this.RequireControlModifierForClick;
            return linkText;
        }

        /// <summary>
        /// Fetches the URI from the regex match. Returns null if the URI format is invalid.
        /// </summary>
        protected virtual Uri GetUriFromMatch(Match match)
        {
            string targetUrl = match.Value;
            if (targetUrl.StartsWith("www.", StringComparison.Ordinal))
                targetUrl = "http://" + targetUrl;
            if (Uri.IsWellFormedUriString(targetUrl, UriKind.Absolute))
                return new Uri(targetUrl);

            return null;
        }
    }*/
}
