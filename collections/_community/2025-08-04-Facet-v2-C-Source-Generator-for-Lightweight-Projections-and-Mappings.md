---
layout: post
title: 'Facet v2: C# Source Generator for Lightweight Projections and Mappings'
author: Voiden0
canonical_url: https://www.reddit.com/r/dotnet/comments/1mh7x1b/facet_v2_a_source_generator_for_projections_and/
viewing_mode: external
feed_name: Reddit DotNet
feed_url: https://www.reddit.com/r/dotnet/.rss
date: 2025-08-04 08:30:19 +00:00
permalink: /coding/community/Facet-v2-C-Source-Generator-for-Lightweight-Projections-and-Mappings
tags:
- .NET
- API Models
- C#
- Compile Time
- Custom Mappings
- DTOs
- Facet
- LINQ
- Partial Classes
- Projections
- Source Generator
section_names:
- coding
---
Authored by Voiden0, this article introduces Facet v2, a C# source generator for creating projections and mappings efficiently, highlighting recent improvements driven by community feedback.<!--excerpt_end-->

## Overview

**Facet** is a C# source generator designed to help developers define lightweight projections—such as Data Transfer Objects (DTOs) and API models—directly from existing domain models. By leveraging source generation, it automates the process of creating partial classes, records, structs, and LINQ projection code at compile time.

### Key Features

- **Creates Projections:** Generate lightweight DTOs and API models based on domain models.
- **Zero Runtime Cost:** All code is generated during build, eliminating reflection or mapping logic at runtime.
- **Partial Classes, Records, Structs:** Flexible code generation tailored to project needs.
- **LINQ Projections:** Source-generated projections simplify and standardize data transformations in LINQ queries.
- **Custom Mappings:** Developers can define projections with custom property mappings to fit specific requirements.

### Community Feedback and Improvements

Facet v2 builds on the original version with enhancements driven by user feedback. Previous issues have been addressed, contributing to increased stability and functionality.

### Getting Started

To integrate Facet v2 into a project, developers can:

1. Include the Facet source generator as a dependency.
2. Define projection attributes or settings on domain models.
3. Compile the project to trigger generation of the desired projection types.

### Links & Resources

- [Project Repository](https://github.com/Tim-Maes/Facet)
- [Reddit Discussion](https://www.reddit.com/r/dotnet/comments/1mh7x1b/facet_v2_a_source_generator_for_projections_and/)

Facet v2 exemplifies how source generation in C# can optimize developer productivity and code maintainability.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mh7x1b/facet_v2_a_source_generator_for_projections_and/)
