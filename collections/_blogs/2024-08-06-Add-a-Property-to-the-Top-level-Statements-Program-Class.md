---
external_url: https://khalidabuhakmeh.com/add-a-property-to-the-top-level-statements-program-class
title: Add a Property to the Top-level Statements Program Class
author: Khalid Abuhakmeh
feed_name: Khalid Abuhakmeh's Blog
date: 2024-08-06 00:00:00 +00:00
tags:
- .NET
- C#
- Code Readability
- Compiler
- Console Application
- Partial Class
- Program Class
- Program Structure
- Static Properties
- Top Level Statements
- Coding
- Blogs
section_names:
- coding
primary_section: coding
---
Khalid Abuhakmeh explains how to add static properties to a top-level statements Program class in C# using partial classes. This post guides readers on creating more readable console applications and delves into the compiler-generated code structure.<!--excerpt_end-->

## Summary

In his article, Khalid Abuhakmeh addresses the stylistic changes introduced with top-level statements in modern C#, specifically focusing on ways to enhance utility console applications by adding properties to the implicit `Program` class.

### Key Points

- **Top-level Statements**: C#'s new style allows you to write the entry point of a console program without explicitly wrapping code in a `Main` method or class. The compiler generates a default `Program` class and `static void Main` entry during compilation.

- **Accessing Methods in Top-level Files**: Functions declared after top-level code are translated by the compiler into static methods on the generated `Program` class. However, adding properties directly with the same syntax as methods is not valid.

- **Why Add Properties?**: Using properties instead of methods can improve code readability, especially for values that represent state rather than behavior.

- **How to Add Properties**: You cannot declare properties in the same file using top-level syntax, but you can extend the generated `Program` class with a partial class definition in the same file or another file. The property must be declared as `static`, since the compiler-generated entry method is static.

```csharp
Console.WriteLine($"Hello, {Name}");

partial class Program
{
    static string Name => Environment.GetCommandLineArgs()[1];
}
```

- **Compiler Result**: When compiled, the property is added to the `Program` class and can be accessed from top-level code, provided it is static.

- **Separation of Concerns**: You may move the partial `Program` class implementation to a different file for organizational clarity, maintaining the minimalist appearance in your `Program.cs` entry point.

### Additional Details

- All properties added this way must be static due to their usage in the static context of the generated `Main` method.
- This technique helps keep top-level files concise and maintainable, especially in simple utilities and scripts.

## Takeaways

- Leverage partial classes to supplement top-level statement files with state or derived properties.
- Ensure properties are static to be compatible with the static compilation context.
- This pattern supports both code clarity and separation, aligning with modern C# application style conventions.

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/add-a-property-to-the-top-level-statements-program-class)
