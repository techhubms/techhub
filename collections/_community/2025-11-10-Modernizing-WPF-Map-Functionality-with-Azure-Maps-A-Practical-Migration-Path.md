---
layout: post
title: 'Modernizing WPF Map Functionality with Azure Maps: A Practical Migration Path'
author: IoTGirl
canonical_url: https://techcommunity.microsoft.com/t5/azure-maps-blog/guest-blog-modernizing-your-wpf-app-maps-functionality-with/ba-p/4468755
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-10 22:33:04 +00:00
permalink: /coding/community/Modernizing-WPF-Map-Functionality-with-Azure-Maps-A-Practical-Migration-Path
tags:
- .NET
- Azure
- Azure Maps
- Bing Maps
- C#
- Coding
- Community
- Desktop Applications
- Geospatial
- Map Control
- Migration
- MVVM
- NuGet
- OnTerra Systems
- SDK Integration
- WebView2
- Windows Development
- WPF
- XAML
section_names:
- azure
- coding
---
IoTGirl showcases how organizations can migrate WPF applications from Bing Maps to Azure Maps using the OnTerra Systems WPF Map Control, offering a smooth transition for developers without major code refactoring.<!--excerpt_end-->

# Modernizing WPF Map Functionality with Azure Maps

**By IoTGirl**

Organizations across industries continue to rely on robust Windows Presentation Foundation (WPF) desktop applications. With the deprecation of Bing Maps for Enterprise and its dedicated WPF Control, teams maintaining these applications need a practical approach to leverage modern mapping platforms without extensive rewrites.

## The Challenge

Many enterprise WPF applications, built using XAML for UI and C# or VB.NET for logic, depend on embedded mapping features via the Bing Maps WPF Control. With this control now deprecated, teams face the dilemma of rewriting their map functionality—an expensive, disruptive process—or finding a way to modernize in place.

## The Solution: OnTerra Systems WPF Map Control Powered by Azure Maps

One effective approach is to use the OnTerra Systems WPF Map Control:

- **Built on Azure Maps Web SDK:** It delivers the latest performance and features offered by Microsoft's Azure Maps service.
- **Full Feature Parity:** Provides road, aerial, and satellite map styles; supports overlays like pushpins, polygons, and polylines.
- **Minimal Migration Effort:** Existing Bing Maps WPF Control APIs are mapped internally to the Azure Maps platform, so application code changes are minimal.

## How It Works

- **Middle-Layer Wrapper:** The OnTerra control acts as a translation layer, turning Bing Maps WPF Control method calls and property settings into Azure Maps equivalents.
- **WebView2 Integration:** Uses the modern .NET WebView2 browser component to render the Azure Maps Web SDK right within your WPF app, fusing web-based map rendering with the familiar desktop experience.
- **SDK Packaging for Ease of Use:** Delivered as a NuGet package for simple drop-in integration, following existing .NET development practices.

## Migration Steps

1. **Replace Bing Maps Control:** Substitute the Bing Maps WPF Control NuGet package with OnTerra Map Control in your solution.
2. **Update Configuration:** Minor adjustments to reference the new control and its initialization logic.
3. **Retain Core Logic:** Your business and mapping logic continues to work, as the middle-layer interpreter ensures compatibility.

## Key Advantages

- **No Major Refactoring:** Continue supporting mission-critical applications without rewriting major code sections.
- **Up-to-Date Mapping:** Access ongoing improvements and expanded feature set from Azure Maps.
- **Developer Productivity:** SDK approach streamlines maintenance and speeds up delivery of new features.
- **Scalability for Future Needs:** Extend WPF applications with new map styles and interactive features as Azure Maps evolves.

## Resources

- [Azure Maps Overview](https://azure.microsoft.com/products/azure-maps)
- [Azure Maps Web SDK Documentation](https://learn.microsoft.com/azure/azure-maps/how-to-use-map-control)
- [OnTerra Systems WPF Map Control Contact](https://www.onterrasystems.com/contact)

For further details or to discuss migration, visit OnTerra Systems' website.

---

**Author:** IoTGirl

This article leverages insights from OnTerra Systems to help .NET developers and IT teams future-proof their desktop mapping solutions.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-maps-blog/guest-blog-modernizing-your-wpf-app-maps-functionality-with/ba-p/4468755)
