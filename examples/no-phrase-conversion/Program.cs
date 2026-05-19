using OpenCC;

var text = "鼠标和软件可以连接到计算机网络。";

var shapeOnly = OpenCC.OpenCC.Converter("cn", "t");
var taiwanWords = OpenCC.OpenCC.Converter("cn", "tw2");

Console.WriteLine($"原文：{text}");
Console.WriteLine($"只轉字形 cn -> t：{shapeOnly(text)}");
Console.WriteLine($"臺灣詞彙 cn -> tw2：{taiwanWords(text)}");
Console.WriteLine();
Console.WriteLine("差異：cn -> t 保留「鼠標、軟件、計算機」等用詞；cn -> tw2 會轉成臺灣慣用詞。");
