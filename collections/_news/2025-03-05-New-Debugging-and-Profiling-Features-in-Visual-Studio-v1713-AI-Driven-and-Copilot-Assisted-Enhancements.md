---
external_url: https://devblogs.microsoft.com/visualstudio/new-debugging-and-profiling-features-in-visual-studio-v17-13/
title: 'New Debugging and Profiling Features in Visual Studio (v17.13): AI-Driven and Copilot-Assisted Enhancements'
author: Harshada Hole
feed_name: Microsoft DevBlog
date: 2025-03-05 15:39:27 +00:00
tags:
- Async Debugging
- Copilot
- CPU Usage
- Debug
- Debugging
- Debugging And Diagnostics
- Exception Analysis
- IEnumerable Visualizer
- Instrumentation
- Multiprocess Analysis
- Parallel Stacks
- Profiling
- V17.13
- Variable Analysis
- VS
section_names:
- ai
- coding
- github-copilot
---
Harshada Hole presents the latest Visual Studio v17.13 update, detailing AI-powered debugging, GitHub Copilot integration, and extensive improvements to profiling tools for developers.<!--excerpt_end-->

# New Debugging and Profiling Features in Visual Studio (v17.13)

**Author:** Harshada Hole

Visual Studio v17.13 introduces robust enhancements in debugging and profiling, with a strong emphasis on artificial intelligence and GitHub Copilot integration to streamline troubleshooting for developers. The release combines smarter variable analysis, improved profiling tools, and AI-assisted guidance for a more efficient coding workflow.

---

## Highlights of Visual Studio v17.13

- **AI-driven debugging and variable analysis:** Enhanced support for smarter and more intuitive data inspection, helping developers identify and resolve issues quicker.
- **Advanced profiling tools:** New visualization and support features improve multi-process execution, native code analysis, and async workflows.
- **GitHub Copilot integration:** Integrated Copilot features assist with exception and variable analysis, parallel stacks, and code editing through natural language prompts.

For full release notes: [Visual Studio 2022 v17.13 Release Notes](https://learn.microsoft.com/visualstudio/releases/2022/release-notes-preview)

---

## GitHub Copilot Assisted Debugging Features

### Smarter Exception and Variable Analysis

GitHub Copilot now enhances exception and variable analysis by using the context of the user's project. This means:

- **Relevant errors are surfaced more intuitively**
- **Sharper, actionable insights** guide users to the root cause
- **Smarter, context-aware solutions** for workflow streamlining

> ![GitHub Copilot explaining an error](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAu0AAALMAQMAAABqgB63AAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAWUlEQVR4nO3BAQ0AAADCoPdPbQ43oAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAcDCcMAAf94c1gAAAAASUVORK5CYII=)

### AI-Powered Parallel Stacks Window

- **Auto-Summarize:** The Parallel Stacks window can now provide AI-generated summaries of each thread, giving an instant picture of thread activity.
- **Copilot Chat Integration:** Developers can ask questions, receive explanations, and AI-powered suggestions directly within their debugging session, with Copilot identifying probable problems and recommending solutions.

> [![Screenshot](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/03/Screenshot-2025-03-13-114938.png)](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/03/Screenshot-2025-03-13-114938.png)

### Enhanced Editable Expressions in IEnumerable Visualizer

- **Copilot Inline Chat:** Allows developers to refine expressions using natural language.
- **LINQ Query Suggestions:** The tool generates AI-powered LINQ queries for data filtering.
- **Syntax Highlighting:** Easier to read and review changes.

> ![IEnumerable expression Visualizer](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAA1AAAACCAQMAAAByy7vLAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAI0lEQVRoge3BAQEAAACCIP+vbkhAAQAAAAAAAAAAAAAAAPBjNlYAAYWVEhIAAAAASUVORK5CYII=)

---

## Profiling Features

### Targeted Instrumentation for Native Code

- **Customize Instrumentation:** Select specific functions and classes for deeper inspection, streamlining performance monitoring and bug isolation.

> ![Targeted instrumentation](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAzgAAASDAQMAAACP6Aj5AAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAi0lEQVR4nO3BMQEAAADCoPVPbQ0PoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAeDTVRwABWXqU2AAAAABJRU5ErkJggg==)

### Unified Async Stacks in Profiler

- **Summary and Detail Call Trees:** Display stitched async stacks for better clarity in .NET app execution tracing.
- **Follow async execution:** Streamlines understanding of asynchronous operations.

> ![Detailed call stacks](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAv0AAAGuAQMAAAA5+iuTAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAP0lEQVR4nO3BAQ0AAADCoPdPbQ43oAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD4MaLuAAGh26KeAAAAAElFTkSuQmCC)

### Multiprocess Analysis in CPU Usage Tool

- **Color-Coded Swimlanes:** Each process is shown with a unique color in stacked area charts.
- **Process Filtering:** Quick filtering by process for focused analysis.
- **Enhanced visualization:** Isolate and optimize resource usage efficiently.

> ![Multiprocess analysis graphs](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABHYAAAC9AQMAAADsn2inAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAMUlEQVR4nO3BMQEAAADCoPVPbQwfoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAeBpqUAABDBpKfgAAAABJRU5ErkJggg==)

---

## Feedback and Continued Improvement

The team encourages feedback to ensure future enhancements match developer needs. These improvements aim to maintain Visual Studio as a powerful and user-friendly tool for modern development workflows.

**Happy coding!**

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/new-debugging-and-profiling-features-in-visual-studio-v17-13/)
