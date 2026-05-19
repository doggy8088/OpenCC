# OpenCC for C# 範例

這個資料夾收錄可直接執行的使用者範例。每個範例都是獨立的 .NET console app，並以專案參考使用本 repo 的 `src/OpenCC/OpenCC.csproj`。

## 範例列表

| 範例 | 用途 |
| --- | --- |
| [basic-conversion](basic-conversion/) | 最基本的簡體中文轉臺灣繁體中文，示範 `OpenCC.Converter("cn", "tw2")`。 |
| [no-phrase-conversion](no-phrase-conversion/) | 只做簡繁字形轉換，不套用臺灣/香港詞彙轉換，適合要保留原文用詞的場景。 |
| [locale-differences](locale-differences/) | 比較 `t`、`tw`、`tw2`、`twp`、`hk`、`jp` 等詞庫的特性。 |
| [custom-dictionary](custom-dictionary/) | 使用自訂詞典、轉換順序與最長匹配規則。 |
| [html-conversion](html-conversion/) | 轉換 XML/HTML 片段文字、`lang` 屬性與可還原流程。 |

## 執行方式

在 `OpenCC/` 目錄下執行：

```bash
dotnet run --project examples/basic-conversion
dotnet run --project examples/no-phrase-conversion
dotnet run --project examples/locale-differences
dotnet run --project examples/custom-dictionary
dotnet run --project examples/html-conversion
```
