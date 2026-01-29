using OpenCC.Internal;

using Xunit;

namespace OpenCC.Tests;

public class InternalDataTests
{
    [Fact]
    public void DictData_ContainsEntries()
    {
        Assert.False(string.IsNullOrEmpty(DictData.STCharacters));
        Assert.False(string.IsNullOrEmpty(DictData.TSCharacters));
        Assert.False(string.IsNullOrEmpty(DictData.TWVariants));
        Assert.False(string.IsNullOrEmpty(DictData.TGCharacters));
        Assert.False(string.IsNullOrEmpty(DictData.TGPhrases));
    }

    [Fact]
    public void LocaleData_MapsContainExpectedKeys()
    {
        Assert.True(LocaleData.FromMap.ContainsKey("cn"));
        Assert.True(LocaleData.FromMap.ContainsKey("hk"));
        Assert.True(LocaleData.FromMap.ContainsKey("tw2"));
        Assert.True(LocaleData.FromMap.ContainsKey("gov"));
        Assert.True(LocaleData.ToMap.ContainsKey("cn"));
        Assert.True(LocaleData.ToMap.ContainsKey("tw"));
        Assert.True(LocaleData.ToMap.ContainsKey("tw2"));
        Assert.True(LocaleData.ToMap.ContainsKey("gov"));
    }

    [Fact]
    public void LocalePresets_Full_HasMappings()
    {
        Assert.NotNull(LocalePresets.Full);
        Assert.True(LocalePresets.Full.From.Count > 0);
        Assert.True(LocalePresets.Full.To.Count > 0);
    }

    [Fact]
    public void LocalePresets_Cn2t_UsesCnAsFrom()
    {
        Assert.True(LocalePresets.Cn2t.From.ContainsKey("cn"));
        Assert.False(LocalePresets.Cn2t.From.ContainsKey("tw"));
    }

    [Fact]
    public void LocalePresets_Cn2t_IncludesTw2AsTo()
    {
        Assert.True(LocalePresets.Cn2t.To.ContainsKey("tw2"));
    }

    [Fact]
    public void LocalePresets_T2cn_UsesCnAsTo()
    {
        Assert.True(LocalePresets.T2cn.To.ContainsKey("cn"));
        Assert.False(LocalePresets.T2cn.To.ContainsKey("tw"));
    }

    [Fact]
    public void LocalePresets_T2cn_IncludesTw2AsFrom()
    {
        Assert.True(LocalePresets.T2cn.From.ContainsKey("tw2"));
    }
}
