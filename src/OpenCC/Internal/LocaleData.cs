using OpenCC;

namespace OpenCC.Internal;

internal static class LocaleData
{
    internal static readonly DictGroup FromCn = DictGroup.FromStrings(DictData.STCharacters, DictData.STPhrases);
    internal static readonly DictGroup FromHk = DictGroup.FromStrings(DictData.HKVariantsRev, DictData.HKVariantsRevPhrases);
    internal static readonly DictGroup FromTw = DictGroup.FromStrings(DictData.TWVariantsRev, DictData.TWVariantsRevPhrases);
    internal static readonly DictGroup FromTw2 = DictGroup.FromStrings(DictData.TWVariantsRev, DictData.TWPhrasesCustomRev);
    internal static readonly DictGroup FromTwp = DictGroup.FromStrings(DictData.TWVariantsRev, DictData.TWVariantsRevPhrases, DictData.TWPhrasesRev);
    internal static readonly DictGroup FromJp = DictGroup.FromStrings(DictData.JPVariantsRev, DictData.JPShinjitaiCharacters, DictData.JPShinjitaiPhrases);

    internal static readonly DictGroup ToCn = DictGroup.FromStrings(DictData.TSCharacters, DictData.TSPhrases);
    internal static readonly DictGroup ToHk = DictGroup.FromStrings(DictData.HKVariants);
    internal static readonly DictGroup ToTw = DictGroup.FromStrings(DictData.TWVariants);
    internal static readonly DictGroup ToTw2 = DictGroup.FromStrings(DictData.TWVariants, DictData.TWPhrasesCustom);
    internal static readonly DictGroup ToTwp = DictGroup.FromStrings(DictData.TWVariants, DictData.TWPhrasesIT, DictData.TWPhrasesName, DictData.TWPhrasesOther);
    internal static readonly DictGroup ToJp = DictGroup.FromStrings(DictData.JPVariants);

    internal static readonly IReadOnlyDictionary<string, DictGroup> FromMap =
        new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["cn"] = FromCn,
            ["hk"] = FromHk,
            ["tw"] = FromTw,
            ["tw2"] = FromTw2,
            ["twp"] = FromTwp,
            ["jp"] = FromJp,
        };

    internal static readonly IReadOnlyDictionary<string, DictGroup> ToMap =
        new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["cn"] = ToCn,
            ["hk"] = ToHk,
            ["tw"] = ToTw,
            ["tw2"] = ToTw2,
            ["twp"] = ToTwp,
            ["jp"] = ToJp,
        };
}