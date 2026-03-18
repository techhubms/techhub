---
title: Pin Clustering in .NET MAUI Maps
feed_name: Microsoft .NET Blog
tags:
- .NET
- .NET 11
- .NET MAUI 11 Preview 2
- Android
- C#
- C# Dev Kit
- ClusterClicked Event
- Clustering
- ClusteringIdentifier
- Controls
- Ios
- Mac Catalyst
- Map Control
- MapKit
- Maps
- MAUI
- MAUI Samples
- Microsoft Learn Documentation
- MKClusterAnnotation
- Mobile
- Mobile Development
- News
- Pin Clustering
- VS
- VS Code
- XAML
external_url: https://devblogs.microsoft.com/dotnet/pin-clustering-in-dotnet-maui-maps/
date: 2026-03-18 18:00:00 +00:00
section_names:
- dotnet
primary_section: dotnet
author: David Ortinau
---

David Ortinau explains how .NET MAUI 11 Preview 2 adds built-in pin clustering to the Map control, including how to enable clustering, split pins into separate clustering groups, and respond to cluster taps on Android and iOS/Mac Catalyst.<!--excerpt_end-->

## Overview

If you’ve ever loaded a map with dozens (or hundreds) of pins, the UI quickly becomes hard to use because pins overlap. Starting in **.NET MAUI 11 Preview 2**, the **Map** control supports **pin clustering** out of the box on **Android** and **iOS/Mac Catalyst**.

## What is pin clustering?

Pin clustering groups nearby pins into a single *cluster marker* when zoomed out. As the user zooms in, clusters expand back into individual pins.

This was a long-requested feature (see issue [#11811](https://github.com/dotnet/maui/issues/11811)).

## Enable clustering

Enable clustering with a single property on the map:

```xml
<maps:Map IsClusteringEnabled="True" />
```

With clustering enabled, nearby pins are grouped into clusters with a badge showing the number of pins in the cluster.

## Separate clustering groups

If you want different types of pins to cluster independently (for example, coffee shops vs parks), set `ClusteringIdentifier` on each `Pin`.

```csharp
map.Pins.Add(new Pin
{
    Label = "Pike Place Coffee",
    Location = new Location(47.6097, -122.3331),
    ClusteringIdentifier = "coffee"
});

map.Pins.Add(new Pin
{
    Label = "Occidental Square",
    Location = new Location(47.6064, -122.3325),
    ClusteringIdentifier = "parks"
});
```

Pins with the same identifier cluster together; different identifiers form separate clusters even when they are geographically close.

## Handle cluster taps

When a user taps a cluster marker, the `ClusterClicked` event fires and provides `ClusterClickedEventArgs` with access to the pins in the cluster.

```csharp
map.ClusterClicked += async (sender, e) =>
{
    string names = string.Join("\n", e.Pins.Select(p => p.Label));
    await DisplayAlert($"Cluster ({e.Pins.Count} pins)", names, "OK");

    // Suppress default zoom-to-cluster behavior:
    // e.Handled = true;
};
```

Event args include:

- `Pins` — the pins in the cluster
- `Location` — the cluster’s geographic center
- `Handled` — set to `true` to override the default zoom behavior

## Platform notes

- **Android**: Uses a custom grid-based clustering algorithm that recalculates as zoom changes; no external dependencies.
- **iOS and Mac Catalyst**: Uses MapKit’s native `MKClusterAnnotation` for smooth, platform-native cluster animations.

## Samples and docs

- Sample: the **Maps** sample in `maui-samples` includes a new *Clustering* page: https://github.com/dotnet/maui-samples/tree/main/10.0/UserInterface/Views/Map/MapDemo/WorkingWithMaps
- Documentation: Maps control → pin clustering: https://learn.microsoft.com/dotnet/maui/user-interface/controls/map#pin-clustering

## Summary

Pin clustering in .NET MAUI Maps provides a production-ready way to make map-heavy apps easier to use: enable clustering with one property, split clustering with `ClusteringIdentifier`, and handle cluster taps via `ClusterClicked`.

To try it, install **.NET 11 Preview 2** and update/install the **.NET MAUI workload**. On Windows, the post recommends **Visual Studio 2026 Insiders**, or you can use **Visual Studio Code** with the **C# Dev Kit** extension.

[Read the entire article](https://devblogs.microsoft.com/dotnet/pin-clustering-in-dotnet-maui-maps/)

