# HTML / XML 片段轉換

這個範例使用 `HtmlConverter` 轉換 XML/HTML 片段中的文字與 `lang` 屬性。

## 重點

- 只會轉換符合來源語言標籤的節點。
- 會更新 `lang` 屬性，例如 `zh-HK` 轉成 `zh-CN`。
- 可呼叫 `Restore()` 還原轉換前內容。
- 這個 C# 版本以 `System.Xml.Linq` 為基礎，輸入需是可解析的 XML 或 XHTML 片段。

## 執行

```bash
dotnet run --project examples/html-conversion
```
