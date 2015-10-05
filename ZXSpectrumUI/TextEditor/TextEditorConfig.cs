using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ZXSpectrumUI.TextEditor
{
    public static class TextEditorConfig
    {
        /// <summary>
        /// If file "foo.txt" is stored in project subdirectory "bar",
        /// relativeFilePath input parameter should be "bar/foo.txt"
        /// </summary>
        public static Stream GetStreamForProjectFile(string relativeFilePath)
        {
            string resourcePath = String.Format("ZXSpectrumUI.TextEditor.{0}", relativeFilePath.Replace('/', '.'));
            Stream resourceStream = typeof(TextEditorConfig).GetTypeInfo().Assembly.GetManifestResourceStream(resourcePath);
            return resourceStream;
        }

        public static IHighlightingDefinition LoadAsmHighlightingDefinition()
        {
            using (var stream = GetStreamForProjectFile("AsmHighlighting.xshd"))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    return HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }

    }
}
