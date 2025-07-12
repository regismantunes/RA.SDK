using System.IO;
using System.Security.AccessControl;

namespace RA.Utilities.Folder
{
    public class FolderSearcherOptions
    {
        public string InitialPath { get; set; }

        public int Taken { get; set; }

        public FileSystemRights? FileSystemRights { get; set; }

        public FileAttributes AttributesToSkip { get; set; } = FileAttributes.None;

        public bool ReturnSpecialDirectories { get; set; } = true;

        public string DirectorySearchPattern { get; set; } = "*";

        public string FileSearchPattern { get; set; } = "*";

        public bool FindForFiles { get; set; } = true;

        public bool FindForDirectories { get; set; } = true;
    }
}
