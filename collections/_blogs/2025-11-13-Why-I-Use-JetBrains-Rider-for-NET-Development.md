---
title: Why I Use JetBrains Rider for .NET Development
external_url: https://dev.to/playfulprogramming/why-i-use-jetbrains-rider-for-net-development-2a8k
tags:
- .editorconfig
- .http Files
- .NET
- .NET Core
- AI
- AI Code Completion
- Alt+Enter
- ASP.NET Core
- Blogs
- C#
- Code Inspections
- Cross Platform Development
- Database Explorer
- Debugging
- Git Integration
- GitHub Copilot
- HTTP Client
- JetBrains Rider
- LINQ
- Postman
- Productivity
- Refactoring
- ReSharper
- Search Everywhere
- Shift+Shift
- Terminal Integration
- VS
- VS Code
- Windows Forms
author: Emanuele Bartolesi
primary_section: github-copilot
date: 2025-11-13 09:55:00 +00:00
section_names:
- ai
- dotnet
- github-copilot
feed_name: Emanuele Bartolesi's Blog
---

Emanuele Bartolesi explains why he switched from Visual Studio (and previously VS Code) to JetBrains Rider for day-to-day .NET development, highlighting performance, built-in refactorings/inspections, `.editorconfig` support, an integrated HTTP client, database tools, and how GitHub Copilot fits into his workflow.<!--excerpt_end-->

## Why I Use JetBrains Rider for .NET Development

Finding the ideal IDE for .NET work is rarely straightforward. As a .NET developer, I spend a lot of time in tooling, and small annoyances—slow loading, awkward workflows, missing features—can disrupt focus.

Visual Studio used to dominate my day-to-day usage, but in recent years JetBrains Rider earned a permanent place on my machine. At this point, I don’t have Visual Studio installed anymore.

![My icons on the taskbar](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fsugfl4nltqtezkk5lwz1.png)

## Why I Chose Rider Over Visual Studio

My Rider journey started as curiosity and quickly turned into appreciation for how many pain points it removes.

- **Cross-platform experience**: I routinely work across Windows and macOS. Rider works consistently across machines, and being able to carry settings between OSes is a big deal.
- **VS Code wasn’t enough for my .NET workflow**: I used VS Code as my main .NET environment before, but I missed too many features.
- **Performance on large solutions**: Rider opens big solutions fast and stays responsive even during refactors and code analysis.
- **Customizable UI and navigation**: Once keybindings and navigation click, it becomes easy to move through files, types, and references quickly.
- **Built-in tooling depth**: Rider ships with **ReSharper-level inspections, refactorings, and quick-fixes** out of the box.
- **Useful built-ins**:
  - Integrated Git support (even if I still often use GitKraken/GitHub Desktop/console)
  - Database explorer
  - Built-in HTTP client

### Why the Built-in HTTP Client Matters

I still keep Postman installed, but when I can, I use Rider’s HTTP client.

- You can save **`.http` files directly in the solution**.
- They live in source control, so the whole team can use them as part of the repo.

## Living With Rider Day to Day

Switching IDEs is a commitment, and I was skeptical about replacing years of Visual Studio muscle memory.

- At first, I kept **Visual Studio and Rider side by side**, but I often defaulted back to VS out of habit.
- On a new laptop, I installed only **VS Code and Rider** to force the switch.

After about a week, Rider’s workflow felt natural.

### What Works Well in Daily Use

- **Solution management** feels painless.
- I can open **.NET Core**, **ASP.NET**, and **Windows Forms** projects in the same window.
- **Multi-targeted projects** work without friction.
- **Git operations and branching** are tightly integrated.
- The built-in **terminal** reduces context switching.
- The **task runner** makes it easy to automate builds/tests with a couple of keystrokes.
- **Debugging** feels quick and reliable (breakpoints, variable inspection, attach to process, edit-and-continue).
- **Navigation in large codebases** stays fluid (Go to Definition, symbol search).

I also keep my environment comfortable with theming:

- I use Rider’s default Dark theme.
- To keep a consistent feel in VS Code, I use a port of the same theme.

## 5 Hidden Gems in Rider

Beyond the obvious features, these are tools that can quietly change how you work.

### 1) Dynamic code analysis + instant quick-fixes

Rider runs code analysis continuously and flags:

- potential bugs
- code smells
- style violations

The real value is the quick-fix workflow:

- Use **Alt+Enter** to reformat, refactor, or rewrite code without breaking focus.

On battery-sensitive setups (like working on a train), I sometimes disable dynamic analysis to preserve battery; Rider makes it easy to toggle from the **File** menu.

### 2) Navigation with “Search Everywhere”

- Triggered with **Shift+Shift**
- Searches files, classes, symbols, settings, and menu commands in one place

![Search Everywhere in Rider](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F85zy3nvwzpemuspjhkmi.png)

### 3) `.editorconfig` integration for consistent style

If you care about consistency across a team:

- Rider enforces formatting and style rules from `.editorconfig`.
- It visually indicates where code diverges.
- There’s a built-in visual editor for adjusting rules.

I reuse an `.editorconfig` from my friend Marco Minerva and move it between projects.

### 4) HTTP client inside the IDE

Rider’s HTTP client lets you:

- create `.http` files in the solution
- send requests and inspect responses
- generate **C# HTTP client code**

This can reduce the need to switch to tools like Postman for many workflows.

### 5) Database explorer

Rider includes a database explorer so you can:

- connect to major databases
- run queries
- preview LINQ queries mapped to your schema

As someone who doesn’t consider themselves a database/Entity Framework expert, I find this integration extremely helpful.

![Database explorer](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fp9hrknqaj64rnqxjro4e.png)

## GitHub Copilot Integration

Rider has tight integration with **GitHub Copilot**.

- Copilot provides context-aware suggestions as you type (sometimes whole methods or boilerplate from comments/signatures).
- Copilot works alongside Rider’s own completion system.
- Suggestions appear inline and can be accepted/cycled with familiar shortcuts.
- Works across languages including **C#**, **JavaScript**, **TypeScript**, and **Razor**.

![GitHub Copilot Integration](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F3ydbdmptmfw3g4vylrej.png)

For me, Copilot in Rider is a real productivity boost: faster prototyping, learning APIs in context, and avoiding repetitive coding.

## Drawbacks and Considerations

No tool is perfect.

- **Cost**: Rider requires a commercial license, though there is a free version for hobbyists/open-source developers with all features enabled.
- **Extension ecosystem differences**: Rider’s plugin ecosystem is strong, but some Visual Studio extensions/integrations don’t exist yet.
- **UI learning curve**: menus differ and not all VS wizards/designers are present.
- **Windows-specific workloads and Azure integrations**: if you rely on Windows-only features (legacy .NET workloads or advanced Azure integrations), Rider may not cover everything.
- **Org constraints**: some companies standardize on Visual Studio, which can make Rider harder to adopt.

## Author

Emanuele Bartolesi (Kasuken)

- LinkedIn: https://www.linkedin.com/in/bartolesiemanuele


[Read the entire article](https://dev.to/playfulprogramming/why-i-use-jetbrains-rider-for-net-development-2a8k)

