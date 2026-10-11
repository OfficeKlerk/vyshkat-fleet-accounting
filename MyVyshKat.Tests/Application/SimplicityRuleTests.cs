using MyVyshKat.Application;
using MyVyshKat.Tests.Fakes;

namespace MyVyshKat.Tests.Application;

public class SimplicityRuleTests
{
    [Theory]
    [InlineData(1, false)]
    [InlineData(5, false)]
    [InlineData(6, true)]
    [InlineData(10, true)]
    public void BaseRule_SimplicityAtLeastSix(int simplicity, bool expected)
    {
        var rule = new SimplicityRule(6);

        Assert.Equal(expected, rule.IsSuitable(Samples.Scooter(simplicity: simplicity)));
    }

    [Fact]
    public void Description_ShowsThreshold()
    {
        Assert.Equal("Simplicity >= 8", new SimplicityRule(8).Description);
    }
}
