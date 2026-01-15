---
layout: post
title: Understanding Nullable Reference Types in .NET
author: dotnet
canonical_url: https://www.youtube.com/watch?v=7Sz4heIk_lM
viewing_mode: internal
feed_name: DotNet YouTube
feed_url: https://www.youtube.com/feeds/videos.xml?channel_id=UCvtT19MZW8dq5Wwfu6B0oxw
date: 2025-11-14 08:30:06 +00:00
permalink: /coding/videos/Understanding-Nullable-Reference-Types-in-NET
tags:
- .NET
- .NET Conf
- C#
- C# 8.0
- Code Safety
- Coding
- Default Project Settings
- Modern C# Features
- Non Nullable References
- Nullability
- Nullable Reference Types
- Programming Best Practices
- Type System
- Videos
section_names:
- coding
---
dotnet presents a session on the practical reasons for using Nullable Reference Types in .NET, showing how this feature enhances code safety and readability.<!--excerpt_end-->

{% youtube 7Sz4heIk_lM %}

# Nullable Reference Types: It's Actually About Non-Nullable Reference Types

## Overview

Nullable Reference Types, recently introduced to .NET and now enabled by default in new C# projects, are designed to help developers manage the presence of `null` in reference types more effectively. This session examines the motivation behind the feature, common misconceptions, and practical benefits.

## Why Nullable Reference Types?

- **Historical Challenge:** Null references have long complicated codebases, requiring extensive safeguards and checks.
- **The Shift:** Nullable Reference Types enforce clear intent in your code—whether a reference type may be null or is guaranteed to always contain a value.
- **Default Behavior:** In modern .NET project templates, Nullable Reference Types are enabled by default, encouraging safer code from the outset.

## Benefits for Developers

- **Safer Code:** Explicitly define variables as nullable or non-nullable, reducing the risk of null reference exceptions.
- **Simplified Maintenance:** Improve code readability and maintain fewer null-checks throughout your project.
- **Compiler Guidance:** Leverage compiler warnings to catch potential issues at build time rather than at runtime.

## Best Practices

- Don't disable Nullable Reference Types—use them to improve code clarity.
- Use the `?` annotation for references that can be null, otherwise declare references as non-nullable.
- Respond to compiler warnings, refactoring code where necessary to fix nullability issues.

## Further Learning

Watch more .NET Conf 2025 videos at [YouTube .NET Conf Playlist](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXtIlvq1tuORUtZqVG-HdCt).

---
