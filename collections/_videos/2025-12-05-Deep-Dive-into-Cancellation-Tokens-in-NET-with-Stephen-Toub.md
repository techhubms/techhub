---
external_url: https://www.youtube.com/watch?v=h1GvSPaRQ-U
title: Deep Dive into Cancellation Tokens in .NET with Stephen Toub
author: dotnet
feed_name: DotNet YouTube
date: 2025-12-05 00:20:57 +00:00
tags:
- .NET
- AMPM
- Async Programming
- BackgroundWorker
- C#
- CancellationToken
- Cancellationtokens
- CancellationTokenSource
- Cooperative Cancellation
- Demo
- Dotnetdeveloper
- EPM
- Explicit Token Passing
- Lock Free Programming
- Microsoft
- Multi Threading
- Scott Hanselman
- Scotthanselman
- Software Developer
- Stephen Toub
- Stephentoub
- Threading
- Tokens
- Volatile
- Coding
- Videos
section_names:
- coding
primary_section: coding
---
Scott Hanselman and Stephen Toub offer a practical breakdown of cancellation tokens in .NET, explaining their evolution and usage for robust async code.<!--excerpt_end-->

{% youtube h1GvSPaRQ-U %}

# Deep Dive into Cancellation Tokens in .NET

## Overview

In this episode of Deep .NET, hosts Scott Hanselman and Stephen Toub provide a comprehensive exploration of Cancellation Tokens—a core tool for cooperative cancellation in modern .NET asynchronous and multi-threaded programming.

## Key Topics Covered

- **Historical Context:**
  - Early .NET relied on thread aborts, a "violent" and error-prone method for stopping work.
  - BackgroundWorker and other early mechanisms lacked a composable, shared token approach.
- **Pattern Evolution:**
  - The move from Asynchronous Programming Model (APM) and Event-based Asynchronous Pattern (EPM), which had limited or no cancellation support.
  - The introduction of `CancellationToken` and `CancellationTokenSource` for explicit, safe, and reusable cancellation across APIs.
- **Explicit Token Passing:**
  - Demonstrates why passing cancellation tokens explicitly in APIs is preferred over ambient (global) scopes—providing safer and more predictable control over task lifetimes.
- **Propagation Across Async Calls:**
  - Explains how cancellation signals travel through async methods and why proper design is crucial.
- **Registration Callbacks:**
  - Using callbacks to register actions upon cancellation for graceful resource management.
- **Sources and Linking:**
  - How `CancellationTokenSource` produces tokens and how multiple tokens can be linked to enable shared cancellation triggers.
- **Observing vs. Requesting Cancellation:**
  - Separation of roles: some objects request cancellation, while others observe it to clean up.
- **Low-level Implementation Details:**
  - Use of polling, register/throw helpers.
  - Why the `volatile` keyword matters: ensuring up-to-date cancellation flag visibility between threads.
  - Lock-free programming, memory models, and compiler optimizations.

## Why Cancellation Tokens Matter

- Enable the graceful, cooperative stopping of tasks, reducing wasted work and improving application responsiveness.
- Allow developers to write robust async code that can react to application shutdowns, user actions, or system events.

## Further Learning

- [Microsoft Docs: Cancellation in Managed Threads](https://learn.microsoft.com/dotnet/standard/threading/cancellation-in-managed-threads)

---

### Featuring

- Scott Hanselman
- Stephen Toub

For .NET news, blogs, forums, and Q&A, check the episode's many referenced Microsoft resources.
