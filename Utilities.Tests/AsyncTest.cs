using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Structura.Shared.Utilities.Tests
{
    public class AsyncTest
    {
        [Fact]
        public async Task ShouldReportTimeout()
        {
            var task = Task.Delay(500);
            var completed = await AsyncHelper.RunWithTimeout(task, TimeSpan.FromMilliseconds(50));
            completed.ShouldBe(false);
        }

        [Fact]
        public async Task ShouldReportCompleted()
        {
            var task = Task.Delay(50);
            var completed = await AsyncHelper.RunWithTimeout(task, TimeSpan.FromMilliseconds(500));
            completed.ShouldBe(true);
        }
    }
}
