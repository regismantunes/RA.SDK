using RA.Utilities.Windows.Security;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly Dictionary<string, Exception> _searchErrors = [];
        private Action<string, Exception>? _onError;
        private Action<string>? _onFind;
        private readonly object _locker = new();

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
        public IReadOnlyDictionary<string, Exception> SearchErrors => _searchErrors;

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
        public async Task SearchAsync(FolderSearcherOptions options, Action<string> onFind, Action<string,Exception>? onError = null)
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
                _onFind = onFind;
                _onError = onError;
                await ExecuteSearchAsync();
            }
            finally
            {
                IsRunning = false;
                _onFind = null;
                _onError = null;
            }
        }

        private async Task ExecuteSearchAsync()
        {
            _searchErrors.Clear();
            var countTask = 0;
            var userSidForFiles = Options!.SecurityOptionsForFiles?.UserSID ?? Identity.GetCurrentSID();
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

            void searchInFolder(string path)
            {
                LastPath = path;

                bool includeFolder = false;
                ExecuteWithErrorTreatment(path, () =>
                    includeFolder = RulesAreApplyed(
                        new DirectoryInfo(path).GetAccessControl(),
                        Options.SecurityOptionsForDirectories,
                        userSidForDirectories)
                );
                
                if (!includeFolder)
                    return;

                if (Options.FindForDirectories)
                    _onFind!(path);

                if (Options.FindForFiles)
                {
                    var files = Directory.GetFiles(path, Options.FileSearchPattern, enumerationOptionsForFiles);
                    foreach (var file in files)
                    {
                        ExecuteWithErrorTreatment(file, () =>
                        {
                            if (RulesAreApplyed(
                                new FileInfo(file).GetAccessControl(),
                                Options.SecurityOptionsForFiles,
                                userSidForFiles))
                                _onFind!(file);
                        });
                    }
                }

                searchInSubFolders(path);
            }

            var semaphore = new SemaphoreSlim(0, 1);
            void searchInSubFolders(string path)
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

            searchInFolder(Options.InitialPath);

            await semaphore.WaitAsync();
        }

        private static bool RulesAreApplyed(CommonObjectSecurity objectSecurity, SecurityOptions? securityOptions, string userSid)
        {
            if (securityOptions is null)
                return true;

            var rules = objectSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));
            foreach (FileSystemAccessRule rule in rules)
            {
                if (rule.IdentityReference.Value != userSid)
                    continue;

                if (securityOptions.FileSystemRights.HasValue &&
                    ((rule.FileSystemRights & securityOptions.FileSystemRights) != securityOptions.FileSystemRights ||
                    rule.AccessControlType != securityOptions.AccessControlType))
                    continue;

                return true;
            }

            return false;
        }

        private void ExecuteWithErrorTreatment(string path, Action action)
        {
            try
            {
                action();
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore unauthorized access
            }
            catch (Exception ex)
            {
                _searchErrors.Add(path, ex);
                _onError?.Invoke(path, ex);
            }
        }
    }
}
