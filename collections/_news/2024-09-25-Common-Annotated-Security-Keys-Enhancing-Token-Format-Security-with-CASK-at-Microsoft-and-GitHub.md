---
layout: "post"
title: "Common Annotated Security Keys: Enhancing Token Format Security with CASK at Microsoft and GitHub"
description: "Michael C. Fanning explores improvements in security token formats by GitHub and Microsoft through the Common Annotated Security Standard (CASK). The post details fixed signatures, checksums, high-entropy BASE62 keys, standardized detection, and Microsoft's open approach to safer secret management across cloud providers."
author: "Michael C. Fanning"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/engineering-at-microsoft/common-annotated-security-keys/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/engineering-at-microsoft/feed/"
date: 2024-09-25 18:50:15 +00:00
permalink: "/2024-09-25-Common-Annotated-Security-Keys-Enhancing-Token-Format-Security-with-CASK-at-Microsoft-and-GitHub.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["1ES", "ADO Azure DevOps GHAS", "Azure", "Azure DevOps", "BASE62", "CASK", "Checksum", "DevOps", "Engineering@Microsoft", "Entropy", "Fixed Signatures", "GitHub Advanced Security", "Identity Management", "Metadata", "News", "Open Standard", "Sdl", "Secret Management", "Security", "Security Tokens", "Static Analysis", "Test Keys", "Token Detection"]
tags_normalized: ["1es", "ado azure devops ghas", "azure", "azure devops", "base62", "cask", "checksum", "devops", "engineeringatmicrosoft", "entropy", "fixed signatures", "github advanced security", "identity management", "metadata", "news", "open standard", "sdl", "secret management", "security", "security tokens", "static analysis", "test keys", "token detection"]
---

In this article, Michael C. Fanning details how GitHub and Microsoft are advancing secure secret management through the Common Annotated Security Standard (CASK), outlining its technical features and its role in strengthening engineering and DevOps security.<!--excerpt_end-->

## Common Annotated Security Keys: Advancing Secret Management in the Cloud

**Author:** Michael C. Fanning  
**Source:** [Engineering@Microsoft](https://devblogs.microsoft.com/engineering-at-microsoft/common-annotated-security-keys/)  

In April 2021, GitHub announced [significant changes to their security token format](https://github.blog/engineering/platform-security/behind-githubs-new-authentication-token-formats/), introducing enhancements to combat the risks associated with accidental exposure of secure tokens.

### Improvements in Security Token Formats

The primary improvements involved two key techniques:

- **Fixed Signature in Generated Tokens:** Automatically appends a consistent, unique identifier to each token, making them easily recognizable in source code and other artifacts.
- **Checksum:** Integrates a simple validity check that reduces the likelihood of false positives (unnecessary alerts) and false negatives (missed real exposures) during automated scans.

These techniques are now commonly implemented by Microsoft across their service providers, including Azure DevOps, where tokens—known as 'identifiable keys'—are designed to be efficiently scanned, detected, and rotated when necessary.

### Benefits for Security and DevOps

- **Open-Source Scan Tools Compatibility:** Identifiable secrets empower open-source and commercial scanners (e.g., GitHub Advanced Security for Azure DevOps) to improve detection without causing needless alerts or developer friction.
- **Productivity Preservation:** Given the low chance of false positives, Microsoft can confidently enforce policies that block identifiable keys from being checked into source code or persisted in work tracking items, minimizing wasted effort.

### Evolution Toward a Common Standard (CASK)

Microsoft has evolved these approaches into the **Common Annotated Security Standard (CASK)**—an open specification to further strengthen secret management and detection. The standard’s core features include:

#### 1. Free of Special Characters

A CASK key utilizes only alphanumeric characters (BASE62 alphabet), ensuring compatibility in any context without concerns about escaping or encoding.

#### 2. Strong Entropy

Keys are generated with 52 BASE62 characters, offering approximately 310 bits of entropy—making brute-force attacks computationally infeasible, even on post-quantum computers.

#### 3. Fixed Signatures

Each CASK key incorporates:

- A core standard signature (e.g., `JQQJ`), which is rare in source code and allows high-precision, high-performance scanning.
- A provider-specific signature (e.g., `AZDO` for Azure DevOps) to identify the source service.

#### 4. Additional Features

- **Creation Timestamps:** Embeds month and year, aiding incident response and key rotation policies.
- **Dedicated Test Keys:** Allows developers and tool authors to safely test detection and response mechanisms with non-sensitive sample secrets.
- **Platform-Reserved Metadata:** Extra bits encoded for provider-specific data (e.g., Azure-specific metadata), increasing extensibility and auditability.

### Implementation and Open Standards

Microsoft minimizes the cost and effort required for secret scanners to secure its services by standardizing the key format. Other service providers are also encouraged to adopt CASK and take advantage of its open, extensible format. Microsoft plans to release full technical documentation for CASK as open source to promote broader adoption.

### Call to Action

Service providers and security teams are encouraged to:

- Review and consider implementing the CASK format.
- Provide feedback and suggestions for further improvements as the standard evolves.

Microsoft will soon share more details about Azure-specific implementations and invites the community to engage with the CASK specification.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/engineering-at-microsoft/common-annotated-security-keys/)
