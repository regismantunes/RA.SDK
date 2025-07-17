using RA.Utilities.Windows;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace RA.Utilities.Folder
{
    /// <summary>
    /// Searches for files and directories in a specified folder based on the provided options.
    /// </summary>
    public class FolderSearcher(FolderSearcherOptions? options)
    {
        /// <summary>
        /// Initializes the FolderSearcher without options. The options should be set on SearchAsync method.
        /// </summary>
        public FolderSearcher()
            : this(null)
        { }

        /// <summary>
        /// Options setted for the folder searcher.
        /// </summary>
        public FolderSearcherOptions? Options { get; private set; } = options;

        /// <summary>
        /// Inicates whether the search is currently running.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Indicates the last path that was searched. This can be used to follow the search progress.
        /// </summary>
        public string LastPath { get; private set; } = string.Empty;
        
        private readonly object _locker = new();

        /// <summary>
        /// Initiates a search for files and directories based on the current options.
        /// </summary>
        /// <param name="onFind">
        /// Action to be called when a file or directory is found. The path of the found item will be passed as an argument.
        /// </param>
        public async Task SearchAsync(Action<string> onFind)
        {
            await SearchAsync(Options, onFind);
        }

        /// <summary>
        /// Initiates a search for files and directories based on the provided options.
        /// </summary>
        /// <param name="options">
        /// New options for the folder searcher.
        /// </param>
        /// <param name="onFind">
        /// Action to be called when a file or directory is found. The path of the found item will be passed as an argument.
        /// </param>
        public async Task SearchAsync(FolderSearcherOptions options, Action<string> onFind)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            ArgumentNullException.ThrowIfNull(onFind, nameof(onFind));
            
            lock (_locker)
            {
                if (IsRunning)
                    throw new InvalidOperationException("Search is already running.");

                IsRunning = true;
            }

            try
            {
                Options = options;
                await ExecuteSearchAsync(onFind);
            }
            finally
            {
                IsRunning = false;
            }
        }

        private async Task ExecuteSearchAsync(Action<string> onFind)
        {
            var currentUser = Identity.GetCurrentSID();
            var countTask = 0;
            var enumerationOptions = new EnumerationOptions
            {
                RecurseSubdirectories = false,
                AttributesToSkip = Options.AttributesToSkip,
                IgnoreInaccessible = true,
                ReturnSpecialDirectories = Options.ReturnSpecialDirectories
            };

            void searchInFolder(string path)
            {
                LastPath = path;

                if (Options.FindForDirectories)
                    onFind(path);

                if (Options.FindForFiles)
                {
                    try
                    {
                        var files = Directory.GetFiles(path, Options.FileSearchPattern, enumerationOptions);
                        foreach (var file in files)
                        {
                            var fileInfo = new FileInfo(file);
                            var rules = fileInfo.GetAccessControl()
                                                .GetAccessRules(true, true, typeof(SecurityIdentifier));
                            foreach (FileSystemAccessRule rule in rules)
                            {
                                if ((!Options.FileSystemRights.HasValue ||
                                    (rule.IdentityReference.Value == currentUser && (rule.FileSystemRights & Options.FileSystemRights) == Options.FileSystemRights)) &&
                                    rule.AccessControlType == AccessControlType.Allow)
                                {
                                    onFind(file);
                                    break;
                                }
                            }
                        }
                    }
                    catch { }
                }

                searchInSubFolders(path);
            }

            var semaphore = new SemaphoreSlim(0, 1);
            void searchInSubFolders(string path)
            {
                try
                {
                    var folders = Directory.GetDirectories(path, Options.DirectorySearchPattern, enumerationOptions);
                    foreach (var folder in folders)
                    {
                        if (folder.EndsWith("\\..") ||
                            folder.EndsWith("\\."))
                            continue;

                        Interlocked.Increment(ref countTask);
                        Task.Run(() =>
                        {
                            searchInFolder(folder);
                            Interlocked.Decrement(ref countTask);
                            if (countTask == 0)
                                semaphore.Release();
                        });
                    }
                }
                catch { }
            }

            searchInFolder(Options.InitialPath);

            await semaphore.WaitAsync();
        }
    }
}
