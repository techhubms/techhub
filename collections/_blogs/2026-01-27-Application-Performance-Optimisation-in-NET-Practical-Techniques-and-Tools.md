---
layout: "post"
title: "Application Performance Optimisation in .NET: Practical Techniques and Tools"
description: "This blog post by Steve Gordon presents a detailed look at practical performance optimisation techniques for .NET applications. It covers the full lifecycle of performance tuning, from identifying bottlenecks to using profiling and benchmarking tools. Developers will learn actionable strategies for improving ASP.NET Core applications and making informed optimisation decisions."
author: "Steve Gordon"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.stevejgordon.co.uk/talk-application-performance-optimisation-in-practice-60-mins"
viewing_mode: "external"
feed_name: "Steve Gordon's Blog"
feed_url: "https://www.stevejgordon.co.uk/feed"
date: 2026-01-27 16:31:56 +00:00
permalink: "/2026-01-27-Application-Performance-Optimisation-in-NET-Practical-Techniques-and-Tools.html"
categories: ["Coding"]
tags: [".NET", "ASP.NET Core", "BenchmarkDotNet", "Benchmarking", "Best Practices", "Blogs", "Code Optimisation", "Coding", "Dotmemory", "Dottrace", "Memory Management", "Monitoring", "Performance Optimisation", "Profiling", "Software Engineering"]
tags_normalized: ["dotnet", "aspdotnet core", "benchmarkdotnet", "benchmarking", "best practices", "blogs", "code optimisation", "coding", "dotmemory", "dottrace", "memory management", "monitoring", "performance optimisation", "profiling", "software engineering"]
---

Steve Gordon offers practical advice for optimising .NET application performance, providing developers with actionable strategies, tool guidance, and real-world examples.<!--excerpt_end-->

# Application Performance Optimisation in .NET: Practical Techniques and Tools

**By Steve Gordon**

## Overview

Application performance is always important, impacting user experience and resource efficiency. In this talk, Steve Gordon explores practical approaches to performance optimisation in .NET, ensuring that developers don’t fall into the trap of undertaking premature optimisation or, conversely, ignoring performance issues until they become serious.

## Key Topics Covered

- **Performance Lifecycle**: Understanding the importance of monitoring applications in production and determining when and where to focus optimisation efforts.
- **Profiling and Benchmarking**: Using tools like dotTrace, dotMemory, and BenchmarkDotNet for analysing bottlenecks and validating improvements.
- **Iterative Improvement**: Walking through a practical example, Steve demonstrates the full optimisation cycle: profiling, designing benchmarks, theorising improvements, and refining implementation to reduce allocations and execution time.
- **Memory and Code Efficiency**: Discussion of common optimisation areas including memory usage, allocation reduction, and more efficient execution flows.
- **Judgment in Optimisation**: Balancing performance gains with maintainability, readability, and engineering resources.

## Practical Example

Through a real-world feature, the post illustrates:

- Profiling the application
- Designing and running benchmarks
- Theory-crafting and discussing bottlenecks
- Making incremental changes to code and measuring the impact
- Evaluating the trade-offs between readability and performance

## Tools Highlighted

- **dotTrace**: For code profiling and identifying performance hot-spots.
- **dotMemory**: For tracking memory usage and finding allocation-heavy sections.
- **BenchmarkDotNet**: For reliable, repeatable benchmarking of .NET code changes.

## Resources

- [Talk Slides](https://speakerdeck.com/stevejgordon/application-performance-optimisation-in-practice-60-mins)
- [Demo Code on GitHub](https://github.com/stevejgordon/practical-performance-demo)

## About the Author

Steve Gordon is a .NET engineer and Microsoft MVP with deep expertise in ASP.NET and performance analysis. He is active in the .NET community as a speaker, blogger, and OSS maintainer.

## Further Support

If you found this content useful, consider supporting Steve Gordon:

- [Buy me a coffee](https://www.buymeacoffee.com/stevejgordon)
- [Donate with PayPal](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=WV4JPPV9FS34L&source=url)

---

For more insights and technical talks, visit [Steve Gordon's Blog](https://www.stevejgordon.co.uk).

This post appeared first on "Steve Gordon's Blog". [Read the entire article here](https://www.stevejgordon.co.uk/talk-application-performance-optimisation-in-practice-60-mins)
