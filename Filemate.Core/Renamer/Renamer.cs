using Filemate.Core.Common;
using Filemate.Core.Renamer.TextProcessors;

namespace Filemate.Core.Renamer;

/// <summary>
/// Renamer allows to recursively manipulate names of files and folders
/// - Folder, File (extension: any|specific)
/// - Functions: Replace Text, Delete Text.
/// - Recursive renaming across the file/folder hierarchy
/// </summary>
public class Renamer
{
    string _rootPath;
    TargetFileOrFolder _targetFileOrFolder = TargetFileOrFolder.None;
    bool _recursive = false;
    bool _sanitizeFileNames = false;
    bool _isDebugMode = false;
    List<string> _extensionsToIgnore = new List<string>();
    List<ITextProcessorCore> _textProcessors;

    public Renamer(
        List<ITextProcessorCore> textProcessors,
        TargetFileOrFolder targetFileOrFolder,
        string rootPath,
        bool recursive,
        List<string> extensionsToIgnore,
        bool sanitizeFileNames,
        bool isDebugMode
        )
    {
        _textProcessors = textProcessors;
        _targetFileOrFolder = targetFileOrFolder;
        _rootPath = rootPath;
        _recursive = recursive;
        _extensionsToIgnore = extensionsToIgnore;
        _sanitizeFileNames = sanitizeFileNames;
        _isDebugMode = isDebugMode;
    }
        
    public void Run()
    {
        IterateEntries(_rootPath);
    }

    void IterateEntries(string path)
    {
        // Get list of files and folders
        foreach (string sourceFilePath in Directory.EnumerateFileSystemEntries(path))
        {
            FileAttributes attributes = File.GetAttributes(sourceFilePath);
            bool entryIsFolder = attributes.HasFlag(FileAttributes.Directory);

            // Check if current file's extension in the list of extensions to ignore
            if (_extensionsToIgnore.Contains(Path.GetExtension(sourceFilePath)))
            {
                continue;
            }

            string processedFilePath = ProcessText(sourceFilePath);
            
            // If any changes should be made to original name
            bool isNameHasChanged = processedFilePath == sourceFilePath;
           
            // Check if this file/folder can be processed
            if (entryIsFolder)
            {
                if (_recursive)
                    IterateEntries(sourceFilePath);

                if (_targetFileOrFolder.HasFlag(TargetFileOrFolder.Folder) && !isNameHasChanged)
                {
                    Console.WriteLine("\n--- Directory: ---");
                    Console.WriteLine($"OLD: {sourceFilePath}");
                    Console.WriteLine($"NEW: {processedFilePath}");

                    if (!_isDebugMode)
                    {
                        try
                        {
                            Directory.Move(sourceFilePath, processedFilePath);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"(!) Error : Renamer : Can't rename directory {sourceFilePath}");
                            Console.WriteLine(e);
                        }
                    }
                }
            // Processing File
            } else
            {
                if (_targetFileOrFolder.HasFlag(TargetFileOrFolder.File) && !isNameHasChanged)
                {
                    Console.WriteLine("\n--- File: ---");
                    Console.WriteLine($"OLD: {sourceFilePath}");
                    Console.WriteLine($"NEW: {processedFilePath}");
                    
                    if (!_isDebugMode)
                    {
                        try
                        {   
                            File.Move(sourceFilePath, processedFilePath);
                        } catch (Exception e)
                        {
                            Console.WriteLine($"(!) Error : Renamer : Can't rename file {sourceFilePath}");
                            Console.WriteLine(e);
                        }
                    }
                }
            }
        }
    }

    string ProcessText(string entry)
    {
        (
            string directory, 
            string filenameWithoutExtension, 
            string extension
        ) = DeconstructPath(entry);

        string newFilename = filenameWithoutExtension;
        
        foreach (ITextProcessorCore textProcessor in _textProcessors)
        {
            newFilename = textProcessor.Run(newFilename);
        }

        if (_sanitizeFileNames)
            newFilename = TextSanitizer.Sanitize(newFilename);

        string newFilenameWithExt = newFilename + extension;

        return Path.Combine(directory, newFilenameWithExt);        
    }

    (string directory, string filenameWithoutExtension, string extension) DeconstructPath(string entry)
    {
        // Deconstruct the path
        string? directory = Path.GetDirectoryName(entry);
        string? filenameWithoutExtension = Path.GetFileNameWithoutExtension(entry);
        string? extension = Path.GetExtension(entry);

        if (directory == null || filenameWithoutExtension == null || extension == null)
            throw new FileNotFoundException();

        return (directory, filenameWithoutExtension, extension);
    }
}

