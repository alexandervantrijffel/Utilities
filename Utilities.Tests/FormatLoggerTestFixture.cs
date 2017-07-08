using log4net;
using NSubstitute;
using NSubstitute.Core;
using Xunit.Abstractions;

namespace Structura.Shared.Utilities.Tests
{
    /// <summary>
    /// Sends all logged messages to XUnit's TestOutputHelper
    /// </summary>
    public class FormatLoggerTestFixture
    {
        public ITestOutputHelper TestOutputHelper { get; set; }
        public FormatLoggerTestFixture()
        {
            var mock = Substitute.For<ILog>();
            mock.When(x => x.Debug(Arg.Any<string>()))
                .Do(c =>
                {
                    LogOutput("debug", c);
                });

            mock.When(x => x.Info(Arg.Any<string>()))
                .Do(c =>
                {
                    LogOutput("info", c);
                });

            mock.When(x => x.Warn(Arg.Any<string>()))
                .Do(c =>
                {
                    LogOutput("warn", c);
                });

            mock.When(x => x.Error(Arg.Any<string>()))
                .Do(c =>
                {
                    LogOutput("error", c);
                });

            FormatLoggerAccessor.Initialize(() => mock);

            void LogOutput(string level, CallInfo callInfo)
            {
                TestOutputHelper?.WriteLine($"{level.ToUpper()}: {callInfo.Arg<string>()}");
            }
        }
    }
}