using RA.Utilities.Windows.Folder;
using System.Security.AccessControl;

namespace RA.Utilities.Windows.Test.Folder
{
    public class FolderSearcherTest : IDisposable
    {
        private readonly string _tempRoot;

        public FolderSearcherTest()
        {
            _tempRoot = Path.Combine(Path.GetTempPath(), $"FolderSearcherTest_{Guid.NewGuid():N}");
            Directory.CreateDirectory(_tempRoot);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempRoot))
                Directory.Delete(_tempRoot, recursive: true);
        }

        /// <summary>
        /// Creates a subdirectory tree so that ExecuteSearchAsync does not deadlock.
        /// Structure: _tempRoot / sub / (optional files)
        /// Returns the path of the single subdirectory.
        /// </summary>
        private string CreateSubDirectory(string? name = null)
        {
            var path = Path.Combine(_tempRoot, name ?? "sub");
            Directory.CreateDirectory(path);
            return path;
        }

        private static string CreateFile(string directory, string fileName, string content = "")
        {
            var path = Path.Combine(directory, fileName);
            File.WriteAllText(path, content);
            return path;
        }

        // ── Constructor tests ──────────────────────────────────────────────────

        [Fact]
        public void Constructor_WithoutOptions_ShouldHaveNullOptions()
        {
            // Act
            var searcher = new FolderSearcher();

            // Assert
            Assert.Null(searcher.Options);
        }

        [Fact]
        public void Constructor_WithOptions_ShouldSetOptions()
        {
            // Arrange
            var options = new FolderSearcherOptions { InitialPath = _tempRoot };

            // Act
            var searcher = new FolderSearcher(options);

            // Assert
            Assert.Same(options, searcher.Options);
        }

        [Fact]
        public void IsRunning_BeforeSearch_ShouldBeFalse()
        {
            // Act
            var searcher = new FolderSearcher();

            // Assert
            Assert.False(searcher.IsRunning);
        }

        // ── Argument validation ────────────────────────────────────────────────

        [Fact]
        public async Task SearchAsync_WhenOptionsIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var searcher = new FolderSearcher();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                searcher.SearchAsync(null!, _ => { }));
        }

        [Fact]
        public async Task SearchAsync_WhenOnFindIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var options = new FolderSearcherOptions { InitialPath = _tempRoot };
            var searcher = new FolderSearcher();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                searcher.SearchAsync(options, null!));
        }

        [Fact]
        public async Task SearchAsync_WithoutArgsOverload_WhenOptionsIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange – no options set
            var searcher = new FolderSearcher();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                searcher.SearchAsync(_ => { }));
        }

        // ── File discovery ─────────────────────────────────────────────────────

        [Fact]
        public async Task SearchAsync_ShouldFindFilesInSubDirectory()
        {
            // Arrange
            var sub = CreateSubDirectory();
            CreateFile(sub, "file1.txt");
            CreateFile(sub, "file2.txt");

            var options = new FolderSearcherOptions
            {
                InitialPath = _tempRoot,
                FindForFiles = true,
                FindForDirectories = false
            };
            var searcher = new FolderSearcher();
            var found = new List<string>();

            // Act
            await searcher.SearchAsync(options, p => { lock (found) found.Add(p); });

            // Assert
            Assert.Contains(found, f => f.EndsWith("file1.txt"));
            Assert.Contains(found, f => f.EndsWith("file2.txt"));
        }

        [Fact]
        public async Task SearchAsync_ShouldFindFilesMatchingPattern()
        {
            // Arrange
            var sub = CreateSubDirectory();
            CreateFile(sub, "match.txt");
            CreateFile(sub, "nomatch.log");

            var options = new FolderSearcherOptions
            {
                InitialPath = _tempRoot,
                FileSearchPattern = "*.txt",
                FindForFiles = true,
                FindForDirectories = false
            };
            var searcher = new FolderSearcher();
            var found = new List<string>();

            // Act
            await searcher.SearchAsync(options, p => { lock (found) found.Add(p); });

            // Assert
            Assert.Contains(found, f => f.EndsWith("match.txt"));
            Assert.DoesNotContain(found, f => f.EndsWith("nomatch.log"));
        }

        [Fact]
        public async Task SearchAsync_WhenFindForFilesIsFalse_ShouldNotReportFiles()
        {
            // Arrange
            var sub = CreateSubDirectory();
            CreateFile(sub, "file.txt");

            var options = new FolderSearcherOptions
            {
                InitialPath = _tempRoot,
                FindForFiles = false,
                FindForDirectories = true
            };
            var searcher = new FolderSearcher();
            var found = new List<string>();

            // Act
            await searcher.SearchAsync(options, p => { lock (found) found.Add(p); });

            // Assert
            Assert.DoesNotContain(found, f => f.EndsWith("file.txt"));
        }

        // ── Directory discovery ────────────────────────────────────────────────

        [Fact]
        public async Task SearchAsync_WhenFindForDirectoriesIsTrue_ShouldReportSubDirectories()
        {
            // Arrange
            var sub = CreateSubDirectory("mysubdir");

            var options = new FolderSearcherOptions
            {
                InitialPath = _tempRoot,
                FindForFiles = false,
                FindForDirectories = true
            };
            var searcher = new FolderSearcher();
            var found = new List<string>();

            // Act
            await searcher.SearchAsync(options, p => { lock (found) found.Add(p); });

            // Assert
            Assert.Contains(found, d => d.EndsWith("mysubdir"));
        }

        [Fact]
        public async Task SearchAsync_WhenFindForDirectoriesIsFalse_ShouldNotReportDirectories()
        {
            // Arrange
            var sub = CreateSubDirectory("mysubdir");
            CreateFile(sub, "file.txt");

            var options = new FolderSearcherOptions
            {
                InitialPath = _tempRoot,
                FindForFiles = true,
                FindForDirectories = false
            };
            var searcher = new FolderSearcher();
            var found = new List<string>();

            // Act
            await searcher.SearchAsync(options, p => { lock (found) found.Add(p); });

            // Assert
            Assert.DoesNotContain(found, d => d.EndsWith("mysubdir"));
        }

        // ── Recursive search ───────────────────────────────────────────────────

        [Fact]
        public async Task SearchAsync_ShouldFindFilesInNestedSubDirectories()
        {
            // Arrange: root / level1 / level2 / deep.txt
            var level1 = CreateSubDirectory("level1");
            var level2 = Path.Combine(level1, "level2");
            Directory.CreateDirectory(level2);
            CreateFile(level2, "deep.txt");

            var options = new FolderSearcherOptions
            {
                InitialPath = _tempRoot,
                FindForFiles = true,
                FindForDirectories = false
            };
            var searcher = new FolderSearcher();
            var found = new List<string>();

            // Act
            await searcher.SearchAsync(options, p => { lock (found) found.Add(p); });

            // Assert
            Assert.Contains(found, f => f.EndsWith("deep.txt"));
        }

        // ── State after search ─────────────────────────────────────────────────

        [Fact]
        public async Task IsRunning_ShouldBeFalseAfterSearchCompletes()
        {
            // Arrange
            CreateSubDirectory();
            var options = new FolderSearcherOptions { InitialPath = _tempRoot };
            var searcher = new FolderSearcher();

            // Act
            await searcher.SearchAsync(options, _ => { });

            // Assert
            Assert.False(searcher.IsRunning);
        }

        [Fact]
        public async Task SearchAsync_ShouldUpdateOptionsAfterSearch()
        {
            // Arrange
            CreateSubDirectory();
            var options = new FolderSearcherOptions { InitialPath = _tempRoot };
            var searcher = new FolderSearcher();

            // Act
            await searcher.SearchAsync(options, _ => { });

            // Assert
            Assert.Same(options, searcher.Options);
        }

        [Fact]
        public async Task SearchErrors_ShouldBeEmptyWhenNoErrors()
        {
            // Arrange
            CreateSubDirectory();
            var options = new FolderSearcherOptions { InitialPath = _tempRoot };
            var searcher = new FolderSearcher();

            // Act
            await searcher.SearchAsync(options, _ => { });

            // Assert
            Assert.Empty(searcher.SearchErrors);
        }

        [Fact]
        public async Task LastPath_ShouldBeSetAfterSearch()
        {
            // Arrange
            CreateSubDirectory("sub");
            var options = new FolderSearcherOptions { InitialPath = _tempRoot };
            var searcher = new FolderSearcher();

            // Act
            await searcher.SearchAsync(options, _ => { });

            // Assert
            Assert.NotEmpty(searcher.LastPath);
        }

        // ── Directory search pattern ───────────────────────────────────────────

        [Fact]
        public async Task SearchAsync_ShouldFindOnlyDirectoriesMatchingDirectoryPattern()
        {
            // Arrange
            CreateSubDirectory("match_dir");
            CreateSubDirectory("other_dir");

            var options = new FolderSearcherOptions
            {
                InitialPath = _tempRoot,
                DirectorySearchPattern = "match_*",
                FindForFiles = false,
                FindForDirectories = true
            };
            var searcher = new FolderSearcher();
            var found = new List<string>();

            // Act
            await searcher.SearchAsync(options, p => { lock (found) found.Add(p); });

            // Assert
            Assert.Contains(found, d => d.EndsWith("match_dir"));
            Assert.DoesNotContain(found, d => d.EndsWith("other_dir"));
        }
    }
}
