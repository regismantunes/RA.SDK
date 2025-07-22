using System.IO;
using System.Security.AccessControl;

namespace RA.Utilities.Windows.Folder
{
    /// <summary>
    /// Provides options for configuring the security settings during folder searches.
    /// </summary>
    public record SecurityOptions
    {
        /// <summary>
        /// Indicates the SID of the user whose files or directories should be searched. If null, the search will be applyed for the current user.
        /// </summary>
        public string? UserSID { get; init; }

        /// <summary>
        /// Indicates the rights of the files or directories to be searched.
        /// </summary>
        public FileSystemRights? FileSystemRights { get; init; }

        /// <summary>
        /// Indicates the type of access control to be applied for FileSystemRights during the search.
        /// </summary>
        public AccessControlType AccessControlType { get; init; } = AccessControlType.Allow;

        /// <summary>
        /// Indicates the attributes of the files or directories to be look for during the search.
        /// </summary>
        public FileAttributes AttributesToFind { get; init; } = FileAttributes.None;

        /// <summary>
        /// Indicates the attributes of the files or directories to be skipped during the search.
        /// </summary>
        public FileAttributes AttributesToSkip { get; init; } = FileAttributes.None;
    }
}
