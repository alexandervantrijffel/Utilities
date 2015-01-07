using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Structura.SharedComponents.Utilities
{
    public interface IInitializeAtStartup
    {
        void Initialize();
    }
    /// <summary>
    /// Executes the initialize method on all implementations of IInitializeAtStartup in the provided assemblies
    /// </summary>
    public class InitializeAtStartupBootstrapper
    {
        public static void Execute(IEnumerable<Assembly> applicationAssemblies)
        {
            foreach(var a in applicationAssemblies)
            {
                var startups = a.GetTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(IInitializeAtStartup)));
                foreach (var startup in startups)
                    ((IInitializeAtStartup)Activator.CreateInstance(startup)).Initialize();
            }
        }

        public static void Execute(Func<Assembly[],IEnumerable<Assembly>> applicationAssembliesFilter)
        {
            Execute(applicationAssembliesFilter(AppDomain.CurrentDomain.GetAssemblies()));
        }
    }
}
