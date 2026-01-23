using Xunit;

namespace OpenCC.Tests;

public class OpenCCTests
{
    [Fact]
    public void ConverterBuilder_NullLocalePreset_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.ConverterBuilder(null!));
    }

    [Fact]
    public void ConverterBuilder_NullOptions_Throws()
    {
        var builder = OpenCC.ConverterBuilder(CreatePreset());

        Assert.Throws<ArgumentNullException>(() => builder(null!));
    }

    [Fact]
    public void ConverterBuilder_ConvertsSequentially()
    {
        var builder = OpenCC.ConverterBuilder(CreatePreset());
        var converter = builder(new ConverterOptions("from", "to"));

        var result = converter("a");

        Assert.Equal("c", result);
    }

    [Fact]
    public void ConverterFactory_NullGroups_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.ConverterFactory(null!));
    }

    [Fact]
    public void ConverterFactory_NullGroup_Throws()
    {
        Assert.Throws<ArgumentException>(() => OpenCC.ConverterFactory(new DictGroup[] { null! }));
    }

    [Fact]
    public void ConverterFactory_ConvertsInput()
    {
        var group1 = DictGroup.FromEntries(new[] { new DictEntry("a", "b") });
        var group2 = DictGroup.FromEntries(new[] { new DictEntry("b", "c") });

        var converter = OpenCC.ConverterFactory(group1, group2);

        Assert.Equal("c", converter("a"));
    }

    [Fact]
    public void ConverterFactory_NullInput_Throws()
    {
        var group = DictGroup.FromEntries(new[] { new DictEntry("a", "b") });
        var converter = OpenCC.ConverterFactory(group);

        Assert.Throws<ArgumentNullException>(() => converter(null!));
    }

    [Fact]
    public void CustomConverter_String_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.CustomConverter((string)null!));
    }

    [Fact]
    public void CustomConverter_Entries_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.CustomConverter((IEnumerable<DictEntry>)null!));
    }

    [Fact]
    public void CustomConverter_KeyValuePairs_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.CustomConverter((IEnumerable<KeyValuePair<string, string>>)null!));
    }

    [Fact]
    public void CustomConverter_String_Converts()
    {
        var converter = OpenCC.CustomConverter("a b");

        Assert.Equal("b", converter("a"));
    }

    [Fact]
    public void CustomConverter_Entries_Converts()
    {
        var converter = OpenCC.CustomConverter(new[] { new DictEntry("a", "b") });

        Assert.Equal("b", converter("a"));
    }

    [Fact]
    public void CustomConverter_KeyValuePairs_Converts()
    {
        var converter = OpenCC.CustomConverter(new Dictionary<string, string> { ["a"] = "b" });

        Assert.Equal("b", converter("a"));
    }

    [Fact]
    public void HtmlConverter_Wrappers_ReturnInstances()
    {
        var document = new System.Xml.Linq.XDocument(new System.Xml.Linq.XElement("root"));

        var converter1 = OpenCC.HtmlConverter(s => s, document, "a", "b");
        var converter2 = OpenCC.HTMLConverter(s => s, document, "a", "b");

        Assert.NotNull(converter1);
        Assert.NotNull(converter2);
        Assert.IsType<HtmlConverter>(converter1);
        Assert.IsType<HtmlConverter>(converter2);
    }

    [Fact]
    public void LocaleHelpers_AreAvailable()
    {
        Assert.NotNull(OpenCC.Locale.FromMap);
        Assert.NotNull(OpenCC.Locale.ToMap);
        Assert.True(OpenCC.Locale.FromMap.ContainsKey("cn"));
        Assert.True(OpenCC.Locale.ToMap.ContainsKey("cn"));
    }

    private static LocalePreset CreatePreset()
    {
        var from = new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["from"] = DictGroup.FromEntries(new[] { new DictEntry("a", "b") }),
        };

        var to = new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["to"] = DictGroup.FromEntries(new[] { new DictEntry("b", "c") }),
        };

        return new LocalePreset(from, to);
    }
}