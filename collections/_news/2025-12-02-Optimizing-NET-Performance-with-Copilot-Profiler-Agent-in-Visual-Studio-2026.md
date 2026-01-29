---
external_url: https://devblogs.microsoft.com/visualstudio/delegate-the-analysis-not-the-performance/
title: Optimizing .NET Performance with Copilot Profiler Agent in Visual Studio 2026
author: Nik Karpinsky
feed_name: Microsoft VisualStudio Blog
date: 2025-12-02 15:00:16 +00:00
tags:
- .NET Performance
- Agent
- BenchmarkDotNet
- Code Benchmarking
- Code Optimization
- Copilot
- Copilot Profiler Agent
- CsvHelper
- Delegate Invocation
- Delegate Optimization
- Expression Trees
- Natural Language Queries
- NuGet Packages
- Performance
- Performance Profiling
- Profiler Integration
- Profiling
- VS
- AI
- Coding
- GitHub Copilot
- News
section_names:
- ai
- coding
- github-copilot
primary_section: github-copilot
---
Nik Karpinsky showcases the new Copilot Profiler Agent in Visual Studio 2026, guiding developers on leveraging AI-powered profiling to analyze and optimize performance bottlenecks in .NET applications.<!--excerpt_end-->

# Optimizing .NET Performance with Copilot Profiler Agent in Visual Studio 2026

Nik Karpinsky introduces **Copilot Profiler Agent**, a new AI-powered assistant for Visual Studio 2026 that leverages the capabilities of GitHub Copilot and the Visual Studio Profiler to help developers identify and resolve performance issues in their codebases.

## Copilot Profiler Agent Overview

- Integrates GitHub Copilot's natural language processing with Visual Studio's performance profiling tools
- Enables direct queries about code performance, hot paths, and optimization opportunities via Copilot Chat

## Real-World Example: Optimizing CsvHelper

- Uses the open-source [CsvHelper](https://joshclose.github.io/CsvHelper/) library to illustrate the workflow
- Provides step-by-step instructions to clone the CSV Helper repository and prepare the benchmarking setup

### Setting Up Benchmarks

- Copilot Profiler Agent assists in generating benchmarks for the `WriteRecords` method using natural language queries (`@Profiler Help me write a benchmark for the #WriteRecords method`)
- Agent installs relevant NuGet packages ([Microsoft.VisualStudio.DiagnosticsHub.BenchmarkDotNetDiagnosers](https://www.nuget.org/packages/Microsoft.VisualStudio.DiagnosticsHub.BenchmarkDotNetDiagnosers)) and adapts benchmarks to fit the repository style
- Builds and validates the benchmark setup

```csharp
public class BenchmarkWriteCsv {
    private const int entryCount = 10000;
    private readonly List<Simple> records = new(entryCount);
    public class Simple {
        public int Id1 { get; set; }
        public int Id2 { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
    }
    [GlobalSetup]
    public void GlobalSetup() {
        var random = new Random(42);
        var chars = new char[10];
        string getRandomString() {
            for (int i = 0; i < 10; ++i) chars[i] = (char)random.Next('a', 'z' + 1);
            return new string(chars);
        }
        for (int i = 0; i < entryCount; ++i) {
            records.Add(new Simple {
                Id1 = random.Next(),
                Id2 = random.Next(),
                Name1 = getRandomString(),
                Name2 = getRandomString(),
            });
        }
    }
    [Benchmark]
    public void WriteRecords() {
        using var stream = new MemoryStream();
        using var streamWriter = new StreamWriter(stream);
        using var writer = new CsvHelper.CsvWriter(streamWriter, CultureInfo.InvariantCulture);
        writer.WriteRecords(records);
        streamWriter.Flush();
    }
}
```

### Analyzing and Optimizing with Profiler Agent

- Agent runs the benchmark, applies `BenchmarkSwitcher` for flexible test selection
- Presents diagnostic session results and identifies performance bottlenecks (e.g., delegate compilation and invocation overhead)
- Profiler Agent explains issues and suggests optimizations via chat:
  - Recognizes frequent delegate invocation as a hot path due to high frequency (40,000 invocations over 10,000 records and 4 fields)
  - Recommends combining field writes into a single delegate using `Expression.Block` to reduce multicast delegate overhead

```csharp
var expressions = new List<Expression>(members.Count);
foreach (var memberMap in members) {
    // ... field writing logic ...
    expressions.Add(writeFieldMethodCall);
}
if (expressions.Count == 0) {
    return new Action<T>((T parameter) => { });
}
var block = Expression.Block(expressions);
return Expression.Lambda<Action<T>>(block, recordParameter).Compile();
```

## Measuring the Impact

- Copilot Profiler Agent reruns benchmarks post-optimization
- Reports approximately 24% performance improvement in current tests; confirmed by CPU profiler traces
- Staged pull request in the CsvHelper repo shows ~15% performance gain in broader usage
- Optimization benefits applications writing large CSV files, speeding up record processing and reducing CPU time

## Key Takeaways

- Copilot Profiler Agent streamlines performance analysis and remediation using AI-assisted code review and profiling
- Offers actionable insights and clear explanations to guide developers on optimal performance strategies
- Automation of benchmark setup, analysis, and result measurement accelerates the feedback cycle and improves developer productivity

---

**Feedback:** The article invites readers to share their experiences and suggestions for further improvement. [Share your feedback](https://www.surveymonkey.com/r/CNLLPSQ).

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/delegate-the-analysis-not-the-performance/)
