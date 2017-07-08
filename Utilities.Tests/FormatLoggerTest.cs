using Xunit;
using Xunit.Abstractions;

namespace Structura.Shared.Utilities.Tests
{
    public class FormatLoggerTest : IClassFixture<FormatLoggerTestFixture>
    {
        public FormatLoggerTest(ITestOutputHelper outputHelper, FormatLoggerTestFixture formatLoggerTestFixture)
        {
            formatLoggerTestFixture.TestOutputHelper = outputHelper;
        }

        [Fact]
        public void Should_show_output_in_test_run()
        {
            FormatLoggerAccessor.Locate().Error($"An error");
        }
    }
}