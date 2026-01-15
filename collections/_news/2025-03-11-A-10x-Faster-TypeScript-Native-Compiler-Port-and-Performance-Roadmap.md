---
layout: post
title: 'A 10x Faster TypeScript: Native Compiler Port and Performance Roadmap'
author: Anders Hejlsberg
canonical_url: https://devblogs.microsoft.com/typescript/typescript-native-port/
viewing_mode: external
feed_name: Microsoft TypeScript Blog
feed_url: https://devblogs.microsoft.com/typescript/feed/
date: 2025-03-11 14:31:27 +00:00
permalink: /coding/news/A-10x-Faster-TypeScript-Native-Compiler-Port-and-Performance-Roadmap
tags:
- Coding
- Developer Experience
- Editor Responsiveness
- Go
- JavaScript Ecosystem
- Language Services
- LSP
- Native Compiler
- News
- Open Source
- Performance Improvement
- Project Roadmap
- Source Code Management
- TypeScript
- TypeScript 7.0
- VS Code
section_names:
- coding
---
Anders Hejlsberg unveils major plans for TypeScript: a native compiler port offering significant speed and memory gains. This announcement details the benchmarks, roadmap, and expected impact on the developer workflow.<!--excerpt_end-->

# A 10x Faster TypeScript: Native Compiler Port and Performance Roadmap

*By Anders Hejlsberg*

## Introduction

Today, I’m excited to announce the next steps we’re taking to radically improve TypeScript performance. The core value proposition of TypeScript is an excellent developer experience. As your codebase grows, so does the value of TypeScript itself, but in many cases, TypeScript has historically struggled to scale to the very largest codebases. Developers working in big projects often experience long load and check times, faced with difficult trade-offs between reasonable editor startup and getting a complete view of their source.

## Why Performance Matters

TypeScript is beloved for enabling features such as renaming variables with confidence, quickly finding references, navigating huge codebases, and enjoying a frictionless developer experience. These tasks rely on swift semantic analysis and navigation tools. Modern AI-powered tooling also depends on lightning-fast semantic information, demanding lower latency and increased throughput.

We want not only fast command-line builds for validation, but editors that provide instant, reliable feedback, regardless of project size.

## Native Compiler Initiative

We have begun work on a native port of the TypeScript compiler and associated tools. The native implementation will

- **Drastically improve editor startup times**
- **Reduce most build times by 10x**
- **Substantially reduce memory usage**

By porting the current codebase, we expect to preview a native implementation of `tsc` available for command-line typechecking by mid-2025, aiming for a feature-complete solution for project builds and language service support by year’s end.

The implementation is being written in Go, and the repository is publicly available: [https://github.com/microsoft/typescript-go](https://github.com/microsoft/typescript-go). The project is under the same license as the existing TypeScript codebase, and the README provides instructions for building and running both `tsc` and the language server.

## Performance Benchmarks

The current native implementation is already capable of loading many popular TypeScript projects, including the [TypeScript compiler itself](https://github.com/microsoft/TypeScript/tree/main/src/compiler). Here are some benchmarks comparing the current JS-based `tsc` compiler with the native port, measured on several notable GitHub repositories:

| Codebase                                     | Size (LOC) | Current | Native | Speedup  |
|-----------------------------------------------|------------|---------|--------|----------|
| [VS Code](https://github.com/microsoft/vscode)| 1,505,000  | 77.8s   | 7.5s   | 10.4x    |
| [Playwright](https://github.com/microsoft/playwright) | 356,000    | 11.1s   | 1.1s   | 10.1x    |
| [TypeORM](https://github.com/typeorm/typeorm) | 270,000    | 17.5s   | 1.3s   | 13.5x    |
| [date-fns](https://github.com/date-fns/date-fns) | 104,000    | 6.5s    | 0.7s   | 9.5x     |
| [tRPC](https://github.com/trpc/trpc) (combined)| 18,000     | 5.5s    | 0.6s   | 9.1x     |
| [rxjs](https://github.com/ReactiveX/rxjs)     | 2,100      | 1.1s    | 0.1s   | 11.0x    |

While the implementation is not yet feature-complete, these numbers are representative of the order-of-magnitude improvement you can expect across most codebases.

This sets the stage for capabilities that were once unattainable:

- Instant, project-wide error listings
- Support for more advanced refactorings
- Deeper static analysis and insights
- Foundation for richer AI tools and experiences

## Editor Speed and Language Services

Developer time is mostly spent within editors, so performance here counts most. Modern editors like Visual Studio and VS Code have great performance that is reliant on underlying language services. The native port targets this layer specifically.

- **Project load time in editors (VS Code, fast machine):**
  - Current: ~9.6 seconds
  - Native: ~1.2 seconds (8x faster)

This means a near-instant developer experience from startup to first keystroke, regardless of project size. Memory usage is currently roughly half of the current implementation, with prospects for further improvement. All editor operations (completion lists, quick info, go to definition, find all references, etc.) will see major speed improvements.

Additionally, the language service will move to the Language Server Protocol (LSP), making TypeScript easier to integrate with editor ecosystems and aligning with standard language tooling infrastructure.

## Versioning and Roadmap

- **Latest release:** TypeScript 5.8 (with 5.9 soon)
- **JS codebase:** Will progress into TypeScript 6.x, introducing some deprecations and breaking changes as preparation for native migration.
- **Native codebase:** Will appear as TypeScript 7.0, released when it achieves sufficient feature parity and stability.

_Naming conventions:_

- **TypeScript 6 (JS)** and **TypeScript 7 (native)**
- Internal codenames: “Strada” (original TypeScript), “Corsa” (native port effort)

_Joint maintenance:_ TypeScript 6.x (JS) will be supported until TypeScript 7+ (native) achieves maturity and widespread adoption, enabling projects to transition at their own pace.

## What’s Next

Over the coming months, we will provide deeper technical dives on performance, the compiler API, LSP, and other architectural changes. [FAQs](https://github.com/microsoft/typescript-go/discussions/categories/faqs) are available on GitHub, and we are planning an AMA at the [TypeScript Community Discord](https://discord.gg/typescript) on **March 13th, 10 AM PDT | 5 PM UTC**.

A 10x performance improvement represents a substantial leap for TypeScript and JavaScript developers. We look forward to sharing more, engaging with the community, and helping everyone confidently scale their development with TypeScript.

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/typescript-native-port/)
