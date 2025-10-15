using Filemate.Core.Common;
using Filemate.Core.Renamer.TextProcessors;

namespace Filemate.Core.Renamer;

/// <summary>
/// Creates a Renamer with predefined settings.
/// </summary>
public class RenamerBuilder()
{
    string? _rootPath;
    TargetFileOrFolder _targetFileOrFolder = TargetFileOrFolder.None;
    bool _recursive = false;
    List<string> _extensionsToIgnore = new List<string>();
    bool _sanitizeFileNames = false;
    bool _isDebugMode = false;

    readonly Dictionary<string, ITextProcessorCore> _textProcessors = new();
        
    public RenamerBuilder SetRootPath(string rootPath)
    {
        _rootPath = rootPath;
        return this;
    }


    public RenamerBuilder ConfigureProcessor<TProcessor>(Action<TProcessor> configure)
        where TProcessor : class, ITextProcessor
    {
        string textProcessorName = TProcessor.GetName();

        // Get or create the processor instance
        if (!_textProcessors.TryGetValue(textProcessorName, out var processor))
        {
            processor = TProcessor.Create();
            _textProcessors[textProcessorName] = processor;
        }

        // Invoke the user's configuration logic on the specific processor type
        configure((TProcessor)processor);

        return this;
    }

    public RenamerBuilder SetRecursive(bool recursiveMode)
    {
        _recursive = recursiveMode;
        return this;
    }

    public RenamerBuilder SetExtensionToIgnore(string extension)
    {
        string ext = extension.Replace(".", "");
        ext = "." + ext;
        _extensionsToIgnore.Add(ext);
        
        return this;
    }

    public RenamerBuilder SetDoRenameFiles()
    {
        _targetFileOrFolder |= TargetFileOrFolder.File;
        return this;
    }

    public RenamerBuilder SetDoRenameFolders()
    {
        _targetFileOrFolder |= TargetFileOrFolder.Folder;
        return this;
    }

    public RenamerBuilder SetSanitizeFileNames()
    {
        _sanitizeFileNames = true;
        return this;
    }

    public RenamerBuilder SetDebugMode() 
    {
        _isDebugMode = true;
        return this;
    }

    public Renamer? Build()
    {
        if (_rootPath == null)
        {
            Console.WriteLine("Error: Set Root Path before start!");
            return null;
        }
        if (_targetFileOrFolder == TargetFileOrFolder.None)
        {
            Console.WriteLine("Error: Select if we rename Files, Folders, or Both.");
            return null;
        }
        if (_rootPath == null)
        {
            Console.WriteLine("Error: Set Root Path!");
            return null;
        }

        Console.WriteLine(string.Join(", ", _textProcessors.Keys.ToList()));
        List<ITextProcessorCore> textProcessors = _textProcessors.Values.ToList();

        return new Renamer(
            textProcessors: textProcessors,
            rootPath: _rootPath,
            sanitizeFileNames: _sanitizeFileNames,
            targetFileOrFolder: _targetFileOrFolder,
            recursive: _recursive,
            extensionsToIgnore: _extensionsToIgnore,
            isDebugMode: _isDebugMode
        );
    }

}
