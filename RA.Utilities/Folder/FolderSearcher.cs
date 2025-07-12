using RA.Utilities.Extensions;
using RA.Utilities.Output;
using RA.Utilities.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace RA.Utilities.Folder
{
    public class FolderSearcher(FolderSearcherOptions options)
    {
        public FolderSearcher()
            : this(new FolderSearcherOptions())
        { }

        public FolderSearcherOptions Options { get; private set; } = options ?? throw new ArgumentNullException(nameof(options));

        public bool IsRunning { get; private set; } = false;
        public string LastPath { get; private set; } = string.Empty;
        private readonly object _locker = new();

        public async Task SearchAsync(Action<string> onFind)
        {
            await SearchAsync(Options, onFind);
        }

        public async Task SearchAsync(FolderSearcherOptions options, Action<string> onFind)
        {
            ArgumentNullException.ThrowIfNull(onFind, nameof(onFind));

            lock (_locker)
            {
                if (IsRunning)
                    throw new InvalidOperationException("Search is already running.");

                IsRunning = true;
            }

            try
            {
                Options = options ?? throw new ArgumentNullException(nameof(options));
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

                        if (Options.FindForDirectories)
                            onFind(folder);

                        Interlocked.Increment(ref countTask);

                        Task.Run(() =>
                        {
                            searchInFolder(folder);
                            Interlocked.Decrement(ref countTask);
                        });
                    }
                }
                catch { }
            }

            searchInFolder(Options.InitialPath);

            while (countTask > 0)
                await Task.Delay(100);
        }
    }
}
