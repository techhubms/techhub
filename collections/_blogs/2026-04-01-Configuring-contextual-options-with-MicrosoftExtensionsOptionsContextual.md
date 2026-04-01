---
title: Configuring contextual options with Microsoft.Extensions.Options.Contextual
external_url: https://andrewlock.net/configuring-contextual-options-with-microsoft-extensions-options-contextual/
feed_name: Andrew Lock's Blog
author: Andrew Lock
tags:
- .NET
- .NET CLI
- .NET Core
- ASP.NET Core
- Blogs
- Configuration
- Configuration Binding
- Csproj
- Dependency Injection
- Experimental APIs
- EXTEXP0018
- Feature Flags
- IContextualOptions
- IOptions
- IOptionsContext
- IOptionsContextReceiver
- Microsoft.Extensions.Options.Contextual
- Microsoft.FeatureManagement
- Named Options
- NoWarn
- OpenFeature
- Options Pattern
- Preview NuGet Packages
- Source Generators
section_names:
- dotnet
date: 2026-04-01 10:00:00 +00:00
primary_section: dotnet
---

Andrew Lock walks through what Microsoft.Extensions.Options.Contextual is, how to wire it up in an ASP.NET Core app (including its source generator and receiver pattern), and why he considers it experimental and usually not worth adopting compared to established feature-flag approaches.<!--excerpt_end-->

## Overview

This post explores the experimental `Microsoft.Extensions.Options.Contextual` package: what it’s for, how it works, and whether it’s worth using.

The library describes itself as:

> APIs for dynamically configuring options based on a given context

Andrew uses a simple “weather forecast” style sample to show what “contextual options” means in practice.

## What problem is it trying to solve?

With typical .NET options, you configure `TOptions` “globally” (even if the options are used as singleton/transient/scoped):

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<WeatherForecastOptions>()
    .Configure(x => x.ForecastDays = 7)
    .BindConfiguration(builder.Configuration["Weather"]);
```

Contextual options aim to let you configure an options instance based on some *caller-provided* context object.

### Example options type

```csharp
internal class WeatherForecastOptions
{
    public string TemperatureScale { get; set; } = "Celsius"; // Celsius or Fahrenheit
    public int ForecastDays { get; set; }
}
```

### Example context type

The “context” might be user-associated data like country:

```csharp
internal class AppContext
{
    public Guid UserId { get; set; }
    public string? Country { get; set; }
}
```

Andrew notes this differs from *named options*, where you configure multiple named sets of otherwise-global options.

## What the call site looks like

With contextual options, you resolve an `IContextualOptions<TOptions, TContext>` from DI and request options for a specific context:

```csharp
// Get an IContextualOptions<TOptions, TContext> from DI
IContextualOptions<WeatherForecastOptions, AppContext> _contextualOptions;

AppContext context; // Get an instance of the context object from somewhere

// Get an instance of WeatherForecastOptions using the AppContext instance
WeatherForecastOptions options = await _contextualOptions.GetAsync(
    context,
    cancellationToken: default);
```

## Installing Microsoft.Extensions.Options.Contextual

The package appears to still be *preview only*, so you need `--prerelease`:

```bash
dotnet add package Microsoft.Extensions.Options.Contextual --prerelease
```

Example project reference shown in the post:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Options.Contextual" Version="10.4.0-preview.1.26160.2" />
  </ItemGroup>

</Project>
```

Andrew notes the latest version supports .NET Framework and .NET 8+.

## Wiring up contextual options (the moving parts)

The post describes three main steps:

- Add `[OptionsContext]` to the context type (drives a source generator)
- Implement an `IOptionsContextReceiver` to extract values from the context
- Configure how extracted values are applied to the options

### 1) Mark the context type with [OptionsContext]

You mark the context type `partial` and add `[OptionsContext]`:

```csharp
[OptionsContext]
internal partial class AppContext
{
    public Guid UserId { get; set; }
    public string? Country { get; set; }
}
```

This drives a source generator (`ContextualOptionsGenerator`) that generates code to implement `IOptionsContext` by “populating” a receiver with key/value pairs based on the context’s properties.

The relevant interfaces shown:

```csharp
public interface IOptionsContext
{
    void PopulateReceiver<T>(T receiver) where T : IOptionsContextReceiver;
}
```

```csharp
public interface IOptionsContextReceiver
{
    void Receive<T>(string key, T value);
}
```

### 2) Implement an IOptionsContextReceiver

In the example, the receiver only cares about the `Country` property and derives a default temperature unit:

```csharp
internal class CountryTemperatureReceiver : IOptionsContextReceiver
{
    public string? DefaultUnit { get; private set; }

    public void Receive<T>(string key, T value)
    {
        if (key == "Country")
        {
            DefaultUnit = value is "USA" ? "Fahrenheit" : "Celsius";
        }
    }
}
```

Andrew points out that this is not truly decoupled: you’re now coupled via the string key (e.g., `"Country"`), so renaming the property can silently break things.

### 3) Configure WeatherForecastOptions using contextual configuration

You can still do “normal” options configuration, plus add a contextual configure callback that uses `IOptionsContext`:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<WeatherForecastOptions>(x => x.ForecastDays = 7)
    .Configure((IOptionsContext ctx, WeatherForecastOptions opts) =>
    {
        var receiver = new CountryTemperatureReceiver();

        ctx.PopulateReceiver(receiver);

        if (receiver.DefaultUnit is not null)
        {
            opts.TemperatureScale = receiver.DefaultUnit;
        }
    });
```

At runtime, `IContextualOptions<,>` gets a “globally configured” `IOptions<WeatherForecastOptions>` and then applies the contextual loader to produce the returned options instance.

## What is it *actually* for?

Andrew found an API proposal issue that was closed as “not planned / needs work” and notes discussion implying the feature may have been aimed at feature-flag/experimentation scenarios (LaunchDarkly is mentioned).

He contrasts that with existing approaches like `Microsoft.FeatureManagement` (widely used) and suggests that if the goal is feature flags/experimentation, you might consider OpenFeature.

## Definitely experimental (and annoying to adopt)

Key points from the post:

- No stable package versions (preview only).
- APIs are decorated with `[Experimental]`, producing compile-time error `EXTEXP0018` unless you opt in.
- Source-generated code also triggers the experimental warning/error, which effectively forces a broader suppression.

He shows suppressing via project file:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);EXTEXP0018</NoWarn>
</PropertyGroup>
```

## Takeaway

Andrew’s conclusion: he generally doesn’t think `Microsoft.Extensions.Options.Contextual` is worth adopting due to complexity, weak “decoupling” benefits, preview/experimental status, and low apparent adoption. If you’re already heavily invested in the `IOptions` approach and have a specific need, it might be worth exploring—but for feature flags/experimentation, he suggests looking at alternatives such as OpenFeature.


[Read the entire article](https://andrewlock.net/configuring-contextual-options-with-microsoft-extensions-options-contextual/)

