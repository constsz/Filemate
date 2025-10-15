namespace Filemate.Core.Renamer.TextProcessors;

/// <summary>
/// What an instance of the TextProcessor does
/// </summary>
public interface ITextProcessorCore
{
    public string Run(string sourceText);
}

/// <summary>
/// Factory Inherits that inherits Core interface to provide 
/// additional functionality to create an Instance of the TextProcessor.
/// </summary>
public interface ITextProcessor : ITextProcessorCore
{
    public static abstract string GetName();
    public static abstract ITextProcessor Create();
}
