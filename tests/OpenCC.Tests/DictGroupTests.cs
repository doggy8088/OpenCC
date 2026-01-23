using Xunit;

namespace OpenCC.Tests;

public class DictGroupTests
{
    [Fact]
    public void Constructor_NullDicts_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new DictGroup(null!));
    }

    [Fact]
    public void Concat_NullDict_Throws()
    {
        var group = new DictGroup(Array.Empty<IDictLike>());

        Assert.Throws<ArgumentNullException>(() => group.Concat((IDictLike)null!));
    }

    [Fact]
    public void Concat_NullDicts_Throws()
    {
        var group = new DictGroup(Array.Empty<IDictLike>());

        Assert.Throws<ArgumentNullException>(() => group.Concat((IEnumerable<IDictLike>)null!));
    }

    [Fact]
    public void Concat_AddsDict()
    {
        var group = DictGroup.FromStrings("a b");
        var dict = Dict.FromString("c d");

        var combined = group.Concat(dict);

        Assert.Single(group);
        Assert.Equal(2, combined.Count);
        Assert.Same(group[0], combined[0]);
        Assert.Same(dict, combined[1]);
    }

    [Fact]
    public void Concat_AddsDicts()
    {
        var group = DictGroup.FromStrings("a b");
        var dicts = new[] { Dict.FromString("c d"), Dict.FromString("e f") };

        var combined = group.Concat(dicts);

        Assert.Equal(3, combined.Count);
    }

    [Fact]
    public void FromStrings_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => DictGroup.FromStrings(null!));
    }

    [Fact]
    public void FromEntries_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => DictGroup.FromEntries(null!));
    }

    [Fact]
    public void FromStrings_CreatesGroup()
    {
        var group = DictGroup.FromStrings("a b", "c d");

        Assert.Equal(2, group.Count);
    }

    [Fact]
    public void FromEntries_CreatesGroup()
    {
        var group = DictGroup.FromEntries(new[] { new DictEntry("a", "b") }, new[] { new DictEntry("c", "d") });

        Assert.Equal(2, group.Count);
    }
}