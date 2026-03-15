---
external_url: https://devblogs.microsoft.com/powershell/powershell-openssh-and-dsc-team-investments-for-2025/
title: 'PowerShell, OpenSSH, and DSC Team Investments for 2025: Security, Integrations, and Community Priorities'
author: Steve Lee
feed_name: Microsoft PowerShell Blog
date: 2025-04-14 13:40:29 +00:00
tags:
- AI Shell
- Community Contributions
- Desired State Configuration
- DSC
- EntraID
- MCP
- Microsoft Artifact Registry
- OpenSSH
- PowerShell
- PowerShell Gallery
- PSResourceGet
- Script Analyzer
- VS Code Extension
- WinGet
- AI
- Azure
- DevOps
- Security
- News
- .NET
section_names:
- ai
- azure
- dotnet
- devops
- security
primary_section: ai
---
Steve Lee shares an in-depth look at the planned investments for PowerShell, OpenSSH, and Desired State Configuration (DSC) in 2025, emphasizing security, community input, and modernization across Microsoft's open tooling ecosystem.<!--excerpt_end-->

# PowerShell, OpenSSH, and DSC Team Investments for 2025

*By Steve Lee*

## Introduction

The PowerShell team outlines its priorities and expected investments for 2025, focusing on PowerShell, OpenSSH, Desired State Configuration (DSC), and tooling updates. The roadmap emphasizes security, community-driven development, integration improvements, support for new authentication standards, and the evolving role of AI within the shell environment.

---

## Key Focus Areas for 2025

### Security Improvements

- **Security remains the top priority.** Newly discovered or reported security issues are prioritized over new features.

### Bug Fixes & Community Participation

- Ongoing bug fixes and an enhanced process for reviewing and merging community pull requests.
- A public [GitHub project](https://github.com/orgs/PowerShell/projects/44) helps track and provide transparency on issues and pull requests currently under team focus based on community feedback.

### PowerShell 7.6

- The next LTS (Long-Term Servicing) release, aligned with .NET 10 cycles.
- Four preview releases are already available.

#### Notable Features and Changes

- **PowerShell Content Folder Migration**: Addressing community concerns about performance and syncing with OneDrive. Plans are underway to move PowerShell’s content folder out of `MyDocuments`, with an upcoming proposal to be shared for feedback.
- **Integration with Native Commands**: Designing ways for native applications (like Azure CLI, Winget) to integrate natively with PowerShell features such as Feedback Providers and Tab Completion, without extra modules. [RFC proposal here](https://github.com/PowerShell/PowerShell-RFC/pull/386).
- **WinGet and `PATH` Updates**: Making changes so that installations via WinGet automatically update the session's `PATH` variable, improving immediate usability.
- **PowerShell 7 Configuration as a DSC v3 Resource**: Allowing PowerShell 7 settings to be managed declaratively through DSC, supporting enterprise configuration at scale.

### PowerShell Gallery

- Backend migration from Azure Cloud Services (classic) to Azure Kubernetes Service (AKS) for reliability and scalability, aiming for a seamless transition for end users.
- Introduction of EntraID (formerly Azure AD) server-side authentication—enables managed identity publishing instead of API keys.

### PSResourceGet

- **EntraID Client-side Support**: Matching changes on the server, the client now also integrates with EntraID for authentication.
- **Microsoft Artifact Registry (MAR) Support**: Advancing towards General Availability for MAR in PSResourceGet, to provide a Microsoft-trusted default repository for modules and scripts. Built atop earlier support for Azure Container Registry, with roadmap plans to encompass any OCI-compliant container registry (via ORAS).

### Windows OpenSSH

- Continuation of integrating upstream OpenSSH changes into Windows OpenSSH, with previews and releases available for testing and stable use.
- Ongoing development of a DSC v3 resource for `SSHD_CONFIG`, targeting previews later in the year.

### Desired State Configuration v3 (DSC)

- DSC 3.0 is generally available across Windows, macOS, and Linux (non-Windows releases via GitHub).
- Active development for DSC v3.1, with multiple previews available; ongoing feature consideration and approval managed on GitHub.

### AI Shell

- Growing integration of AI into the PowerShell ecosystem to enhance productivity.
- Key future improvements:
  - Improved macOS support
  - Enhanced Azure PowerShell integration
  - Introduction of new Agents
  - Deeper integration with PowerShell core
  - Support for [Model Context Protocol (MCP)](https://modelcontextprotocol.io/introduction)

### Tooling Updates

- Continued focus on the PowerShell VSCode extension and Script Analyzer, ensuring robust issue handling and incorporating community pull requests as needs arise.

---

## Conclusion

Security is foundational to all planned work. The team is committed to secure software delivery, ongoing improvement of security practices, and direct collaboration with the community to address top issues. All areas outlined—whether stability, modernization, new integration, or AI-driven enhancements—support a vision of secure, developer-focused tooling within the Microsoft ecosystem.

**Links for more information:**

- [PowerShell Team Blog](https://devblogs.microsoft.com/powershell)
- [PowerShell on GitHub](https://github.com/PowerShell)

---

*For community tracking and to participate in discussions and proposals, visit the [PowerShell GitHub RFC repository](https://github.com/powershell/powershell-rfc).*

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/powershell-openssh-and-dsc-team-investments-for-2025/)
