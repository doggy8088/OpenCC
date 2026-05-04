using OpenCC;

namespace OpenCC.Internal;

internal static class LocaleData
{
    private const string TWPhrasesCustomAdditions = @"網絡服務	網路服務|應用程序網關	應用程式閘道|鏡像文件	映像檔|保存更改	儲存變更|儲存更改	儲存變更|文件名	檔名|文件系統	檔案系統|文件描述符	檔案描述子|函數調用	函式呼叫|渲染管線	算繪管線|內存分配	記憶體配置|網絡棧	網路堆疊|網絡適配器	網路介面卡";
    private const string TWPhrasesCustomAdditionsRev = @"網路服務	網絡服務|應用程式閘道	應用程序網關|映像檔	鏡像文件|儲存變更	儲存更改|檔名	文件名|檔案系統	文件系統|檔案描述子	文件描述符|函式呼叫	函數調用|算繪管線	渲染管線|記憶體配置	內存分配|網路堆疊	網絡棧|網路介面卡	網絡適配器";

    internal static readonly DictGroup FromCn = DictGroup.FromStrings(DictData.STCharacters, DictData.STPhrases);
    internal static readonly DictGroup FromHk = DictGroup.FromStrings(DictData.HKVariantsRev, DictData.HKVariantsRevPhrases);
    internal static readonly DictGroup FromTw = DictGroup.FromStrings(DictData.TWVariantsRev, DictData.TWVariantsRevPhrases);
    internal static readonly DictGroup FromTw2 = DictGroup.FromStrings(DictData.TWVariantsRev, DictData.TWPhrasesCustomRev, TWPhrasesCustomAdditionsRev);
    internal static readonly DictGroup FromTwp = DictGroup.FromStrings(DictData.TWVariantsRev, DictData.TWVariantsRevPhrases, DictData.TWPhrasesRev);
    internal static readonly DictGroup FromJp = DictGroup.FromStrings(DictData.JPVariantsRev, DictData.JPShinjitaiCharacters, DictData.JPShinjitaiPhrases);

    internal static readonly DictGroup ToCn = DictGroup.FromStrings(DictData.TSCharacters, DictData.TSPhrases);
    internal static readonly DictGroup ToHk = DictGroup.FromStrings(DictData.HKVariants);
    internal static readonly DictGroup ToTw = DictGroup.FromStrings(DictData.TWVariants);
    internal static readonly DictGroup ToTw2 = DictGroup.FromStrings(DictData.TWVariants, DictData.TWPhrasesCustom, TWPhrasesCustomAdditions);
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
