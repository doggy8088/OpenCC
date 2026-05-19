# 自訂詞典

這個範例示範如何加入自己的詞彙規則，並說明 OpenCC 的最長匹配與多階段轉換。

## 重點

- `CustomConverter()` 適合只使用自己的詞典。
- `ConverterFactory()` 可以把內建詞庫與自訂詞庫串在一起。
- 較長詞條會優先匹配，例如「用户界面」會優先於「用户」。

## 執行

```bash
dotnet run --project examples/custom-dictionary
```
