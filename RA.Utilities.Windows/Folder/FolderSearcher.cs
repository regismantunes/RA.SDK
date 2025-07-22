using RA.Utilities.Windows.Security;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace RA.Utilities.Windows.Folder
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

        /// <summary>
        /// List of errors that occurred during the search. Each item contains the path where the error occurred and the exception that was thrown.
        /// </summary>
        public ObservableCollection<(string, Exception)> SearchErrors { get; } = [];

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
            SearchErrors.Clear();
            var countTask = 0;
            var userSidForFiles = Options.SecurityOptionsForFiles?.UserSID ?? Identity.GetCurrentSID();
            var userSidForDirectories = Options.SecurityOptionsForDirectories?.UserSID ?? Identity.GetCurrentSID();
            var enumerationOptionsForFiles = new EnumerationOptions
            {
                RecurseSubdirectories = false,
                AttributesToSkip = Options.SecurityOptionsForFiles?.AttributesToSkip ?? FileAttributes.None,
                IgnoreInaccessible = true,
                ReturnSpecialDirectories = Options.ReturnSpecialDirectories
            };
            var enumerationOptionsForDirectories = new EnumerationOptions
            {
                RecurseSubdirectories = false,
                AttributesToSkip = Options.SecurityOptionsForDirectories?.AttributesToSkip ?? FileAttributes.None,
                IgnoreInaccessible = true,
                ReturnSpecialDirectories = Options.ReturnSpecialDirectories
            };

            bool rulesAreApplyed(CommonObjectSecurity objectSecurity, SecurityOptions? securityOptions)
            {
                if (securityOptions is null)
                    return true;

                var rules = objectSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));
                foreach (FileSystemAccessRule rule in rules)
                {
                    if (rule.IdentityReference.Value != userSidForFiles)
                        continue;

                    if (securityOptions.FileSystemRights.HasValue &&
                        ((rule.FileSystemRights & securityOptions.FileSystemRights) != securityOptions.FileSystemRights ||
                        rule.AccessControlType != securityOptions.AccessControlType))
                        continue;
                    
                    return true;
                }

                return false;
            }

            void searchInFolder(string path)
            {
                LastPath = path;

                var includeFolder = rulesAreApplyed(
                    new DirectoryInfo(path).GetAccessControl(),
                    Options.SecurityOptionsForDirectories);
                
                if (!includeFolder)
                    return;

                if (Options.FindForDirectories)
                    onFind(path);

                if (Options.FindForFiles)
                {
                    try
                    {
                        var files = Directory.GetFiles(path, Options.FileSearchPattern, enumerationOptionsForFiles);
                        foreach (var item in files
                            .Where(file => rulesAreApplyed(
                                new FileInfo(file).GetAccessControl(),
                                Options.SecurityOptionsForFiles)))
                            onFind(item);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Ignore unauthorized access
                    }
                    catch (Exception ex)
                    { 
                        SearchErrors.Add((path, ex));
                    }
                }

                searchInSubFolders(path);
            }

            var semaphore = new SemaphoreSlim(0, 1);
            void searchInSubFolders(string path)
            {
                try
                {
                    var folders = Directory.GetDirectories(path, Options.DirectorySearchPattern, enumerationOptionsForFiles);
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
                catch (UnauthorizedAccessException)
                {
                    // Ignore unauthorized access
                }
                catch (Exception ex)
                {
                    SearchErrors.Add((path, ex));
                }
            }

            searchInFolder(Options.InitialPath);

            await semaphore.WaitAsync();
        }
    }
}
