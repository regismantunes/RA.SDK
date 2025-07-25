# RA.SDK Solution

Welcome to the **RA.SDK** repository! This solution provides utilities packaged as NuGet libraries `RegisAntunes.Utilities` and `RegisAntunes.Utilities.Windows` along some tests. Below, you'll find guidance about the structure, purpose, and usage of the contained projects.

## Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Usage Examples](#usage-examples)
- [Contributing](#contributing)
- [License](#license)
- [Questions](#questions)

## About the Project

The **RegisAntunes.Utilities** and **RegisAntunes.Utilities.Windows** NuGet package offers a collection of time-saving utilities for developers with general-purpose utility classes and extensions for .NET, designed to centralize, improve, and simplify common development tasks. The test project use [xUnit](https://xunit.net/) to ensure reliability, fill free to sugest new implementations.

## **RegisAntunes.Utilities** Features

### 1. Stopwatch Extensions

- **StopwatchExtensions**:  
  - Adds `GetElapsedTimeText()` to `System.Diagnostics.Stopwatch`
  - Returns human-friendly elapsed time.

### 2. Task Extensions

- **TaskExtensions**:  
  - Adds `DelayOrCompleted()` to `System.Threading.Tasks.Task`
  - Helper for easy waiting for task completion or delay.
  
### 3. Error Handling
- **CustomErrorHandler**:
  - A class to help you to use defalt errors treatments inside a method or class.
  
 - **ActionExtensions**:
   - Adds `TryExecute` to `System.Action`
   - Try to execute some action and handle with the error if it occours

### 4. Output Abstractions

- **Output**:  
  - Extensible interfaces and implementations for outputting information.
  - Console output currently implemented, other output types can be added.

## **RegisAntunes.Utilities.Windows** Features

### 1. Folder Searcher

- **FolderSearcher**:  
  - Powerful class for recursively searching files and directories.
  - Flexible API to filter results by file name, attributes, and rights.
  - Options to control recursion and filtering.

### 2. Identity Utilities

- **Identity.GetCurrentSID**:  
  - Static helper to retrieve the SID of the current Windows user session.

## Project Structure

```
RA.SDK/
│
├── RegisAntunes.Utilities/        	# NuGet utility library
├── RegisAntunes.Utilities.Tests/  	# xUnit test project
├── RegisAntunes.Utilities.Windows/	# NuGet utility library
└── README.md
```

## Getting Started

#### 1. Clone the Repository

```bash
git clone https://github.com/regismantunes/RA.SDK.git
cd RA.SDK
```

#### 2. Build the Solution

Using .NET CLI:

```bash
dotnet build
```

#### 3. Run Tests

Using .NET CLI:

```bash
dotnet test
```
Tests are written using the [xUnit](https://xunit.net/) framework.

#### 4. Install the NuGet Package

Inside your project:

```bash
dotnet add package RegisAntunes.Utilities
```

```bash
dotnet add package RegisAntunes.Utilities.Windows
```

Or with NuGet Package Manager:

```powershell
Install-Package RegisAntunes.Utilities
```

```powershell
Install-Package RegisAntunes.Utilities.Windows
```

## Usage Examples

**StopwatchExtensions**
```csharp
using System.Diagnostics;
using RegisAntunes.Utilities.Extensions;

var sw = Stopwatch.StartNew();
// ... your code ...
Console.WriteLine(sw.GetElapsedTimeText());
```

**TaskExtensions**
```csharp
using System.Threading.Tasks;
using RegisAntunes.Utilities.Extensions;

Task myTask = ...; // any Task
await myTask.DelayOrCompleted(1000); //miliseconds
```

**CustomErrorHandler**
```csharp
using RegisAntunes.Utilities.ErrorHandling;

var errorHandler = new CustomErrorHandler((Exception ex) => {
	// Custom error handling logic
},typeof(IgnoredException1), typeof(IgnoredException2),...);

bool noErrorExecution = errorHandler.TryExecute(() => {
	// Code that might throw exceptions
});
```

**CustomErrorHandler<T>**
```csharp
using RegisAntunes.Utilities.ErrorHandling;

var errorHandler = new CustomErrorHandler<string>((string parameter, Exception ex) => {
	// Custom error handling logic
},typeof(IgnoredException1), typeof(IgnoredException2),...);

bool noErrorExecution = errorHandler.TryExecute(() => {
	// Code that might throw exceptions
}, parameter);
```

**ActionExtensions**
```csharp
using RegisAntunes.Utilities.ErrorHandling;

new Action(() => {
	// Your action code here
}).TryExecute((Exception ex) => {
	// Custom error handling logic
}, typeof(IgnoredException1), typeof(IgnoredException2), ...);

new Action(() => {
	// Your action code here
}).TryExecute<string>((string parameter, Exception ex) => {
	// Custom error handling logic
}, parameter, typeof(IgnoredException1), typeof(IgnoredException2), ...);
```

**Output**
```csharp
using RegisAntunes.Utilities.Output;

IOutput output = OutputFactory.GetOutput(OutpytType.Console);
output.WriteLine("Hello World!");
output.WriteLine("IOutput usage example!");
output.ClearLine();
```

**FolderSearcher**
A usage example was impleemted by this repository:
https://github.com/regismantunes/FolderSizeSearcher

**Identity.GetCurrentSID**
```csharp
using RegisAntunes.Utilities.Windows;

string sid = Identity.GetCurrentSID();
Console.WriteLine($"Current SID: {sid}");
```

## Contributing

Contributions are welcome!  
Please open issues for feature requests or bug reports, and submit pull requests for any improvements.

## License

This project is licensed under the [MIT License](./LICENSE).

## Questions

Have questions or want to discuss features?  
Open an [issue](https://github.com/regismantunes/RA.SDK/issues) or contact the maintainer.

If you’d like any more details added (such as advanced usage, CI status, etc), please let me know!
