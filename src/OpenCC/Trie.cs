namespace OpenCC;

public sealed class Trie
{
    private sealed class Node
    {
        public Dictionary<int, Node> Children { get; } = new();

        public string? Value { get; set; }
    }

    private readonly Node _root = new();

    public void AddWord(string source, string target)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        var node = _root;
        foreach (var rune in source.EnumerateRunes())
        {
            var codePoint = rune.Value;
            if (!node.Children.TryGetValue(codePoint, out var next))
            {
                next = new Node();
                node.Children[codePoint] = next;
            }

            node = next;
        }

        node.Value = target;
    }

    public void LoadDict(string dict)
    {
        if (dict is null)
        {
            throw new ArgumentNullException(nameof(dict));
        }

        var lines = dict.Split('|');
        foreach (var line in lines)
        {
            if (line.Length == 0)
            {
                continue;
            }

            var parts = line.Split(' ');
            if (parts.Length < 2)
            {
                continue;
            }

            AddWord(parts[0], parts[1]);
        }
    }

    public void LoadDict(IEnumerable<DictEntry> dict)
    {
        if (dict is null)
        {
            throw new ArgumentNullException(nameof(dict));
        }

        foreach (var (source, target) in dict)
        {
            AddWord(source, target);
        }
    }

    public void LoadDict(IDictLike dict)
    {
        if (dict is null)
        {
            throw new ArgumentNullException(nameof(dict));
        }

        dict.LoadInto(this);
    }

    public void LoadDictGroup(IEnumerable<IDictLike> dicts)
    {
        if (dicts is null)
        {
            throw new ArgumentNullException(nameof(dicts));
        }

        foreach (var dict in dicts)
        {
            LoadDict(dict);
        }
    }

    public string Convert(string input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var length = input.Length;
        if (length == 0)
        {
            return string.Empty;
        }

        var parts = new List<string>();
        int? pendingStart = 0;

        for (var i = 0; i < length;)
        {
            var currentNode = _root;
            var matchedEnd = 0;
            string? matchedValue = null;

            for (var j = i; j < length;)
            {
                var codePoint = GetCodePointAt(input, j, out var step);
                if (!currentNode.Children.TryGetValue(codePoint, out var next))
                {
                    break;
                }

                j += step;
                currentNode = next;

                if (currentNode.Value is not null)
                {
                    matchedEnd = j;
                    matchedValue = currentNode.Value;
                }
            }

            if (matchedEnd > 0)
            {
                if (pendingStart is not null)
                {
                    parts.Add(input.Substring(pendingStart.Value, i - pendingStart.Value));
                    pendingStart = null;
                }

                parts.Add(matchedValue ?? string.Empty);
                i = matchedEnd;
            }
            else
            {
                if (pendingStart is null)
                {
                    pendingStart = i;
                }

                GetCodePointAt(input, i, out var step);
                i += step;
            }
        }

        if (pendingStart is not null)
        {
            parts.Add(input.Substring(pendingStart.Value, length - pendingStart.Value));
        }

        return string.Concat(parts);
    }

    private static int GetCodePointAt(string text, int index, out int codeUnitLength)
    {
        var c = text[index];
        if (char.IsHighSurrogate(c) && index + 1 < text.Length && char.IsLowSurrogate(text[index + 1]))
        {
            codeUnitLength = 2;
            return char.ConvertToUtf32(c, text[index + 1]);
        }

        codeUnitLength = 1;
        return c;
    }
}