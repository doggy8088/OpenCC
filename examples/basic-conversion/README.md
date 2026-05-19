# 基本簡繁轉換

這個範例示範最常見的使用方式：建立一個由中國大陸簡體中文 `cn` 轉成臺灣繁體中文 `tw2` 的轉換器。

## 重點

- `OpenCC.Converter("cn", "tw2")` 會先把簡體轉成 OpenCC 標準繁體，再套用臺灣詞彙。
- 回傳值是 `Func<string, string>`，可以重複用在多段文字上。
- 適合文章、使用者介面文字、搜尋索引前處理等純文字轉換。

## 執行

```bash
dotnet run --project examples/basic-conversion
```
