---
external_url: https://khalidabuhakmeh.com/intersperse-values-for-enumerable-collections
title: Intersperse Values for Enumerable Collections in C#
author: Khalid Abuhakmeh
feed_name: Khalid Abuhakmeh's Blog
date: 2024-09-10 00:00:00 +00:00
tags:
- .NET
- C#
- Collections
- Extension Methods
- Functional Programming
- IEnumerable
- Intersperse
- Separator
- Standard Library
- Yield
section_names:
- coding
primary_section: coding
---
In this post, Khalid Abuhakmeh explores how to create an 'intersperse' extension method for IEnumerable in C#, inspired by a JavaScript utility. The article includes code samples, expected outputs, and a detailed explanation of the implementation.<!--excerpt_end-->

# Intersperse Values for Enumerable Collections in C#

*Author: Khalid Abuhakmeh*

![Intersperse Values for Enumerable Collections](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/intersperse-values-enumerable-csharp.jpg)

Photo by [Super Snapper](https://unsplash.com/@supersnapper27)

## Introduction

JavaScript comes equipped with a relatively small standard library compared to the .NET Base Class Library (BCL). While the JavaScript community has produced several attempts to provide a more standard set of utilities ([see npmjs.com standard library search](https://www.npmjs.com/search?q=standard%20library)), many .NET developers are used to a broad and mature set of built-ins.

One such utility from the JavaScript ecosystem is [`intersperse`](https://js-std.pages.dev/Array/intersperse), which inserts a separator element between the items of a list. Inspired by this, this post demonstrates how to create an `intersperse` extension method for `IEnumerable` collections in C#.

## Intersperse Implementation in C#

Let's look at a few practical examples and their expected output before diving into the code implementation.

### Usage Examples

```csharp
var hello = new string("Hello".Intersperse('-').ToArray());
var one = new string("1".Intersperse('x').ToArray());
var @null = ((IEnumerable<object>)null!).Intersperse(',').ToArray();
var array = new[] { 1, 2, 3 }.Intersperse(42).ToArray();
var menu = new [] {"Home", "About", "Privacy" }
    .Intersperse(" > ")
    .Aggregate((a, b) => $"{a}{b}");

Console.WriteLine($"'{hello}' interspersed with '-' is {hello}");
Console.WriteLine($"1 interspersed is {one}");
Console.WriteLine($"null interspersed is {@null}");
Console.WriteLine($"array interspersed is {string.Join(", ", array)}");
Console.WriteLine($"The menu is {menu}");
```

#### Expected Output

```text
'H-e-l-l-o' interspersed with '-' is H-e-l-l-o
1 interspersed is 1
null interspersed is System.Object[]
array interspersed is 1, 42, 2, 42, 3
The menu is Home > About > Privacy
```

## Implementation

Below is the C# implementation for the `Intersperse` extension method. This method is generic and can be applied to any `IEnumerable<T>`. By using the `yield` keyword, the implementation is both efficient and avoids unnecessary iterations.

```csharp
public static class EnumerableExtensions
{
    public static IEnumerable<T> Intersperse<T>(this IEnumerable<T>? source, T delimiter)
    {
        if (source is null)
            yield break;

        using var enumerator = source.GetEnumerator();
        var hasFirstElement = enumerator.MoveNext();

        if (hasFirstElement == false)
            yield break;

        yield return enumerator.Current;

        while (enumerator.MoveNext())
        {
            yield return delimiter;
            yield return enumerator.Current;
        }
    }
}
```

### Notes and Rationale

- The method checks if the source is null and breaks early if so.
- It uses an explicit enumerator to detect the first element and ensures correct placement of separator values.
- The use of `yield` keeps enumeration lazy and minimizes memory overhead.
- While constructs like `Zip` could also achieve a similar result, this approach is simpler and more straightforward for the purpose.

## Conclusion

By borrowing a useful concept from JavaScript's ecosystem, .NET developers can enhance the flexibility of collection manipulation in C#. The `Intersperse` extension method provides a concise and reusable way to insert delimiters in collections, following idiomatic .NET patterns.

Thank you for reading this quick blog post. As always, cheers!

---

![Khalid Abuhakmeh's Picture](/assets/images/authorimage.jpg)

## About Khalid Abuhakmeh

Khalid is a developer advocate at JetBrains focusing on .NET technologies and tooling.

## Read Next

- [Checked and Unchecked Arithmetic Operations in .NET](/checked-and-unchecked-arithmetic-operations-in-dotnet)
- [Htmx and Playwright Tests in C#](/htmx-and-playwright-tests-in-csharp)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/intersperse-values-for-enumerable-collections)
