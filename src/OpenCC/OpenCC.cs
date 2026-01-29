using OpenCC.Internal;

namespace OpenCC;

public static class OpenCC
{
    public static Func<string, string> Converter(ConverterOptions options)
    {
        return ConverterBuilder(LocalePresets.Full)(options);
    }

    public static Func<string, string> Converter(string from, string to)
    {
        return Converter(new ConverterOptions(from, to));
    }

    public static Func<ConverterOptions, Func<string, string>> ConverterBuilder(LocalePreset localePreset)
    {
        if (localePreset is null)
        {
            throw new ArgumentNullException(nameof(localePreset));
        }

        return options =>
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var dictGroups = new List<DictGroup>(2);

            AddDictGroup("from", options.From, localePreset.From, dictGroups);
            AddDictGroup("to", options.To, localePreset.To, dictGroups);

            return ConverterFactory(dictGroups.ToArray());
        };
    }

    public static Func<string, string> ConverterFactory(params DictGroup[] dictGroups)
    {
        if (dictGroups is null)
        {
            throw new ArgumentNullException(nameof(dictGroups));
        }

        var tries = new List<Trie>(dictGroups.Length);
        foreach (var group in dictGroups)
        {
            if (group is null)
            {
                throw new ArgumentException("Dictionary group cannot be null.", nameof(dictGroups));
            }

            var trie = new Trie();
            trie.LoadDictGroup(group);
            tries.Add(trie);
        }

        return input =>
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var result = input;
            foreach (var trie in tries)
            {
                result = trie.Convert(result);
            }

            return result;
        };
    }

    public static Func<string, string> CustomConverter(string dict)
    {
        if (dict is null)
        {
            throw new ArgumentNullException(nameof(dict));
        }

        return ConverterFactory(new DictGroup(new[] { Dict.FromString(dict) }));
    }

    public static Func<string, string> CustomConverter(IEnumerable<DictEntry> dict)
    {
        if (dict is null)
        {
            throw new ArgumentNullException(nameof(dict));
        }

        return ConverterFactory(new DictGroup(new[] { Dict.FromEntries(dict) }));
    }

    public static Func<string, string> CustomConverter(IEnumerable<KeyValuePair<string, string>> dict)
    {
        if (dict is null)
        {
            throw new ArgumentNullException(nameof(dict));
        }

        return ConverterFactory(new DictGroup(new[] { Dict.FromEntries(dict) }));
    }

    public static HtmlConverter HtmlConverter(
        Func<string, string> converter,
        System.Xml.Linq.XContainer rootNode,
        string fromLangTag,
        string toLangTag)
    {
        return new HtmlConverter(converter, rootNode, fromLangTag, toLangTag);
    }

    public static HtmlConverter HTMLConverter(
        Func<string, string> converter,
        System.Xml.Linq.XContainer rootNode,
        string fromLangTag,
        string toLangTag)
    {
        return HtmlConverter(converter, rootNode, fromLangTag, toLangTag);
    }

    public static LocalePreset Preset => LocalePresets.Full;

    public static class Locale
    {
        public static class From
        {
            public static DictGroup Cn => LocaleData.FromCn;
            public static DictGroup Hk => LocaleData.FromHk;
            public static DictGroup Tw => LocaleData.FromTw;
            public static DictGroup Tw2 => LocaleData.FromTw2;
            public static DictGroup Twp => LocaleData.FromTwp;
            public static DictGroup Jp => LocaleData.FromJp;
            public static DictGroup Gov => LocaleData.FromGov;
        }

        public static class To
        {
            public static DictGroup Cn => LocaleData.ToCn;
            public static DictGroup Hk => LocaleData.ToHk;
            public static DictGroup Tw => LocaleData.ToTw;
            public static DictGroup Tw2 => LocaleData.ToTw2;
            public static DictGroup Twp => LocaleData.ToTwp;
            public static DictGroup Jp => LocaleData.ToJp;
            public static DictGroup Gov => LocaleData.ToGov;
        }

        public static IReadOnlyDictionary<string, DictGroup> FromMap => LocaleData.FromMap;
        public static IReadOnlyDictionary<string, DictGroup> ToMap => LocaleData.ToMap;
    }

    private static void AddDictGroup(
        string type,
        string? locale,
        IReadOnlyDictionary<string, DictGroup> map,
        IList<DictGroup> dictGroups)
    {
        if (string.IsNullOrWhiteSpace(locale))
        {
            throw new ArgumentException($"Please provide the `{type}` option", nameof(locale));
        }

        if (string.Equals(locale, "t", StringComparison.Ordinal))
        {
            return;
        }

        if (!map.TryGetValue(locale, out var group))
        {
            throw new ArgumentException($"Unknown locale `{locale}` for `{type}` option", nameof(locale));
        }

        dictGroups.Add(group);
    }
}
