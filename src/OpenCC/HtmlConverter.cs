using System.Xml.Linq;

namespace OpenCC;

public sealed class HtmlConverter
{
    private readonly Func<string, string> _converter;
    private readonly XContainer _rootNode;
    private readonly string _fromLangTag;
    private readonly string _toLangTag;

    private readonly Dictionary<XText, string> _originalText = new();
    private readonly Dictionary<XElement, string> _originalMetaContent = new();
    private readonly Dictionary<XElement, string> _originalImgAlt = new();
    private readonly Dictionary<XElement, string> _originalInputValue = new();
    private readonly HashSet<XElement> _langChanged = new();

    public HtmlConverter(Func<string, string> converter, XContainer rootNode, string fromLangTag, string toLangTag)
    {
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        _rootNode = rootNode ?? throw new ArgumentNullException(nameof(rootNode));
        _fromLangTag = fromLangTag ?? throw new ArgumentNullException(nameof(fromLangTag));
        _toLangTag = toLangTag ?? throw new ArgumentNullException(nameof(toLangTag));
    }

    public void Convert()
    {
        ConvertNode(_rootNode, false);
    }

    public void Restore()
    {
        foreach (var element in _langChanged)
        {
            element.SetAttributeValue("lang", _fromLangTag);
        }

        foreach (var (node, original) in _originalText)
        {
            node.Value = original;
        }

        foreach (var (element, original) in _originalMetaContent)
        {
            element.SetAttributeValue("content", original);
        }

        foreach (var (element, original) in _originalImgAlt)
        {
            element.SetAttributeValue("alt", original);
        }

        foreach (var (element, original) in _originalInputValue)
        {
            element.SetAttributeValue("value", original);
        }
    }

    private void ConvertNode(XNode node, bool langMatched)
    {
        if (node is XElement element)
        {
            if (HasIgnoreClass(element))
            {
                return;
            }

            var langAttr = (string?)element.Attribute("lang") ?? string.Empty;
            if (string.Equals(langAttr, _fromLangTag, StringComparison.Ordinal))
            {
                langMatched = true;
                if (_langChanged.Add(element))
                {
                    element.SetAttributeValue("lang", _toLangTag);
                }
            }
            else if (!string.IsNullOrEmpty(langAttr))
            {
                langMatched = false;
            }

            if (langMatched)
            {
                var tagName = element.Name.LocalName;
                if (tagName.Equals("SCRIPT", StringComparison.OrdinalIgnoreCase)
                    || tagName.Equals("STYLE", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                if (tagName.Equals("META", StringComparison.OrdinalIgnoreCase))
                {
                    var nameAttr = (string?)element.Attribute("name");
                    if (nameAttr != null
                        && (nameAttr.Equals("description", StringComparison.OrdinalIgnoreCase)
                            || nameAttr.Equals("keywords", StringComparison.OrdinalIgnoreCase)))
                    {
                        var contentAttr = element.Attribute("content");
                        if (contentAttr != null)
                        {
                            if (!_originalMetaContent.ContainsKey(element))
                            {
                                _originalMetaContent[element] = contentAttr.Value;
                            }

                            contentAttr.Value = _converter(_originalMetaContent[element]);
                        }
                    }
                }
                else if (tagName.Equals("IMG", StringComparison.OrdinalIgnoreCase))
                {
                    var altAttr = element.Attribute("alt");
                    if (altAttr != null)
                    {
                        if (!_originalImgAlt.ContainsKey(element))
                        {
                            _originalImgAlt[element] = altAttr.Value;
                        }

                        altAttr.Value = _converter(_originalImgAlt[element]);
                    }
                }
                else if (tagName.Equals("INPUT", StringComparison.OrdinalIgnoreCase))
                {
                    var typeAttr = (string?)element.Attribute("type");
                    if (typeAttr != null && typeAttr.Equals("button", StringComparison.OrdinalIgnoreCase))
                    {
                        var valueAttr = element.Attribute("value");
                        if (valueAttr != null)
                        {
                            if (!_originalInputValue.ContainsKey(element))
                            {
                                _originalInputValue[element] = valueAttr.Value;
                            }

                            valueAttr.Value = _converter(_originalInputValue[element]);
                        }
                    }
                }
            }

            foreach (var child in element.Nodes())
            {
                if (child is XText textNode && langMatched)
                {
                    if (!_originalText.ContainsKey(textNode))
                    {
                        _originalText[textNode] = textNode.Value;
                    }

                    textNode.Value = _converter(_originalText[textNode]);
                }
                else
                {
                    ConvertNode(child, langMatched);
                }
            }
        }
        else if (node is XDocument document)
        {
            foreach (var child in document.Nodes())
            {
                ConvertNode(child, langMatched);
            }
        }
        else if (node is XText textNode && langMatched)
        {
            if (!_originalText.ContainsKey(textNode))
            {
                _originalText[textNode] = textNode.Value;
            }

            textNode.Value = _converter(_originalText[textNode]);
        }
    }

    private static bool HasIgnoreClass(XElement element)
    {
        var classAttr = (string?)element.Attribute("class");
        if (string.IsNullOrWhiteSpace(classAttr))
        {
            return false;
        }

        var classes = classAttr.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        return classes.Contains("ignore-opencc", StringComparer.Ordinal);
    }
}