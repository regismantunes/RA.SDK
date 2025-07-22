# RegisAntunes.Utilities

A general-purpose utility library for .NET, consolidating a set of time-saving extensions, helpers, and abstractions to simplify development. Useful classes and methods are gathered from real-world projects for reusability, reliability, and ease of maintenance.

## Features

- **Stopwatch Extensions**
  - `GetElapsedTimeText()` — Extension method for `System.Diagnostics.Stopwatch` to return friendly, readable elapsed time.

- **Task Extensions**
  - `DelayOrCompleted()` — Extension for `System.Threading.Tasks.Task` to help await tasks with a timeout or delay option.

- **Output Abstractions**
  - Console output interface and extensible abstractions for writing and managing output. (Console implementation included.)

All features are reliably tested with [xUnit](https://xunit.net/).

## Installation

Install via NuGet package manager:

```bash
dotnet add package RegisAntunes.Utilities
```

or with Package Manager Console:

```powershell
Install-Package RegisAntunes.Utilities
```

## Usage Examples

**StopwatchExtensions**
```csharp
using System.Diagnostics;
using RegisAntunes.Utilities.Extensions;

var sw = Stopwatch.StartNew();
// ... do some work ...
Console.WriteLine(sw.GetElapsedTimeText());
```

**TaskExtensions**
```csharp
using System.Threading.Tasks;
using RegisAntunes.Utilities.Extensions;

Task myTask = ...;
await myTask.DelayOrCompleted(1000); // milliseconds
```

**Output**
```csharp
using RegisAntunes.Utilities.Output;

IOutput output = OutputFactory.GetOutput(OutpytType.Console);
output.WriteLine("Hello World!");
output.ClearLine();
```

## License

MIT License — use freely in your projects.

## Contributing

Pull requests and issues are welcome in the [main repository](https://github.com/regismantunes/RA.SDK).