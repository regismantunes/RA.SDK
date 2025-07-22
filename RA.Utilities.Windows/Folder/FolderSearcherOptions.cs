using System.IO;
using System.Security.AccessControl;

namespace RA.Utilities.Windows.Folder
{
    /// <summary>
    /// Provides options for configuring the folder searcher.
    /// </summary>
    public record FolderSearcherOptions
    {
        /// <summary>
        /// Indicates the path where the search should occours.
        /// </summary>
        public required string InitialPath { get; init; }

        /// <summary>
        /// Indicates the roles for search with security parameters for files
        /// </summary>
        public SecurityOptions? SecurityOptionsForFiles { get; init; }

        /// <summary>
        /// Indicates the roles for search with security parameters for directories
        /// </summary>
        public SecurityOptions? SecurityOptionsForDirectories { get; init; }

        /// <summary>
        /// Indicates whether special directories (like "System Volume Information") should be included in the search.
        /// </summary>
        public bool ReturnSpecialDirectories { get; init; } = true;

        /// <summary>
        /// Indicates the search pattern for directories.
        /// </summary>
        public string DirectorySearchPattern { get; init; } = "*";

        /// <summary>
        /// Indicates the search pattern for files.
        /// </summary>
        public string FileSearchPattern { get; init; } = "*";

        /// <summary>
        /// Indicates if the onFind callback should be called for files.
        /// </summary>
        public bool FindForFiles { get; init; } = true;

        /// <summary>
        /// Indicates if the onFind callback should be called for directories.
        /// </summary>
        public bool FindForDirectories { get; init; } = false;
    }
}
