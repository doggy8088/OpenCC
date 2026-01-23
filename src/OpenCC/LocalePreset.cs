namespace OpenCC;

public sealed class LocalePreset
{
    public LocalePreset(IReadOnlyDictionary<string, DictGroup> from, IReadOnlyDictionary<string, DictGroup> to)
    {
        From = from ?? throw new ArgumentNullException(nameof(from));
        To = to ?? throw new ArgumentNullException(nameof(to));
    }

    public IReadOnlyDictionary<string, DictGroup> From { get; }

    public IReadOnlyDictionary<string, DictGroup> To { get; }
}