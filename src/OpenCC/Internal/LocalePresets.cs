using OpenCC;

namespace OpenCC.Internal;

internal static class LocalePresets
{
    internal static readonly LocalePreset Full = new(LocaleData.FromMap, LocaleData.ToMap);

    internal static readonly LocalePreset Cn2t = new(
        new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["cn"] = LocaleData.FromCn,
        },
        new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["hk"] = LocaleData.ToHk,
            ["tw"] = LocaleData.ToTw,
            ["tw2"] = LocaleData.ToTw2,
            ["twp"] = LocaleData.ToTwp,
            ["jp"] = LocaleData.ToJp,
        });

    internal static readonly LocalePreset T2cn = new(
        new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["hk"] = LocaleData.FromHk,
            ["tw"] = LocaleData.FromTw,
            ["tw2"] = LocaleData.FromTw2,
            ["twp"] = LocaleData.FromTwp,
            ["jp"] = LocaleData.FromJp,
        },
        new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["cn"] = LocaleData.ToCn,
        });
}