---
layout: "post"
title: "Avoiding Closures in .NET ConcurrentDictionary: The Efficient GetOrAdd Overload"
description: "Khalid Abuhakmeh explores what closures are in C# and illustrates a common tooling suggestion to eliminate closure creation in ConcurrentDictionary. He demonstrates refactoring code to leverage an overload of GetOrAdd that avoids unnecessary allocations, improving performance and preventing subtle bugs."
author: "Khalid Abuhakmeh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://khalidabuhakmeh.com/the-curious-case-of-dotnet-concurrentdictionary-and-closures"
viewing_mode: "external"
feed_name: "Khalid Abuhakmeh's Blog"
feed_url: "https://khalidabuhakmeh.com/feed.xml"
date: 2025-02-18 00:00:00 +00:00
permalink: "/2025-02-18-Avoiding-Closures-in-NET-ConcurrentDictionary-The-Efficient-GetOrAdd-Overload.html"
categories: ["Coding"]
tags: [".NET", "Allocations", "C#", "Closures", "Coding", "ConcurrentDictionary", "Delegates", "Lambda", "LINQ", "Memory Leaks", "Performance", "Posts"]
tags_normalized: ["net", "allocations", "c", "closures", "coding", "concurrentdictionary", "delegates", "lambda", "linq", "memory leaks", "performance", "posts"]
---

In this article, Khalid Abuhakmeh delves into the nuances of closures in C#, focusing specifically on their impact in ConcurrentDictionary. He demonstrates code refactoring to avoid closures and highlights the advantages of using the more efficient overload of GetOrAdd.<!--excerpt_end-->

# The Curious Case of .NET ConcurrentDictionary and Closures

*By Khalid Abuhakmeh*

![The Curious Case of .NET ConcurrentDictionary and Closures](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/dotnet-concurrentdictionary-getoradd-closures.jpg)

Photo by [Chris Arthur-Collins](https://unsplash.com/@anotherleaf)

---

## Introduction

While exploring the [Duende Software](https://duendesoftware.com) codebase, Khalid Abuhakmeh noticed a recurring IDE tooling suggestion involving `ConcurrentDictionary` usage:

> **“Closure can be eliminated: method has overload to avoid closure creation.”**

Despite this suggestion, no automatic fix was available, leading to questions about the underlying issue. This article defines closures, discusses their implications, and demonstrates how to use `ConcurrentDictionary` efficiently to avoid closures altogether.

## What Are Closures?

Closures commonly occur when working with constructs like `Action`, `Func`, `delegate`, or LINQ in C#. A closure is a language feature allowing a function to "capture" variables from its creation scope, letting code pass the function around as an object with state.

For a deep dive, see [C# Closures Explained by Justin Etheredge](https://www.simplethread.com/c-closures-explained/).

### Example: Capturing a Variable

```csharp
void SayHello(string name) {
    var hello = () => {
        // name is captured causing an allocation
        // and potential concurrency issues
        Console.WriteLine($"Hello {name}");
    };
    hello();
}
```

This code captures `name` in the lambda body, meaning the compiler creates a closure to maintain that value across calls. Captures like this can cause problems, which are sometimes unpredictable until runtime.

#### Potential Issues with Closures

1. **Extra Allocations:** Captured variables may result in extra memory allocations, affecting resource usage.
2. **Unintended State Change:** If captured state is a reference type, external changes may introduce bugs or unpredictable behavior.
3. **Long-Lived References:** Captured variables can unintentionally prolong object lifetimes, potentially causing memory leaks.

#### Safer Approach: Pass State Explicitly

To avoid these pitfalls, pass all required state as parameters to lambdas:

```csharp
void SayHello(string name) {
    var hello = (string n) => {
        Console.WriteLine($"Hello {n}");
    };
    hello(name);
}
```

---

## `ConcurrentDictionary.GetOrAdd` and Closure Creation

Consider a typical usage of the `GetOrAdd` method on a `ConcurrentDictionary`:

```csharp
using System.Collections.Concurrent;

ConcurrentDictionary<string, Item> concurrentDictionary = new();

var key = "khalid";
var value = "awesome";

var result = concurrentDictionary.GetOrAdd(key, (k) => {
    Console.WriteLine($"Building {k}");
    return new Item(value, DateTime.Now);
});

Console.WriteLine(result);
```

In the snippet above, `value` is captured by the lambda, forming a closure.

**Tooling Suggestion:**

> Closure can be eliminated: method has overload to avoid closure creation.

### Refactoring to Avoid Closure

`ConcurrentDictionary.GetOrAdd` provides an overload that allows you to pass an additional argument, thus eliminating capture:

```csharp
using System.Collections.Concurrent;

ConcurrentDictionary<string, Item> concurrentDictionary = new();

var key = "khalid";
var value = "awesome";

var result = concurrentDictionary.GetOrAdd(
    key: key,
    valueFactory: (k, arg) => {
        Console.WriteLine($"Building {k}");
        return new Item(arg, DateTime.Now);
    },
    factoryArgument: value
);

Console.WriteLine(result);

record Item(string Value, DateTime Time);
```

#### How This Overload Works

1. **Key:** The lookup key.
2. **Value Factory Lambda:** Function that creates the value if the key is missing. Accepts both the key and the factory argument as parameters.
3. **Factory Argument:** Arbitrary state passed into the factory function. If multiple values are necessary, encapsulate them in a container class or struct.

### Benefits

- **Reduced Allocations:** No closure allocations are necessary.
- **Improved Concurrency:** Avoids subtle concurrency bugs due to shared state.
- **Lower Risk of Memory Leaks:** No unintentionally prolonged object lifetimes.

---

## Conclusion

When using `ConcurrentDictionary`, always check your usage of `GetOrAdd` to ensure you're leveraging the overload that avoids closures where possible. This can result in more efficient, more reliable code—especially useful in high-concurrency scenarios.

---

*Thanks for reading and sharing this with friends and colleagues. Cheers!*

---

## About Khalid Abuhakmeh

Khalid is a developer advocate at JetBrains, focusing on .NET technologies and tooling.

---

## Further Reading

- [ASP.NET Core and Chunking HTTP Cookies](/aspnet-core-and-chunking-http-cookies)
- [Strongly-Typed Markdown for ASP.NET Core Content Apps](/strongly-typed-markdown-for-aspnet-core-content-apps)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/the-curious-case-of-dotnet-concurrentdictionary-and-closures)
