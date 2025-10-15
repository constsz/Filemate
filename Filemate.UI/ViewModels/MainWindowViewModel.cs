using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Filemate.Core.Renamer;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Filemate.UI.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    // === PROPERTIES FOR UI STATE ===

    // --- Target Selection ---
    [ObservableProperty]
    private string _folderPath = null!;


    // --- Checkboxes ---

    [ObservableProperty]
    private bool _targetFiles;

    [ObservableProperty]
    private bool _targetFolders;

    [ObservableProperty]
    private bool _isRecursive = true;

    // General Options 
    [ObservableProperty]
    private bool _sanitize;

    [ObservableProperty]
    private bool _debugMode;


    // --- Lists ---

    // This collection is used for dynamic list of processors 
    public ObservableCollection<TextProcessorViewModel> TextProcessors { get; } = new();

    // Ignored extensions. 
    // Bind the ListBox/Items control to this collection
    public ObservableCollection<string> IgnoredExtensions { get; } = new();

    // Used by the TextBox that is used to add a new extension
    [ObservableProperty]
    private string _newExtensionToAdd = null!;




    // === BUTTONS | COMMANDS FOR UI ACTIONS ===
    
    [RelayCommand]
    private void AddIgnoredExtension()
    {
        if (!string.IsNullOrWhiteSpace(NewExtensionToAdd) && !IgnoredExtensions.Contains(NewExtensionToAdd))
        {
            // adding this will update UI automatically because it is `ObservableCollection`
            IgnoredExtensions.Add(NewExtensionToAdd);
        }
        NewExtensionToAdd = string.Empty; // reset input box
    }

    [RelayCommand]
    private void RemoveIgnoredExtension(string extension)
    {
        if (!string.IsNullOrWhiteSpace(extension))
        {
            IgnoredExtensions.Remove(extension);
        }
    }

    [RelayCommand]
    private void AddTextProcessor()
    {
        // Add new, default text processor ViewModel into the collection
        TextProcessors.Add(new TextProcessorViewModel());
    }

    [RelayCommand]
    private void RemoveTextProcessor(TextProcessorViewModel processor)
    {
        if (processor != null)
        {
            TextProcessors.Remove(processor);
        }
    }

    // Main button: Start the process
    // Using ui state to configure the builder and run it.
    [RelayCommand]
    private void StartRenaming()
    {
        var builder = new RenamerBuilder();

        builder.SetRootPath(FolderPath);

        builder.SetRecursive(IsRecursive);

        foreach (string ext in IgnoredExtensions)
        {
            builder.SetExtensionToIgnore(ext);
        }

        // Text Processors

        if (TargetFiles) builder.SetDoRenameFiles();
        if (TargetFolders) builder.SetDoRenameFolders();
        if (Sanitize) builder.SetSanitizeFileNames();
        if (DebugMode) builder.SetDebugMode();

        Renamer? renamer = builder.Build();

        if (renamer == null)
        {
            // Do some UI Error Message... Inspect why it returns null 
            return;
        }

        renamer.Run();

    }

}
