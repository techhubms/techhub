---
external_url: https://www.reddit.com/r/csharp/comments/1mgwnvs/eguinet_unofficial_c_bindings_for_the_easytouse/
title: 'Egui.NET: unofficial C# bindings for the easy-to-use Rust UI library'
author: The-Douglas
feed_name: Reddit CSharp
date: 2025-08-03 22:33:19 +00:00
tags:
- Bindings
- C#
- Egui
- Game Engines
- Immediate Mode GUI
- Memory Safety
- OpenGL
- Rust
- UI Library
- Vulkan
section_names:
- coding
primary_section: coding
---
In this article, The-Douglas shares the story behind Egui.NET—a C# wrapper for the egui Rust UI library—highlighting its technical foundations, design motivations, and sample usage for game engine UIs.<!--excerpt_end-->

# Egui.NET: Unofficial C# Bindings for the easy-to-use Rust UI Library

Author: The-Douglas

---

## Introduction

Egui.NET is a C# wrapper for [egui](https://github.com/emilk/egui), an immediate-mode GUI library written in Rust. Designed primarily for game engine development, Egui.NET brings most of egui's functionality—including widgets, layouting, styling, and more—into the C# ecosystem. This solution benefits projects requiring a simple, flexible, and portable GUI framework not tied to any platform or frontend technology.

## Motivation

While developing a custom game engine with a C# frontend API, the author needed a user interface library with the following traits:

- Simple, clean, and intuitive API
- Easy integration with custom renderers (OpenGL/Vulkan)
- Minimal dependencies
- Enforced memory safety (preventing undefined behavior)

Although frameworks like WPF and Avalonia suit desktop/mobile, there is a scarcity of general-purpose GUI solutions for game engines in C#. Existing Dear ImGui wrappers weren’t memory safe or user-friendly, and MonoGame-based UI frameworks were not well-documented or sufficiently flexible for immediate-mode programming.

Frustrated by these limitations, the author began working on C# bindings for egui to address these challenges.

## About egui and the Binding Process

The egui library in Rust is praised for its straightforward approach and extensive API—over 2000 methods. Given this scale, creating manual bindings would be impractical. The-Douglas implemented an autobinder, which automatically generates about 75% of the necessary code. This mechanism helps ensure:

- Ease of tracking missing functionality
- Simplicity in updating for new egui versions

Remaining bindings are implemented manually.

### Interoperability Approach

Data communication between C# and Rust relies on binary serialization over the Foreign Function Interface (FFI) boundary:

- Most egui types are represented as copy-by-value C# structs.
- Wrapper classes manage stateful types by referencing pointers.

This approach ensures efficient and safe interoperability while allowing C# code to interact with the underlying Rust library.

## Code Example

The following code demonstrates how Egui.NET can be used to construct a GUI:

```csharp
ui.Heading("My egui Application");
ui.Horizontal(ui => {
    ui.Label("Your name:");
    ui.TextEditSingleline(ref name);
});
ui.Add(new Slider(ref age, 0, 120).Text("age"));
if (ui.Button("Increment").Clicked) {
    age += 1;
}
ui.Label($"Hello '{name}', age {age}");
ui.Image(EguiHelpers.IncludeImageResource("csharp.png"));
```

This simple, expressive code highlights the high-level API design and immediate-mode philosophy central to egui and Egui.NET.

## Conclusion and Availability

Egui.NET provides a much-needed solution for C# game engine developers seeking an immediate-mode, memory-safe GUI library. The bindings are open-source and available on [GitHub](https://github.com/DouglasDwyer/Egui.NET). Detailed discussions, community feedback, and contributions are welcomed via [Reddit](https://www.reddit.com/r/csharp/comments/1mgwnvs/eguinet_unofficial_c_bindings_for_the_easytouse/).

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mgwnvs/eguinet_unofficial_c_bindings_for_the_easytouse/)
