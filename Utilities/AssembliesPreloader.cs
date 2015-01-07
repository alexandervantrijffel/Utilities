using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Structura.SharedComponents.Utilities
{
    public class AssembliesPreLoader
    {
        private IList<Assembly> _loadedAssemblies;
        public void PreLoad(string filter, IEnumerable<string> excludes)
        {
            _loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var privateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (Directory.Exists(privateBinPath)) //  for web apps
                PreLoadAssembliesFromPath(privateBinPath, filter, excludes);
            else
                PreLoadAssembliesFromPath(baseDirectory, filter, excludes);
        }

        private void PreLoadAssembliesFromPath(string path, string filter, IEnumerable<string> excludes)
        {
            var files = new DirectoryInfo(path).GetFiles(filter);
            foreach (var assemblyFile in files)
            {
                TestFile(excludes, assemblyFile);
            }
        }

        private void TestFile(IEnumerable<string> excludes, FileInfo assemblyFile)
        {
            foreach (var exclude in excludes)
            {
                var adj = exclude.Replace(".", @"\.").Replace("*", ".*");
                if (Regex.IsMatch(assemblyFile.FullName, adj))
                    return;
            }
            if (!_loadedAssemblies.Any(assembly =>
                        AssemblyName.ReferenceMatchesDefinition(AssemblyName.GetAssemblyName(assemblyFile.FullName),
                            assembly.GetName())))
                Assembly.LoadFrom(assemblyFile.FullName);
        }
    }
}
