namespace Filemate.Core.Common;

[Flags]
public enum TargetFileOrFolder
{
    None = 0,
    File = 1 << 0,
    Folder = 1 << 1,
    Both = File | Folder,
}