using OpenCC;

var converter = OpenCC.OpenCC.Converter("cn", "tw2");

var samples = new[]
{
    "汉语是一种美丽的语言。",
    "默认用户界面支持数据库和网络请求。",
    "这只鼠标连接到计算机后，可以打开软件设置。",
};

foreach (var sample in samples)
{
    Console.WriteLine($"原文：{sample}");
    Console.WriteLine($"轉換：{converter(sample)}");
    Console.WriteLine();
}
