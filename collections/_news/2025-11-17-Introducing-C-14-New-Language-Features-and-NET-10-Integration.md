---
external_url: https://devblogs.microsoft.com/dotnet/introducing-csharp-14/
title: 'Introducing C# 14: New Language Features and .NET 10 Integration'
author: Bill Wagner
feed_name: Microsoft .NET Blog
date: 2025-11-17 18:05:00 +00:00
tags:
- .NET
- .NET 10
- C#
- C# 14
- Compound Assignment
- Extension
- Extension Members
- Field Keyword
- Lambda Expressions
- Language Features
- Null Conditional
- Null Conditional Assignment
- Operator Overloading
- Partial Types
- Performance Optimization
- Span Types
- Static Extension
- Unbound Generic Types
section_names:
- coding
primary_section: coding
---
Bill Wagner introduces the key features of C# 14 shipped with .NET 10, emphasizing extension members, productivity boosters, and performance enhancements for developers.<!--excerpt_end-->

# Introducing C# 14: New Language Features and .NET 10 Integration

**Author:** Bill Wagner

C# 14 arrives with .NET 10, bringing significant language updates focused on developer productivity and performance. This release introduces new syntax options, enhanced extension capabilities, and several features that streamline everyday coding tasks.

## Extension Members

The standout update in C# 14 is [extension members](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14#extension-members). With this feature, you can now define extension properties, operators, and static extension members, in addition to traditional extension methods. These allow you to enhance existing types more flexibly. For example:

```csharp
public static class EnumerableExtensions {
    // Instance-style extension members
    extension<TSource>(IEnumerable<TSource> source) {
        public bool IsEmpty => !source.Any();
        public IEnumerable<TSource> Where(Func<TSource, bool> predicate) {
            throw new NotImplementedException();
        }
        public static IEnumerable<TSource> Identity => Enumerable.Empty<TSource>();
        public static IEnumerable<TSource> operator +(IEnumerable<TSource> left, IEnumerable<TSource> right) => left.Concat(right);
    }
}
```

This new approach is backward-compatible, enabling a gradual migration of existing extension methods. For more, see the [C# Guide](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/extension-methods), [`extension` keyword article](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/extension), and [Extensions proposal](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-14.0/extensions).

## Productivity-Focused Features

C# 14 introduces improvements aimed at reducing boilerplate and enabling clearer, more focused code:

- **`field` keyword:** Enables logic in property accessors without leaving auto-property territory. [Details](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/field)
- **Unbound generic types and `nameof`:** You can now use `nameof` on unbound generic types, avoiding placeholder types—`nameof(List<>)` produces "List". [Reference](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/nameof)
- **Simple lambda parameters with modifiers:** Supports `out`, `in`, `ref`, and `scoped` parameter modifiers in lambdas without repeating type annotations. [Reference](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/lambda-expressions#input-parameters-of-a-lambda-expression)
- **Null-conditional assignment:** Apply null checks and assignments directly in a single line. [Feature doc](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14#null-conditional-assignment)
- **Partial events and constructors:** Large partial types can distribute event and constructor logic across multiple files. [Programming guide](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/constructors#partial-constructors)

## Performance-Driven Enhancements

.NET 10 and C# 14 deliver performance by adopting language-level changes into BCL and runtime libraries. Two features stand out:

### Implicit Span Conversions

Enables arrays, spans, and slices to be implicitly convertible, simplifying working with `Span<T>` and `ReadOnlySpan<T>`. This reduces temporary variables and unlocks more efficient APIs. [Spec](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-14.0/first-class-span-types)

### User-Defined Compound Assignment

Custom types can define how compound assignments like `+=` and `-=` work, making accumulation more efficient. [Operator overloading](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/operator-overloading), [specification](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-14.0/user-defined-compound-assignment).

## Further Resources and Links

- [What’s new in C# 14](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14)
- [.NET 10 Performance Improvements](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-10/)
- [Compiler breaking changes (.NET 10)](https://learn.microsoft.com/dotnet/csharp/whats-new/breaking-changes/compiler%20breaking%20changes%20-%20dotnet%2010)

## Conclusion

C# 14 brings practical, productivity-focused improvements to developers, along with performance gains due to tighter language/runtime integration. Start exploring these features to write cleaner, faster .NET 10 applications.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/introducing-csharp-14/)
