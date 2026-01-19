---
layout: post
title: Checked and Unchecked Arithmetic Operations in .NET
author: Khalid Abuhakmeh
canonical_url: https://khalidabuhakmeh.com/checked-and-unchecked-arithmetic-operations-in-dotnet
viewing_mode: external
feed_name: Khalid Abuhakmeh's Blog
feed_url: https://khalidabuhakmeh.com/feed.xml
date: 2024-08-27 00:00:00 +00:00
permalink: /coding/blogs/Checked-and-Unchecked-Arithmetic-Operations-in-NET
tags:
- .NET
- Arithmetic Overflow
- Best Practices
- C#
- Checked Keyword
- Fibonacci
- Integer Types
- INumber
- OverflowException
- Project Settings
- Spectre.Console
- System.Numerics
- Unchecked Keyword
section_names:
- coding
---
In this post, Khalid Abuhakmeh demonstrates how easily integer overflow can occur in .NET when generating the Fibonacci sequence, and provides practical strategies to avoid overflow errors in critical .NET applications.<!--excerpt_end-->

# Checked and Unchecked Arithmetic Operations in .NET

*Author: Khalid Abuhakmeh*

![Checked and Unchecked Arithmetic Operations in .NET](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/overflow-checked-unchecked-dotnet-math.jpg)
Photo by [Chris Liverani](https://unsplash.com/photos/red-pencil-on-top-of-mathematical-quiz-paper-rD2dc_2S3i0)

## Introduction

The author explores what happens when working with arithmetic in .NET, specifically using the Fibonacci sequence as an example. When attempting to generate the Fibonacci sequence for 48 iterations using an `int`, the computation overflows:

```text
Enter an integer: 48
Fibonacci sequence for 48: 0,1,1,2,3,5,8,13,21,34,55,89,144,233,377,610,987,1597,2584,4181,6765,10946,17711,28657,46368,
75025,121393,196418,317811,514229,832040,1346269,2178309,3524578,5702887,9227465,14930352,24157817,39088169,63245986,102334155,
165580141,267914296,433494437,701408733,1134903170,1836311903,-1323752223
```

Notice the final value is negative, indicating integer overflow.

## Cause of Overflow in .NET

By default, .NET performs arithmetic in an `unchecked` mode, meaning numbers can silently wrap around their bounds. For a signed 32-bit integer, the maximum value is `2,147,483,647`, and for an unsigned integer `4,294,967,295`. Overflow occurs silently unless explicitly handled.

In code, this looks like:

```csharp
using System.Numerics;
using Spectre.Console;

while (true)
{
    try
    {
        var integer = AnsiConsole.Ask<int>("Enter an integer: ");
        if (integer <= -1) {
            AnsiConsole.MarkupLine("Goodbye!");
            break;
        }
        var numbers = GenerateFibonacci<int>((uint)integer);
        AnsiConsole.MarkupLine($"[bold green]Fibonacci sequence for {integer}:[/]");
        AnsiConsole.MarkupLine($"[bold yellow]{string.Join(",", numbers)}[/]");
        AnsiConsole.MarkupLine("");
    }
    catch (ArgumentOutOfRangeException)
    {
        AnsiConsole.MarkupLine("[red]Error: pick a value greater than 2[/]");
    }
}

static T[] GenerateFibonacci<T>(uint iterations) where T : INumber<T>
{
    ArgumentOutOfRangeException.ThrowIfLessThan<uint>(iterations, 2, "iterations must be greater than or equal to 2");
    T[] fib = new T[iterations];
    fib[0] = T.Zero;
    fib[1] = T.One;
    for (int i = 2; i < iterations; i++) {
        fib[i] = fib[i - 1] + fib[i - 2];
    }
    return fib;
}
```

## How to Prevent Overflow

### Using the `checked` Keyword

To explicitly enable overflow checking (so that overflows throw exceptions rather than wrapping), you can use the `checked` keyword:

```csharp
static T[] GenerateFibonacci<T>(uint iterations) where T : INumber<T>
{
    ArgumentOutOfRangeException.ThrowIfLessThan<uint>(iterations, 2, "iterations must be greater than or equal to 2");
    T[] fib = new T[iterations];
    fib[0] = T.Zero;
    fib[1] = T.One;
    for (int i = 2; i < iterations; i++) {
        checked {
            fib[i] = fib[i - 1] + fib[i - 2];
        }
    }
    return fib;
}
```

This produces:

```text
Enter an integer: 49
Unhandled exception. System.OverflowException: Arithmetic operation resulted in an overflow.
 at System.Int32.System.Numerics.IAdditionOperators<System.Int32,System.Int32,System.Int32>.op_CheckedAddition(Int32 left, Int32 right)
 at Program.<<Main>$>g__GenerateFibonacci|0_0[T](UInt32 iterations) in /.../Program.cs:line 40
 at Program.<Main>$(String[] args) in /.../Program.cs:line 16
```

Using `checked` inserts calls to the intermediate language (IL) `op_CheckedAddition` operator, causing the runtime to check for overflows.

### Setting `CheckForOverflowUnderflow` in Project Settings

Overflow checking can be enabled project-wide by setting the property in your `.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <CheckForOverflowUnderflow>true</CheckForOverflowUnderflow>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Spectre.Console" Version="0.49.1" />
  </ItemGroup>
</Project>
```

With this property enabled, all supported arithmetic operations in the assembly are checked for overflow, without having to use the `checked` keyword explicitly.

### Using the `unchecked` Keyword

To opt out of overflow checking in a specific scope (if project-wide checking is enabled), use `unchecked`:

```csharp
static T[] GenerateFibonacci<T>(uint iterations) where T : INumber<T>
{
    ArgumentOutOfRangeException.ThrowIfLessThan<uint>(iterations, 2, "iterations must be greater than or equal to 2");
    T[] fib = new T[iterations];
    fib[0] = T.Zero;
    fib[1] = T.One;
    for (int i = 2; i < iterations; i++) {
        unchecked {
            fib[i] = fib[i - 1] + fib[i - 2];
        }
    }
    return fib;
}
```

## Conclusion

Arithmetic operations in .NET are unchecked by default, making overflow errors easy to miss unless you’re working with large or unbounded numbers. For critical code where values may exceed type limits, using `checked`, or enabling overflow checking at the project level provides a safety net. This ensures overflows are caught with exceptions, improving application reliability and preventing subtle bugs.

---

## About the Author

**Khalid Abuhakmeh** is a developer advocate at JetBrains specializing in .NET technologies and tooling.

---

## Related Reading

- [Confirmation Dialogs with Htmx and SweetAlert](https://khalidabuhakmeh.com/confirmation-dialogs-with-htmx-and-sweetalert)
- [Intersperse Values for Enumerable Collections](https://khalidabuhakmeh.com/intersperse-values-for-enumerable-collections)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/checked-and-unchecked-arithmetic-operations-in-dotnet)
