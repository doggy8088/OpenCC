using System.Collections;

namespace OpenCC;

public sealed class DictGroup : IReadOnlyList<IDictLike>
{
    private readonly IReadOnlyList<IDictLike> _dicts;

    public DictGroup(IEnumerable<IDictLike> dicts)
    {
        if (dicts is null)
        {
            throw new ArgumentNullException(nameof(dicts));
        }

        _dicts = dicts as IReadOnlyList<IDictLike> ?? dicts.ToArray();
    }

    public int Count => _dicts.Count;

    public IDictLike this[int index] => _dicts[index];

    public DictGroup Concat(IDictLike dict)
    {
        if (dict is null)
        {
            throw new ArgumentNullException(nameof(dict));
        }

        return new DictGroup(_dicts.Concat(new[] { dict }));
    }

    public DictGroup Concat(IEnumerable<IDictLike> dicts)
    {
        if (dicts is null)
        {
            throw new ArgumentNullException(nameof(dicts));
        }

        return new DictGroup(_dicts.Concat(dicts));
    }

    public static DictGroup FromStrings(params string[] dicts)
    {
        if (dicts is null)
        {
            throw new ArgumentNullException(nameof(dicts));
        }

        return new DictGroup(dicts.Select(Dict.FromString));
    }

    public static DictGroup FromEntries(params IEnumerable<DictEntry>[] dicts)
    {
        if (dicts is null)
        {
            throw new ArgumentNullException(nameof(dicts));
        }

        return new DictGroup(dicts.Select(Dict.FromEntries));
    }

    public IEnumerator<IDictLike> GetEnumerator() => _dicts.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}