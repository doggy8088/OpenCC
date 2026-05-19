using OpenCC;

var customOnly = OpenCC.OpenCC.CustomConverter(new[]
{
    new DictEntry("香蕉", "banana"),
    new DictEntry("蘋果", "apple"),
    new DictEntry("用户", "使用者"),
    new DictEntry("用户界面", "使用者介面"),
});

Console.WriteLine(customOnly("香蕉、蘋果和用户界面"));

var productTerms = Dict.FromEntries(new[]
{
    new DictEntry("預設使用者介面", "預設 UI"),
    new DictEntry("資料庫", "DB"),
});

var cnToTwWithProductTerms = OpenCC.OpenCC.ConverterFactory(
    OpenCC.OpenCC.Locale.From.Cn,
    OpenCC.OpenCC.Locale.To.Tw2,
    new DictGroup(new[] { productTerms })
);

var text = "默认用户界面支持数据库和网络请求。";
Console.WriteLine(cnToTwWithProductTerms(text));
