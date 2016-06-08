using System.Reflection;
using Shouldly;
using Xunit;

namespace Structura.Shared.Utilities.Tests
{
    public class InitializeStub : IInitializeAtStartup
    {
        public static bool Initialized;
        public void Initialize()
        {
            Initialized = true;
        }
    }

    public class WhenInitializingAtStartup
    {
        [Fact]
        public void ShouldExecuteInitialize()
        {
            InitializeAtStartupBootstrapper.Execute(new []{Assembly.GetExecutingAssembly()});
            InitializeStub.Initialized.ShouldBeTrue();
        }
    }
}
