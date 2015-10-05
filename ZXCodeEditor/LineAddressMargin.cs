using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Utils;
using ICSharpCode.AvalonEdit.Editing;
using Z80Simulator.Assembly;

namespace ZXCodeEditor
{
    /// <summary>
    /// Margin showing line numbers.
    /// </summary>
    public class LineAddressMargin : AbstractMargin
    {
        static LineAddressMargin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineAddressMargin),
                                                     new FrameworkPropertyMetadata(typeof(LineAddressMargin)));
        }

        AsmCodeEditor codeEditor;

        int maxLineAddressLength = 4;

        public LineAddressMargin(AsmCodeEditor codeEditor)
        {
            this.codeEditor = codeEditor;
        }


        Typeface typeface;
        double emSize;

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            typeface = this.CreateTypeface();
            emSize = (double)GetValue(TextBlock.FontSizeProperty);

            FormattedText text = CreateFormattedText(
                new string('9', maxLineAddressLength),
                typeface,
                emSize,
                (Brush)GetValue(Control.ForegroundProperty)
            );
            return new Size(text.Width, 0);
        }

        private FormattedText CreateFormattedText(string text, Typeface typeface, double? emSize, Brush foreground)
		{			
			if (text == null)
				throw new ArgumentNullException("text");
			if (typeface == null)
				typeface = CreateTypeface();
			if (emSize == null)
				emSize = TextBlock.GetFontSize(this);
			if (foreground == null)
				foreground = TextBlock.GetForeground(this);
			return new FormattedText(
				text,
				CultureInfo.CurrentCulture,
				FlowDirection.LeftToRight,
				typeface,
				emSize.Value,
				foreground,
				null,
				TextOptions.GetTextFormattingMode(this)
			);
        }

        private Typeface CreateTypeface()
        {
            return new Typeface((FontFamily)GetValue(TextBlock.FontFamilyProperty),
                                (FontStyle)GetValue(TextBlock.FontStyleProperty),
                                (FontWeight)GetValue(TextBlock.FontWeightProperty),
                                (FontStretch)GetValue(TextBlock.FontStretchProperty));
        }
        
        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            TextView textView = this.TextView;
            Size renderSize = this.RenderSize;
            if (textView != null && textView.VisualLinesValid && codeEditor.CurrentProgram != null)
            {
                var foreground = (Brush)GetValue(Control.ForegroundProperty);
                foreach (VisualLine line in textView.VisualLines)
                {
                    int lineNumber = line.FirstDocumentLine.LineNumber;
                    if (codeEditor.CurrentProgram.Lines.Count >= lineNumber)
                    {
                        ProgramLine prgLine = codeEditor.CurrentProgram.Lines[lineNumber - 1];
                        if (prgLine.Type == ProgramLineType.OpCodeInstruction ||
                        (prgLine.Type == ProgramLineType.AssemblerDirective && prgLine.DirectiveName.StartsWith("DEF")))
                        {
                            string hexAddress = prgLine.LineAddress.ToString("X4");

                            FormattedText text = CreateFormattedText(
                                hexAddress,
                                typeface, emSize, foreground
                            );
                            double y = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextTop);
                            drawingContext.DrawText(text, new Point(renderSize.Width - text.Width, y - textView.VerticalOffset));
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnTextViewChanged(TextView oldTextView, TextView newTextView)
        {
            if (oldTextView != null)
            {
                oldTextView.VisualLinesChanged -= TextViewVisualLinesChanged;
            }
            base.OnTextViewChanged(oldTextView, newTextView);
            if (newTextView != null)
            {
                newTextView.VisualLinesChanged += TextViewVisualLinesChanged;
            }
            InvalidateVisual();
        }

        void TextViewVisualLinesChanged(object sender, EventArgs e)
        {
            InvalidateVisual();
        }
    }
}
