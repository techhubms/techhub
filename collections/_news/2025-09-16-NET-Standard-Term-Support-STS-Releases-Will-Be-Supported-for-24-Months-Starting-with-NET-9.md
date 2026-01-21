---
external_url: https://devblogs.microsoft.com/dotnet/dotnet-sts-releases-supported-for-24-months/
title: .NET Standard Term Support (STS) Releases Will Be Supported for 24 Months Starting with .NET 9
author: Jamshed Damkewala
feed_name: Microsoft .NET Blog
date: 2025-09-16 17:00:00 +00:00
tags:
- .NET
- .NET 8
- .NET 9
- C#
- Development Lifecycle
- Lifecycle
- Long Term Support
- LTS
- Maintenance & Updates
- Microsoft
- OOB Releases
- Package Dependencies
- Release Management
- Standard Term Support
- STS
- Support
- Support Policy
- Upgrade Strategy
section_names:
- coding
---
Jamshed Damkewala outlines Microsoft's decision to extend support for .NET STS releases to 24 months, starting with .NET 9, and explains how this impacts lifecycle policy and developers' upgrade cycles.<!--excerpt_end-->

# .NET Standard Term Support (STS) Releases Will Be Supported for 24 Months Starting with .NET 9

**Author:** Jamshed Damkewala  

Microsoft has announced that support for .NET Standard Term Support (STS) releases will be extended from 18 months to 24 months, beginning with .NET 9. This means .NET 9 will now be supported until November 10, 2026, aligning its end-of-support date with .NET 8 (an LTS release).

## Key Points

- **STS Release Cycle Change:** STS (odd-numbered) .NET releases were previously supported for 18 months (6 months after the next release). With this update, they will now receive support for 24 months (12 months after their successor ships).
- **Scope:** This change starts with .NET 9. Long Term Support (LTS) releases remain unchanged (with 3 years support or 12 months after their successor ships), and out-of-band (OOB) releases or components with their own support policies are not affected.
- **Lifecycle Implications:** Both .NET 8 (LTS) and .NET 9 (STS) now reach end-of-support on the same date (Nov 10, 2026). This helps organizations avoid unexpected loss of support if they install out-of-band releases that depend on STS packages.

## Background

.NET typically ships a new major release in November each year. Even-numbered versions are LTS releases, while odd-numbered versions are STS. If an OOB package relies on an STS package, support for that component follows the shorter STS lifecycle, which previously could end sooner than the LTS package the organization intended to stick with. The new policy mitigates this problem.

### Version Support Timeline Examples

| Version | Release Date        | Type | End of Support      |
|---------|--------------------|------|--------------------|
| .NET 8  | November 14, 2023  | LTS  | November 10, 2026  |
| .NET 9  | November 12, 2024  | STS  | November 10, 2026* |
| .NET 10 | November 11, 2025  | LTS  | (TBD)              |

*New support date under updated policy

## Why Change the STS Policy?

- **Customer Need:** Many organizations stick to LTS versions for the longer support window. STS releases fell out of favor due to their shorter cycle, especially if OOB components forced an STS dependency, making life-cycle management complex.
- **Out-of-Band Releases:** If an OOB release depends on an STS package, an upgrade could inadvertently bring parts of a system onto the shorter STS timeline. The new policy means both LTS and STS end at the same time, simplifying support planning.
- **Easier Adoption:** The extended STS policy makes considering STS for production needs less risky, increasing flexibility for organizations.

## What Should Organizations Do?

- Continue current upgrade or adoption plans. .NET 10 will bring new features and performance improvements, so expect further lifecycle updates in the future.

Find more information and see the full support lifecycle table at the official [Microsoft .NET support policy](https://dotnet.microsoft.com/platform/support/policy/) page.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-sts-releases-supported-for-24-months/)
