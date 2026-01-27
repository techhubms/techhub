---
external_url: https://andrewlock.net/making-foreach-on-an-ienumerable-allocation-free-using-reflection-and-dynamic-methods/
title: Making foreach on an IEnumerable Allocation-Free in .NET with Reflection and Dynamic Methods
author: Andrew Lock
feed_name: Andrew Lock's Blog
date: 2026-01-20 10:00:00 +00:00
tags:
- .NET
- .NET 10
- .NET 9
- .NET Core
- .NET Framework
- BenchmarkDotNet
- Boxing
- C#
- Custom Collections
- Datadog
- DynamicMethod
- Enumerator
- Generic Collections
- Heap Allocation
- IEnumerable
- IL Generation
- Performance
- Performance Optimization
- Reflection.Emit
- Stack Allocation
- Struct
section_names:
- coding
primary_section: coding
---
Andrew Lock explains how to make foreach iteration allocation-free on IEnumerable<T> in .NET, leveraging Reflection.Emit and DynamicMethod for advanced memory optimization, with benchmarks and implications for earlier and modern .NET runtimes.<!--excerpt_end-->

# Making foreach on an IEnumerable Allocation-Free in .NET with Reflection and Dynamic Methods

In this detailed post, Andrew Lock explores an advanced technique for reducing or eliminating heap allocations when using `foreach` on `IEnumerable<T>` in .NET. This discussion is rooted in performance optimization, especially for scenarios such as SDK development (e.g., the Datadog .NET SDK) where every byte and microsecond can matter.

## Allocation Pitfalls in foreach

While `foreach` is ubiquitous in C# code—appearing thousands of times even within the .NET runtime—its behavior varies depending on the actual static type of the collection variable. Iterating over a concrete type like `List<T>` is allocation-free because the enumerator is a stack-allocated struct. However, when using `foreach` against an `IEnumerable<T>` variable (even if it points to a List<T>), the struct enumerator must be boxed to satisfy the interface, resulting in a heap allocation (in .NET versions before 10).

### Benchmarking the Problem

Using BenchmarkDotNet, the post demonstrates that iterating over a `List<int>` is significantly faster and allocation-free compared to iterating over the same list via an `IEnumerable<int>` reference, especially noticeable on .NET Framework and .NET Core versions prior to 10.

````csharp
[Benchmark]
public long List() {
    var value = 0;
    foreach (int i in _list) { value += i; }
    return value;
}

[Benchmark]
public long IEnumerable() {
    var value = 0;
    foreach (int i in _enumerable) { value += i; }
    return value;
}
````

The `IEnumerable` version causes boxing and allocation, as shown in benchmark results.

## How foreach is Lowered by the Compiler

C# compilers pattern match to find a `GetEnumerator()` method returning an enumerator with `MoveNext` and `Current`. For concrete types like `List<T>`, this is a struct enumerator, stack-allocated. But for the generic interface, the struct is boxed.

## Improvements in .NET 10: Deabstraction

.NET 10 improves this scenario through devirtualization and stack allocation optimizations, removing these heap allocations even for interface-based usage in many cases. For pre-.NET 10 runtimes and where backward compatibility is needed, alternative strategies are still valuable.

## Allocation-Free Workarounds

### If Concrete Type is Known

If you know the concrete collection type at runtime, cast to that type before enumerating. This enables the compiler to use the stack-allocated enumerator.

````csharp
if (someCollection is List<int> list) {
    foreach (var value in list) { ... }
} else {
    foreach (var value in someCollection) { ... }
}
````

### If Concrete Type is Internal or Unavailable

Often, especially in SDKs or when instrumenting the runtime, you can't reference the actual type (it's internal or not public). To sidestep allocation in such cases, Andrew Lock describes a technique using `Reflection.Emit` to dynamically generate a `DynamicMethod` that does allocation-free iteration by directly using the struct enumerator via emitted IL.

#### Example Delegate with Reflection.Emit

The code uses Reflection to:

- Find a non-interface returning `GetEnumerator`.
- Build a delegate with a custom callback for each element.
- Use IL generation to create a lowered, allocation-free foreach.

```csharp
public static AllocationFreeForEachDelegate BuildAllocationFreeForEachDelegate(Type enumerableType) {
    ...
    // Generates custom IL for allocation-free enumeration as described
}
```

This technique enables allocation-free iteration even when the concrete collection type is not public or known at compile time.

## Further Benchmarks

Andrew tests this Reflection.Emit approach across various .NET versions and both built-in (`List<T>`) and custom collection types with struct enumerators. The dynamic method enables allocation-free iteration in older runtimes—even for custom collections—where the default `foreach` over `IEnumerable<T>` would cause allocations.

- On .NET 9 and earlier: The DynamicMethod approach is allocation-free and sometimes faster.
- On .NET 10: The default behavior is already allocation-free; DynamicMethod offers no further benefits and is slower.

## Conclusion and Practical Guidance

- If performance profiling reveals heap allocations from `foreach` on `IEnumerable<T>`, and you're targeting older runtimes, these techniques can help eliminate those allocations.
- For .NET 10 and beyond, the compiler/runtime already addresses the issue.
- This approach is best reserved for hot code paths and is not necessary for general usage, given its complexity.

## References & Further Reading

- [GitHub Source Example](https://github.com/open-telemetry/opentelemetry-dotnet/blob/73bff75ef653f81fe6877299435b21131be36dc0/src/OpenTelemetry/Internal/EnumerationHelper.cs#L58)
- [MSDN Docs: IEnumerable.GetEnumerator](https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerable.getenumerator?view=net-10.0)
- [Eric Lippert: Following the Pattern](https://ericlippert.com/2011/06/30/following-the-pattern/)
- [.NET 10 Performance Improvements](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-10/#deabstraction)

---

**Author:** Andrew Lock  
**Blog:** .NET Escapades

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/making-foreach-on-an-ienumerable-allocation-free-using-reflection-and-dynamic-methods/)
