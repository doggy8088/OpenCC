using Xunit;

namespace OpenCC.Tests;

public class GovConversionTests
{
    [Fact]
    public void Converter_T2Gov_NormalizesTraditionalVariants()
    {
        var converter = OpenCC.Converter("t", "gov");

        Assert.Equal("啓用裏面净化僞", converter("啟用裡面淨化偽"));
    }

    [Fact]
    public void Converter_T2Gov_HandlesSimplifiedInsideTraditional()
    {
        var converter = OpenCC.Converter("t", "gov");

        Assert.Equal("啓緑", converter("启绿"));
    }

    [Fact]
    public void Converter_Cn2Gov_ConvertsToGovernmentStandardTraditional()
    {
        var converter = OpenCC.Converter("cn", "gov");

        Assert.Equal("啓緑", converter("启绿"));
    }
}

