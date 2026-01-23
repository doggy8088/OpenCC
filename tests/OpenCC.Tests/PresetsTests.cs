using OpenCC.Presets;

using Xunit;

namespace OpenCC.Tests;

public class PresetsTests
{
    [Fact]
    public void Full_Locale_IsAvailable()
    {
        Assert.NotNull(Full.Locale);
        Assert.True(Full.Locale.From.Count > 0);
        Assert.True(Full.Locale.To.Count > 0);
    }

    [Fact]
    public void Cn2t_Locale_IsAvailable()
    {
        Assert.NotNull(Cn2t.Locale);
        Assert.True(Cn2t.Locale.From.ContainsKey("cn"));
    }

    [Fact]
    public void T2cn_Locale_IsAvailable()
    {
        Assert.NotNull(T2cn.Locale);
        Assert.True(T2cn.Locale.To.ContainsKey("cn"));
    }

    [Fact]
    public void Full_Converter_NullOptions_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Full.Converter((ConverterOptions)null!));
    }

    [Fact]
    public void Cn2t_Converter_NullOptions_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Cn2t.Converter((ConverterOptions)null!));
    }

    [Fact]
    public void T2cn_Converter_NullOptions_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => T2cn.Converter((ConverterOptions)null!));
    }
}