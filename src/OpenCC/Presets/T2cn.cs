using OpenCC.Internal;

namespace OpenCC.Presets;

public static class T2cn
{
    public static LocalePreset Locale => LocalePresets.T2cn;

    public static Func<string, string> Converter(ConverterOptions options)
    {
        return OpenCC.ConverterBuilder(Locale)(options);
    }

    public static Func<string, string> Converter(string from, string to)
    {
        return Converter(new ConverterOptions(from, to));
    }
}