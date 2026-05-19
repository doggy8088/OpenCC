using System.Xml.Linq;
using OpenCC;

var document = XDocument.Parse("""
<html lang="zh-HK">
  <body>
    <h1>漢語轉換示例</h1>
    <p lang="zh-HK">伺服器與網絡服務已啟動。</p>
    <p lang="en">This paragraph should stay unchanged.</p>
  </body>
</html>
""");

var converter = OpenCC.OpenCC.Converter("hk", "cn");
var htmlConverter = OpenCC.OpenCC.HtmlConverter(converter, document, "zh-HK", "zh-CN");

htmlConverter.Convert();
Console.WriteLine("轉換後：");
Console.WriteLine(document);

htmlConverter.Restore();
Console.WriteLine();
Console.WriteLine("還原後：");
Console.WriteLine(document);
