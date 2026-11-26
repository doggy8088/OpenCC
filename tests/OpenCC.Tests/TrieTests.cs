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

    [Fact]
    public void Convert_SurrogatePairKeys_UsesLongestMatch()
    {
        var trie = new Trie();
        trie.AddWord("\U00020BB7", "X");
        trie.AddWord("\U00020BB7\U00020BB7", "Y");

        Assert.Equal("X", trie.Convert("\U00020BB7"));
        Assert.Equal("Y", trie.Convert("\U00020BB7\U00020BB7"));
        Assert.Equal("\uD842", trie.Convert("\uD842"));
    }

    [Fact]
    public void Convert_LoneHighSurrogateKey_Matches()
    {
        var trie = new Trie();
        trie.AddWord("\uD800", "H");

        Assert.Equal("aHb", trie.Convert("a\uD800b"));
    }

    [Fact]
    public void Convert_LoneLowSurrogateKey_Matches()
    {
        var trie = new Trie();
        trie.AddWord("\uDC00", "L");

        Assert.Equal("aLb", trie.Convert("a\uDC00b"));
    }

    [Fact]
    public void Convert_LoneSurrogatesNotInDictionary_PreservedVerbatim()
    {
        var trie = new Trie();

        Assert.Equal("x\uD800y", trie.Convert("x\uD800y"));
        Assert.Equal("x\uDC00y", trie.Convert("x\uDC00y"));
    }

    [Fact]
    public void Convert_LoneSurrogate_DoesNotMatchReplacementCharacterKey()
    {
        var trie = new Trie();
        trie.AddWord("\uFFFD", "R");

        Assert.Equal("\uD800", trie.Convert("\uD800"));
        Assert.Equal("R", trie.Convert("\uFFFD"));
    }
}