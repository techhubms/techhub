---
layout: "post"
title: "PowerShell, OpenSSH, and DSC: Planned Team Investments for 2026"
description: "This announcement from Steve Lee outlines the PowerShell, OpenSSH, and DSC teams' planned investments for 2026. It highlights work on security improvements, upcoming PowerShell 7.7 features, enhancements to PSReadLine, OpenSSH with Entra ID, DSC v3 releases, and PowerShell Gallery/PSResourceGet developments. Key improvements target security, performance, module development, and new AI-driven automation capabilities."
author: "Steve Lee"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/powershell/powershell-openssh-and-dsc-team-investments-for-2026/"
viewing_mode: "external"
feed_name: "Microsoft PowerShell Blog"
feed_url: "https://devblogs.microsoft.com/powershell/feed/"
date: 2026-02-17 19:21:34 +00:00
permalink: "/2026-02-17-PowerShell-OpenSSH-and-DSC-Planned-Team-Investments-for-2026.html"
categories: ["Azure", "Coding", "Security"]
tags: ["AI", "AI Automation", "Auth Integration", "Azure", "Azure Container Registry", "Coding", "Desired State Configuration", "DSC", "Microsoft Artifact Registry", "Microsoft Entra ID", "News", "OpenSSH", "PowerShell", "PowerShell 7.7", "PowerShellGallery", "PSReadLine", "PSResourceGet", "Python Adapter", "Security"]
tags_normalized: ["ai", "ai automation", "auth integration", "azure", "azure container registry", "coding", "desired state configuration", "dsc", "microsoft artifact registry", "microsoft entra id", "news", "openssh", "powershell", "powershell 7dot7", "powershellgallery", "psreadline", "psresourceget", "python adapter", "security"]
---

Steve Lee shares the 2026 investment plans for the PowerShell, OpenSSH, and DSC teams, highlighting upcoming improvements in security, coding productivity, Entra ID integration, and automation.<!--excerpt_end-->

# PowerShell, OpenSSH, and DSC Team Investments for 2026

**Author: Steve Lee**

## Community Appreciation

The team extends thanks to the community for contributions and feedback on PowerShell, OpenSSH, DSC, and related tools, reinforcing community-driven development.

## Security Improvements

Security and compliance remain a top priority. The team will continue to prioritize fixing security issues and maintaining compliance, even when such work may not be directly visible to end users.

## Bug Fixes and Community PRs

Prioritization continues for critical bug fixes and reviewing community pull requests, ensuring that community contributions help shape PowerShell’s roadmap.

## PowerShell 7.7 Roadmap

- **PSUserContentPath Relocation**: Addressing longstanding issues with user content storage by providing a new design for storing modules, profiles, and help files outside the default Documents folder, particularly to resolve issues with OneDrive sync.
- **Non-Profile Based Module Loading**: Making it possible for developers to register tab-completers and feedback providers without modifying the user's profile script. [Design proposal available](https://github.com/PowerShell/PowerShell-RFC/pull/386).
- **Delayed Update Notifications**: Updates to version notification timing to improve the experience for users with varying package managers.
- **Bash-Style Aliases/Macros**: Exploring advanced aliasing features akin to Bash, such as parameter passing and conditional execution.
- **AI-Assisted Scripting with MCP Server/Tools**: Plans to develop Model Context Protocol (MCP) server and tools to support integrating AI with PowerShell in a secure and controlled manner, focusing on automation and code assistance use cases.

## PSReadLine Enhancements

- **Context-Aware Predictive IntelliSense**: Working to make command line predictions responsive to context, such as the current directory or Git repository status.
- **Decoupling Keyboard Input and Terminal Rendering**: Refactoring for future feature growth, requiring fundamental architectural changes.

## PowerShellGallery/PSResourceGet Updates

- **MAR Migration**: Completing migration to Microsoft Artifact Registry (MAR), offering more reliability and security.
- **Concurrency and Performance**: Improving PSResourceGet for faster installations through concurrent downloads.
- **General Gallery Reliability and Scalability**: Infrastructure improvements to support module publishing and discovery.

## Windows OpenSSH Plans

- **Entra ID Authentication Support**: Investigating ways to support Microsoft Entra ID (formerly Azure AD) authentication for SSH sessions on Windows, helping enterprise security.

## Desired State Configuration (DSC) v3

- **DSC v3.2 General Availability**: Targeting a first-half 2026 GA, expanding on the preview momentum.
- **Python Adapter for Linux**: Allowing DSC resources to be authored in Python, expanding cross-platform support for DSC.
- **Post-v3.2 Enhancements (DSC v3.3)**: Ongoing customer- and partner-driven enhancements after general release.

## Continuing Focus

The team reiterates ongoing commitments to security, bug fixing, performance, and ongoing community engagement.

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/powershell-openssh-and-dsc-team-investments-for-2026/)
