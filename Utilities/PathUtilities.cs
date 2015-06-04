using System;
using System.IO;
using System.Reflection;

namespace Structura.SharedComponents.Utilities
{
    public static class PathUtilities
    {
        public static string GetFilePathRelativeToAssembly()
        {
            return GetFilePathRelativeToAssembly("");
        }

        public static string GetFilePathRelativeToAssembly(string relativePath)
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return Path.Combine(dirPath, relativePath);
        }
    }
}
