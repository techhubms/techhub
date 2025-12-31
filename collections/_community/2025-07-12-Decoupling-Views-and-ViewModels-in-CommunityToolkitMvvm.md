---
layout: "post"
title: "Decoupling Views and ViewModels in CommunityToolkit.Mvvm"
description: "A detailed exploration of strategies to decouple views from view models in WPF applications using CommunityToolkit.Mvvm, including comparisons with ViewModelLocator patterns and discussion of dependency injection approaches for reusable, maintainable XAML views."
author: "CSharpDev"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/app-development/how-to-decouple-views-from-view-models-using-communitytoolkit/m-p/4432591#M1261"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=dotnet"
date: 2025-07-12 17:03:00 +00:00
permalink: "/community/2025-07-12-Decoupling-Views-and-ViewModels-in-CommunityToolkitMvvm.html"
categories: ["Coding"]
tags: ["Coding", "Community", "CommunityToolkit.Mvvm", "DataContext", "Dependency Injection", "Design Patterns", "DI", "Microsoft .NET", "MVVM", "ServiceProvider", "View Reuse", "ViewModel Decoupling", "ViewModelLocator", "WPF", "XAML", "XAML Binding"]
tags_normalized: ["coding", "community", "communitytoolkitdotmvvm", "datacontext", "dependency injection", "design patterns", "di", "microsoft dotnet", "mvvm", "serviceprovider", "view reuse", "viewmodel decoupling", "viewmodellocator", "wpf", "xaml", "xaml binding"]
---

CSharpDev delves into practical approaches for decoupling WPF views from view models using CommunityToolkit.Mvvm, weighing classic ViewModelLocator and modern DI solutions.<!--excerpt_end-->

# Decoupling Views from View Models with CommunityToolkit.Mvvm

**Author:** CSharpDev

## Introduction

When building WPF/MVVM applications, it's common to want to decouple views from specific view model classes, particularly for reusability across multiple apps or scenarios. In the context of [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/ioc), this brings up questions about best practices, patterns like ViewModelLocator versus dependency injection (DI), and XAML vs code-behind DataContext assignment.

## Problem Statement

The standard approach in CommunityToolkit.Mvvm involves setting the DataContext in code-behind, often like this:

```csharp
this.DataContext = App.Current.Services.GetService<ContactsViewModel>();
```

While functional, this directly couples the view to a concrete ViewModel type, making it much harder to reuse that view with different view models or in different apps/libraries.

## Classic Solution: ViewModelLocator

Historically, frameworks like MVVM Light recommended the **ViewModelLocator** pattern. Here, a ViewModelLocator class exposes properties for your view models. In your view XAML, you'd bind like so:

```xaml
DataContext="{Binding Main, Source={StaticResource Locator}}"
```

The `Locator` instance is placed in `App.xaml` resources and the property (e.g., `Main`) returns the appropriate view model, typically via a service locator/DI container.

**Benefits:**

- Decouples view from specific ViewModel class
- Permits multiple views to share or swap view models via locator properties
- Works well for design-time data

## Modern Approaches with CommunityToolkit.Mvvm

**1. ViewModelLocator Remains Possible**

You can use the ViewModelLocator pattern with CommunityToolkit.Mvvm. You would set up a ViewModelLocator class (registered as a resource), returning view models using the built-in IoC container or another service locator.

**2. Dependency Injection and DataContext**

With modern DI, you can inject the ViewModel into the view's constructor (or assign at runtime), and if you want to avoid directly referencing a concrete class:

- Use interfaces for your view models
- Rely on convention or runtime assignment
- Register mapping in DI container
- Or, set the DataContext explicitly from outside the view, e.g., during navigation or app composition

**3. Passing the ViewModel In**

For scenarios where a view might be used with different view models (e.g., the same chart control bound to different data), you have two main options:

- **Expose a ViewModel property (or DataContext) on the view** (for user controls): Let the parent/window assign it.
- **Use Dependency Injection:** Provide the view model instance when constructing or displaying the view.
- **Use DI scopes or named registrations** to resolve the correct instance.

**Example for shared view:**

Suppose you have a ChartView user control. You can:

- Define its DataContext as a dependency property or regular property
- The parent (e.g., MainWindow) assigns whichever ViewModel instance is required for each usage

**Sample XAML:**

```xaml
<controls:ChartView DataContext="{Binding ChartViewModelA}" />
<controls:ChartView DataContext="{Binding ChartViewModelB}" />
```

**Or with DI:**

- When creating the view, pass in the ViewModel as a constructor or initialization argument

## Summary/Best Practices

- **ViewModelLocator** remains a valid approach and is fully compatible with CommunityToolkit.Mvvm if you want XAML-only assignment and design-time support.
- **Dependency Injection** via the ServiceProvider and DataContext assignment is encouraged for larger apps. Consider abstracting your view models or using factory patterns for more flexible assignment.
- If a single view must be used with multiple view models at the same time, design the view to let its DataContext (or a dedicated property) be set externally, rather than resolved inside itself.

## References

- [Microsoft CommunityToolkit.Mvvm IoC Documentation](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/ioc)
- [C# Corner: Getting Started with MVVM Light with WPF](https://www.c-sharpcorner.com/article/getting-started-with-mvvm-light-with-wpf/)

## Key Takeaways

- **ViewModelLocator** is still supported for XAML-only decoupling
- **Dependency Injection** is modern and preferred in large-scale, testable apps
- **ViewModel assignment should be externalized for maximum reusability**

By adopting these strategies, you gain flexibility in sharing views, supporting design-time data, and building maintainable WPF applications.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/app-development/how-to-decouple-views-from-view-models-using-communitytoolkit/m-p/4432591#M1261)
