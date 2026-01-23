using System.Xml.Linq;

using Xunit;

namespace OpenCC.Tests;

public class HtmlConverterTests
{
    [Fact]
    public void Convert_ChangesTextAndAttributes_WhenLangMatches()
    {
        var document = BuildDocument();
        var converter = new HtmlConverter(s => s.ToUpperInvariant(), document, "zh", "zh-Hant");

        converter.Convert();

        var root = document.Root!;
        Assert.Equal("zh-Hant", (string?)root.Attribute("lang"));

        Assert.Equal("HELLO", document.Descendants("p").Single().Value);
        Assert.Equal("HELLO", GetMetaContent(document, "description"));
        Assert.Equal("KEYWORDS", GetMetaContent(document, "keywords"));
        Assert.Equal("ignore", GetMetaContent(document, "other"));
        Assert.Equal("HELLO", document.Descendants("img").Single().Attribute("alt")?.Value);

        var button = document.Descendants("input").First(e => (string?)e.Attribute("type") == "button");
        Assert.Equal("HELLO", (string?)button.Attribute("value"));

        var textInput = document.Descendants("input").First(e => (string?)e.Attribute("type") == "text");
        Assert.Equal("hello", (string?)textInput.Attribute("value"));

        var ignore = document.Descendants("div").Single(e => ((string?)e.Attribute("class"))!.Contains("ignore-opencc", StringComparison.Ordinal));
        Assert.Equal("hello", ignore.Value);

        var span = document.Descendants("span").Single();
        Assert.Equal("en", (string?)span.Attribute("lang"));
        Assert.Equal("hello", span.Value);

        Assert.Equal("hello", document.Descendants("script").Single().Value);
        Assert.Equal("hello", document.Descendants("style").Single().Value);
    }

    [Fact]
    public void Restore_RevertsChanges()
    {
        var document = BuildDocument();
        var converter = new HtmlConverter(s => s.ToUpperInvariant(), document, "zh", "zh-Hant");

        converter.Convert();
        converter.Restore();

        var root = document.Root!;
        Assert.Equal("zh", (string?)root.Attribute("lang"));
        Assert.Equal("hello", document.Descendants("p").Single().Value);
        Assert.Equal("hello", GetMetaContent(document, "description"));
        Assert.Equal("keywords", GetMetaContent(document, "keywords"));
        Assert.Equal("ignore", GetMetaContent(document, "other"));
        Assert.Equal("hello", document.Descendants("img").Single().Attribute("alt")?.Value);

        var button = document.Descendants("input").First(e => (string?)e.Attribute("type") == "button");
        Assert.Equal("hello", (string?)button.Attribute("value"));
    }

    private static XDocument BuildDocument()
    {
        var xml = @"<html lang='zh'>
  <head>
    <meta name='description' content='hello' />
    <meta name='keywords' content='keywords' />
    <meta name='other' content='ignore' />
  </head>
  <body>
    <p>hello</p>
    <img alt='hello' />
    <input type='button' value='hello' />
    <input type='text' value='hello' />
    <div class='note ignore-opencc'>hello</div>
    <span lang='en'>hello</span>
    <script>hello</script>
    <style>hello</style>
  </body>
</html>";

        return XDocument.Parse(xml);
    }

    private static string? GetMetaContent(XDocument document, string name)
    {
        return document
            .Descendants("meta")
            .Single(e => string.Equals((string?)e.Attribute("name"), name, StringComparison.OrdinalIgnoreCase))
            .Attribute("content")
            ?.Value;
    }
}