---
layout: "post"
title: "Simpler XAML in .NET MAUI 10"
description: "David Ortinau introduces implicit and global XAML namespaces in .NET MAUI 10 Preview. The update reduces verbosity by allowing global namespace declarations, eliminating repetitive namespace and prefix usage in XAML files. The article covers usage, configuration, code examples, and current limitations."
author: "David Ortinau"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/simpler-xaml-in-dotnet-maui-10/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-06-26 19:15:00 +00:00
permalink: "/2025-06-26-Simpler-XAML-in-NET-MAUI-10.html"
categories: ["Coding"]
tags: [".NET", ".NET 10", "AlohaKit Animations", "Coding", "Community Toolkit", "Global Namespaces", "Hot Reload", "Implicit Namespaces", "MAUI", "Namespace Prefixes", "News", "NuGet Packages", "Syncfusion Toolkit", "VS", "Xaml", "XmlnsDefinition", "XmlnsPrefix"]
tags_normalized: ["dotnet", "dotnet 10", "alohakit animations", "coding", "community toolkit", "global namespaces", "hot reload", "implicit namespaces", "maui", "namespace prefixes", "news", "nuget packages", "syncfusion toolkit", "vs", "xaml", "xmlnsdefinition", "xmlnsprefix"]
---

David Ortinau discusses new features in .NET MAUI 10 that streamline XAML development, focusing on implicit and global namespaces to minimize verbosity and improve maintainability.<!--excerpt_end-->

## Overview

In this article, David Ortinau presents new features introduced in .NET MAUI 10 Preview relating to XAML namespace management. The changes are designed to simplify and streamline XAML file authoring by introducing implicit and global namespaces, reducing the need for verbose namespace and prefix declarations in every file.

## Background: XAML in .NET MAUI

Traditionally, building UI in .NET MAUI with XAML involved including multiple `xmlns` namespace declarations and prefixes in each file, especially as projects grew to use custom types and third-party libraries. While XAML supports robust features like data binding and hot reload, the verbosity of namespace declarations often made files unwieldy and error-prone.

## What’s New in .NET MAUI 10

### Implicit Namespaces

- .NET MAUI 10 Preview 5 introduces implicit namespaces, shifting from the previous approach to a truly global namespace (`http://schemas.microsoft.com/dotnet/maui/global`).
- By opting in (via project properties), two default namespaces (`http://schemas.microsoft.com/dotnet/2021/maui` and `http://schemas.microsoft.com/winfx/2009/xaml`) are included in every XAML file.
- The main impact: you only need to add a prefix for the `x:` namespace due to the XAML inflator.
- As a result, boilerplate in each file is minimized. For example:
  - **Before:**

    ```xml
    <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" x:Class="MyApp.Pages.MyContentPage">
    </ContentPage>
    ```

  - **After:**

    ```xml
    <ContentPage x:Class="MyApp.Pages.MyContentPage">
    </ContentPage>
    ```

### Global Namespaces

- Frequent use of custom classes and third-party controls historically forced developers to add many namespace declarations (using prefixes) at the top of every XAML file.
- With .NET MAUI 10, developers can centralize namespace mappings in a single file (`GlobalXmlns.cs`) using `XmlnsDefinition` attributes. This globalizes namespaces, so their types can be used anywhere in XAML without repeating declarations.
- Example:

  ```csharp
  [assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/maui/global", "YourApp.Models")]
  ```

- This also applies to third-party toolkits (e.g., Syncfusion, Community Toolkit), allowing for much leaner XAML files.

### Eliminating Namespace Prefixes

- With types globally available, prefixes are often unnecessary, making XAML code cleaner.
- Example transformation:
  - **Before:**

    ```xml
    <controls:TaskView />
    ```

  - **After:**

    ```xml
    <TaskView />
    ```

- If name collisions occur (e.g., with commonly used type names), explicit prefixing—globally added with `XmlnsPrefix`—is supported where disambiguation is needed.

## Limitations and Preview Notes

- The feature is in preview, so limitations exist:
  - Slower debug builds and runtime view inflation
  - XAML editors may display errors due to incomplete tooling support
  - Prefixes for namespaces require `clr-namespace:`
- The development team welcomes feedback via GitHub issues or direct contact.

## Resources & Getting Started

- Read the [official announcement](https://learn.microsoft.com/dotnet/maui/whats-new/dotnet-10#xaml-with-implicit--global-xml-namespaces-preview-5).
- Download .NET 10 Preview and Visual Studio 17.14 Preview to try it out.
- Review the [XAML Namespaces Documentation](https://learn.microsoft.com/dotnet/maui/xaml/namespaces/?view=net-maui-9.0).
- Create a new project:

  ```sh
  dotnet new maui -n LessXamlPlease
  ```

## Community Discussion

Some experienced developers drew parallels to similar approaches in WPF and offered pros and cons regarding maintainability and potential naming conflicts, while others praised the reduction in verbosity and greater simplicity.

## Conclusion

The introduction of global and implicit XAML namespaces in .NET MAUI 10 significantly streamlines the developer experience. With the reduction of repetitive declarations and enhanced maintainability, XAML files become cleaner and easier to maintain, though developers should be aware of current preview limitations.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/simpler-xaml-in-dotnet-maui-10/)
