using System.Text.RegularExpressions;

namespace Filemate.Core.Renamer.TextProcessors;

public class RegexReplacer : ITextProcessor
{
	const string _name = "RegexReplacer";
	List<(string Pattern, string Replacement)> _rules = new();

	public RegexReplacer()
	{

	}

	public static string GetName() => _name;

	public void AddRule(string regexPattern, string replacement)
	{
		if (regexPattern != string.Empty)
			_rules.Add((regexPattern, replacement));
	}

    /// <summary>
    /// Runs all registered regex replacement rules sequentially on the source text.
    /// </summary>
    /// <param name="sourceText">The original text to transform.</param>
    /// <returns>The transformed text after all rules have been applied.</returns>
    public string Run(string sourceText)
    {
        // If the input is null or there are no rules, there's nothing to do.
        if (sourceText is null || _rules.Count == 0)
        {
            return string.Empty;
        }

        string currentText = sourceText;

        // Apply each rule in the order it was added.
        // The output of one replacement becomes the input for the next.
        foreach (var (pattern, replacement) in _rules)
        {
            currentText = Regex.Replace(currentText, pattern, replacement);
        }

        return currentText;
    }

    public static ITextProcessor Create()
	{
		return new RegexReplacer();
	}

}
