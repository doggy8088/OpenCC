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
    }

    [Fact]
    public void LocaleData_MapsContainExpectedKeys()
    {
        Assert.True(LocaleData.FromMap.ContainsKey("cn"));
        Assert.True(LocaleData.FromMap.ContainsKey("hk"));
        Assert.True(LocaleData.FromMap.ContainsKey("tw2"));
        Assert.True(LocaleData.ToMap.ContainsKey("cn"));
        Assert.True(LocaleData.ToMap.ContainsKey("tw"));
        Assert.True(LocaleData.ToMap.ContainsKey("tw2"));
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

    [Fact]
    public void TW2Tai_EveryProperNoun_StaysIntactInContext()
    {
        var properNouns = DictData.TW2Tai
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(line => line.Split('\t')[1])
            .Where(value => value != "台")
            .Distinct()
            .ToList();
        var cnToTw2 = OpenCC.Converter("cn", "tw2");
        var tw2ToCn = OpenCC.Converter("tw2", "cn");
        var twToTw2 = OpenCC.Converter("tw", "tw2");
        var tw2ToTw2 = OpenCC.Converter("tw2", "tw2");

        Assert.NotEmpty(properNouns);
        foreach (var noun in properNouns)
        {
            var text = $"在{noun}前";
            Assert.Equal(text, cnToTw2(tw2ToCn(text)));
            Assert.Equal(text, twToTw2(text));
            Assert.Equal(text, tw2ToTw2(text));
        }
    }
}