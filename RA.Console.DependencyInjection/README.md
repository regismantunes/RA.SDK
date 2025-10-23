# RA.Console.DependencyInjection

A lightweight console application framework for .NET that leverages Microsoft.Extensions.DependencyInjection to build discoverable, attribute-driven commands with optional async execution, custom argument builders, and an integrated help system.

## Table of Contents

- [About](#about)
- [Features](#features)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Commands and Arguments](#commands-and-arguments)
- [Help System](#help-system)
- [Optimized Initialization](#optimized-initialization)
- [API Surface](#api-surface)

## About

`RA.Console.DependencyInjection` helps you compose console apps by scanning your assemblies for command handlers and registering them in DI. Commands are simple methods annotated with attributes, and parameters are automatically bound from args (with support for custom builders).

Target framework: `net10.0`.

## Features

- **Builder-first setup**: `ConsoleAppBuilder` with `Services` to register dependencies.
- **Assembly scanning**: `AddAssembly<T>()` and `AddAssemblies(...)` to discover commands/builders.
- **Attribute-based commands**:
  - `CommandAttribute`, `CommandAsyncAttribute`.
  - Variants with generic ArgsBuilder types: `CommandWithArgsBuilderAttribute<T>`, `CommandAsyncWithArgsBuilderAttribute<T>`, and async builder interfaces.
- **Argument binding**:
  - Default binding via `DefaultArgsBuilder`.
  - Custom binding via `IArgsBuilder` / `IArgsBuilderAsync`.
  - Parameter customization via `ParameterAttribute` (name and case-sensitivity).
- **Integrated help**:
  - Default help command with `UseDefaultHelpResources()`.
  - Custom help via `IHelpCommand`/`IHelpCommandAsync`.
- **Optimized initialization**: Load only the requested command path for faster startup.
- **CancellationToken support**: Method parameters can include `CancellationToken`.

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

## Optimized Initialization

Call `UseOptimizedInitialization()` to reduce startup time by loading only the requested command and the help metadata.

```csharp
var app = new ConsoleAppBuilder(args)
    .UseOptimizedInitialization()
    .UseDefaultHelpResources()
    .AddAssembly(typeof(Program).Assembly)
    .Build();
```

## API Surface

- **Builder**: `ConsoleAppBuilder`
  - `Services`, `AddAssembly<T>()`, `AddAssemblies(...)`, `UseOptimizedInitialization()`, help configuration methods, `Build()`
- **Runtime**: `IConsoleApp` with `Services` and `RunAsync(...)`
- **Attributes**: `CommandAttribute` (+ async), generic variants, `ParameterAttribute`
- **Args**: `IArgsBuilder`, `IArgsBuilderAsync`, `DefaultArgsBuilder`
- **Help**: `IHelpCommand`, `IHelpCommandAsync`, `DefaultHelpCommand`
