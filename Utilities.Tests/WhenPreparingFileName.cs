using FluentAssertions;
using Xunit;

namespace Structura.SharedComponents.Utilities.Tests
{
    public class WhenPreparingFileName
    {
        [Fact]
        public void AllIllegalCharactersAreRemoved()
        {
            // Arrange
            var input = "<t>h:i\"s/\\|?*";

            // Act
            var result = input.RemoveIllegalFileNameCharacters();
            
            // Assert
            result.Should().Be("this", "not expected result string: " + result);
        }
    }
}
