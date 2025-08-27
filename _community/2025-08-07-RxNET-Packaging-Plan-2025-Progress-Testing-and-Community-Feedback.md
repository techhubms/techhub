---
layout: "post"
title: "Rx.NET Packaging Plan 2025: Progress, Testing, and Community Feedback"
description: "This community discussion highlights Ian Griffiths' recent update on Rx.NET's packaging approach for the upcoming v7 release. The conversation covers efforts to reduce package bloat, the introduction of the Rx Gauntlet test suite, and the use of Power BI for reporting. It also features debate over moving Rx into the .NET Base Class Library, as well as commentary on Microsoft’s ongoing support for the library."
author: "hm_vr"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/dotnet/comments/1mjtvuv/rxnet_packaging_plan_2025/"
viewing_mode: "external"
feed_name: "Reddit DotNet"
feed_url: "https://www.reddit.com/r/dotnet/.rss"
date: 2025-08-07 07:47:10 +00:00
permalink: "/2025-08-07-RxNET-Packaging-Plan-2025-Progress-Testing-and-Community-Feedback.html"
categories: ["Coding"]
tags: [".NET", ".NET 10", "AsyncEnumerable", "Automation", "Base Class Library", "Coding", "Community", "IObservable", "NuGet Packages", "Open Source Maintenance", "Power BI", "Rx.NET", "Software Packaging", "System.Linq.Async", "System.Reactive", "Unit Testing"]
tags_normalized: ["dotnet", "dotnet 10", "asyncenumerable", "automation", "base class library", "coding", "community", "iobservable", "nuget packages", "open source maintenance", "power bi", "rxdotnet", "software packaging", "systemdotlinqdotasync", "systemdotreactive", "unit testing"]
---

hm_vr and other community members discuss Ian Griffiths' update on Rx.NET's packaging improvements, automated testing, and future plans for v7, including the challenges of BCL integration.<!--excerpt_end-->

# Rx.NET Packaging Plan 2025: Progress and Community Discussion

Ian Griffiths has provided an update on Rx.NET's development since June, focusing on the persistent 'package bloat' issue that has affected library distribution and uptake. To address this, a new automated test suite named **Rx Gauntlet** has been implemented, utilizing comprehensive testing and Power BI-generated reports to validate prospective packaging changes as development targets the v7 stable release.

Key points discussed include:

- **Package Bloat Mitigation**: Strategies for breaking up and optimizing Rx.NET’s NuGet packages to improve usability and minimize unnecessary dependencies.
- **Rx Gauntlet Test Suite**: An automated suite that rigorously evaluates proposed packaging solutions. Outputs from these tests are reported using Power BI dashboards, adding transparency and enabling data-driven decision making.
- **v7 Design Approaches**: Two main design paths are compared for the upcoming Rx.NET version 7, with Griffiths actively soliciting community feedback to determine the most practical and impactful direction.
- **BCL Integration Debate**: Community members express frustration that `IObservable` exists in the .NET platform while most Rx utilities remain external, arguing for deeper integration into the Base Class Library (BCL). Others voice concern that deeper integration could jeopardize Rx.NET’s long-term maintenance if Microsoft reduces resourcing, referencing the migration of `System.Linq.Async` to the BCL in .NET 10 as a recent precedent.

**Related Resources:**

- [AsyncEnumerable in .NET 10](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/asyncenumerable)

The discussion is ongoing, and contributors are encouraged to provide feedback on both packaging strategies and the broader direction of Rx.NET within the .NET ecosystem.

---

*Note: This post originated from a community forum. Please observe moderation guidelines and search for prior discussions before posting.*

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mjtvuv/rxnet_packaging_plan_2025/)
