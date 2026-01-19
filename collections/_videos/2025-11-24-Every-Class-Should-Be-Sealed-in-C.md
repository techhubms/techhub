---
external_url: https://www.youtube.com/watch?v=u79quQu4t_s
title: Every Class Should Be Sealed in C#
author: Nick Chapsas
viewing_mode: internal
feed_name: Nick Chapsas YouTube
date: 2025-11-24 12:01:57 +00:00
tags:
- .NET
- C#
- Class Design
- Code Safety
- Development Practices
- Maintainability
- Object Oriented Programming
- OOP Principles
- Performance
- Sealed Classes
- Software Engineering
section_names:
- coding
---
Nick Chapsas explains the rationale behind sealing every class in C#, covering benefits, potential drawbacks, and its impact on code safety and maintainability.<!--excerpt_end-->

{% youtube u79quQu4t_s %}

# Every Class Should Be Sealed in C#

Nick Chapsas discusses the practice of sealing all classes in C# projects. The video analyzes the benefits, such as preventing unintended inheritance, ensuring predictable behavior, and potentially improving performance by aiding compiler optimizations. Nick explains how sealed classes contribute to safer and more maintainable code by reducing risks of subclassing and making APIs easier to reason about.

## Key Points Covered

- **Why Seal Classes?**
  - Prevents accidental inheritance
  - Protects API surface from extension by consumers
  - Enables certain compiler optimizations that improve performance
  - Encourages composition over inheritance

- **Practical Considerations**
  - Not every scenario benefits from sealing
  - Framework/library design may prefer extensibility
  - Understanding base classes and design intent is essential

## Code Example

```csharp
public sealed class CustomerRepository
{
    // Implementation details here...
}
```

## Trade-Offs and Limitations

- Sealing may not be appropriate for extensible frameworks
- Use architectural judgment: prefer sealed for app or library code unless inheritance is explicitly required

For more in-depth discussion and code demos, visit Nick's Dometrain workshops and follow him online.
