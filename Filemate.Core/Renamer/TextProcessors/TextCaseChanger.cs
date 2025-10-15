using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Filemate.Core.Renamer.TextProcessors;

/// <summary>
/// Defines the various text cases that can be applied.
/// </summary>
public enum TextCase
{
    // Common Typographical Cases
    SentenceCase,
    TitleCase,
    UpperCase,
    LowerCase,

    // Common Programming Cases
    CamelCase,
    PascalCase,
    SnakeCase,
    KebabCase,
}

/// <summary>
/// A text processor that changes the case of the input text.
/// </summary>
public class TextCaseChanger : ITextProcessor
{
    private const string _name = "Change Case";
    private TextCase _targetCase = TextCase.SentenceCase; // Default case

    /// <summary>
    /// Sets the desired case transformation to be applied.
    /// </summary>
    /// <param name="targetCase">The enum value representing the target text case.</param>
    public void SetCase(TextCase targetCase)
    {
        _targetCase = targetCase;
    }

    /// <inheritdoc/>
    public string Run(string sourceText)
    {
        if (string.IsNullOrWhiteSpace(sourceText))
        {
            return sourceText;
        }

        switch (_targetCase)
        {
            case TextCase.UpperCase:
                return sourceText.ToUpper();

            case TextCase.LowerCase:
                return sourceText.ToLower();

            case TextCase.SentenceCase:
                return ToSentenceCase(sourceText);

            case TextCase.TitleCase:
                // ToTitleCase works best on lowercase text.
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sourceText.ToLower());

            case TextCase.CamelCase:
                return ToCamelCase(sourceText);

            case TextCase.PascalCase:
                return ToPascalCase(sourceText);

            case TextCase.SnakeCase:
                return ToSnakeCase(sourceText);

            case TextCase.KebabCase:
                return ToKebabCase(sourceText);

            default:
                // Fallback for any unhandled enum values.
                return sourceText;
        }
    }

    #region Static Factory Members

    /// <inheritdoc/>
    public static string GetName() => _name;

    /// <inheritdoc/>
    public static ITextProcessor Create()
    {
        return new TextCaseChanger();
    }

    #endregion

    #region Private Case Conversion Helpers

    // Splits text into words, handling existing casing like camelCase or snake_case.
    private static string[] GetWords(string text)
    {
        // This regex splits on spaces, underscores, hyphens, and before capital letters.
        return Regex.Split(text, @"(?<=[a-z])(?=[A-Z])|[\s_-]+")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();
    }

    private static string ToSentenceCase(string text)
    {
        var lowerCaseText = text.ToLower();
        var sb = new StringBuilder(lowerCaseText);

        // Find the first letter and capitalize it.
        for (int i = 0; i < sb.Length; i++)
        {
            if (char.IsLetter(sb[i]))
            {
                sb[i] = char.ToUpper(sb[i]);
                break;
            }
        }
        return sb.ToString();
    }

    private static string ToCamelCase(string text)
    {
        var words = GetWords(text);
        if (words.Length == 0) return string.Empty;

        var firstWord = words[0].ToLower();
        var subsequentWords = words.Skip(1)
            .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower());

        return string.Concat(firstWord, string.Concat(subsequentWords));
    }

    private static string ToPascalCase(string text)
    {
        var words = GetWords(text);
        var capitalizedWords = words
            .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower());

        return string.Concat(capitalizedWords);
    }

    private static string ToSnakeCase(string text)
    {
        var words = GetWords(text);
        return string.Join("_", words).ToLower();
    }

    private static string ToKebabCase(string text)
    {
        var words = GetWords(text);
        return string.Join("-", words).ToLower();
    }

    #endregion
}