using System;
using FluentAssertions;
using Structura.SharedComponents.Utilities;
using Xunit;

namespace Utilities.Tests
{
    public class WhenChecking
    {
        [Fact]
        public void Precondition_that_succeeds_should_not_throw_error()
        {
            // Arrange
            // Act
            Check.Require(true, "abc{0}", "d");
            // Assert
        }

        [Fact]
        public void Precondition_that_fails_should_throw_correct_error()
        {
            // Arrange
            Action a = () => Check.Require(false, "abc{0}", "d");

            // Act
            // Assert
            a.ShouldThrow<PreconditionException>()
                .Which.Message.StartsWith(
                    "abcd. At method Utilities.Tests.WhenChecking.<Precondition_that_fails_should_throw_correct_error>");
        }

        [Fact]
        public void Precondition_with_exception_type_that_fails_should_throw_argument_error()
        {
            // Arrange
            Action a = () => Check.Require<ArgumentException>(false, "abc{0}", "d");

            // Act
            // Assert
            a.ShouldThrow<ArgumentException>()
                .Which.Message.StartsWith(
                    "abcd. At method Utilities.Tests.WhenChecking.<Precondition_with_exception_type_that_fails_should_throw_argument_error>");
        }

        [Fact]
        public void Precondition_not_null_that_fails_should_throw_argument_error()
        {
            // Arrange
            Action a = () => Check.RequireNotNull(null, "abc{0}", "d");

            // Act
            // Assert
            a.ShouldThrow<PreconditionException>().Which.Message.StartsWith("abcd. At method Utilities.Tests.WhenChecking." +
                                                               "<Precondition_not_null_that_fails_should_throw_argument_error>");
        }

        [Fact]
        public void Precondition_not_null_with_exception_type_that_fails_should_throw_argument_error()
        {
            // Arrange
            Action a = () => Check.RequireNotNull<ArgumentException,WhenChecking>(null, "abc{0}", "d");

            // Act
            // Assert
            a.ShouldThrow<ArgumentException>().Which.Message.StartsWith(
                "abcd. Object of type 'Utilities.Tests.WhenChecking' cannot be null.. " +
                "At method Utilities.Tests.WhenChecking." +
                "<Precondition_not_null_with_exception_type_that_fails_should_throw_argument_error>");
        }

        [Fact]
        public void Assert_that_fails_should_throw_argument_error()
        {
            // Arrange
            Action a = () => Check.RequireNotNull<ArgumentException, WhenChecking>(null, "abc{0}", "d");

            // Act
            // Assert
            a.ShouldThrow<ArgumentException>().Which.Message.StartsWith(
                "abcd. Object of type 'Utilities.Tests.WhenChecking' cannot be null.. " +
                "At method Utilities.Tests.WhenChecking." +
                "<Precondition_not_null_with_exception_type_that_fails_should_throw_argument_error>");
        }
    }
}
