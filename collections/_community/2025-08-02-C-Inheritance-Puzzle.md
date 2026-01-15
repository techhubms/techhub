---
layout: post
title: C# Inheritance Puzzle
author: calorap99
canonical_url: https://www.reddit.com/r/csharp/comments/1mfzryw/c_inheritance_puzzle/
viewing_mode: external
feed_name: Reddit CSharp
feed_url: https://www.reddit.com/r/csharp/.rss
date: 2025-08-02 20:00:17 +00:00
permalink: /coding/community/C-Inheritance-Puzzle
tags:
- Base Class
- C#
- Code Puzzle
- Coding
- Community
- Console Output
- Constructor
- Derived Class
- Inheritance
- Method Overriding
- Object Oriented Programming
- Virtual Methods
section_names:
- coding
---
calorap99 shares a challenging C# inheritance puzzle, inviting readers to predict the console output of a code snippet involving constructors and method overriding.<!--excerpt_end-->

## Summary

In this post, calorap99 presents a C# programming puzzle focused on class inheritance, constructor behavior, and virtual methods. The central challenge is to guess the console output for a specific code sample.

### The Code

```csharp
public class Program {
    public static void Main() {
        BaseClass result = new DerivedClass();
        Console.WriteLine(result.Value);
    }
}

public class BaseClass {
    public string Value;
    public BaseClass() {
        Value = Func();
    }
    public virtual string Func() {
        return "Base Function";
    }
}

public class DerivedClass : BaseClass {
    public DerivedClass() : base() { }
    public override string Func() {
        return "Overridden Function";
    }
}
```

### Analysis

- The `Main` method instantiates a `DerivedClass`, storing it in a `BaseClass` reference.
- The `BaseClass` constructor assigns `Value` by calling the `Func()` method.
- `Func()` is a virtual method, and `DerivedClass` overrides it.
- In C#, when a base class constructor is running *and* the derived class overrides a virtual method, the overridden method in the derived class is called, even before the derived constructor executes.

### Expected Console Output

- Because the `BaseClass` constructor sets `Value` to the result of `Func()`, and `Func()` is overridden in `DerivedClass`, the statement

  ```csharp
  Value = Func();
  ```

  during base construction will call `DerivedClass.Func()`, returning "Overridden Function".

- Therefore, the output of `Console.WriteLine(result.Value);` will be:
  **Overridden Function**

### Key Takeaways

- Calling virtual methods in a base class constructor can lead to overridden method execution, even though the derived object is not fully constructed.
- This C# behavior is important for developers to understand, as it may cause unexpected results or subtle bugs.

### Learning Points

- Avoid calling virtual methods in constructors unless you are aware of the complexity and risks involved.
- Understanding how inheritance and constructor execution order interact helps write more predictable and robust object-oriented code.

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mfzryw/c_inheritance_puzzle/)
