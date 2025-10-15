namespace Filemate.Core.Renamer.TextProcessors;

public class TextDeleter : ITextProcessor
{
    const string _name = "TextDeleter";

    List<string> _rules = new();

    public TextDeleter()
    {

    }

    public static string GetName() => _name;

    public void AddRule(string textToDelete)
    {
        if (textToDelete != string.Empty)
            _rules.Add(textToDelete);
    }

    public string Run(string sourceText)
    {
        string updatedText = sourceText;
        
        foreach (string textPattern in _rules)
            updatedText = updatedText.Replace(textPattern, "");

        return updatedText;
    }

    public static ITextProcessor Create()
    {
        return new TextDeleter();
    }

}
