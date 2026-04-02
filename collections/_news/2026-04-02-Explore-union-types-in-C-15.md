---
primary_section: dotnet
feed_name: Microsoft .NET Blog
section_names:
- dotnet
date: 2026-04-02 17:05:00 +00:00
external_url: https://devblogs.microsoft.com/dotnet/csharp-15-union-types/
author: Bill Wagner
tags:
- .NET
- .NET 11
- .NET 11 Preview 2
- C#
- C# 15
- C# Dev Kit Insiders
- Closed Enums
- Closed Hierarchies
- Compiler Warnings
- Discriminated Unions
- Exhaustive Switch
- Implicit Conversions
- IUnion Interface
- LangVersion Preview
- Net11.0
- News
- Nullable Reference Types
- Pattern Matching
- RuntimePolyfill
- Switch Expressions
- System.Runtime.CompilerServices.UnionAttribute
- Union Types
- Value Property
- Visual Studio Insiders
title: Explore union types in C# 15
---

Bill Wagner introduces union types in C# 15, explaining the new `union` keyword, how it enables exhaustive pattern matching over a closed set of case types, and how to try the feature today using .NET 11 Preview 2 with `<LangVersion>preview</LangVersion>`.<!--excerpt_end-->

# Explore union types in C# 15

Union types have been frequently requested for C#, and they’re here. Starting with **.NET 11 Preview 2**, **C# 15** introduces the `union` keyword. A `union` declares that a value is **exactly one of a fixed set of types**, with **compiler-enforced exhaustive pattern matching**.

If you’ve used discriminated unions in F# (or similar features in other languages), the idea will feel familiar. C# unions aim for a C#-native experience: they compose existing types, integrate with existing pattern matching, and work with the rest of the language.

## What are union types?

Before C# 15, returning “one of several possible types” had trade-offs:

- Using `object` doesn’t constrain what can be stored, forcing defensive checks.
- Marker interfaces / abstract base classes restrict types, but the set can’t be **closed** (others can implement/derive), so the compiler can’t treat it as complete.
- Both require a shared ancestor, which fails for unrelated types like `string` and `Exception`.

Union types address these:

- A union declares a **closed** set of case types (no other types can be added).
- The compiler can guarantee `switch` expressions are **exhaustive** without `_`/`default`.
- You can compose unions from unrelated existing types into a single, compiler-verified contract.

### Basic union declaration

```csharp
public record class Cat(string Name);
public record class Dog(string Name);
public record class Bird(string Name);

public union Pet(Cat, Dog, Bird);
```

This declares `Pet` as a new type that can hold a `Cat`, `Dog`, or `Bird`.

### Implicit conversions from case types

The compiler provides implicit conversions from each case type:

```csharp
Pet pet = new Dog("Rex");
Console.WriteLine(pet.Value); // Dog { Name = Rex }

Pet pet2 = new Cat("Whiskers");
Console.WriteLine(pet2.Value); // Cat { Name = Whiskers }
```

Assigning a value that is not one of the case types is a compiler error.

### Exhaustive pattern matching

Because the compiler knows the complete set of case types, a `switch` expression that covers all of them is exhaustive:

```csharp
string name = pet switch
{
    Dog d => d.Name,
    Cat c => c.Name,
    Bird b => b.Name,
};
```

- If case types are non-nullable, a null-check is not required.
- If any case types are nullable (e.g., `Bird?`), you must handle `null` for exhaustiveness.
- If you later add a case type to the union, existing switches that don’t handle it produce a compiler warning.

### How patterns apply: `Value` unwrapping

Patterns apply to the union’s `Value` property, not the union struct itself. This unwrapping is automatic.

Exceptions:

- `var` and `_` apply to the union value itself (capture/ignore the whole union).

### `null` and default unions

For `union` types, the `null` pattern checks whether `Value` is null. The default value of a union struct has a null `Value`:

```csharp
Pet pet = default;

var description = pet switch
{
    Dog d => d.Name,
    Cat c => c.Name,
    Bird b => b.Name,
    null => "no pet",
};
// description is "no pet"
```

## OneOrMore<T>: single value or collection

Unions can have a body with helper members. Example: an API that accepts either a single value or a collection.

```csharp
public union OneOrMore<T>(T, IEnumerable<T>)
{
    public IEnumerable<T> AsEnumerable() => Value switch
    {
        T single => [single],
        IEnumerable<T> multiple => multiple,
        null => []
    };
}
```

Notes:

- `AsEnumerable` must handle `null` because the default null-state of `Value` is *maybe-null*.
- This provides proper warnings for arrays of a union type and for default union values.

Usage:

```csharp
OneOrMore<string> tags = "dotnet";
OneOrMore<string> moreTags = new[] { "csharp", "unions", "preview" };

foreach (var tag in tags.AsEnumerable())
    Console.Write($"[{tag}] "); // [dotnet]

foreach (var tag in moreTags.AsEnumerable())
    Console.Write($"[{tag}] "); // [csharp] [unions] [preview]
```

## Custom unions for existing libraries

The `union` declaration is shorthand. The compiler generates a struct with:

- A constructor for each case type
- A `Value` property of type `object?` holding the underlying value

This means:

- The union stores contents as a single `object?` reference.
- Value types are boxed.

Community libraries can still participate without switching syntax. Any class/struct with a `[System.Runtime.CompilerServices.Union]` attribute is recognized as a union type if it follows the basic pattern:

- One or more public single-parameter constructors (case types)
- A public `Value` property

For performance-sensitive scenarios (especially when case types include value types), libraries can implement a non-boxing access pattern by adding:

- `HasValue` property
- `TryGetValue` methods

That allows pattern matching without boxing.

See: [union types language reference](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/union#custom-union-types)

## Related proposals

Union types are part of a broader “exhaustiveness” roadmap:

- [Closed hierarchies](https://github.com/dotnet/csharplang/blob/main/proposals/closed-hierarchies.md): a `closed` modifier prevents derived classes outside the defining assembly.
- [Closed enums](https://github.com/dotnet/csharplang/blob/main/proposals/closed-enums.md): a `closed` enum prevents creating values beyond the declared members.

Together:

- **Union types** — exhaustive matching over a closed set of types
- **Closed hierarchies** — exhaustive matching over a sealed class hierarchy
- **Closed enums** — exhaustive matching over a fixed set of enum values

These proposals are active, but not yet committed to a final release.

## Try it yourself

Union types are available starting with **.NET 11 Preview 2**.

1. Install the [.NET 11 Preview SDK](https://dotnet.microsoft.com/download/dotnet).
2. Create or update a project targeting `net11.0`.
3. Set `<LangVersion>preview</LangVersion>` in your project file.

IDE support notes:

- Visual Studio support will be available in the next Visual Studio Insiders build.
- It is included in the latest C# DevKit Insiders build.

### Early preview: declare runtime types yourself

In .NET 11 Preview 2, the `UnionAttribute` and `IUnion` interface aren’t included in the runtime yet, so you must declare them in your project.

You can add these (or grab `RuntimePolyfill.cs` from the docs repo: https://github.com/dotnet/docs/blob/e68b5dd1e557b53c45ca43e61b013bc919619fb9/docs/csharp/language-reference/builtin-types/snippets/unions/RuntimePolyfill.cs):

```csharp
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public sealed class UnionAttribute : Attribute;

    public interface IUnion
    {
        object? Value { get; }
    }
}
```

Then you can declare and use a union:

```csharp
public record class Cat(string Name);
public record class Dog(string Name);

public union Pet(Cat, Dog);

Pet pet = new Cat("Whiskers");
Console.WriteLine(pet switch
{
    Cat c => $"Cat: {c.Name}",
    Dog d => $"Dog: {d.Name}",
});
```

Some items from the full proposal aren’t implemented yet (including union member providers). See: https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/unions

## Share your feedback

Union types are in preview, and feedback shapes the final design.

- Discussion: https://github.com/dotnet/csharplang/discussions/9663

More references:

- [Union types — language reference](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/union)
- [Unions — feature specification](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/unions)
- [What’s new in C# 15](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-15)


[Read the entire article](https://devblogs.microsoft.com/dotnet/csharp-15-union-types/)

