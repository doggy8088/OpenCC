# `gov`：內地《通用規範漢字表》(2013) 規範繁體筆記

這份筆記說明 OpenCC（本 repo 的 C# port）新增的 `gov` 目標：將輸入文字（可能混雜港/台/舊字形或部分簡體）轉為**內地《通用規範漢字表》(2013) 附件 1 所對應的規範繁體字形**。

## 背景與來源

- `gov` 字庫來源：`OpenCC-Traditional-Chinese-characters-according-to-Chinese-government-standards` 專案中的 `t2gov/`（`TGCharacters.txt`、`TGPhrases.txt`）。
- 授權：來源字庫為 Apache-2.0（見 `src/OpenCC/Internal/DictData.Gov.cs` 檔頭註記）；本 repo 仍為 MIT，但已保留來源註記以符合再散布需求。

## 建議用法（不要多段轉換）

### 1) 簡體（內地）→ 規範繁體（建議）

用 `cn -> gov`：先做完整簡轉繁，再做 `gov` 正規化。

```csharp
using OpenCC;

var converter = OpenCC.Converter("cn", "gov");
Console.WriteLine(converter("启绿")); // 啓緑
```

### 2) 台灣繁體（含 `tw2` 習慣）→ 規範繁體

用 `tw2 -> gov`：

```csharp
using OpenCC;

var converter = OpenCC.Converter("tw2", "gov");
Console.WriteLine(converter("啟用裡面淨化偽")); // 啓用裏面净化僞
```

### 3) 已是繁體但可能混雜部分簡體 → 規範繁體

用 `t -> gov`（`t` 代表**不套用 from 字庫**，直接套用 `gov` 正規化）：

```csharp
using OpenCC;

var converter = OpenCC.Converter("t", "gov");
Console.WriteLine(converter("啟用裡面淨化偽")); // 啓用裏面净化僞
```

> 注意：`t -> gov` 只會命中 `gov` 字庫中「有覆蓋到的簡體→規範繁體」條目，**不等同完整簡轉繁**；純簡體文本請用 `cn -> gov`。

## 什麼時候才需要「先 cn 轉 tw2 再 gov」？

一般不需要。

只有在你**刻意想先套用 `tw2` 的用詞/轉換**（例如偏台灣用語或專案既有需求），再把字形收斂到 `gov`，才會做多段鏈路；在本 repo 可用 `ConverterFactory` 一次串起來：

```csharp
using OpenCC;

var converter = OpenCC.ConverterFactory(
    OpenCC.Locale.From.Cn,
    OpenCC.Locale.To.Tw2,
    OpenCC.Locale.To.Gov
);
```

## 實作備註（品質相關）

- `gov` 的字/詞典分別在 `src/OpenCC/Internal/DictData.Gov.cs` 內嵌為字串常數，並透過 `LocaleData.ToGov` 併入 `ToMap`（`to = "gov"` 可用）。
- 本 repo 的 Trie 轉換支援兩種字典格式：
  - 傳統 OpenCC `key value`（以空白或 tab 分隔，可用 `|` 或換行分隔多筆）
  - opencc-js 類 JSON 陣列片段的「quoted pair」格式（用於既有的 `tw2` 自訂詞庫）

