---
external_url: https://www.stevejgordon.co.uk/the-grand-mystery-of-the-missing-18-bytes
title: 'Solving .NET Memory Allocation Discrepancies: The Case of the Missing 18 Bytes'
author: Steve Gordon
primary_section: coding
feed_name: Steve Gordon's Blog
date: 2026-01-28 15:33:29 +00:00
tags:
- .NET
- Allocation Analysis
- ASP.NET Core
- BenchmarkDotNet
- Blogs
- C#
- Coding
- Dotmemory
- GC.GetTotalAllocatedBytes
- Heap Allocations
- Memory Investigation
- Memory Profiling
- Object Alignment
- Performance Tuning
- Profiling Tools
- StringBuilder
- Visual Studio Profiler
section_names:
- coding
---
Steve Gordon explores an 18-byte discrepancy in .NET memory allocation reports by different profiling tools, offering a technical investigation that benefits developers focused on performance optimization.<!--excerpt_end-->

# Solving .NET Memory Allocation Discrepancies: The Case of the Missing 18 Bytes

**Author:** Steve Gordon

## Introduction

When optimizing real-world .NET applications, accurate memory profiling is crucial. In this blog post, Steve Gordon shares a detailed technical investigation sparked by an apparent 18-byte discrepancy when measuring memory allocations using BenchmarkDotNet, dotMemory, and Visual Studio Profiler during the performance tuning of an ASP.NET Core-derived code sample.

## The Investigation Begins

While preparing a conference session on practical application performance optimization, Steve compared allocation data from BenchmarkDotNet and JetBrains dotMemory. For a parsing method, BenchmarkDotNet showed a 312-byte allocation, whereas dotMemory reported only 294 bytes—a puzzling 18-byte difference. Further validation with Visual Studio's .NET Object Allocation Tracker confirmed the dotMemory result.

### Profiling Setup

- **Benchmarked Method:** Parses a SQL statement and returns two strings
- **Tools Used:**
  - BenchmarkDotNet
  - JetBrains dotMemory (full collection tracking, custom profiling harness)
  - Visual Studio Profiler (.NET Object Allocation Tracking tool)
- **Sample Benchmark Output:**
  - BenchmarkDotNet: 312 bytes allocated
  - dotMemory/VS Profiler: 294 bytes allocated

## Digging Deeper: Manual Calculation

Steve examined the tested class (`SqlProcessorState`) and associated string allocations:

- `SqlProcessorState` contains two `StringBuilder` references (8 bytes each) and two `bool` fields (1 byte each), plus object overhead
- Four string objects (two outputs, two temporary) traced and size-calculated

Total observed allocations matched profiler reports but not the benchmark's data.

## Finding the Explanation

The breakthrough came after reviewing BenchmarkDotNet's use of `GC.GetTotalAllocatedBytes(precise: true)`, which reports total managed bytes allocated, including per-object alignment overhead. The garbage collector aligns objects on 8-byte boundaries for performance, causing subtle differences in measured allocations:

**Example Padding:**

- 100B string → padded by 4B to 104B
- 74B string → padded by 6B to 80B
- 34B string → padded by 6B to 40B
- 46B string → padded by 2B to 48B
- 34B class → padded by 6B to 40B

Added together, these match the 312 bytes reported by BenchmarkDotNet, explaining the discrepancy.

## Key Takeaways

- **Benchmark vs Profiler:** Benchmarks include object alignment padding; profilers report actual raw object sizes
- **Tool Choice Affects Numbers:** Be aware of what you're measuring and why numbers differ between tools
- **Profiling for Structure, Benchmarking for Comparison:** Use profiles to study allocation contributors; use benchmarks to validate optimizations

## Resources

- [dotMemory](https://www.jetbrains.com/dotmemory)
- [BenchmarkDotNet on GitHub](https://github.com/dotnet/BenchmarkDotNet)
- [Steve's Demo Code](https://github.com/stevejgordon/practical-performance-demo/)

## About the Author

Steve Gordon is a Pluralsight author, 7× Microsoft MVP, .NET engineer, and active contributor to the .NET community. He writes, presents, and maintains several popular open-source projects.

This post appeared first on "Steve Gordon's Blog". [Read the entire article here](https://www.stevejgordon.co.uk/the-grand-mystery-of-the-missing-18-bytes)
