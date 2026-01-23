namespace OpenCC;

public interface IDictLike
{
    void LoadInto(Trie trie);
}

public static class Dict
{
    public static IDictLike FromString(string dict)
    {
        if (dict is null)
        {
            throw new ArgumentNullException(nameof(dict));
        }

        return new StringDict(dict);
    }

    public static IDictLike FromEntries(IEnumerable<DictEntry> entries)
    {
        if (entries is null)
        {
            throw new ArgumentNullException(nameof(entries));
        }

        return new EntryDict(entries);
    }

    public static IDictLike FromEntries(IEnumerable<KeyValuePair<string, string>> entries)
    {
        if (entries is null)
        {
            throw new ArgumentNullException(nameof(entries));
        }

        return new EntryDict(entries.Select(e => new DictEntry(e.Key, e.Value)));
    }
}

internal sealed class StringDict : IDictLike
{
    public StringDict(string data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }

    public string Data { get; }

    public void LoadInto(Trie trie)
    {
        trie.LoadDict(Data);
    }
}

internal sealed class EntryDict : IDictLike
{
    private readonly IReadOnlyList<DictEntry> _entries;

    public EntryDict(IEnumerable<DictEntry> entries)
    {
        if (entries is null)
        {
            throw new ArgumentNullException(nameof(entries));
        }

        _entries = entries as IReadOnlyList<DictEntry> ?? entries.ToArray();
    }

    public void LoadInto(Trie trie)
    {
        trie.LoadDict(_entries);
    }
}