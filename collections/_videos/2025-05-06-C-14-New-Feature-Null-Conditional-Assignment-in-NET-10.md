---
external_url: https://www.youtube.com/watch?v=q0xyeA1xPng
title: 'C# 14 New Feature: Null-Conditional Assignment in .NET 10'
author: Learn Microsoft AI
feed_name: Learn Microsoft AI Youtube
date: 2025-05-06 05:27:38 +00:00
tags:
- .NET 10
- C# 14
- C# New Features
- C# Programming
- Clean Code
- CleanCode
- Code Quality
- Conditional Operators
- CSharp14
- CSharpFeatures
- DotNet10
- Microsoft .NET
- Null Conditional Assignment
- NullConditional
- Safer Coding
- VS
- Coding
- Videos
section_names:
- coding
primary_section: coding
---
Learn Microsoft AI demonstrates the new null-conditional assignment feature in C# 14 and .NET 10, showing how these updates can help you write safer, cleaner C# code.<!--excerpt_end-->

{% youtube q0xyeA1xPng %}

# C# 14 New Feature: Null-Conditional Assignment in .NET 10

Learn Microsoft AI presents a walkthrough of a C# 14 feature introduced in .NET 10: **null-conditional assignment using `?.` and `?[]` as assignment targets**. This update allows you to write expressions such as `myObj?.Property = value;` and `myArray?[index] = value;`, so the assignment only occurs if the target is not null.

## What is Null-Conditional Assignment?

- Traditionally, assigning values through an object or array required explicit null checks to avoid exceptions.
- C# 14 enables the use of null-conditional (`?.`, `?[]`) directly on the assignment's left-hand side.
- **Benefit:** Only evaluates the right-hand side if the assignment target is not null, delivering cleaner and safer code.

## Example Usage

```csharp
person?.Name = "Alex";
items?[3] = "New Value";
```

- If `person` or `items` is null, no assignment or exception occurs.

## Advantages

- Reduces boilerplate null checks
- Prevents `NullReferenceException` in common patterns
- Keeps code more readable and concise

## When to Use

- Working with optional objects or collections
- Refactoring older codebases for safety improvements

## Learn More

- This feature is available starting in C# 14 with .NET 10.
- See official documentation and sample projects for real-world examples.

*Author: Learn Microsoft AI*
