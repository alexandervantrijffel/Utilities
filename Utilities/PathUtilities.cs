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

        /// <summary>
        /// If the given fileName exists, (number) will be added to the file title to find a unique filename.
        /// e.g. myfile.zip becomes myfile(1).zip
        /// </summary>
        public static string GenerateUniqueFilePath(string directoryName, string fileName)
        {
            var destination = Path.Combine(directoryName, fileName);
            if (!File.Exists(destination)) return destination;
            return GenerateUniqueFilePath(
                Path.Combine(directoryName,
                    $"{Path.GetFileNameWithoutExtension(fileName)}({{0}}){Path.GetExtension(fileName)}"));
        }

        private static string GenerateUniqueFilePath(string filePath, int startingNumber = 1)
        {
            var destination = string.Format(filePath, startingNumber);
            return !File.Exists(destination)
                ? destination
                : GenerateUniqueFilePath(filePath, ++startingNumber);
        }

        public static string RemoveIllegalFileNameCharacters(string input)
        {
            return Regex.Replace(input, "[<>:\"/\\\\\\|\\?\\*]", "");
        }
    }
}
