using Xunit;

namespace OpenCC.Tests;

public class ConverterOptionsTests
{
    [Fact]
    public void DefaultConstructor_InitializesEmptyStrings()
    {
        var options = new ConverterOptions();

        Assert.Equal(string.Empty, options.From);
        Assert.Equal(string.Empty, options.To);
    }

    [Fact]
    public void Constructor_WithValues_SetsProperties()
    {
        var options = new ConverterOptions("cn", "tw");

        Assert.Equal("cn", options.From);
        Assert.Equal("tw", options.To);
    }
}