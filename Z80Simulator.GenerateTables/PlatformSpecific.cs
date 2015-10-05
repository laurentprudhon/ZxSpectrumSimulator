using System;
using System.IO;
using System.Reflection;

namespace Z80Simulator.GenerateTables
{
    public static class PlatformSpecific
    {
        /// <summary>
        /// If file "foo.txt" is stored in project subdirectory "bar",
        /// relativeFilePath input parameter should be "bar/foo.txt"
        /// </summary>
        public static Stream GetStreamForProjectFile(string relativeFilePath)
        {
            string resourcePath = String.Format("Z80Simulator.GenerateTables.{0}", relativeFilePath.Replace('/', '.'));
#if NETFX_CORE
            Stream resourceStream = typeof(PlatformSpecific).GetTypeInfo().Assembly.GetManifestResourceStream(resourcePath);
#else
            Stream resourceStream = typeof(PlatformSpecific).Assembly.GetManifestResourceStream(resourcePath);
#endif
            return resourceStream;
        }
    }
}
