using Xunit;

namespace OpenCC.Tests;

public class OpenCCTests
{
    [Fact]
    public void ConverterBuilder_NullLocalePreset_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.ConverterBuilder(null!));
    }

    [Fact]
    public void ConverterBuilder_NullOptions_Throws()
    {
        var builder = OpenCC.ConverterBuilder(CreatePreset());

        Assert.Throws<ArgumentNullException>(() => builder(null!));
    }

    [Fact]
    public void ConverterBuilder_ConvertsSequentially()
    {
        var builder = OpenCC.ConverterBuilder(CreatePreset());
        var converter = builder(new ConverterOptions("from", "to"));

        var result = converter("a");

        Assert.Equal("c", result);
    }

    [Fact]
    public void ConverterFactory_NullGroups_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.ConverterFactory(null!));
    }

    [Fact]
    public void ConverterFactory_NullGroup_Throws()
    {
        Assert.Throws<ArgumentException>(() => OpenCC.ConverterFactory(new DictGroup[] { null! }));
    }

    [Fact]
    public void ConverterFactory_ConvertsInput()
    {
        var group1 = DictGroup.FromEntries(new[] { new DictEntry("a", "b") });
        var group2 = DictGroup.FromEntries(new[] { new DictEntry("b", "c") });

        var converter = OpenCC.ConverterFactory(group1, group2);

        Assert.Equal("c", converter("a"));
    }

    [Fact]
    public void ConverterFactory_NullInput_Throws()
    {
        var group = DictGroup.FromEntries(new[] { new DictEntry("a", "b") });
        var converter = OpenCC.ConverterFactory(group);

        Assert.Throws<ArgumentNullException>(() => converter(null!));
    }

    [Fact]
    public void CustomConverter_String_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.CustomConverter((string)null!));
    }

    [Fact]
    public void CustomConverter_Entries_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.CustomConverter((IEnumerable<DictEntry>)null!));
    }

    [Fact]
    public void CustomConverter_KeyValuePairs_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => OpenCC.CustomConverter((IEnumerable<KeyValuePair<string, string>>)null!));
    }

    [Fact]
    public void CustomConverter_String_Converts()
    {
        var converter = OpenCC.CustomConverter("a b");

        Assert.Equal("b", converter("a"));
    }

    [Fact]
    public void CustomConverter_String_SupportsTabSeparatedEntriesWithSpaces()
    {
        var converter = OpenCC.CustomConverter("a b|Web 平台庫\tWeb 平台函式庫");

        Assert.Equal("b", converter("a"));
        Assert.Equal("Web 平台函式庫", converter("Web 平台庫"));
    }

    [Fact]
    public void CustomConverter_Entries_Converts()
    {
        var converter = OpenCC.CustomConverter(new[] { new DictEntry("a", "b") });

        Assert.Equal("b", converter("a"));
    }

    [Fact]
    public void CustomConverter_KeyValuePairs_Converts()
    {
        var converter = OpenCC.CustomConverter(new Dictionary<string, string> { ["a"] = "b" });

        Assert.Equal("b", converter("a"));
    }

    [Fact]
    public void HtmlConverter_Wrappers_ReturnInstances()
    {
        var document = new System.Xml.Linq.XDocument(new System.Xml.Linq.XElement("root"));

        var converter1 = OpenCC.HtmlConverter(s => s, document, "a", "b");
        var converter2 = OpenCC.HTMLConverter(s => s, document, "a", "b");

        Assert.NotNull(converter1);
        Assert.NotNull(converter2);
        Assert.IsType<HtmlConverter>(converter1);
        Assert.IsType<HtmlConverter>(converter2);
    }

    [Fact]
    public void LocaleHelpers_AreAvailable()
    {
        Assert.NotNull(OpenCC.Locale.FromMap);
        Assert.NotNull(OpenCC.Locale.ToMap);
        Assert.True(OpenCC.Locale.FromMap.ContainsKey("cn"));
        Assert.True(OpenCC.Locale.ToMap.ContainsKey("cn"));
    }

    [Theory]
    [InlineData("电子邮件", "電子郵件")]
    [InlineData("视频", "影片")]
    [InlineData("音频", "音訊")]
    [InlineData("软件", "軟體")]
    [InlineData("硬件", "硬體")]
    [InlineData("程序", "程式")]
    [InlineData("进程", "行程")]
    [InlineData("进程间通信", "行程間通訊")]
    [InlineData("线程", "執行緒")]
    [InlineData("数据", "資料")]
    [InlineData("数据库", "資料庫")]
    [InlineData("网络服务", "網路服務")]
    [InlineData("应用程序网关", "應用程式閘道")]
    [InlineData("镜像文件", "映像檔")]
    [InlineData("保存更改", "儲存變更")]
    [InlineData("网络", "網路")]
    [InlineData("信息", "資訊")]
    [InlineData("质量", "品質")]
    [InlineData("用户", "使用者")]
    [InlineData("默认", "預設")]
    [InlineData("创建", "建立")]
    [InlineData("实现", "實作")]
    [InlineData("运行", "執行")]
    [InlineData("发布", "發表")]
    [InlineData("屏幕", "螢幕")]
    [InlineData("界面", "介面")]
    [InlineData("文档", "文件")]
    [InlineData("操作系统", "作業系統")]
    [InlineData("剑指", "針對")]
    [InlineData("痛点", "要害")]
    [InlineData("硬伤", "罩門")]
    public void Converter_CnToTw2_ConvertsPreferredTaiwanTerms(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("数据结构数据库", "資料結構資料庫")]
    [InlineData("响应式编程响应头", "回應式程式設計回應標頭")]
    [InlineData("进程间通信和多线程", "行程間通訊和多執行緒")]
    public void Converter_CnToTw2_PrefersLongestTaiwanTechTerms(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("Web 平台库", "Web 平台函式庫")]
    [InlineData("for 循环和while 循环", "for 迴圈和while 迴圈")]
    [InlineData("元数据 API", "Metadata API")]
    [InlineData("类（ Class ）加载器", "類別（ Class ）載入器")]
    public void Converter_CnToTw2_ConvertsEnglishMixedTaiwanTechTerms(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("“数据库”, “网络请求”", "“資料庫”, “網路請求”")]
    [InlineData("项目设置：默认值", "專案設定：預設值")]
    [InlineData("「类」", "「類別」")]
    [InlineData("（视频）", "（影片）")]
    public void Converter_CnToTw2_ConvertsAcrossPunctuationBoundaries(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("软件发布", "軟體發表")]
    [InlineData("发布响应式编程教程", "發表回應式程式設計課程")]
    [InlineData("发布数据库迁移脚本", "發表資料庫遷移指令碼")]
    [InlineData("发布公告", "發表公告")]
    [InlineData("发布新版本", "發表新版本")]
    public void Converter_CnToTw2_DocumentsCurrentPublishReleaseBehavior(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("千钧一发", "千鈞一髮")]
    [InlineData("一触即发", "一觸即發")]
    [InlineData("百发百中", "百發百中")]
    [InlineData("爆发发布", "爆發發表")]
    public void Converter_CnToTw2_PreservesFaHairIdiomsAndPipelineForms(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    private static LocalePreset CreatePreset()
    {
        var from = new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["from"] = DictGroup.FromEntries(new[] { new DictEntry("a", "b") }),
        };

        var to = new Dictionary<string, DictGroup>(StringComparer.Ordinal)
        {
            ["to"] = DictGroup.FromEntries(new[] { new DictEntry("b", "c") }),
        };

        return new LocalePreset(from, to);
    }
}
