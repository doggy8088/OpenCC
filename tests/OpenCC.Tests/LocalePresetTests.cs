using Xunit;

namespace OpenCC.Tests;

public class LocalePresetTests
{
    [Fact]
    public void Constructor_NullFrom_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new LocalePreset(null!, new Dictionary<string, DictGroup>()));
    }

    [Fact]
    public void Constructor_NullTo_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new LocalePreset(new Dictionary<string, DictGroup>(), null!));
    }

    [Fact]
    public void Properties_AreAssigned()
    {
        var from = new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["cn"] = DictGroup.FromStrings("a b"),
        };
        var to = new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["tw"] = DictGroup.FromStrings("c d"),
        };

        var preset = new LocalePreset(from, to);

        Assert.Same(from, preset.From);
        Assert.Same(to, preset.To);
    }
}