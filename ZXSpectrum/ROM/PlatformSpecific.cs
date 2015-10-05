﻿using System;
using System.IO;
using System.Reflection;

namespace ZXSpectrum
{
    public static class PlatformSpecific
    {
        /// <summary>
        /// If file "foo.txt" is stored in project subdirectory "bar",
        /// relativeFilePath input parameter should be "bar/foo.txt"
        /// </summary>
        public static Stream GetStreamForProjectFile(string relativeFilePath)
        {
            string resourcePath = String.Format("ZXSpectrum.ROM.{0}", relativeFilePath.Replace('/', '.'));
            Stream resourceStream = typeof(PlatformSpecific).GetTypeInfo().Assembly.GetManifestResourceStream(resourcePath);
            return resourceStream;
        }
    }
}
