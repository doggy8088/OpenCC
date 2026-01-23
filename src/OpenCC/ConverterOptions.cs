namespace OpenCC;

public sealed class ConverterOptions
{
    public ConverterOptions()
    {
    }

    public ConverterOptions(string from, string to)
    {
        From = from;
        To = to;
    }

    public string From { get; init; } = string.Empty;

    public string To { get; init; } = string.Empty;
}