using Shouldly;
using Xunit;

namespace Structura.Shared.Utilities.Tests
{
    public class WhenRetrievingConfiguration
    {
        [Fact]
        public void AppSetting_that_does_not_exist_throws_correct_error()
        {
            Should
                .Throw<PreconditionException>(() => new SettingsRetriever().Get<string>("DoesNotExist"))
                .Message.StartsWith(
                    "AppSetting with name DoesNotExist not found. Please check the application configuration file.. At method ");
        }

        [Fact]
        public void Existing_appsetting_is_found()
        {
            new SettingsRetriever().Get<string>("DoesExist").ShouldBe("empty");
        }
    }
}
