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

        var added = 0;
        var entries = dict.Split(new[] { '|', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var entry in entries)
        {
            var line = entry.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            if (line.StartsWith('#'))
            {
                continue;
            }

            // Quote-pair dictionaries (e.g., opencc-js style arrays) should be handled by LoadQuotedPairs.
            if (line.Contains('"') || line.Contains('[') || line.Contains(']'))
            {
                continue;
            }

            var splitIndex = IndexOfWhitespace(line);
            if (splitIndex < 0)
            {
                continue;
            }

            var source = line.Substring(0, splitIndex);
            var targetPart = line.Substring(splitIndex).Trim();
            if (targetPart.Length == 0)
            {
                continue;
            }

            var inlineCommentIndex = targetPart.IndexOf('#');
            if (inlineCommentIndex == 0)
            {
                continue;
            }

            if (inlineCommentIndex > 0)
            {
                targetPart = targetPart.Substring(0, inlineCommentIndex).TrimEnd();
                if (targetPart.Length == 0)
                {
                    continue;
                }
            }

            var target = TakeFirstToken(targetPart);
            if (target.Length == 0)
            {
                continue;
            }

            AddWord(source, target);
            added++;
        }

        if (added > 0)
        {
            return;
        }

        LoadQuotedPairs(dict);
    }

    private static int IndexOfWhitespace(string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            if (char.IsWhiteSpace(text[i]))
            {
                return i;
            }
        }

        return -1;
    }

    private static string TakeFirstToken(string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            if (char.IsWhiteSpace(text[i]))
            {
                return text.Substring(0, i);
            }
        }

        return text;
    }

    private void LoadQuotedPairs(string dict)
    {
        if (dict.Length == 0)
        {
            return;
        }

        var tokens = new List<string>(256);

        // Some embedded dicts are copied from opencc-js JSON arrays and trimmed,
        // which can leave the first token without a leading quote.
        var i = 0;
        while (i < dict.Length && char.IsWhiteSpace(dict[i]))
        {
            i++;
        }

        if (i < dict.Length && dict[i] != '"')
        {
            var firstQuote = dict.IndexOf('"', i);
            if (firstQuote > i)
            {
                var hadLeadingToken = false;
                var leading = dict.Substring(i, firstQuote - i).Trim();
                if (leading.Length > 0 && !leading.Contains('[') && !leading.Contains(']') && !leading.Contains(','))
                {
                    tokens.Add(leading);
                    hadLeadingToken = true;
                }

                i = hadLeadingToken ? firstQuote + 1 : firstQuote;
            }
        }

        while (i < dict.Length)
        {
            var startQuote = dict.IndexOf('"', i);
            if (startQuote < 0)
            {
                break;
            }

            var endQuote = dict.IndexOf('"', startQuote + 1);
            if (endQuote < 0)
            {
                break;
            }

            tokens.Add(dict.Substring(startQuote + 1, endQuote - startQuote - 1));
            i = endQuote + 1;
        }

        for (var t = 0; t + 1 < tokens.Count; t += 2)
        {
            AddWord(tokens[t], tokens[t + 1]);
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
