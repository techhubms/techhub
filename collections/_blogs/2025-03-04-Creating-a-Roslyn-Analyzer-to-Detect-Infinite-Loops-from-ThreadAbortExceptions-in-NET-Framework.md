---
external_url: https://andrewlock.net/creating-an-analyzer-to-detect-infinite-loops-caused-by-threadabortexception/
title: Creating a Roslyn Analyzer to Detect Infinite Loops from ThreadAbortExceptions in .NET Framework
author: Andrew Lock
feed_name: Andrew Lock's Blog
date: 2025-03-04 09:00:00 +00:00
tags:
- .NET Framework
- C#
- Code Fix Provider
- Concurrency
- Configuration
- Hosting
- Infinite Loops
- Parallel Programming
- Roslyn Analyzer
- Runtime Bugs
- Static Analysis
- ThreadAbortException
section_names:
- coding
primary_section: coding
---
Andrew Lock presents an in-depth guide on identifying and mitigating infinite loops caused by ThreadAbortExceptions in .NET Framework applications, illustrating the problem with examples and sharing a custom Roslyn Analyzer solution.<!--excerpt_end-->

## Introduction

Infinite loops caused by `ThreadAbortException` are a subtle pitfall in .NET Framework applications, particularly when working with threads and exception handling. In this comprehensive post, Andrew Lock describes how these issues arise, why they're linked to a bug in the runtime, and how you can proactively avoid them. He also introduces a custom Roslyn Analyzer to detect potentially problematic code patterns.

---

## Background: Thread.Abort and ThreadAbortException

In .NET, parallel programming typically leverages the Task Parallel Library (`Task`, `Task<T>`, async/await), but direct thread management using `Thread.Start()` and related APIs is also possible. Cooperative cancellation is recommended (with `CancellationToken`), but in limited scenarios, such as when running third-party or legacy code, developers might resort to `Thread.Abort()` in .NET Framework (not supported in .NET Core or later).

Invoking `Thread.Abort()` throws a `ThreadAbortException` in the target thread. This exception can be caught, but the runtime re-throws it automatically at the end of the catch block unless explicitly cancelled via `ResetAbort()` (covered elsewhere).

**Sample Scenario:**

```csharp
// Start a new thread
delegate void DoWork();
var myThread = new Thread(new ThreadStart(DoWork));
myThread.Start();

Thread.Sleep(300); // Pause main thread
Console.WriteLine("Main - aborting thread");
myThread.Abort(); // Triggers ThreadAbortException
myThread.Join();
Console.WriteLine("Main ending");

static void DoWork() {
    try {
        for (var i = 0; i < 100; i++) {
            Console.WriteLine($"Thread - working {i}");
            Thread.Sleep(100);
        }
    } catch (ThreadAbortException e) {
        Console.WriteLine($"Thread - caught ThreadAbortException: {e.Message}");
        // The runtime re-throws the exception here
    }
    Console.WriteLine("Thread - outside the catch block"); // Never reached
}
```

Running this sample demonstrates that even after catching `ThreadAbortException`, the thread terminates as the exception is re-thrown.

---

## The Infinite Loop Bug: Tight Loops and Exception Handling

A nuanced bug emerges when nesting a `try-catch` directly inside a `while (true)` loop:

```csharp
static void DoWork() {
    var i = 0;
    while (true) {
        try {
            Console.WriteLine($"Thread - working {i}");
            i++;
            Thread.Sleep(100);
        } catch (ThreadAbortException e) {
            Console.WriteLine($"Thread - caught ThreadAbortException {e.Message}");
            // Expected: termination after catch
        }
    }
    Console.WriteLine("Thread - outside the catch block."); // Unreachable
}
```

**Observed Behavior:** On .NET Framework (with RyuJIT in Release mode), aborting the thread triggers a never-ending re-execution of the catch block, causing an infinite loop of caught exceptions.

This is due to a [runtime bug in RyuJIT](https://github.com/dotnet/runtime/issues/9633), not present in the legacy JIT. Workarounds include enabling the legacy JIT or altering loop structure (e.g., using a `finally` block, adding code outside the try-catch, or not nesting in a tight loop).

---

## Proactive Detection: Writing a Roslyn Analyzer

Since this problematic pattern is not always obvious, Andrew Lock developed a Roslyn Analyzer to flag occurrences, enabling teams to fix them early.

**Detection Logic:**

- Target `while` loops whose body is exclusively a `try-catch`.
- The catch clause handles `ThreadAbortException` (or its ancestors).
- The catch block does **not** include an explicit `throw;` to re-propagate the exception.

**Simplified Example of Problematic Pattern to Detect:**

```csharp
while(...) {
    try {
        // ...
    } catch {
        // ...
        // missing: throw;
    }
}
```

A minimal fix is simply re-throwing the exception in the catch block:

```csharp
while(...) {
    try {
        // ...
    } catch {
        // ...
        throw; // Required to avoid infinite recursion
    }
}
```

---

### Analyzer Implementation Overview

```csharp
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ThreadAbortAnalyzer : DiagnosticAnalyzer {
    public const string DiagnosticId = "ABRT0001";
    // Diagnostic registration and message setup
    public override void Initialize(AnalysisContext context) {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyseSyntax, SyntaxKind.WhileStatement);
    }
    // ... Analysis method that inspects loops and catch blocks ...
}
```

A helper class inspects whether the loop-catch structure matches the bug pattern, returning the problematic `catch` clause if so.

---

### Code Fix Provider

When the analyzer flags a problematic catch block, a `CodeFixProvider` can recommend and even auto-apply a fix by inserting `throw;` in the appropriate place.

```csharp
[ExportCodeFixProvider(LanguageNames.CSharp)]
public class ThreadAbortCodeFixProvider : CodeFixProvider {
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context) {
        // Logic to identify the affected catch block and register a code action to add 'throw;'
    }
    // ... Correction implementation ...
}
```

**Important Note:** This is a crude but effective fix. For robust code, catching `ThreadAbortException` explicitly and only rethrowing it, while handling other exceptions as needed, is preferable.

---

### Analyzer and Code Fix Limitations

- Conservative: Assumes all `while (true)` constructs are infinite loops, possibly flagging benign code.
- Ignores exception filters and finally blocks (which could mitigate the issue).
- Only detects `throw;` directly in the catch block (not after control flow statements).
- The automatic fix rethrows all exceptions caught; better practice may involve more granular handling.

**Preferred manual fix (handle explicitly):**

```csharp
while(true) {
    try {
        // ...
    } catch(ThreadAbortException) {
        // Log or handle
        throw; // avoid infinite loop
    } catch(Exception) {
        // Handle other exceptions as needed
    }
}
```

---

## Summary

- .NET Framework (with RyuJIT) has a bug where `ThreadAbortException` in certain loops leads to infinite catch block recursion.
- The safest fix is explicitly rethrowing such exceptions within problematic patterns.
- A custom Roslyn Analyzer can proactively spot these code patterns, flagging them for remediation and even providing a quick-fix.

This approach has helped prevent hard-to-diagnose production issues, especially in cross-targeted library development where .NET Framework support remains necessary.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-an-analyzer-to-detect-infinite-loops-caused-by-threadabortexception/)
