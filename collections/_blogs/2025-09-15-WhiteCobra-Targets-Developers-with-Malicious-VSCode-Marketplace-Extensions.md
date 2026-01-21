---
external_url: https://devops.com/whitecobra-targets-developers-with-dozens-of-malicious-extensions/?utm_source=rss&utm_medium=rss&utm_campaign=whitecobra-targets-developers-with-dozens-of-malicious-extensions
title: WhiteCobra Targets Developers with Malicious VSCode Marketplace Extensions
author: Jeff Burt
feed_name: DevOps Blog
date: 2025-09-15 16:56:48 +00:00
tags:
- Attack Chain
- C2 Servers
- Cloud Infrastructure
- Cryptocurrency
- Cryptocurrency Theft
- Cybersecurity
- Developer Security
- DevOps And Open Technologies
- Extension.js
- Infostealer
- Koi Security
- LummaStealer
- Malicious Extensions
- Malware
- Marketplace Trust
- Open VSX
- PowerShell
- Prompt.js
- Social Facebook
- Social LinkedIn
- Social X
- Software Supply Chain
- VS Code
- WhiteCobra
section_names:
- devops
- security
---
Jeff Burt’s article exposes how the WhiteCobra group exploits VSCode marketplace trust, detailing malware delivery tactics targeting developer environments. Essential background for development teams on supply chain security.<!--excerpt_end-->

# WhiteCobra Targets Developers with Malicious VSCode Marketplace Extensions

## Overview

Security researchers from Koi Security have uncovered ongoing attacks by the threat group WhiteCobra, which is distributing over two dozen malicious extensions in the VSCode and Open VSX marketplaces. These attacks primarily target developers using popular source code editing tools such as VSCode, Cursor, and Windsurf. The group’s end goal: to compromise developer environments and drain cryptocurrency wallets.

## Attack Timeline and Tactics

- WhiteCobra regularly uploads new malicious extensions, often replacing those taken down by marketplace operators.
- Promotions for these extensions utilize social media platforms, automated bots, and scripts that generate tens of thousands of fake downloads to simulate legitimacy.
- Marketplace signals (such as high install counts and ratings) can be manipulated, making it difficult for both junior and senior developers to spot the malicious packages.

## Technical Attack Chain

- Malicious VSIX extensions typically disguise themselves with boilerplate code (e.g., a standard `extension.js` with the "Hello World" template) to evade static or automated marketplace reviews.
- Malicious code is isolated in a secondary file, `prompt.js`, triggered only at runtime, reducing the chance of detection.
- The attack chain proceeds with a download from Cloudflare Pages—tailored for the developer's platform (including Windows).
- On Windows systems, a PowerShell script is launched, which executes a Python script that runs arbitrary shellcode, culminating in the deployment of the LummaStealer malware.
- LummaStealer harvests data from crypto wallets, remote access services (AnyDesk, VPNs, VNC), cloud accounts, messaging apps, and browser extensions related to password management.
- Communication with two separate Command and Control servers is established for data exfiltration.

## Impact and Revenue Model

- Victims have already reported major losses (e.g., a Russian blockchain developer lost $500,000, and prominent Ethereum developer Zak Cole was compromised via a malicious Cursor extension).
- WhiteCobra’s internal playbook suggests the group views the operation as a scalable, automated business, forecasting potential revenues of $10,000–$500,000 per hour given widespread infections.

## Analysis of Industry Vulnerability

- As code editor marketplaces grow in size and popularity, attackers exploit the lack of robust verification and trust mechanisms.
- Automated scripts can create tens of thousands of fake downloads, misleading the community and sometimes tricking automated reviews.
- Even seasoned security professionals can become victims due to the industrial precision and obfuscation tactics used.

## Key Takeaways for Developers and DevOps Teams

- Exercise caution when installing third-party extensions, especially those with recent spikes in popularity.
- Internal code review policies should include audits of all VSCode and compatible extension usage.
- Monitor for abnormal system or network behavior following the installation of new extensions.
- Encourage team-wide security updates and awareness on emerging supply chain threats like those detailed in this report.

## References

- [Koi Security’s Disclosure](https://securelist.com/open-source-package-for-cursor-ai-turned-into-a-crypto-heist/116908/)
- [Ethereum developer’s account of compromise](https://x.com/0xzak/status/1955265807807545763)
- [Analysis of LummaStealer](https://www.trendmicro.com/en_us/research/25/g/lumma-stealer-returns.html)

*Written by Jeff Burt. Read more DevOps and security insights at DevOps.com*

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/whitecobra-targets-developers-with-dozens-of-malicious-extensions/?utm_source=rss&utm_medium=rss&utm_campaign=whitecobra-targets-developers-with-dozens-of-malicious-extensions)
