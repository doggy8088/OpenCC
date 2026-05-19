using OpenCC;

var text = "鼠标、软件、打印机和服务器都连接到计算机网络。";
var targets = new[] { "t", "tw", "tw2", "twp", "hk", "jp" };

Console.WriteLine($"原文：{text}");
Console.WriteLine();

foreach (var target in targets)
{
    var converter = OpenCC.OpenCC.Converter("cn", target);
    Console.WriteLine($"cn -> {target,-3} {converter(text)}");
}

Console.WriteLine();
Console.WriteLine("選擇建議：只轉字形用 t；臺灣產品介面多半用 tw2；面向香港使用者可用 hk。");
