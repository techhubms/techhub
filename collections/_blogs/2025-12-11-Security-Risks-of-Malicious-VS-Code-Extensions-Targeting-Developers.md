---
layout: post
title: Security Risks of Malicious VS Code Extensions Targeting Developers
author: Jeff Burt
canonical_url: https://devops.com/malicious-vs-code-extensions-take-screenshots-steal-info/
viewing_mode: external
feed_name: DevOps Blog
feed_url: https://devops.com/feed/
date: 2025-12-11 01:03:47 +00:00
permalink: /devops/blogs/Security-Risks-of-Malicious-VS-Code-Extensions-Targeting-Developers
tags:
- AI
- AI Coding Assistant
- CI/CD
- Crates.io
- Crates.io Security
- DevSecOps
- DLL Hijacking
- GitHub
- Information Security
- Infostealer
- Koi Security
- Lightshot
- Malicious Extensions
- npm Registry
- OpenAI
- Social Facebook
- Social LinkedIn
- Social X
- Software Supply Chain
- Threat Intelligence
- VS Code
section_names:
- devops
- security
---
Jeff Burt reports on the discovery of two malicious VS Code extensions targeting developers, detailing how these plugins exploited developer environments and posed significant software supply chain security risks.<!--excerpt_end-->

# Security Risks of Malicious VS Code Extensions Targeting Developers

By Jeff Burt

## Overview

Recent investigations by cybersecurity researchers at Koi Security uncovered two malicious Microsoft Visual Studio Code (VS Code) extensions, Bitcoin Black and Codo AI, designed to compromise developer environments. These extensions targeted developers through social engineering and delivered information-stealing malware with a focus on harvesting credentials, screen captures, WiFi passwords, and more.

## Key Findings

- **Malicious Extensions**: Bitcoin Black disguised as a “premium dark theme,” and Codo AI posed as an AI-powered coding assistant. Both delivered the same infostealer payload using different lures and methods.
- **Infection Vector**: The extensions abused the trust in the VS Code marketplace by mimicking legitimate tools and using techniques like DLL hijacking. A signed Lightshot executable was paired with a malicious DLL, allowing easy bypass of signature-based security checks.
- **Target Audience**: Developers, especially those using code repositories (GitHub, npm, crates.io), are increasingly targeted, with attackers aiming for credentials and sensitive organizational information.
- **Marketplace Response**: Microsoft removed both extensions (as well as another related one) from the marketplace once detected.
- **Technical Sophistication**: The attackers used advanced methods (activation events, malicious PowerShell scripts, and code obfuscation) yet left evidence in comments, suggesting ongoing development and experimentation.

## How the Attack Worked

- **Theme Extension Abuse**: While legitimate VS Code themes are typically harmless JSON files specifying color values, the Bitcoin Black theme extension had unnecessary activation events, entry points, and executed PowerShell—clear red flags.
- **AI Coding Assistant Ruse**: Codo AI operated as a functional AI coding assistant powered by OpenAI’s ChatGPT or DeepSeek models, but also hid malicious payloads within its codebase.
- **DLL Hijacking**: Both extensions dropped a legitimate Lightshot screenshot tool with an adjacent malicious DLL. This method takes advantage of trusted signatures while delivering the infostealer once the executable is run.

## Broader Implications

- **Growing Trend**: Threat actors are increasing attacks on developer tools and third-party package ecosystems, injecting vulnerabilities and malware at the supply chain level.
- **Risk to CI/CD**: As DevOps and CI/CD pipelines rely on trusted dependencies, a compromise at the extension or package level can propagate deep into development workflows.
- **Mitigation**: Vigilance is required in extension reviews, permission audits, and monitoring unusual behaviors (unexpected activation, network traffic, or script execution).

## Lessons for Developers

- Carefully inspect extensions or third-party packages before installation, especially those requesting broad permissions or containing executable code.
- Limit the use of extraneous plugins and regularly audit installed tools.
- Stay engaged with security advisories from trusted platforms and communities.

## References

- [Koi Security Report: The VS Code Malware That Captures Your Screen](https://www.koi.ai/blog/the-vs-code-malware-that-captures-your-screen)
- [GitHub List of Removed VS Code Packages](https://github.com/microsoft/vsmarketplace/blob/main/RemovedPackages.md)
- [SentinelOne on Threats in Code Repositories](https://www.sentinelone.com/blog/exploiting-repos-6-ways-threat-actors-abuse-github-other-devops-platforms/)

## About the Author

Jeff Burt is a technology journalist reporting on DevOps, security, and the software industry. [Author profile](https://devops.com/author/jeffrey-burt/)

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/malicious-vs-code-extensions-take-screenshots-steal-info/)
