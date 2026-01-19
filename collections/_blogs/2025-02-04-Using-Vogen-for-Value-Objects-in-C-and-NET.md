---
external_url: https://khalidabuhakmeh.com/vogen-and-value-objects-with-csharp-and-dotnet
title: Using Vogen for Value Objects in C# and .NET
author: Khalid Abuhakmeh
viewing_mode: external
feed_name: Khalid Abuhakmeh's Blog
date: 2025-02-04 00:00:00 +00:00
tags:
- .NET
- C#
- Domain Modeling
- NuGet
- Pac Man
- Source Generators
- Type Safety
- Validation
- Value Objects
- Vogen
section_names:
- coding
---
In this post, Khalid Abuhakmeh demonstrates how value objects and the Vogen library can enhance code correctness and readability in C# and .NET. The article is packed with practical code examples and advice for applying value object patterns.<!--excerpt_end-->

# Vogen and Value Objects with C# and .NET

*By Khalid Abuhakmeh*

![Vogen and Value Objects with C# and .NET](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/vogen-value-objects-csharp-dotnet.jpg)

Photo by [Captain Caveman](https://en.wikipedia.org/wiki/Captain_Caveman_and_the_Teen_Angels)

## Introduction

Programming is not just about writing code that works—it's about ensuring that the code behaves correctly under all circumstances. To prevent logic errors and unexpected behavior, developers frequently try to constrain and model potential outliers in their code. One way to achieve this in .NET is by leveraging value objects, a tactic that can add both clarity and safety, especially with the help of tools like the Vogen library.

This post explores how value objects work, their benefits, and how Vogen can help automate and enforce these patterns in your codebase.

## What is a Value Object?

A **value object** is a simple, immutable object that represents a specific domain concept using a .NET primitive value such as `int`, `bool`, or `string`. For example, consider representing a person's birth date:

```csharp
DateTime birthDate = new DateTime(1990, 1, 1);
```

On its own, this code doesn't prevent confusion between, for example, a birth date and a movie release date. Both are stored as `DateTime` values and could be incorrectly interchanged:

```csharp
DateTime birthDate = new DateTime(1990, 1, 1);
SetMovieReleaseDate(birthDate);

void SetMovieReleaseDate(DateTime date) { /* ... */ }
```

This lack of specificity can introduce logical bugs. To clarify intent, you can define distinct value objects:

```csharp
var birthDate = new BirthDateTime(new DateTime(1990, 1, 1));
var movieReleaseDate = new MovieReleaseDateTime(birthDate.Value);
SetMovieReleaseDate(movieReleaseDate);

void SetMovieReleaseDate(MovieReleaseDateTime date) { /* ... */ }

// value objects
public record MovieReleaseDateTime(DateTime Value);
public record BirthDateTime(DateTime Value);
```

Now, the code enforces logical separation between different uses of `DateTime`. While such an approach may appear verbose, it enhances clarity and reduces logical errors.

## What is Vogen?

[Vogen](https://stevedunn.github.io/Vogen/overview.html) is a NuGet library for .NET that uses source generators to automate the creation of value objects. Vogen reduces boilerplate around value objects by generating factory methods, equality checks, validation routines, and serializer support for partial `struct` and `class` definitions.

### Getting Started

Add Vogen to a .NET project via NuGet:

```xml
<PackageReference Include="Vogen" Version="7.0.0-beta.1" />
```

### Example: Modeling Pac-Man's Favorite Ghost

Suppose you want a `PacMan` class with a property `FavoriteGhost` of type `Ghost` (which should only allow known Pac-Man ghosts).

Unconstrained approach using Vogen:

```csharp
[ValueObject<string>]
[Instance("Blinky", "Blinky")]
[Instance("Pinky", "Pinky")]
[Instance("Inky", "Inky")]
[Instance("Clyde", "Clyde")]
public partial struct Ghost;
```

The generated code allows usage like:

```csharp
var pacMan = new PacMan { FavoriteGhost = Ghost.Blinky };

var aNewGhost = Ghost.From("Khalid"); // Valid, but not desired
```

### Constraining Possible Values

To tightly constrain the `Ghost` value object so only specific names are allowed:

```csharp
[ValueObject<string>]
public partial struct Ghost
{
    public static readonly Ghost Blinky = new("Blinky");
    public static readonly Ghost Pinky = new("Pinky");
    public static readonly Ghost Inky = new("Inky");
    public static readonly Ghost Clyde = new("Clyde");

    public static IReadOnlyCollection<Ghost> All { get; } = new[] { Blinky, Pinky, Inky, Clyde }.AsReadOnly();

    private static Validation Validate(string input) =>
        All.Any(g => g.Equals(input))
            ? Validation.Ok
            : Validation.Invalid($"Ghost must be {string.Join(", ", All)}");
}
```

This setup provides:

- A limited, explicit set of valid values
- Compile-time and runtime validation
- Cleaner, safer modeling of domain logic

Usage examples:

```csharp
// Throws an exception (invalid ghost name)
var aNewGhost = Ghost.From("Khalid");

// Passes validation
var aKnownGhost = Ghost.From("Blinky");
```

### Complete Sample

Putting it all together:

```csharp
using Vogen;

var pacMan = new PacMan { FavoriteGhost = Ghost.Blinky };

foreach (var ghost in Ghost.All)
{
    Console.WriteLine(ghost);
}

Console.WriteLine(pacMan);

[ValueObject<string>]
public partial struct Ghost
{
    public static readonly Ghost Blinky = new("Blinky");
    public static readonly Ghost Pinky = new("Pinky");
    public static readonly Ghost Inky = new("Inky");
    public static readonly Ghost Clyde = new("Clyde");

    public static IReadOnlyCollection<Ghost> All { get; } = new[] { Blinky, Pinky, Inky, Clyde }.AsReadOnly();

    private static Validation Validate(string input) =>
        All.Any(g => g.Equals(input))
            ? Validation.Ok
            : Validation.Invalid($"Ghost must be {string.Join(", ", All)}");
}

public class PacMan
{
    public Ghost FavoriteGhost { get; set; }

    public override string ToString() => $"Pac Man's favorite ghost is {FavoriteGhost}.";
}
```

## Conclusion

Value objects in .NET provide logical constraints and improve code readability. With Vogen, you can automate much of the boilerplate, reducing developer errors while supporting explicit domain modeling. Consider experimenting by converting some of your existing code to use value objects and see if it helps with both correctness and maintainability.

---

*About Khalid Abuhakmeh*

Khalid is a developer advocate at JetBrains with a focus on .NET technologies and tooling.

---

### Further Reading

- [Initialize ASP.NET Core TagHelpers with Shared Data](/initialize-aspnet-core-taghelpers-with-shared-data)
- [ASP.NET Core and Chunking HTTP Cookies](/aspnet-core-and-chunking-http-cookies)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/vogen-and-value-objects-with-csharp-and-dotnet)
