---
layout: "post"
title: "Semantic Kernel: Package Previews, Graduations & Deprecations"
description: "The article summarizes recent updates to the Semantic Kernel .NET codebase, including package graduations, deprecations, API alignments, new plugin improvements, and enhanced maintainability focused on structure, APIs, and security. Migration guides and integration tests are highlighted for smooth transitions and reliability."
author: "Sophia Lagerkrans-Pandey, Sergey Menshykh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-package-previews-graduations-deprecations/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2025-05-16 08:32:47 +00:00
permalink: "/news/2025-05-16-Semantic-Kernel-Package-Previews-Graduations-and-Deprecations.html"
categories: ["AI", "Coding"]
tags: [".NET", "AI", "API Updates", "Azure Code Interpreter", "Coding", "HTTP Consistency", "Integration Tests", "Migration Guide", "News", "OpenAPI", "Package Deprecation", "Package Graduation", "Plugin Development", "Python Plugin", "Semantic Kernel"]
tags_normalized: ["dotnet", "ai", "api updates", "azure code interpreter", "coding", "http consistency", "integration tests", "migration guide", "news", "openapi", "package deprecation", "package graduation", "plugin development", "python plugin", "semantic kernel"]
---

Authored by Sophia Lagerkrans-Pandey and Sergey Menshykh, this article details the latest Semantic Kernel .NET updates, highlighting package maturations, deprecations, and technical enhancements for developers.<!--excerpt_end-->

# Semantic Kernel: Package Previews, Graduations & Deprecations

*By Sophia Lagerkrans-Pandey & Sergey Menshykh*

This post summarizes the latest updates, package lifecycle changes, and clean-up efforts across the Semantic Kernel .NET codebase, all geared toward improving maintainability, API alignment, and user experience. Here are the key details regarding package graduations, deprecations, and significant improvements:

## Graduations

### Plugins.Core Package Graduated to Preview

- The `Microsoft.SemanticKernel.Plugins.Core` package moves from “alpha” to “preview,” indicating it is now considered mature and stable for broader utilization.
- No new features were added, but the change signals that the package is suitable for those building on Semantic Kernel’s core plugins.

### PromptTemplates.Liquid Package Graduated

- The `Microsoft.SemanticKernel.Liquid` prompt template package has also graduated.
- Package validation is now enabled to enforce quality and compatibility requirements.

## Spring Cleaning – Deprecations

### Markdown Package Deprecated

- The `Microsoft.SemanticKernel.Markdown` package has been deprecated and removed due to lack of usage.
- For those still using it, a [migration guide](https://learn.microsoft.com/en-us/semantic-kernel/support/migration/functions-markdown-migration-guide) is available.

### Math and Wait Plugins Removed

- The Math and Wait plugins, present since the project’s early stages, were removed due to diminished relevance.

### OpenAI and Handlebars Planners Deprecated

- The `Microsoft.SemanticKernel.Planners.Handlebars` and `Microsoft.SemanticKernel.Planners.OpenAI` planners have been deprecated in favor of more robust alternatives, such as function calling.
- These are now discontinued on NuGet. Users should consult the [stepwise planner migration guide](https://learn.microsoft.com/en-us/semantic-kernel/support/migration/stepwise-planner-migration-guide?pivots=programming-language-csharp).

## Improvements & Updates

### Plugins.Core Package API

- The "experimental" designation was removed, reflecting stable production usage.

### Stable OpenAPI API

- The experimental tag was also removed from the OpenAPI API, and package validation was enabled to ensure ongoing robustness.

### SessionsPythonPlugin Updates

- **API Migration:** Updated to the latest Azure code interpreter API version (`2024-10-02-preview`), introducing breaking changes to the public API. [Migration guide here.](https://learn.microsoft.com/en-us/semantic-kernel/support/migration/sessions-python-plugin-migration-guide-2025)
- **Structured Results:** Results from Python code execution are now a dedicated type, allowing for cleaner, more structured downstream integration.
- **Request Handling:** Propagation of cancellation tokens through kernel functions enables graceful termination and improved management of resources and responsiveness.
- **Domain Control:** Ability to restrict outbound requests to certain domains, improving security and configurability.
- **HTTP Consistency:** Refactored for consistent `SendWithSuccessCheckAsync` HTTP extension method usage, aligning with core Semantic Kernel practices.
- **Code Clean-up:** Consolidation of functionality, improved logging, and readability.
- **Integration Tests:** New tests were added for reliability and early issue detection.

These changes are part of ongoing efforts to keep Semantic Kernel clean, robust, and developer-friendly. For migration from deprecated or changed components, refer to the linked Microsoft documentation.

Community feedback and questions are welcomed—join the discussion on [GitHub](https://github.com/microsoft/semantic-kernel).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-package-previews-graduations-deprecations/)
