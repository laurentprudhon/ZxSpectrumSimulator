using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICSharpCode.AvalonEdit.Rendering;
using System.Windows.Media.TextFormatting;
using System.Windows;
using System.Windows.Input;
using Z80Simulator.Assembly;

namespace ZXCodeEditor
{/*
    class VisualLineAddressLink  : VisualLineText
    {        
        public int? TargetAddress { get; set; }
               
        /// <summary>
        /// Gets/Sets whether the user needs to press Control to click the link.
        /// The default value is true.
        /// </summary>
        public bool RequireControlModifierForClick { get; set; }

        AsmCodeEditor codeEditor;

        /// <summary>
        /// Creates a visual line text element with the specified length.
        /// It uses the <see cref="ITextRunConstructionContext.VisualLine"/> and its
        /// <see cref="VisualLineElement.RelativeTextOffset"/> to find the actual text string.
        /// </summary>
        public VisualLineAddressLink(VisualLine parentVisualLine, int length, AsmCodeEditor codeEditor)
            : base(parentVisualLine, length)
        {
            this.RequireControlModifierForClick = true;
            this.codeEditor = codeEditor;
        }

        /// <inheritdoc/>
        public override TextRun CreateTextRun(int startVisualColumn, ITextRunConstructionContext context)
        {
            //this.TextRunProperties.SetForegroundBrush(context.TextView.LinkTextForegroundBrush);
            //this.TextRunProperties.SetBackgroundBrush(context.TextView.LinkTextBackgroundBrush);
            this.TextRunProperties.SetTextDecorations(TextDecorations.Underline);
            return base.CreateTextRun(startVisualColumn, context);
        }

        /// <summary>
        /// Gets whether the link is currently clickable.
        /// </summary>
        /// <remarks>Returns true when control is pressed; or when
        /// <see cref="RequireControlModifierForClick"/> is disabled.</remarks>
        protected bool LinkIsClickable()
        {
            if (TargetAddress == null)
                return false;
            if (RequireControlModifierForClick)
                return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            else
                return true;
        }

        protected internal override void OnQueryCursor(QueryCursorEventArgs e)
        {
            if (LinkIsClickable())
            {
                e.Handled = true;
                e.Cursor = Cursors.Hand;
            }
        }

        protected internal override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && !e.Handled && LinkIsClickable() && TargetAddress.HasValue)
            {
                if (codeEditor.CurrentProgram != null)
                {
                    ProgramLine targetPrgLine = codeEditor.CurrentProgram.GetLineFromAddress(TargetAddress.Value);
                    if (targetPrgLine != null)
                    {
                        codeEditor.TextEditor.ScrollToLine(targetPrgLine.LineNumber - 1);
                    }
                }
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override VisualLineText CreateInstance(int length)
        {
            return new VisualLineAddressLink(ParentVisualLine, length, codeEditor)
            {
                TargetAddress = this.TargetAddress,
                RequireControlModifierForClick = this.RequireControlModifierForClick
            };
        }
    }*/
}
