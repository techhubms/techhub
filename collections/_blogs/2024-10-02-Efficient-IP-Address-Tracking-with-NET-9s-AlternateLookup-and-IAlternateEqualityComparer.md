---
external_url: https://www.stevejgordon.co.uk/efficient-dictionary-for-ipaddress-tracking-using-net-9-with-alternatelookup-and-ialternateequalitycomparer
title: Efficient IP Address Tracking with .NET 9's AlternateLookup and IAlternateEqualityComparer
author: Steve Gordon
feed_name: Steve Gordon's Blog
date: 2024-10-02 08:58:07 +00:00
tags:
- .NET
- .NET 9
- AlternateLookup
- C#
- C# 13
- Collections
- Dictionary
- Equality Comparer
- IAlternateEqualityComparer
- IPAddress Tracking
- Low Allocation
- Performance
- ReadOnlyMemory
- ReadOnlySpan
- Span
- Stackalloc
section_names:
- coding
primary_section: coding
---
In this in-depth article, Steve Gordon demonstrates enhancements in .NET 9 and C# 13 collections, focusing on building an efficient, low-allocation dictionary for IP address tracking using AlternateLookup and custom equality comparers.<!--excerpt_end-->

# Efficient IP Address Tracking with .NET 9's AlternateLookup and IAlternateEqualityComparer

**Author:** Steve Gordon

![Header image for blog post about creating an efficient Dictionary for IPAddress tracking using .NET 9 with AlternateLookup and IAlternateEqualityComparer](https://www.stevejgordon.co.uk/wp-content/uploads/2024/10/An-Efficient-Dictionary-for-IPAddress-Tracking-using-.NET-9-with-AlternateLookup-and-IAlternateEqualityComparer-750x410.png)

## Introduction

In this post, Steve Gordon introduces key enhancements to collections in .NET 9 and C# 13 that enable low-allocation code paths. The focus is on using a custom `IAlternateEqualityComparer` and the new `AlternateLookup` capability on `Dictionary` for efficiently tracking IP address information, using the bytes of IP addresses as keys.

## Use Case

Steve's primary use case for these techniques was to track the number of requests originating from non-GitHub IP addresses during the handling of GitHub webhooks. The objective was to store a small, performant, and memory-efficient amount of state, keyed by the IP address. This article simplifies the scenario to focus on the practical aspects of the new `AlternateLookup` feature in C# 13 and .NET 9.

## Basic Approach

Whenever the application encounters an IP address not known to be from GitHub, it should add the IP to a dictionary if it is new, or retrieve the existing state if it is already present.

### Dictionary Initialization

The data is stored as follows:

```csharp
var dictionary = new Dictionary<ReadOnlyMemory<byte>, string>(ReadOnlyMemoryComparer.Default);
```

- **Key Type:** `ReadOnlyMemory<byte>` (representing the IP address bytes)
- **Value Type:** `string` (could be a more complex type in real scenarios)
- **Comparer:** A custom comparer, described below

### Preparing the Key (Low-Allocation Technique)

To avoid unnecessary memory allocations when checking remote IP addresses, Steve uses a `Span<byte>` allocated on the stack:

```csharp
var ipAddress = IPAddress.Parse("172.100.50.50");
var size = ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ? 4 : 16;
Span<byte> key = stackalloc byte[size];
if (!ipAddress.TryWriteBytes(key, out var bytesWritten)) { /* Should never happen */ }
key = key[..bytesWritten];
```

This technique avoids heap allocations by using `stackalloc` for temporary buffers and populates them via `IPAddress.TryWriteBytes`.

## The Need for AlternateLookup

Before .NET 9, it wasn't possible to directly use a `Span<byte>` as a lookup key in a `Dictionary` keyed by a different structure, such as `ReadOnlyMemory<byte>`. .NET 9 introduces the `AlternateLookup` feature to allow for such scenarios.

### Custom IAlternateEqualityComparer Implementation

To take advantage of `AlternateLookup`, you must implement an `IAlternateEqualityComparer<TAlternate, TKey>`. Steve shares the full implementation:

```csharp
internal sealed class ReadOnlyMemoryComparer : IEqualityComparer<ReadOnlyMemory<byte>>, IAlternateEqualityComparer<ReadOnlySpan<byte>, ReadOnlyMemory<byte>> {
    public static IEqualityComparer<ReadOnlyMemory<byte>> Default { get; } = new ReadOnlyMemoryComparer();

    public ReadOnlyMemory<byte> Create(ReadOnlySpan<byte> alternate) => alternate.ToArray();

    public bool Equals(ReadOnlySpan<byte> alternate, ReadOnlyMemory<byte> other) => alternate.SequenceEqual(other.Span);

    public bool Equals(ReadOnlyMemory<byte> x, ReadOnlyMemory<byte> y) => x.Span.SequenceEqual(y.Span);

    public int GetHashCode(ReadOnlySpan<byte> alternate) {
        HashCode hc = default;
        hc.AddBytes(alternate);
        return hc.ToHashCode();
    }

    public int GetHashCode(ReadOnlyMemory<byte> obj) => GetHashCode(obj.Span);
}
```

**Key Points:**

- Implements both `IEqualityComparer<ReadOnlyMemory<byte>>` and `IAlternateEqualityComparer<ReadOnlySpan<byte>, ReadOnlyMemory<byte>>`.
- Methods for equality and hashing support both `ReadOnlyMemory<byte>` and `ReadOnlySpan<byte>`.
- The `Create` method converts a span to an array for safe dictionary storage (with a minimal heap allocation).
- Caches the comparer instance via a static property.

## Using the AlternateLookup Feature

With the custom comparer in place, you can now create an alternate-lookup view of the dictionary:

```csharp
var alternateLookup = dictionary.GetAlternateLookup<ReadOnlySpan<byte>>();
```

This enables span-based access to the dictionary for both reads and writes:

```csharp
alternateLookup[buffer] = "Hello, world!"; // Adds or updates entry
var exists = alternateLookup.ContainsKey(buffer); // Checks for existence
```

## Performance Considerations

- Stack-allocated buffers and span-based access reduce memory allocations and can improve lookup performance.
- While this pattern could be replicated using `IPAddress` as the key, using byte arrays avoids holding onto larger objects and can provide better performance, though measuring this can be complex.

## Conclusion and Further Application

The `AlternateLookup` feature in .NET 9 collections enables new patterns for high-performance, low-allocation code, especially when working with non-string keys like bytes or chars. Although this article covers a basic introduction, similar techniques can be applied to other data types (e.g., string keys using spans instead of allocations).

### Support and About the Author

If you found this post useful, consider [supporting Steve Gordon](https://www.buymeacoffee.com/stevejgordon) or via [PayPal](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=WV4JPPV9FS34L&source=url).

**About Steve Gordon:** Steve is a .NET engineer, Pluralsight author, Microsoft MVP, and maintains the .NET APM agent at Elastic. He is active in the .NET community through blogging, OSS contribution, and speaking engagements.

For more, visit [Steve Gordon's Blog](https://www.stevejgordon.co.uk) or follow him on [Twitter](https://twitter.com/stevejgordon).

This post appeared first on "Steve Gordon's Blog". [Read the entire article here](https://www.stevejgordon.co.uk/efficient-dictionary-for-ipaddress-tracking-using-net-9-with-alternatelookup-and-ialternateequalitycomparer)
