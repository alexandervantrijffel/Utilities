using System;
using System.IO;
using System.Reflection;

namespace Structura.SharedComponents.Utilities.Tests
{
    public static class CurrentDirectoryForUnitTests
    {
        public static string DirectoryName => new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
    }
}