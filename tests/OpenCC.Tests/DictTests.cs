using Xunit;

namespace OpenCC.Tests;

public class DictTests
{
    [Fact]
    public void FromString_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Dict.FromString(null!));
    }

    [Fact]
    public void FromEntries_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Dict.FromEntries((IEnumerable<DictEntry>)null!));
    }

    [Fact]
    public void FromEntries_KeyValuePairs_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Dict.FromEntries((IEnumerable<KeyValuePair<string, string>>)null!));
    }

    [Fact]
    public void FromString_ReturnsStringDict()
    {
        var dict = Dict.FromString("a b");

        Assert.Equal("StringDict", dict.GetType().Name);
    }

    [Fact]
    public void FromEntries_ReturnsEntryDict()
    {
        var dict = Dict.FromEntries(new[] { new DictEntry("a", "b") });

        Assert.Equal("EntryDict", dict.GetType().Name);
    }
}

public class StringDictTests
{
    [Fact]
    public void Constructor_NullData_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new StringDict(null!));
    }

    [Fact]
    public void LoadInto_LoadsEntries()
    {
        var dict = new StringDict("a b|c d");
        var trie = new Trie();

        dict.LoadInto(trie);

        Assert.Equal("b", trie.Convert("a"));
        Assert.Equal("d", trie.Convert("c"));
    }
}

public class EntryDictTests
{
    [Fact]
    public void Constructor_NullEntries_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new EntryDict(null!));
    }

    [Fact]
    public void LoadInto_LoadsEntries()
    {
        var dict = new EntryDict(new[] { new DictEntry("a", "b"), new DictEntry("c", "d") });
        var trie = new Trie();

        dict.LoadInto(trie);

        Assert.Equal("b", trie.Convert("a"));
        Assert.Equal("d", trie.Convert("c"));
    }
}