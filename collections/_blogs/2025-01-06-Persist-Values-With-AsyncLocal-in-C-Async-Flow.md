---
layout: post
title: Persist Values With AsyncLocal in C# Async Flow
author: Bozo Spoljaric
canonical_url: https://code-maze.com/csharp-persist-values-with-asynclocal-in-async-flow/
viewing_mode: external
feed_name: Code Maze Blog
feed_url: https://code-maze.com/feed/
date: 2025-01-06 05:55:32 +00:00
permalink: /coding/blogs/Persist-Values-With-AsyncLocal-in-C-Async-Flow
tags:
- .NET
- Async Programming
- Asynchronous Programming
- AsyncLocal
- Blogs
- C#
- Coding
- Context Propagation
- Correlation ID
- ExecutionContext
- Structured Logging
- Task Based Programming
- Thread Context
section_names:
- coding
---
Bozo Spoljaric introduces the AsyncLocal class, revealing how it enables value persistence throughout asynchronous flows in C#. The article offers step-by-step examples and guidance on when and how to effectively use AsyncLocal for more maintainable .NET code.<!--excerpt_end-->

# Persist Values With AsyncLocal in C# Async Flow

**Author:** Bozo Spoljaric

---

## Introduction

In this article, we explain the AsyncLocal class in C# and how to persist values across asynchronous flows. This tackles a common challenge in .NET async programming: passing data like user context or request identifiers through deeply nested async calls without excessive parameter passing or tightly coupling classes. We'll review the problems AsyncLocal solves, its core mechanics, concrete code examples, and best scenarios for its use.

---

## Background: Challenges of Data Flow in Async Operations

In developing asynchronous .NET applications, it's routine to require persistent context data such as:

- Authenticated user identity
- Correlation IDs for logging (to trace a request end-to-end)

Naively, you might propagate these values by adding more parameters to methods. However, this leads to "tramp data"—unnecessary parameters included only for the sake of passing them down the call chain. It increases method signatures' complexity and causes unnecessary coupling between unrelated classes.

---

## AsyncLocal and the ExecutionContext

A more robust alternative is offered through .NET's built-in global context mechanism—`ExecutionContext`—which attaches to tasks and is copied to worker threads as they execute. However, since you can't manipulate `ExecutionContext` directly, .NET exposes AsyncLocal fields for value access and storage scoped to a particular async flow. Typically, these fields are declared as `static`.

### Thread-Local Storage Does Not Suffice

While thread-local storage might seem attractive, task-based asynchronous programming means your continuation may not run on the same thread. Thus, thread-local approaches fail to provide robust value propagation through the async flow.

For a deep dive into the differences between tasks and threads, see: [Tasks vs Threads in C#](https://code-maze.com/csharp-tasks-vs-threads/).

---

## AsyncLocal Class Mechanics

The `AsyncLocal<T>` class provides two constructors:

- `AsyncLocal<T>()` (parameterless)
- `AsyncLocal<T>(Action<AsyncLocalValueChangedArgs<T>>)` — allows specifying an action for value change notifications

It exposes a single property:

- `Value` — gets or sets the context-attached value

---

## Example 1: Using AsyncLocal to Persist Values in Async Flow

Below is a static `AsyncLocalExample` class demonstrating the core workflow:

```csharp
public static class AsyncLocalExample
{
    public static readonly AsyncLocal<int> AsyncLocalInt = new();

    public static async Task DoWork()
    {
        AsyncLocalInt.Value = 1;
        Console.WriteLine($"AsyncLocal value in DoMainWork method: {AsyncLocalInt.Value}");

        await DoSubTaskLevel1();

        Console.WriteLine($"AsyncLocal value in DoMainWork method after executing DoSubTaskLevel1: {AsyncLocalInt.Value}");
    }

    private static async Task DoSubTaskLevel1()
    {
        Console.WriteLine($"AsyncLocal value when entering DoSubTaskLevel1: {AsyncLocalInt.Value}");
        AsyncLocalInt.Value++;
        Console.WriteLine($"AsyncLocal value after change in DoSubTaskLevel1: {AsyncLocalInt.Value}");

        await DoSubTaskLevel2();

        Console.WriteLine($"AsyncLocal value in DoSubTaskLevel1 after DoSubTaskLevel2: {AsyncLocalInt.Value}");
    }

    private static async Task DoSubTaskLevel2()
    {
        Console.WriteLine($"AsyncLocal value when entering DoSubTaskLevel2: {AsyncLocalInt.Value}");
        AsyncLocalInt.Value++;
        Console.WriteLine($"AsyncLocal value after change in DoSubTaskLevel2: {AsyncLocalInt.Value}");
        await Task.Delay(100);
    }
}
```

**Observed Output:**

```
AsyncLocal value in DoMainWork method: 1
AsyncLocal value when entering DoSubTaskLevel1 method: 1
AsyncLocal value after changing in DoSubTaskLevel1 method: 2
AsyncLocal value when entering DoSubTaskLevel2 method: 2
AsyncLocal value after changing in DoSubTaskLevel2 method: 3
AsyncLocal value in DoSubTaskLevel1 method after executing DoSubTaskLevel2: 2
AsyncLocal value in DoMainWork method after executing DoSubTaskLevel1: 1
```

**Key Points:**

- `AsyncLocalInt`'s value is copied to the `ExecutionContext` of every child task via copy-on-write (due to `ExecutionContext` immutability), not shared.
- Changes made in a child async method are not visible to the parent after resuming, illustrating the value-scoped isolation per async flow segment.

---

## Example 2: Value Change Notification with AsyncLocal

You can also subscribe to value changes via a notification action:

```csharp
public static class AsyncLocalNotifyExample
{
    public static readonly AsyncLocal<string> AsyncLocalString = new(AsyncLocalValueChangedAction);

    static Action<AsyncLocalValueChangedArgs<string>> AsyncLocalValueChangedAction => args => Console.WriteLine($"Current: {args.CurrentValue}, Previous: {args.PreviousValue}, Thread: {Environment.CurrentManagedThreadId}, ThreadContextChanged: {args.ThreadContextChanged}");

    public static async Task DoWork()
    {
        AsyncLocalString.Value = "Enter DoWork method";
        await DoSubTaskLevel1();
        AsyncLocalString.Value = "Exit DoWork method";
    }

    private static async Task DoSubTaskLevel1()
    {
        AsyncLocalString.Value = "Enter DoSubTaskLevel1 method";
        await DoSubTaskLevel2();
        AsyncLocalString.Value = "Exit DoSubTaskLevel1 method";
    }

    private static async Task DoSubTaskLevel2()
    {
        AsyncLocalString.Value = "Enter DoSubTaskLevel2 method";
        await Task.Delay(100);
        AsyncLocalString.Value = "Exit DoSubTaskLevel2 method";
    }
}
```

This approach triggers the action with the previous and current values and indicates if a thread/context switch has occurred:

```
Current: Enter DoWork method, Previous: , Thread: 5, ThreadContextChanged: False
Current: Enter DoSubTaskLevel1 method, Previous: Enter DoWork method, Thread: 5, ThreadContextChanged: False
Current: Enter DoSubTaskLevel2 method, Previous: Enter DoSubTaskLevel1 method, Thread: 5, ThreadContextChanged: False
... (many more transitions)
```

---

## When to Use AsyncLocal

AsyncLocal is not limited to simple types; you can use it with any .NET object, such as dictionaries or lists.

**Recommended scenarios include:**

- Storing request-specific data (user identity, session data, transaction IDs)
- Correlation IDs in structured logging for tracing

**Avoid using AsyncLocal for:**

- Data needed across completely unrelated asynchronous operations
- Application-wide global state (the context gets reset with each async flow)
- Cases where you expect thread context to be preserved

---

## Conclusion

`AsyncLocal<T>` provides a powerful tool to simplify async value propagation. It allows cleaner APIs and less boilerplate compared to passing tramp data through method parameters. However, it comes with an execution model you need to understand well: values flow down the async chain but changes don't propagate back up. Use it thoughtfully for context-scoped data in modern .NET applications.

---

[Download Source Code on GitHub](https://github.com/CodeMazeBlog/CodeMazeGuides/tree/main/csharp-intermediate-topics/PersistValuesInAsyncFlowUsingAsyncLocalInCSharp)

**Further reading:**

- [Tasks vs Threads in C#](https://code-maze.com/csharp-tasks-vs-threads/)
- [ASP.NET Core Web API Best Practices](https://code-maze.com/free-ebook-aspnetcore-webapi-best-practices/)

This post appeared first on "Code Maze Blog". [Read the entire article here](https://code-maze.com/csharp-persist-values-with-asynclocal-in-async-flow/)
