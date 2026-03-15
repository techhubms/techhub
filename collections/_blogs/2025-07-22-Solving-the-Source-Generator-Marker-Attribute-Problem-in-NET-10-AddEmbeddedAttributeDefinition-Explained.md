---
external_url: https://andrewlock.net/exploring-dotnet-10-preview-features-4-solving-the-source-generator-marker-attribute-problem-in-dotnet-10/
title: "Solving the Source Generator 'Marker Attribute' Problem in .NET 10: AddEmbeddedAttributeDefinition() Explained"
author: Andrew Lock
feed_name: Andrew Lock's Blog
date: 2025-07-22 10:00:00 +00:00
tags:
- .NET 10
- AddEmbeddedAttributeDefinition
- Analyzer Development
- CS0436 Warning
- EmbeddedAttribute
- Incremental Generators
- Marker Attributes
- Microsoft.CodeAnalysis
- NuGet Packages
- RegisterPostInitializationOutput
- Roslyn
- Source Generators
- VS
- Blogs
- .NET
section_names:
- dotnet
primary_section: dotnet
---
In this article, Andrew Lock delves into addressing the longstanding marker attribute issue in source generators for .NET 10, unveiling the new AddEmbeddedAttributeDefinition() API and its impact on Roslyn-based development.<!--excerpt_end-->

# Solving the Source Generator 'Marker Attribute' Problem in .NET 10: AddEmbeddedAttributeDefinition() Explained

_**By Andrew Lock**_

---

## Introduction

This post examines the recent addition of the `AddEmbeddedAttributeDefinition()` API introduced in version 4.14 of the Roslyn compiler, shipping with the .NET 10 SDK. The API presents a simpler solution to handling “marker attributes” frequently employed in source generators—solving a pain point contributors have faced for years. The post covers:

- The background and existing solutions to the marker attribute problem.
- How the new API streamlines this process.
- Practical examples and recommended use cases.
- Trade-offs between the old and new approaches for source generator authors.

> _Note: This article is based on the features available in .NET 10 preview 5._

## Background: Marker Attributes in Source Generators

Incremental source generators have been present in .NET for several years, and developers often use “marker attributes” to trigger generator behaviors. For example, in `.NET 6`, the `[LoggerMessage]` attribute from the `Microsoft.Extensions.Logging` library can trigger code generation:

```csharp
using Microsoft.Extensions.Logging;

public partial class TestController {
    // Adding the attribute generates code for LogHelloWorld
    [LoggerMessage(0, LogLevel.Information, "Writing hello world response to {Person}")]
    partial void LogHelloWorld(Person person);
}
```

Similarly, Andrew Lock's `NetEscapades.EnumGenerators` package uses a `[EnumExtensions]` attribute to generate extension methods for enums:

```csharp
[EnumExtensions] // Add this to generate `ColorExtensions`
public enum Color {
    Red = 0,
    Blue = 1,
}
```

The central concern: where do these attributes originate, and how are they made available in the user's compilation without causing conflicts or warnings?

## Traditional Approach: RegisterPostInitializationOutput

A commonly-used Roslyn API, `RegisterPostInitializationOutput()`, allows attributes to be added to a user’s compilation during source generation:

```csharp
[Generator]
public class HelloWorldGenerator : IIncrementalGenerator {
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        context.RegisterPostInitializationOutput(i => {
            i.AddSource("MyExampleAttribute.g.cs", @"
                namespace HelloWorld {
                    internal class MyExampleAttribute: global::System.Attribute {}
                }"
            );
        });
        // ...generator implementation
    }
}
```

While this works for most scenarios, issues arise, especially when:

- The generator is used in multiple projects of a solution.
- One project references another that also uses the generator.
- `[InternalsVisibleTo]` is present in the referenced project.

In these cases, duplicate definitions of the same internal attribute may produce `CS0436` warnings due to type conflicts across assemblies. For example:

```bash
warning CS0436: The type 'MyExampleAttribute' in 'HelloWorldGenerator\MyExampleAttribute.g.cs' conflicts with the imported type 'MyExampleAttribute'...
```

## Well-Established Solution: Shared DLLs for Attributes

To circumvent these type conflicts, a typical pattern is to include shared attribute definitions in a separate DLL within a NuGet package. This way, all projects reference the same attribute type, avoiding type collision. For example:

- The `LoggerMessage` generator uses attributes found in `Microsoft.Extensions.Logging.Abstractions.dll`.
- `NetEscapades.EnumGenerators` includes a `NetEscapades.EnumGenerators.Attributes.dll` apart from the generator DLL itself.

This avoids generating code in every project and centralizes marker attributes:

![NuGet package contents showing both DLLs](https://github.com/andrewlock/NetEscapades.EnumGenerators/blob/main/src/NetEscapades.EnumGenerators/NetEscapades.EnumGenerators.csproj)

However, managing a separate attributes DLL introduces extra complexity in packaging and MSBuild configuration.

## Compiler’s Solution: The [Embedded] Attribute

Internally, Roslyn uses `[Embedded]` to mark compiler-generated types and attributes, ensuring they're only visible within the current compilation. This avoids the cross-assembly type conflict even when attributes are synthesized multiple times (such as supporting new features on older runtimes).

Roslyn's `EmbeddedAttribute` definition:

```csharp
namespace Microsoft.CodeAnalysis {
    internal sealed partial class EmbeddedAttribute : global::System.Attribute { }
}
```

Key constraints:

1. Must be `internal`, not public.
2. Must be `sealed` and non-static.
3. Must have a parameterless constructor (internal or public).
4. Must inherit `System.Attribute`.
5. Must allow usage on any type declaration (class, struct, interface, enum, delegate).

Until .NET 10, this compiler-internal trick wasn’t possible for libraries.

## New in .NET 10: AddEmbeddedAttributeDefinition()

With Roslyn 4.14 and .NET 10, the new `AddEmbeddedAttributeDefinition()` API lets source generators embed the correct `[Embedded]` attribute during compilation, fixing the `[InternalsVisibleTo]` leakage issue elegantly. Here’s how you use it:

```csharp
[Generator]
public class HelloWorldGenerator : IIncrementalGenerator {
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        context.AddEmbeddedAttributeDefinition(); // Adds the [Embedded] definition
        context.RegisterPostInitializationOutput(i => {
            i.AddSource("MyExampleAttribute.g.cs", @"
                namespace HelloWorld {
                    [global::Microsoft.CodeAnalysis.EmbeddedAttribute]
                    internal class MyExampleAttribute: global::System.Attribute {}
                }"
            );
        });
        // ...generator implementation
    }
}
```

Using this two-line approach avoids `CS0436` warnings from duplicate type definitions. The use of the full namespace ensures correctness, even in edge cases.

## Which Approach Should You Use?

### When to Use AddEmbeddedAttributeDefinition()

- If **all consumers** are using .NET 10 SDK (Roslyn 4.14) or later.
- For **new source generators** targeting only recent SDKs, it simplifies development.
- Reduces package complexity (no need for separate attributes DLL).

**Caveats:**

- All users must have at least version 9.0.300 or .NET 10 preview 4; Visual Studio 17.14 minimum.
- Not backwards-compatible for older SDKs.

### When to Use the Shared DLL Approach

- If you support older SDKs or existing packages already use shared attribute DLLs.
- When you need to include more than just attributes (e.g., helper types, enums exposed in public APIs).
- Less risk of breaking changes for current consumers.

> Example: If your code-gen needs to expose a public enum or type (like `TransformType`), you can’t embed it with `[Embedded]` and have it usable across project boundaries; put it in the shared DLL instead.

## Summary

The new `AddEmbeddedAttributeDefinition()` API in .NET 10 is a welcome improvement for source generator authors, fixing the marker attribute leakage problem in a streamlined way—provided you can require the newest SDK for users. For backwards compatibility or packages with public types beyond simple attributes, shared DLLs remain advantageous.

Andrew Lock’s article covers practical patterns, trade-offs, and implementation tips for making the right choice in your own source generator projects.

---

#### Andrew Lock | .Net Escapades

> Stay up to date with the latest posts from Andrew Lock by subscribing to email notifications.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-dotnet-10-preview-features-4-solving-the-source-generator-marker-attribute-problem-in-dotnet-10/)
