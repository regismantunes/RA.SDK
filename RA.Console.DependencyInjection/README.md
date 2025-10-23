# RA.Console.DependencyInjection

A lightweight console application framework for .NET that leverages Microsoft.Extensions.DependencyInjection to build discoverable, attribute-driven commands with optional async execution, custom argument builders, and an integrated help system.

## Table of Contents

- [About](#about)
- [Features](#features)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Commands and Arguments](#commands-and-arguments)
- [Help System](#help-system)
- [Middleware](#middleware)
- [Advanced Assembly Scanning](#advanced-assembly-scanning)
- [Optimized Initialization](#optimized-initialization)
- [Programmatic Execution](#programmatic-execution)
- [Exceptions](#exceptions)
- [Current Instance](#current-instance)
- [API Surface](#api-surface)

## About

`RA.Console.DependencyInjection` helps you compose console apps by scanning your assemblies for command handlers and registering them in DI. Commands are simple methods annotated with attributes, and parameters are automatically bound from args (with support for custom builders).

Target framework: `net10.0`.

## Features

- **Builder-first setup**: `ConsoleAppBuilder` with `Services` to register dependencies.
- **Assembly scanning**: `AddAssembly<T>()` and `AddAssemblies(...)` to discover commands/builders.
- **Granular assembly loading**: `AddHelpCommandFromAssembly(...)`, `AddCommandsFromAssembly(...)`, `AddMiddlewaresFromAssembly(...)` and their plural variants to control what is loaded from each assembly.
- **Attribute-based commands**:
  - `CommandAttribute`, `CommandAsyncAttribute`.
  - Variants with generic ArgsBuilder types: `CommandWithArgsBuilderAttribute<T>`, `CommandAsyncWithArgsBuilderAttribute<T>`, and async builder interfaces.
- **Argument binding**:
  - Default binding via `DefaultArgsBuilder`.
  - Custom binding via `IArgsBuilder` / `IArgsBuilderAsync`.
  - Parameter customization via `ParameterAttribute` (name and case-sensitivity).
- **Default/optional parameters**: Missing args map to default values or `optional` parameters when applicable; non-nullable value types without defaults are validated.
- **Integrated help**:
  - Default help command with `UseDefaultHelpResources()`.
  - Custom help via `IHelpCommand`/`IHelpCommandAsync`.
- **Optimized initialization**: Load only the requested command path for faster startup.
- **CancellationToken support**: Method parameters can include `CancellationToken`.
- **Middleware pipeline**: Register `ICommandMiddleware` to intercept and augment command execution with `CommandContext`.
- **Auto DI registration**: Builders (`IArgsBuilder`, `IArgsBuilderAsync`) and middlewares discovered in scanned assemblies are registered as singletons when applicable.
- **Programmatic control**: Execute commands programmatically via `ConsoleApp.RunCommandAsync(...)` and `RunHelpCommandAsync(...)`.
- **Current app access**: Access the running instance via `ConsoleApp.Current`.

## Installation

Add the project reference or package (when published). Requires:

- `Microsoft.Extensions.DependencyInjection`
- `Microsoft.Extensions.Hosting`

## Quick Start

```csharp
using RA.Console.DependencyInjection;
using System.Reflection;

var builder = new ConsoleAppBuilder(args)
    .UseDefaultHelpResources()
    .AddAssembly(Assembly.GetExecutingAssembly());

var app = builder.Build();
return await app.RunAsync();
```

Define a command handler in any scanned assembly:

```csharp
using RA.Console.DependencyInjection.Attributes;

public class UserCommands
{
    [Command("create", Description = "Creates a new user", Example = "create --name John")]
    public int Create(
        [Parameter(Name = "name")] string name)
    {
        Console.WriteLine($"User '{name}' created.");
        return 0;
    }
}
```

Run:

```bash
myapp create --name John
```

## Commands and Arguments

- **Attributes** in `Attributes/`:
  - `CommandAttribute`, `CommandAsyncAttribute` for sync/async commands.
  - Generic variants to plug custom args builders.
  - `ParameterAttribute` to rename parameters and control case-sensitivity.
- **Default binding**: `DefaultArgsBuilder` maps `--param value` and positional args to method parameters.
- **Custom builders**: Implement `IArgsBuilder` or `IArgsBuilderAsync` and reference it via a generic command attribute variant to fully control parsing/validation.
- **Defaults and optionals**: If a value is not provided and the parameter has a default value or is `optional`, it is used; otherwise validation errors are thrown for required non-nullable parameters.

## Help System

- Enable defaults with:

```csharp
builder.UseDefaultHelpResources();
```

- Or configure explicitly:
  - `UseDefaultHelpCommands()` or `SetHelpCommands(params IEnumerable<string>)`
  - `UseEmptyArgsForHelp()` to show help when no args are passed
  - `SetHelpCommand<T>()` / `SetHelpCommandAsync<T>()` or `UseDefaultHelpCommand()`

The default help lists discovered commands with description, example, group, and order from `CommandAttribute`.

## Middleware

- **Register**:

```csharp
builder
    .UseMiddleware<YourMiddleware>()
    .AddAssembly(typeof(Program).Assembly);
```

- **Implement** `ICommandMiddleware` using `CommandContext` from `Middleware/`:

```csharp
public class YourMiddleware : ICommandMiddleware
{
    public async Task<int> InvokeAsync(CommandContext ctx, Func<CommandContext, Task<int>> next)
    {
        // pre
        var result = await next(ctx);
        // post
        return result;
    }
}
```

- You can also register an instance with `UseMeddleware(ICommandMiddleware)`.

## Advanced Assembly Scanning

- Load selectively from assemblies:
  - `AddHelpCommandFromAssembly(Assembly)` / `AddHelpCommandFromAssembly<T>()`
  - `AddCommandsFromAssembly(Assembly)` / `AddCommandsFromAssembly<T>()` / `AddCommandsFromAssemblies(...)`
  - `AddMiddlewaresFromAssembly(Assembly)` / `AddMiddlewaresFromAssembly<T>()` / `AddMiddlewaresFromAssemblies(...)`

- Notes:
  - When targeting a specific command during optimized init, only the required command and its args builder type are registered.
  - When not targeting a specific command, all discovered builders and middlewares are registered as singletons.

## Optimized Initialization

Call `UseOptimizedInitialization()` to reduce startup time by loading only the requested command and the help metadata.

```csharp
var app = new ConsoleAppBuilder(args)
    .UseOptimizedInitialization()
    .UseDefaultHelpResources()
    .AddAssembly(typeof(Program).Assembly)
    .Build();
```

## Programmatic Execution

- Execute a command directly:

```csharp
var consoleApp = (ConsoleApp)app; // if you hold IConsoleApp
var exitCode = await consoleApp.RunCommandAsync("create", new[] { "--name", "John" }, cancellationToken);
```

- Invoke the help command programmatically:

```csharp
var exitCode = await consoleApp.RunHelpCommandAsync(args, cancellationToken);
```

## Exceptions

- **NoCommandsDefinedException**: No commands (or help command) found in registered assemblies for the current context.
- **MultipleCommandsDefinedException**: Duplicate command names found across methods/assemblies or multiple help commands detected.
- **InvalidOperationException**: Various misconfigurations, e.g., unknown command at runtime or invalid attribute usage.
- **ArgsValidationException**: Thrown by custom builders to signal invalid input.

## Current Instance

- Access the running app via `ConsoleApp.Current` for advanced scenarios. An additional instance cannot start while one is running.

## API Surface

- **Builder**: `ConsoleAppBuilder`
  - `Services`, `AddAssembly<T>()`, `AddAssemblies(...)`, `UseOptimizedInitialization()`, help configuration methods, `Build()`
- **Assembly scanning (advanced)**: `AddHelpCommandFromAssembly(...)`, `AddCommandsFromAssembly(...)`, `AddMiddlewaresFromAssembly(...)` and plural variants
- **Runtime**: `IConsoleApp` with `Services` and `RunAsync(...)`
- **Execution helpers**: `ConsoleApp.RunCommandAsync(...)`, `ConsoleApp.RunHelpCommandAsync(...)`
- **Attributes**: `CommandAttribute` (+ async), generic variants, `ParameterAttribute`
- **Args**: `IArgsBuilder`, `IArgsBuilderAsync`, `DefaultArgsBuilder`
- **Help**: `IHelpCommand`, `IHelpCommandAsync`, `DefaultHelpCommand`
- **Middleware**: `ICommandMiddleware`, `CommandContext`; register via `UseMiddleware<T>()` / `UseMeddleware(instance)`
- **Exceptions**: `NoCommandsDefinedException`, `MultipleCommandsDefinedException`, `ArgsValidationException`
