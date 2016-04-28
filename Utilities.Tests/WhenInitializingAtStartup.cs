using System.Reflection;
using FluentAssertions;
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
            // Arrange
            // Act
            InitializeAtStartupBootstrapper.Execute(new []{Assembly.GetExecutingAssembly()});

            // Assert
            InitializeStub.Initialized.Should().BeTrue();
        }
    }
}
