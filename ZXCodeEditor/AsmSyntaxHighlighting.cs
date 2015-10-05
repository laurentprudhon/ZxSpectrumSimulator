using System;
using System.Windows;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Z80Simulator.Assembly;

namespace ZXCodeEditor
{
	/// <summary>
	/// Highlights Z80 assembly code
	/// </summary>
	public class AsmSyntaxHighlighting : DocumentColorizingTransformer
	{
		protected override void ColorizeLine(DocumentLine line)
		{
			int lineStartOffset = line.Offset;
			string text = CurrentContext.Document.GetText(line);

            try
            {
                ProgramLine asmLine = Assembler.ParseProgramLine(text);
                foreach (AsmToken token in asmLine.AllTokens)
                {
                    base.ChangeLinePart(
                        lineStartOffset + token.StartIndex, // startOffset
                        lineStartOffset + token.EndIndex, // endOffset
                        (VisualLineElement element) =>
                        {
                            switch (token.Type)
                            {
                                case AsmTokenType.COLON:
                                case AsmTokenType.COMMA:
                                case AsmTokenType.OPENINGPAR:
                                case AsmTokenType.CLOSINGPAR:
                                case AsmTokenType.OPENINGBRA:
                                case AsmTokenType.CLOSINGBRA:
                                    break;
                                case AsmTokenType.PLUS:
                                case AsmTokenType.MINUS:
                                case AsmTokenType.MULTIPLY:
                                    break;
                                case AsmTokenType.DOLLAR:
                                case AsmTokenType.SYMBOL:
                                    element.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Color.FromRgb(0x66, 0x00, 0x66)));
                                    break;
                                case AsmTokenType.COMMENT:
                                    element.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Color.FromRgb(0x44,0x88,0x44)));
                                    break;
                                case AsmTokenType.STRING:
                                    element.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Color.FromRgb(0x00,0x00,0xCC)));
                                    break;
                                case AsmTokenType.OPCODE:
                                    element.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Color.FromRgb(0xDD,0x00,0x00)));
                                    Typeface tf = element.TextRunProperties.Typeface;
						            element.TextRunProperties.SetTypeface(new Typeface(
							            tf.FontFamily,
							            FontStyles.Normal,
							            FontWeights.Bold,
							            tf.Stretch
						            ));
                                    break;
                                case AsmTokenType.REGISTER:
                                case AsmTokenType.FLAGS:
                                case AsmTokenType.REGISTER16:
                                    element.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Color.FromRgb(0x88,0x00,0x00)));
                                    break;
                                case AsmTokenType.FLAGCONDITION:
                                    break;
                                case AsmTokenType.DIRECTIVE:
                                    element.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Color.FromRgb(0xCC,0x00,0xCC)));
                                    break;
                                case AsmTokenType.NUMBER:
                                    element.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Color.FromRgb(0x00,0x00,0xCC)));
                                    break;
                                case AsmTokenType.BIT:
                                case AsmTokenType.INTERRUPTMODE:
                                    break;
                                case AsmTokenType.WHITESPACE:
                                    break;            
                            }
                        });
                }
            } catch {}
		}
	}
}
