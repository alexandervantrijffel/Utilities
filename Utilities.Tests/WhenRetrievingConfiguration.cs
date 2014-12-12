using System;
using FluentAssertions;
using Structura.SharedComponents.Utilities;
using Xunit;

namespace Utilities.Tests
{
    public class WhenRetrievingConfiguration
    {
        [Fact]
        public void AppSetting_that_does_not_exist_throws_correct_error()
        {
            // Arrange
            Action a = () => ConfigurationHelper.Get<string>("DoesNotExist");
            
            // Act
            // Assert|
            a.ShouldThrow<PreconditionException>().WithMessage("AppSetting with name DoesNotExist not found. Please check the application configuration file.. At method Structura.SharedComponents.Utilities.ConfigurationHelper.Get.");
            
        }
    }
}
