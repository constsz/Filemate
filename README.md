# Filemate 📂

A cross-platform desktop utility designed for batch renaming of files and folders with a flexible, rule-based system.

## About The Project

Filemate was created to simplify the often tedious task of cleaning up and organizing large sets of files. Whether you're a photographer organizing photos, a developer managing assets, or anyone needing to tidy up a download folder, Filemate provides the tools to do it quickly and efficiently.

The application is built with a clean separation between its core renaming logic and the user interface, making it robust and maintainable.

## Features

- **Flexible Targeting:** Rename files, folders, or both.
- **Recursive Processing:** Process files in subdirectories with a single click.
- **Dynamic Rule Engine:** Chain multiple "Text Processors" to perform complex renaming operations in a single pass:
  - **Replace:** Simple text substitution.
  - **Delete:** Remove specific phrases or characters.
  - **Regex Replace:** Use the power of regular expressions for advanced pattern matching.
  - **Change Case:** Convert filenames to Title Case, lowercase, or UPPERCASE.
  - **Add Prefix/Suffix:** Easily add text to the beginning or end of filenames.
- **Extension Filtering:** Ignore specific file types (e.g., `.exe`, `.dll`) from the renaming process.
- **Filename Sanitizer:** Automatically remove illegal characters to ensure cross-platform compatibility.
- **Preset System:** Save your complex renaming configurations and load them later with one click.
- **Debug Mode:** Perform a "dry run" to see what changes will be made before committing to them.

## Technology Stack

- **.NET 9** and **C# 12**
- **Avalonia UI:** For a modern, cross-platform desktop user interface.
- **CommunityToolkit.Mvvm:** For a clean and robust implementation of the MVVM pattern.

## Project Structure

The solution is intentionally split into two main projects to enforce separation of concerns:

- `Filemate.Core`: A class library containing all the business logic for file discovery, text processing, and renaming. It has no dependency on any UI framework.
- `Filemate.UI`: The Avalonia desktop application that provides the user interface. It consumes the `Filemate.Core` library.


## License

Distributed under the MIT License. See `LICENSE` for more information.
