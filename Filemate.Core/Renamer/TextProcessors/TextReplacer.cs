namespace Filemate.Core.Renamer.TextProcessors;

public class TextReplacer : ITextProcessor
{
    const string _name = "TextReplacer";
    List<(string Pattern, string Replacement)> _rules = new();

	public TextReplacer()
	{

	}

    public static string GetName() => _name;

    public void AddRule(string textPattern, string replacement)
	{
		if (textPattern != string.Empty)
            _rules.Add((textPattern, replacement));
	}

    public string Run(string sourceText)
    {
        string updatedText = sourceText;

        foreach ((string pattern, string replacement) in _rules)
            updatedText = updatedText.Replace(pattern, replacement);

        return updatedText;
    }

    public static ITextProcessor Create()
    {
        return new TextReplacer();
    }

}
