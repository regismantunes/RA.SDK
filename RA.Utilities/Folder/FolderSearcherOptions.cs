using System.IO;
using System.Security.AccessControl;

namespace RA.Utilities.Folder
{
    public record FolderSearcherOptions
    {
        public required string InitialPath { get; init; }

        public FileSystemRights? FileSystemRights { get; init; }

        public FileAttributes AttributesToSkip { get; init; } = FileAttributes.None;

        public bool ReturnSpecialDirectories { get; init; } = true;

        public string DirectorySearchPattern { get; init; } = "*";

        public string FileSearchPattern { get; init; } = "*";

        public bool FindForFiles { get; init; } = true;

        public bool FindForDirectories { get; init; } = false;
    }
}
