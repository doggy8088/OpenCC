using Xunit;

namespace OpenCC.Tests;

public class TrieTests
{
    [Fact]
    public void AddWord_NullSource_Throws()
    {
        var trie = new Trie();

        Assert.Throws<ArgumentNullException>(() => trie.AddWord(null!, "target"));
    }

    [Fact]
    public void AddWord_NullTarget_Throws()
    {
        var trie = new Trie();

        Assert.Throws<ArgumentNullException>(() => trie.AddWord("source", null!));
    }

    [Fact]
    public void LoadDict_String_Null_Throws()
    {
        var trie = new Trie();

        Assert.Throws<ArgumentNullException>(() => trie.LoadDict((string)null!));
    }

    [Fact]
    public void LoadDict_Entries_Null_Throws()
    {
        var trie = new Trie();

        Assert.Throws<ArgumentNullException>(() => trie.LoadDict((IEnumerable<DictEntry>)null!));
    }

    [Fact]
    public void LoadDict_DictLike_Null_Throws()
    {
        var trie = new Trie();

        Assert.Throws<ArgumentNullException>(() => trie.LoadDict((IDictLike)null!));
    }

    [Fact]
    public void LoadDictGroup_Null_Throws()
    {
        var trie = new Trie();

        Assert.Throws<ArgumentNullException>(() => trie.LoadDictGroup(null!));
    }

    [Fact]
    public void Convert_Null_Throws()
    {
        var trie = new Trie();

        Assert.Throws<ArgumentNullException>(() => trie.Convert(null!));
    }

    [Fact]
    public void Convert_Empty_ReturnsEmpty()
    {
        var trie = new Trie();

        Assert.Equal(string.Empty, trie.Convert(string.Empty));
    }

    [Fact]
    public void Convert_UsesLongestMatch()
    {
        var trie = new Trie();
        trie.AddWord("ab", "X");
        trie.AddWord("a", "Y");

        var result = trie.Convert("abca");

        Assert.Equal("XcY", result);
    }

    [Fact]
    public void LoadDict_String_SkipsMalformedLines()
    {
        var trie = new Trie();
        trie.LoadDict("a b|invalid|c d");

        Assert.Equal("b", trie.Convert("a"));
        Assert.Equal("d", trie.Convert("c"));
        Assert.Equal("x", trie.Convert("x"));
    }

    [Fact]
    public void LoadDict_String_SupportsNewlineSeparated()
    {
        var trie = new Trie();
        trie.LoadDict("a b\ninvalid\nc d\r\ne f");

        Assert.Equal("b", trie.Convert("a"));
        Assert.Equal("d", trie.Convert("c"));
        Assert.Equal("f", trie.Convert("e"));
    }
}