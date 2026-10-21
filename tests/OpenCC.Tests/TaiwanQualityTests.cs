using System.Runtime.CompilerServices;

using Xunit;

namespace OpenCC.Tests;

public class TaiwanQualityTests
{
    private static string Fixture([CallerFilePath] string source = "") =>
        Path.Combine(Path.GetDirectoryName(source)!, "taiwan-quality.tsv");

    [Theory]
    [InlineData("tw2")]
    [InlineData("twp")]
    public void Converter_TaiwanQuality_IsConsistentAndIdempotent(string locale)
    {
        var convert = OpenCC.Converter(new ConverterOptions("cn", locale));
        foreach (var line in File.ReadLines(Fixture()))
        {
            var parts = line.Split('\t');
            if (parts[0] != "both" && parts[0] != locale) continue;
            Assert.Equal(parts[2], convert(parts[1]));
            Assert.Equal(parts[2], convert(parts[2]));
            Assert.DoesNotContain("賬", parts[2]);
            Assert.DoesNotContain("羣", parts[2]);
        }
    }
}