using Xunit;

namespace OpenCC.Tests;

public class DictEntryTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var entry = new DictEntry("a", "b");

        Assert.Equal("a", entry.Source);
        Assert.Equal("b", entry.Target);
    }

    [Fact]
    public void Equality_UsesValueSemantics()
    {
        var left = new DictEntry("a", "b");
        var right = new DictEntry("a", "b");

        Assert.Equal(left, right);
        Assert.True(left == right);
    }
}