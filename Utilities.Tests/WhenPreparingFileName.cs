using Shouldly;
using Xunit;

namespace Structura.Shared.Utilities.Tests
{
    public class WhenPreparingFileName
    {
        [Fact]
        public void AllIllegalCharactersAreRemoved()
        {
            PathUtilities
                .RemoveIllegalFileNameCharacters("<t>h:i\"s/\\|?*")
                .ShouldBe("this");
        }
    }
}
