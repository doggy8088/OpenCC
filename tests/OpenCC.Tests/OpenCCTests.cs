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
    [InlineData("汉语", "漢語")]
    [InlineData("台湾", "台灣")]
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
    [InlineData("项目", "項目")]
    [InlineData("項目", "項目")]
    [InlineData("清单项目", "清單項目")]
    [InlineData("每个项目", "每個項目")]
    public void Converter_CnToTw2_PreservesGenericItemTerms(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("命令行工具", "命令列工具")]
    [InlineData("数据结构数据库", "資料結構資料庫")]
    [InlineData("响应式编程响应头", "回應式程式設計回應標頭")]
    [InlineData("进程间通信和多线程", "行程間通訊和多執行緒")]
    [InlineData("文件名和文件系统", "檔名和檔案系統")]
    [InlineData("文件描述符和函数调用", "檔案描述子和函式呼叫")]
    [InlineData("渲染管线和内存分配", "算繪管線和記憶體配置")]
    [InlineData("网络栈和网络适配器", "網路堆疊和網路介面卡")]
    public void Converter_CnToTw2_PrefersLongestTaiwanTechTerms(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("檔名和檔案系統", "文件名和文件系统")]
    [InlineData("檔案描述子和函式呼叫", "文件描述符和函数调用")]
    [InlineData("算繪管線和記憶體配置", "渲染管线和内存分配")]
    [InlineData("網路堆疊和網路介面卡", "网络栈和网络适配器")]
    public void Converter_Tw2ToCn_ConvertsTaiwanTechPhrasesBackToSimplified(string source, string expected)
    {
        var converter = OpenCC.Converter("tw2", "cn");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("專案", "专案")]
    [InlineData("專案設定", "项目设置")]
    [InlineData("專案目錄", "项目目录")]
    [InlineData("專案管理", "项目管理")]
    [InlineData("專案資料夾", "项目文件夹")]
    [InlineData("專案的", "项目的")]
    public void Converter_Tw2ToCn_PreservesStandaloneProjectAndConvertsCompounds(string source, string expected)
    {
        var converter = OpenCC.Converter("tw2", "cn");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("Web 平台库", "Web 平台函式庫")]
    [InlineData("for 循环和while 循环", "for 迴圈和while 迴圈")]
    [InlineData("控制台打印日志", "輸出到 Console記錄")]
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
    [InlineData("类。", "類別。")]
    [InlineData("（视频）", "（影片）")]
    public void Converter_CnToTw2_ConvertsAcrossPunctuationBoundaries(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("项目文件夹", "專案資料夾")]
    [InlineData("项目的", "專案的")]
    [InlineData("项目目录", "專案目錄")]
    [InlineData("项目管理", "專案管理")]
    [InlineData("项目设置", "專案設定")]
    public void Converter_CnToTw2_ConvertsProjectContextCompounds(string source, string expected)
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

    [Theory]
    [InlineData("台湾台球桌", "台灣撞球桌")]
    [InlineData("折叠粘土", "折疊黏土")]
    [InlineData("台球桌", "撞球桌")]
    [InlineData("鼠标事件", "滑鼠事件")]
    [InlineData("菜单", "選單")]
    [InlineData("链接", "連結")]
    [InlineData("账户", "帳戶")]
    [InlineData("账号", "帳號")]
    public void Converter_CnToTw2_ConvertsTaiwanRegionalAndUiTerms(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("默认用户界面支持数据库和网络请求。", "預設使用者介面支援資料庫和網路請求。")]
    [InlineData("命令行工具加载配置文件。", "命令列工具載入組態檔。")]
    [InlineData("创建软件项目目录和项目设置。", "建立軟體專案目錄和專案設定。")]
    [InlineData("调试器显示调用堆栈和断点。", "偵錯工具顯示呼叫堆疊和中斷點。")]
    [InlineData("程序员重构代码库并发布版本。", "程式設計師重構程式碼庫並發表版本。")]
    [InlineData("响应式编程教程包含缓存策略。", "回應式程式設計課程包含快取策略。")]
    [InlineData("我们的程序使用数据库和线程。", "我們的應用程式使用資料庫和執行緒。")]
    [InlineData("请打开用户界面并刷新屏幕。", "請打開使用者介面並重新整理螢幕。")]
    [InlineData("API 返回默认值和错误消息。", "API 返回預設值和錯誤訊息。")]
    [InlineData("在 while 循环中调用函数。", "在 while 迴圈中呼叫函式。")]
    [InlineData("for 循环处理数组和字符串。", "for 迴圈處理陣列和字串。")]
    [InlineData("配置文件位于项目目录。", "組態檔位於專案目錄。")]
    [InlineData("命令行工具输出调试信息。", "命令列工具輸出偵錯資訊。")]
    [InlineData("代码审查发现内存泄漏。", "程式碼審查發現記憶體洩漏。")]
    public void Converter_CnToTw2_ConvertsSentenceLevelTaiwanTechTerms(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("接口和抽象类", "介面和抽象類別")]
    [InlineData("文档注释", "文件註解")]
    [InlineData("监控摄像头", "監視攝影機")]
    [InlineData("赛博朋克", "賽博龐克")]
    [InlineData("黑客帝国", "駭客任務")]
    [InlineData("星球大战", "星際大戰")]
    [InlineData("指环王", "魔戒")]
    [InlineData("泰坦尼克号", "鐵達尼號")]
    [InlineData("星际穿越", "星際效應")]
    [InlineData("钢铁侠", "鋼鐵人")]
    [InlineData("单反相机", "單眼相機")]
    public void Converter_CnToTw2_ConvertsAdditionalReportTerms(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("平台", "平台")]          // 裸詞：cn 平台 → tw2 平台，不應變成 平臺
    [InlineData("跨平台", "跨平台")]      // 常見複合詞
    [InlineData("软件平台", "軟體平台")]
    [InlineData("作业平台", "作業平台")]
    [InlineData("Web 平台库", "Web 平台函式庫")]   // 庫仍正確展開為函式庫
    [InlineData("全平台库列表", "全平台函式庫列表")]
    [InlineData("原生平台库", "原生平台函式庫")]
    public void Converter_CnToTw2_UsesPingTaiNotPingTai(string source, string expected)
    {
        var converter = OpenCC.Converter("cn", "tw2");

        Assert.Equal(expected, converter(source));
    }

    [Theory]
    [InlineData("跨平台", "跨平台")]      // tw2→cn 逆向：平台 映射回 平台
    [InlineData("軟體平台", "软件平台")]
    public void Converter_Tw2ToCn_MapsPingTaiBackToPingTai(string source, string expected)
    {
        var converter = OpenCC.Converter("tw2", "cn");

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
