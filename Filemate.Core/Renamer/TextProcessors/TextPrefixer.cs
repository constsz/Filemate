namespace Filemate.Core.Renamer.TextProcessors;

public class TextPrefixer : ITextProcessor
{
    const string _name = "TextPrefixer";

    List<string> _rules = new();

    public TextPrefixer()
    {

    }

    public static string GetName() => _name;

    public void AddRule(string prefixTextToAdd)
    {
        if (prefixTextToAdd != string.Empty)
            _rules.Add(prefixTextToAdd);
    }

    public string Run(string sourceText)
    {
        string updatedText = sourceText;

        foreach (string prefixText in _rules)
            updatedText = prefixText + updatedText;

        return updatedText;
    }

    public static ITextProcessor Create()
    {
        return new TextPrefixer();
    }

}
