using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Structura.SharedComponents.Utilities;

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
