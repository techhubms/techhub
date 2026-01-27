---
external_url: https://khalidabuhakmeh.com/writing-a-string-numeric-comparer-with-dotnet-9
title: 'Writing a String Numeric Comparer in .NET: Handling Numeric Sorting with Spans'
author: Khalid Abuhakmeh
feed_name: Khalid Abuhakmeh's Blog
date: 2025-01-07 00:00:00 +00:00
tags:
- .NET
- Alphanumeric Ordering
- C#
- Data Structures
- IComparer
- Numeric Comparer
- Software Development
- Sorting
- Span APIs
- String Manipulation
section_names:
- coding
primary_section: coding
---
In this post, Khalid Abuhakmeh explores building a numeric string comparer in .NET using modern C# features like Span APIs. He shares practical code, discusses challenges, and offers guidance for developers handling complex sorting scenarios.<!--excerpt_end-->

# Writing a String Numeric Comparer in .NET: Handling Numeric Sorting with Spans

*By Khalid Abuhakmeh*

![Writing a String Numeric Comparer with .NET 9](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/string-numeric-comparer-dotnet-9.jpg)

*Photo by [Terminator 2: Judgement Day](https://www.imdb.com/title/tt0103064/)*

## Introduction

.NET 10 introduces a numeric comparer that allows sorting string elements containing numeric values—think movie sequels or software version numbers. As Khalid Abuhakmeh points out, this functionality was previously absent from .NET, and implementing it yourself reveals many subtleties and edge cases,
such as:

- Deciding the order of numbers versus Roman numerals
- Parsing decimals
- Determining how to handle subjective sorting scenarios

Khalid shares a custom implementation utilizing new .NET features, particularly C# Span APIs, and reflects on both its strengths and limitations.

## Motivating Example: A List of Numbered Things

Numbers at the end of text elements are common for:

- Movies (e.g., "Godfather 3")
- Software (e.g., "Windows 10")
- Version numbers (e.g., "1.10")

Sample C# code to sort such data:

```csharp
var numberedThings = new List<string> {
    "Godfather",
    "Godfather 3",
    "Scream",
    "Scream 2",
    "Scream 3",
    "Scream 1",
    "Windows 10",
    "Windows 7",
    "Windows 11",
    "Rocky 5",
    "Rocky 2",
    "Rocky 4",
    "Rocky 3",
    "Rock",
    "1.2",
    "1.3",
    "1.1",
    "Rocky",
    "Windows XP",
    "Godfather 2",
    "1.11",
    "1.10",
    "10.0",
    "1.0"
};

var numericOrderer = new NumericOrderer();
var sorted = numberedThings
    .OrderBy(x => x, numericOrderer)
    .ToList();

foreach (var item in sorted) {
    Console.WriteLine(item);
}
```

**Expected output:**

```text
1. 0
2. 1
3. 2
4. 3
5. 10
6. 11
7. 0
Godfather
Godfather 2
Godfather 3
Rock
Rocky
Rocky 2
Rocky 3
Rocky 4
Rocky 5
Scream
Scream 1
Scream 2
Scream 3
Windows 7
Windows 10
Windows 11
Windows XP
```

## Implementing `NumericOrderer` Using Spans

Khalid provides an implementation using C# Span APIs for efficient string manipulation:

```csharp
public sealed class NumericOrderer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        var xSpan = x.AsSpan();
        var ySpan = y.AsSpan();

        var commonPrefixLength = xSpan.CommonPrefixLength(ySpan);
        while (commonPrefixLength > 0)
        {
            xSpan = xSpan[commonPrefixLength..];
            ySpan = ySpan[commonPrefixLength..];
            commonPrefixLength = xSpan.CommonPrefixLength(ySpan);
        }

        if (int.TryParse(xSpan, out var xNumber) && int.TryParse(ySpan, out var yNumber))
        {
            return xNumber.CompareTo(yNumber);
        }

        return xSpan.CompareTo(ySpan, StringComparison.Ordinal);
    }
}
```

### Considerations for Configuration

- `CommonPrefixLength` can accept a custom comparer, useful for case-insensitive comparisons.
- The `CompareTo` method uses `StringComparison.Ordinal` (case-sensitive), which can be changed based on your needs.
- `int.TryParse` compares whole numbers; use `double.TryParse` to support decimals.

## Edge Cases and Limitations

Certain edge cases and subjective choices demand attention, including:

- Should numbers be ordered before/after Roman numerals?
- Should Roman numerals be parsed as numbers?
- Decimals and multi-part values (e.g., "1.10", "1.11")
- Arbitrary string content such as "Windows XP" versus "Windows 11"

As Khalid notes, building a universal comparer is complex and often overkill. Designing for specific use cases is advisable.

## Reflection on the Approach

- The `Span` APIs in C# are efficient for working with string segments without creating extra memory allocations.
- For production systems with more complex data, relying on actual numeric or date fields for sorting is preferable to string-based sorting.
- The shared implementation is ideal as a starting point for custom needs, but likely needs further adaptation for specific edge cases.

## Conclusion

Khalid ultimately recommends customizing numeric comparers as needed and notes the utility of `Span` for low-allocation string work. He suggests that for more robust and reliable sorting in product code, dedicated numeric fields or predictable sortable data are preferable. Still, the presented approach offers a solid foundation for most straightforward scenarios.

---

![Khalid Abuhakmeh's Picture](/assets/images/authorimage.jpg)

### About Khalid Abuhakmeh

Khalid is a developer advocate at JetBrains focusing on .NET technologies and tooling.

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/writing-a-string-numeric-comparer-with-dotnet-9)
