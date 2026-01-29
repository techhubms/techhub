---
external_url: https://devblogs.microsoft.com/dotnet/introducing-fsharp-10/
title: 'Introducing F# 10: Language Features, Performance, and Tooling in .NET 10'
author: Adam Boniecki
feed_name: Microsoft .NET Blog
date: 2025-11-17 18:05:00 +00:00
tags:
- .NET
- .NET 10
- Auto Properties
- Compiler
- Computation Expressions
- F#
- F# 10
- Language Features
- Parallel Compilation
- Performance
- Scoped Warning Suppression
- Scripting
- Tooling
- Type Subsumption Cache
- ValueOption
- VS
- Coding
- News
section_names:
- coding
primary_section: coding
---
Adam Boniecki reviews what's new in F# 10, including significant language improvements, performance upgrades, and tooling innovations released alongside .NET 10 and Visual Studio 2026.<!--excerpt_end-->

# Introducing F# 10: Language Features, Performance, and Tooling in .NET 10

F# 10 arrives alongside .NET 10 and Visual Studio 2026 with a focus on refining clarity, improving performance, and smoothing everyday development tasks. This release brings targeted language improvements, performance optimizations, and investments in developer tooling.

## Getting F# 10

- Install the latest [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Install the [Visual Studio 2026 preview](https://visualstudio.microsoft.com/vs/preview/)

## Key Language Improvements

### 1. Scoped Warning Suppression

F# 10 introduces the `#warnon` directive, which lets developers suppress compiler warnings in specific code sections, pairing it with `#nowarn` for fine control. This ensures that suppressions can be temporary and limited to precise locations.

**Example:**

```fsharp
# nowarn 25

let f (Some x) = // FS0025 suppressed

# warnon 25 // FS0025 enabled again
```

- No more multiline or empty directive support
- No whitespace allowed between `#` and directive name

More info: [RFC-1146](https://github.com/fsharp/fslang-design/blob/main/RFCs/FS-1146-scoped-nowarn.md)

### 2. Access Modifiers on Auto Property Accessors

Now you can assign different access levels directly to getters and setters.

**Before:**

```fsharp
type Ledger() =
  [<DefaultValue>]
  val mutable private _Balance: decimal
  member this.Balance with public get() = this._Balance and private set v = this._Balance <- v
```

**With F# 10:**

```fsharp
type Ledger() =
  member val Balance = 0m with public get, private set
```

### 3. ValueOption in Optional Parameters

Mark optional parameters with `[<Struct>]` to use struct-based `ValueOption<'T>` for better performance and lower allocations, especially in performance-critical code.

**Example:**

```fsharp
type X() =
  static member M([<Struct>] ?x : string) =
    match x with
    | ValueSome v -> printfn "ValueSome %s" v
    | ValueNone -> printfn "ValueNone"
```

### 4. Tail-Call Optimization in Computation Expressions

Computation-expression builders can now define special `*Final` methods (e.g., `ReturnFromFinal`) to enable efficient tail calls in builder workflows, improving performance and resource management for asynchronous and coroutine patterns.

### 5. Type Annotations Without Parentheses in Computation Expressions

Type annotations for `let!`, `use!`, and `and!` bindings no longer require parentheses.

**Example:**

```fsharp
async {
  let! a: int = fetchA()
  and! b: int = fetchB()
  use! d: MyDisposable = acquireAsync()
  return a + b
}
```

### 6. Discards `_` in `use!` Bindings

You can now use `_` in `use!` bindings inside computation expressions to intentionally discard a value.

**Example:**

```fsharp
counterDisposable {
  use! _ = new Disposable()
}
```

### 7. Rejecting Pseudo-Nested Modules in Types

Compiler now rejects module definitions placed at the same indentation within types, eliminating confusion about scoping.

### 8. Deprecation Warning for Omitted `seq`

Using bare `{ 1..10 }` now emits warnings; use `seq { ... }` for clarity and future compatibility.

### 9. Attribute Target Enforcement

The compiler enforces proper attribute targets, reducing subtle bugs caused by misapplied attributes.

**Example:**

```fsharp
[<Fact>]
let ``works correctly`` = Assert.True(true)
```

Attributes now generate warnings if misapplied.

## FSharp.Core: `and!` Support in `task` Expressions

The FSharp.Core library adds support for the `and!` binding in the `task` computation expression, enabling concise concurrent awaits in F# async workflows.

**Example:**

```fsharp
task {
  let! a = fetchA()
  and! b = fetchB()
  return combineAB a b
}
```

## Performance & Tooling Improvements

- **Type Subsumption Cache:** Speeds up type checking by caching type relationship checks. No manual action required.
- **Better Trimming:** Publishing with trimming enabled now auto-generates an `ILLink.Substitutions.xml` for F# resource metadata, reducing output size and maintenance effort.
- **Parallel Compilation (Preview):** Projects using `LangVersion=Preview` benefit from graph-based type checking, parallel IL generation, and optimization. This is planned as a default in .NET 11.
- **Typecheck-Only Mode for Scripts:** The `--typecheck-only` flag now validates `.fsx` script syntax and types without executing code. (Note a [current bug](https://github.com/dotnet/fsharp/issues/19047) with scripts using `#load`, fixed in 10.0.200.)

## Community Acknowledgements

The F# team calls out contributors including @majocha, @Martin521, @edgarfgp, @auduchinok, and amplifier groups like [Amplifying F#](https://amplifyingfsharp.io/) for their continued support and enhancements across infrastructure, diagnostics, and tooling.

## What’s Next?

Work on F# 11 is underway, promising even more performance and language improvements. Community discussions and contributions continue on the [GitHub repo](https://github.com/dotnet/fsharp/discussions).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/introducing-fsharp-10/)
