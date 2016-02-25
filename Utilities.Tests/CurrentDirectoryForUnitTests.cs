using System;
using System.IO;
using System.Reflection;

namespace Structura.SharedComponents.Utilities.Tests
{
	[Serializable]
	public class EncryptData
	{
		public DateTime Date { get; set; }
		public string SecretText { get; set; }
	}

	public static class CurrentDirectoryForUnitTests
    {
        public static string DirectoryName => new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
    }
}