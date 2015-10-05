using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using ICSharpCode.AvalonEdit.Search;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Document;
using Z80Simulator.Assembly;
using System.Text;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Editing;

namespace ZXCodeEditor
{
	/// <summary>
	/// Interaction logic for ZXCodeEditor.xaml
	/// </summary>
	public partial class AsmCodeEditor : Window
	{
        public AsmCodeEditor()
        {
            InitializeComponent();

            TextView textView = textEditor.TextArea.TextView;

            // Line numbers
            textEditor.ShowLineNumbers = true;

            // Line addresses
            LineAddressMargin lineAddresses = new LineAddressMargin(this);
            lineAddresses.SetValue(Control.ForegroundProperty, System.Windows.Media.Brushes.LightBlue);
            textEditor.TextArea.LeftMargins.Add(lineAddresses);
            Line verticalSeparator = (Line)DottedLineMargin.Create();
            verticalSeparator.Stroke = System.Windows.Media.Brushes.LightBlue;
            textEditor.TextArea.LeftMargins.Add(verticalSeparator);

            // Assembly syntax highlighting
            textView.LineTransformers.Add(new AsmSyntaxHighlighting());

            // Search box
            textEditor.TextArea.DefaultInputHandler.NestedInputHandlers.Add(new SearchInputHandler(textEditor.TextArea));

            // Compilation errors
            textMarkerService = new TextMarkerService(textEditor);
            textView.BackgroundRenderers.Add(textMarkerService);
            textView.LineTransformers.Add(textMarkerService);
            textView.MouseHover += MouseHover;
            textView.MouseHoverStopped += TextEditorMouseHoverStopped;
            textView.VisualLinesChanged += VisualLinesChanged;

            // Incremental compilation
            textEditor.Document.Changed += Document_Changed;
        }

        void Document_Changed(object sender, EventArgs e)
        {
            DocumentChangeEventArgs chgEvtArgs = (DocumentChangeEventArgs)e;

        }

        public ICSharpCode.AvalonEdit.TextEditor TextEditor 
        {
            get { return textEditor; }
        }

        #region Load / Save source file

        string currentFileName;
        		
		void openFileClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.CheckFileExists = true;
			if (dlg.ShowDialog() ?? false) {
				currentFileName = dlg.FileName;
				textEditor.Load(currentFileName);
                Compile(this, null);
			}
		}

        void saveFileClick(object sender, EventArgs e)
        {
            if (currentFileName == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".asm";
                if (dlg.ShowDialog() ?? false)
                {
                    currentFileName = dlg.FileName;
                }
                else
                {
                    return;
                }
            }
            textEditor.Save(currentFileName);
        }

        #endregion

        #region Compile program

        private static readonly ICommand compileCommand = new RoutedUICommand("Compile assembly code", "Compile", typeof(AsmCodeEditor),
            new InputGestureCollection { new KeyGesture(Key.Space, ModifierKeys.Control) });

        public static ICommand CompileCommand
        {
            get { return compileCommand; }
        }
                
        public Program CurrentProgram { get; set; }
        
        public void Compile(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentProgram = Assembler.ParseProgram(currentFileName, new MemoryStream(Encoding.Default.GetBytes(textEditor.Document.Text)), Encoding.Default, true);
            MemoryMap memoryMap = new MemoryMap(65536); 
            Assembler.CompileProgram(CurrentProgram, 0, memoryMap);
            DisplayErrors(CurrentProgram.LinesWithErrors);
        }

        #endregion

        #region Show errors

        private readonly TextMarkerService textMarkerService;

        public class ErrorDescription
        {
            public int LineNumber { get; set; }
            public int LineAddress { get; set; }
            public int StartColumnIndex { get; set; }
            public int EndColumnIndex { get; set; }
            public string ErrorMessage { get; set; }
        }
        
        private void DisplayErrors(IList<ProgramLine> errorLines)
        {
            IList<ErrorDescription> errors = new List<ErrorDescription>();
            textMarkerService.Clear();
            
            foreach (ProgramLine errorLine in errorLines)
            {
                ErrorDescription error = new ErrorDescription()
                {
                    LineNumber = errorLine.LineNumber,
                    LineAddress = errorLine.LineAddress,
                    StartColumnIndex = errorLine.Error.StartColumnIndex,
                    EndColumnIndex = errorLine.Error.EndColumnIndex,
                    ErrorMessage = errorLine.Error.ErrorMessage
                };
                errors.Add(error);

                int offset = textEditor.Document.GetOffset(new TextLocation(error.LineNumber, error.StartColumnIndex));
                int endOffset = textEditor.Document.GetOffset(new TextLocation(error.LineNumber, error.EndColumnIndex));
                int length = endOffset - offset;
                if (length < 2)
                {
                    length = Math.Min(2, textEditor.Document.TextLength - offset);
                }
                textMarkerService.Create(offset, length, error.ErrorMessage);                
            }

            errorsListView.ItemsSource = errors;
        }


        private ToolTip toolTip;

        private void MouseHover(object sender, MouseEventArgs e)
        {
            var pos = textEditor.TextArea.TextView.GetPositionFloor(e.GetPosition(textEditor.TextArea.TextView) + textEditor.TextArea.TextView.ScrollOffset);
            bool inDocument = pos.HasValue;
            if (inDocument)
            {
                TextLocation logicalPosition = pos.Value.Location;
                int offset = textEditor.Document.GetOffset(logicalPosition);

                var markersAtOffset = textMarkerService.GetMarkersAtOffset(offset);
                TextMarkerService.TextMarker markerWithToolTip = markersAtOffset.FirstOrDefault(marker => marker.ToolTip != null);

                if (markerWithToolTip != null)
                {
                    if (toolTip == null)
                    {
                        toolTip = new ToolTip();
                        toolTip.Closed += ToolTipClosed;
                        toolTip.PlacementTarget = this;
                        toolTip.Content = new TextBlock
                        {
                            Text = markerWithToolTip.ToolTip,
                            TextWrapping = TextWrapping.Wrap
                        };
                        toolTip.IsOpen = true;
                        e.Handled = true;
                    }
                }
            }
        }

        void ToolTipClosed(object sender, RoutedEventArgs e)
        {
            toolTip = null;
        }

        void TextEditorMouseHoverStopped(object sender, MouseEventArgs e)
        {
            if (toolTip != null)
            {
                toolTip.IsOpen = false;
                e.Handled = true;
            }
        }

        private void VisualLinesChanged(object sender, EventArgs e)
        {
            if (toolTip != null)
            {
                toolTip.IsOpen = false;
            }
        }
        
        private void errorsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ErrorDescription error = ((FrameworkElement)e.OriginalSource).DataContext as ErrorDescription;
            if (error != null)
            {
                textEditor.ScrollToLine(error.LineNumber - 1);
                DocumentLine line = textEditor.Document.Lines[error.LineNumber - 1];
                textEditor.Select(line.Offset, line.Length);
            }
        }

        #endregion

    }
}