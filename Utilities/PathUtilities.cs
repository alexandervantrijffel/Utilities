using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Structura.Shared.Utilities
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

        public static string GenerateUniqueFileName(string directory, string filespec)
        {
            Check.Require(filespec.Contains("{0}"), "Filespec must contain {0}");
            var fileName = string.Format(filespec, Randomizer.NextInt(1, Int32.MaxValue));
            return !File.Exists(Path.Combine(directory, fileName)) ? fileName : GenerateUniqueFileName(directory, filespec);
        }

        public static string RemoveIllegalFileNameCharacters(string input)
        {
            return Regex.Replace(input, "[<>:\"/\\\\\\|\\?\\*]", "");
        }
    }
}
