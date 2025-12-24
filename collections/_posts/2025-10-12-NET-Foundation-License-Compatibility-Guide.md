---
layout: "post"
title: ".NET Foundation License Compatibility Guide"
description: "This guide provides a detailed overview of which open source licenses are compatible with the .NET Foundation. It clarifies the Foundation's policy on permissible licenses (favoring permissive OSI-approved ones), explains reasons for incompatibility with copyleft licenses, and addresses common misconceptions. It also outlines business models supported by the .NET Foundation, including dual licensing and commercial services."
author: ".NET Foundation News and Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://dotnetfoundation.org/news-events/detail/license-compatibility-guide"
viewing_mode: "external"
feed_name: ".NET Foundation's Blog"
feed_url: "https://dotnetfoundation.org/feeds/blog"
date: 2025-10-12 02:22:03 +00:00
permalink: "/posts/2025-10-12-NET-Foundation-License-Compatibility-Guide.html"
categories: ["Coding"]
tags: [".NET Foundation", "Apache License 2.0", "AutoMapper", "BSD License", "Coding", "Commercial Services", "Contributor License Agreement", "Copyleft License", "Dual Licensing", "GitHub Sponsors", "GPL", "Licensing Policy", "MIT License", "Open Source", "Permissive License", "Posts", "Project Maintainers", "RPL", "Software Business Models"]
tags_normalized: ["dotnet foundation", "apache license 2dot0", "automapper", "bsd license", "coding", "commercial services", "contributor license agreement", "copyleft license", "dual licensing", "github sponsors", "gpl", "licensing policy", "mit license", "open source", "permissive license", "posts", "project maintainers", "rpl", "software business models"]
---

This .NET Foundation guide, authored by .NET Foundation News and Blog, explains which licenses are compatible, clarifies common licensing myths, and outlines supported commercial strategies for project maintainers.<!--excerpt_end-->

# .NET Foundation License Compatibility Guide

## Understanding What's Compatible (and What's Not)

### What Licenses ARE Compatible

The .NET Foundation accepts projects that use **permissive open source licenses**. Permissive licenses automatically approved if they’re on the [Open Source Initiative (OSI)](https://opensource.org/) approved list.

**Examples of compatible permissive licenses include:**

- MIT License
- Apache License 2.0
- BSD licenses (2-clause, 3-clause)
- ISC License

**Key requirements:**

- The main codebase must use a permissive OSI-approved license
- Mandatory dependencies should also be permissive (with exceptions for platform- or hardware-specific requirements)
- Committers must agree to a Contributor License Agreement (CLA)
- Copyright ownership must be defined and documented

### What Licenses Are NOT Compatible

The .NET Foundation does not accept projects using **copyleft licenses** such as:

- GPL (all versions)
- AGPL
- RPL
- MPL (in some cases)
- Other licenses with strong copyleft provisions

**Reason:** Copyleft licenses require all derivative works to be licensed similarly, which can deter corporate participation and doesn’t align with the Foundation’s priority for broad compatibility.

## Business Models Supported by the Foundation

### Permitted Strategies

- **Dual Licensing:** Offer your project under a permissive license and a commercial license.
- **Commercial Services:** Sell premium support, consulting, training, managed hosting, warranties, and SLAs.
- **GitHub Sponsors:** Fund projects via GitHub’s sponsorship platform.
- **Company Formation:** Spin up a company, provide paid support, enterprise offerings, and premium packages (core must stay permissive).

> **Principle:** As long as the source is free, it's compliant. Premium services and commercial models are allowed.

## Clarifying the AutoMapper Departure

AutoMapper left the Foundation amicably after moving to the copyleft RPL license, which is no longer compatible with Foundation requirements. Monetization was not the issue—only the license change to a non-permissive one. Dual-licensing with a permissive license would have allowed continued membership.

## Guidance for Project Maintainers

You **can**:

- Use dual licensing
- Start a company for your project
- Charge for extra support/services
- Offer paid/enterprise tiers
- Use sponsorship platforms
- Keep some features proprietary (core must be permissive)
- Provide commercial warranties

You **cannot**:

- Use copyleft (GPL, AGPL, RPL, etc.) for the core code
- Require copyleft licenses in core dependencies

## Foundation Policy

The .NET Foundation continues to review dual licensing frameworks but currently requires a permissive OSS license for project eligibility.

## Questions?

For further clarification, contact the Foundation. Commercial sustainability is encouraged, so long as the core remains open and permissive.

This post appeared first on ".NET Foundation's Blog". [Read the entire article here](https://dotnetfoundation.org/news-events/detail/license-compatibility-guide)
