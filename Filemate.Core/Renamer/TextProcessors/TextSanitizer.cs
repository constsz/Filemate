using System.Text.RegularExpressions;

namespace Filemate.Core.Renamer.TextProcessors;

public static class TextSanitizer
{
    const string _name = "TextSanitizer";
    private static readonly List<(string Pattern, string Replacement)> _rules = new()
    {
        // 1. Replace any character that is not a letter, number, underscore, hyphen, dot, or space with a single space.
        // This is the main sanitization step.
        (@"[^a-zA-Z0-9_. -]+", " "),

        // 2. Collapse multiple hyphens or underscores into a single one.
        // e.g., "file---name" -> "file-name", "file___name" -> "file_name"
        (@"[-_]{2,}", "-"), 
        
        // 3. Remove any space that is immediately followed by a file extension dot.
        // This fixes the "photo .jpg" issue. (?=\.) is a positive lookahead.
        (@" \.(?=[a-zA-Z0-9]+$)", "."),

        // 4. Collapse multiple spaces into a single space.
        // This cleans up spaces introduced by the first rule.
        (@" {2,}", " "),
    };

    public static string GetName() => _name;

    /// <summary>
    /// Cleans a string to be a safe and valid file name.
    /// It removes invalid characters, trims whitespace, and handles common edge cases.
    /// </summary>
    /// <param name="sourceText">The original text to sanitize.</param>
    /// <returns>A sanitized string suitable for use as a file name.</returns>
    public static string Sanitize(string sourceText)
    {
        if (string.IsNullOrWhiteSpace(sourceText))
        {
            return string.Empty;
        }

        string currentText = sourceText;

        foreach (var (pattern, replacement) in _rules)
        {
            currentText = Regex.Replace(currentText, pattern, replacement);
        }

        // 5. Trim leading and trailing whitespace, which is crucial for file names.
        currentText = currentText.Trim();

        // Optional: Handle the case where the name becomes empty or just an extension.
        if (string.IsNullOrWhiteSpace(Path.GetFileNameWithoutExtension(currentText)))
        {
            // You could return a default name, or handle it as an error.
            // For example, prepending a default name:
            // return "sanitized_file" + Path.GetExtension(currentText);
            return string.Empty; // Or return a sensible default like "Untitled"
        }

        return currentText;
    }
}