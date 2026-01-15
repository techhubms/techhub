---
layout: post
title: 'MauiReactor: Introducing the MVU Pattern for .NET MAUI'
author: David Ortinau
canonical_url: https://devblogs.microsoft.com/dotnet/mauireactor-mvu-for-dotnet-maui/
viewing_mode: external
feed_name: Microsoft .NET Blog
feed_url: https://devblogs.microsoft.com/dotnet/feed/
date: 2025-08-25 17:15:00 +00:00
permalink: /coding/news/MauiReactor-Introducing-the-MVU-Pattern-for-NET-MAUI
tags:
- .NET
- App Architecture
- C#
- Coding
- Component Integration
- Cross Platform
- Hot Reload
- MAUI
- MauiReactor
- MVU
- MVU Pattern
- MVVM
- News
- Open Source
- Performance
- State Management
- Testing
- UI Development
- XAML
section_names:
- coding
---
David Ortinau features guest author Adolfo Marinucci, who explores MauiReactor—a library that makes the MVU architectural pattern accessible for .NET MAUI developers, offering insights on productivity, technical implementation, and practical usage.<!--excerpt_end-->

# MauiReactor: An MVU Approach for .NET MAUI

*Guest author: [Adolfo Marinucci](https://github.com/adospace)*

MauiReactor is an open-source .NET library bringing the Model-View-Update (MVU) pattern, widely known from React and Elm, to the .NET MAUI ecosystem. It provides developers with a powerful alternative to the standard MVVM approach, focused on code-based UI, predictable state management, and productivity.

## Background & Motivation

- MauiReactor started as an experiment with Xamarin.Forms and evolved to a mature, auto-generated layer over .NET MAUI.
- The MVU pattern promotes immutable state and streamlined UI development entirely in C#—utilizing IDE features like IntelliSense, refactoring, and code analysis.
- Suitable for those comfortable with declarative approaches from frameworks like React Native and Flutter.

## MVU vs. MVVM in .NET MAUI

- While MVVM is the default in .NET MAUI, MauiReactor’s MVU approach reduces boilerplate and eliminates the need for value converters and command objects.
- Example of MVU syntax for a login button:

  ```csharp
  Button("Login")
    .IsEnabled(!string.IsNullOrWhiteSpace(State.FirstName) && !string.IsNullOrWhiteSpace(State.LastName))
    .OnClicked(OnLogin)
  ```

- More flexible conditional UI and robust hot reload, retaining user input between iterations.

## Real-World Applications

- MauiReactor powers production systems at companies like [Real Time Research, Inc.](https://realtimeresearch.com), integrating field data collection, Microsoft Azure synchronization, and Power BI reporting.
- Example: A fish survey app with offline support, GPS integration, and barcode scanning.
- Reliable state management supports complex workflows and synchronization.

## Technical Features

### Hot Reload Implementation

- MVU architecture simplifies hot reload so state is preserved during code changes.
- Compatible with .NET CLI standard reload or the custom Reactor.Maui.HotReload tool.
- Works across Android, iOS, Mac, and Windows (emulator/real devices); supports Visual Studio, VS Code, and Rider.

### Code-Based UI & Theming

- All UI logic and structure are coded in C#, supporting XAML theming, or a C#-based style system.

  ```csharp
  public class AppTheme : Theme
  {
    protected override void OnApply()
    {
      LabelStyles.Default = _ => _
        .FontFamily("OpenSansRegular")
        .FontSize(FontNormal)
        .TextColor(Black);
    }
  }
  ```

### Data Binding & Conditional Rendering

- No more value converters: logic is written directly in code (e.g., `Label().IsVisible(!MyValue)`)
- Conditional rendering uses C# evaluation:

  ```csharp
  State.IsBusy ? ActivityIndicator() : null
  ```

### Readability & Testing

- Large views are split into components or render functions for maintainable, readable code.
- Framework-agnostic integration testing, including support for MSTest, XUnit, NUnit, etc.

### Performance

- Minimal use of reflection improves compatibility with Ahead-of-Time (AOT) compilation, reducing startup time and runtime inefficiencies.
- Default to compiled pages, avoiding XAML parsing bottlenecks.
- Sample profiling project available for in-depth comparison.

### Integration

- Supports third-party controls (e.g. Syncfusion, Telerik, DevExpress) with source generators and simple wrappers.
- Example:

  ```csharp
  [Scaffold(typeof(Syncfusion.Maui.Buttons.ToggleButton))]
  partial class ToggleButton { }
  ```

## Getting Started

1. Install the template:

   ```bash
   dotnet new install Reactor.Maui.TemplatePack
   ```

2. Create a new project:

   ```bash
   dotnet new maui-reactor-startup -o my-new-project
   ```

3. Run the application:

   ```bash
   dotnet build -t:Run -f net9.0-android
   ```

Find more at the [MauiReactor repository](https://github.com/adospace/reactorui-maui) or check the [MauiReactor documentation](https://adospace.gitbook.io/mauireactor/getting-started).

## Conclusion

MauiReactor is an established, productive, and flexible alternative for .NET MAUI UI development, praised by developers for its simplicity and efficiency. Its community-driven evolution ensures ongoing improvements and integration capabilities, giving developers more options in cross-platform app architecture.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/mauireactor-mvu-for-dotnet-maui/)
