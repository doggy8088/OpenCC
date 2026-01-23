using OpenCC.Internal;

namespace OpenCC.Presets;

public static class Cn2t
{
    public static LocalePreset Locale => LocalePresets.Cn2t;

    public static Func<string, string> Converter(ConverterOptions options)
    {
        return OpenCC.ConverterBuilder(Locale)(options);
    }

    public static Func<string, string> Converter(string from, string to)
    {
        return Converter(new ConverterOptions(from, to));
    }
}