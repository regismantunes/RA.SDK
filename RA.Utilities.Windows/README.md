# RegisAntunes.Utilities.Windows

A general-purpose utility with Windows compatibility library for .NET, consolidating a set of time-saving extensions, helpers, and abstractions to simplify development. Useful classes and methods are gathered from real-world projects for reusability, reliability, and ease of maintenance.

## Features

- **Folder Searcher**
  - Deep directory and file search with flexible filtering (names, attributes, rights) and control over recursion.

- **Identity Utilities**
  - `GetCurrentSID()` — Static helper to get the current Windows session user's SID.

All features are reliably tested with [xUnit](https://xunit.net/).

## Installation

Install via NuGet package manager:

```bash
dotnet add package RegisAntunes.Utilities.Windows
```

or with Package Manager Console:

```powershell
Install-Package RegisAntunes.Utilities.Windows
```

## Usage Examples

**FolderSearcher**
Full example: [FolderSizeSearcher](https://github.com/regismantunes/FolderSizeSearcher)

**Identity.GetCurrentSID**
```csharp
using RegisAntunes.Utilities.Windows;

string sid = Identity.GetCurrentSID();
Console.WriteLine($"Current SID: {sid}");
```

## License

MIT License — use freely in your projects.

## Contributing

Pull requests and issues are welcome in the [main repository](https://github.com/regismantunes/RA.SDK).