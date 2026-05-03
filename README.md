# OpenCC for C#

純 C# 版本的 OpenCC 函式庫，無任何 NuGet 套件依賴。

## 安裝

```bash
dotnet add package OpenCC
```

## 專案結構

- `src/OpenCC/`：函式庫原始碼
- `tests/OpenCC.Tests/`：xUnit 單元測試
- `OpenCC.sln`：解決方案檔，便於一鍵建置與測試

## 基本用法

```csharp
using OpenCC;

var converter = OpenCC.Converter("cn", "tw2");
Console.WriteLine(converter("汉语")); // 漢語
```

## 自訂轉換器

### 使用陣列定義

```csharp
using OpenCC;

var converter = OpenCC.CustomConverter(new[]
{
    new DictEntry("香蕉", "banana"),
    new DictEntry("蘋果", "apple"),
});

Console.WriteLine(converter("我喜歡吃香蕉和蘋果"));
```

### 使用字串定義

```csharp
using OpenCC;

var converter = OpenCC.CustomConverter("香蕉 banana|蘋果 apple|梨 pear");
Console.WriteLine(converter("香蕉 蘋果 梨"));
```

## 進階組合

```csharp
using OpenCC;

var customDict = new[]
{
    new DictEntry("“", "「"),
    new DictEntry("”", "」"),
};

var converter = OpenCC.ConverterFactory(
    OpenCC.Locale.From.Cn,
    OpenCC.Locale.To.Tw.Concat(new[] { Dict.FromEntries(customDict) })
);

Console.WriteLine(converter("悟空道:“师父又来了。”"));
```

## HTML / XML 轉換 (XDocument)

`HtmlConverter` 以 `System.Xml.Linq` 為基礎，適用於 XML 或可解析為 XML 的 HTML 片段。

```csharp
using System.Xml.Linq;
using OpenCC;

var converter = OpenCC.Converter("hk", "cn");
var doc = XDocument.Parse("<html lang='zh-HK'><body><p lang='zh-HK'>漢語</p></body></html>");

var htmlConverter = OpenCC.HtmlConverter(converter, doc, "zh-HK", "zh-CN");
htmlConverter.Convert();

Console.WriteLine(doc.ToString());

htmlConverter.Restore();
```

## NuGet 封裝

```bash
dotnet pack src/OpenCC/OpenCC.csproj -c Release
```

產出的 `.nupkg` 會在 `src/OpenCC/bin/Release/`。

## License

MIT
