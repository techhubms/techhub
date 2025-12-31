---
layout: "post"
title: "WhiteCobra’s Malicious VSCode Extensions Pose Major Security Risk for Developers"
description: "This article explores how the WhiteCobra threat group has been deploying dozens of malicious extensions through the VSCode and Open VSX marketplaces, targeting software developers with the aim of stealing cryptocurrency and sensitive credentials. It covers their campaign sophistication, exploitation techniques, and the impact on the engineering community, as well as the challenges in detecting such threats given current marketplace verification systems."
author: "Jeff Burt"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/whitecobra-targets-developers-with-dozens-of-malicious-extensions/"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-09-15 16:56:48 +00:00
permalink: "/blogs/2025-09-15-WhiteCobras-Malicious-VSCode-Extensions-Pose-Major-Security-Risk-for-Developers.html"
categories: ["DevOps", "Security"]
tags: ["Attack Chain", "C2 Server", "Cloud Infrastructure", "Cryptocurrency", "Cryptocurrency Theft", "Cybersecurity", "Developer Security", "DevOps", "DevOps And Open Technologies", "DevOps Security", "Extension.js", "LummaStealer", "Malicious Extensions", "Malware", "Marketplace Threat", "Open VSX", "Blogs", "PowerShell", "Python Script", "Secure Software Development", "Security", "Social Engineering", "Social Facebook", "Social LinkedIn", "Social X", "Software Supply Chain", "VS Code", "WhiteCobra", "Windows Security"]
tags_normalized: ["attack chain", "c2 server", "cloud infrastructure", "cryptocurrency", "cryptocurrency theft", "cybersecurity", "developer security", "devops", "devops and open technologies", "devops security", "extensiondotjs", "lummastealer", "malicious extensions", "malware", "marketplace threat", "open vsx", "blogs", "powershell", "python script", "secure software development", "security", "social engineering", "social facebook", "social linkedin", "social x", "software supply chain", "vs code", "whitecobra", "windows security"]
---

Jeff Burt analyzes how the WhiteCobra group is targeting developers through malicious VSCode and Open VSX extensions, unveiling sophisticated techniques for cryptocurrency and credential theft.<!--excerpt_end-->

# WhiteCobra’s Malicious VSCode Extensions Pose Major Security Risk for Developers

**Author: Jeff Burt**

## Overview

A new security threat targets developers through VSCode and Open VSX marketplaces. The WhiteCobra group has released dozens of malicious extensions with the goal of stealing cryptocurrency wallets and sensitive information. These attacks affect developers using IDEs such as VSCode, Cursor, and Windsurf.

## How the Campaign Works

Researchers from Koi Security have tracked WhiteCobra’s activities, revealing a relentless campaign: new malicious extensions are pushed out weekly, maintaining a presence even as others are removed. Notably, WhiteCobra was behind a significant theft in June, stealing $500,000 in crypto from a Russian developer using a compromised extension. In August, veteran Ethereum developer Zak Cole reported having his wallet drained after using a malicious Cursor extension.

Koi’s investigation found that attackers could launch a new campaign in under three hours, from packaging the extension to launching promotional efforts. Social media—such as X (formerly Twitter)—is exploited to generate buzz, while bots create up to 50,000 fake downloads to bolster the extensions’ legitimacy.

## Technical Details of the Attack

WhiteCobra creates VSIX extensions that appear legitimate. The primary file, `extension.js`, mimics boilerplate code but offloads malicious behavior to a secondary script, `prompt.js`. This segmentation enables the attackers to circumvent common code reviews and security scans.

Once installed, these extensions initiate a multi-stage attack:

- **Download and Execution:** A stage-specific payload is fetched from Cloudflare Pages. On Windows systems, this involves running a PowerShell script that downloads and initiates a Python script, which then executes shellcode.
- **Malware Deployment:** The final payload is LummaStealer—a sophisticated malware tool. LummaStealer collects:  
  - Cryptocurrency wallet data  
  - Credentials for remote access and cloud infrastructure (AnyDesk, VPNs, VNC, etc.)  
  - Password manager information  
  - Messaging platform details
- **C2 Communication:** The malware connects with command-and-control (C2) servers to exfiltrate stolen data.

## Impact and Wider Implications

WhiteCobra’s organized approach—complete with playbooks and revenue projections—shows the industrialization of attacks against developers. Attackers can earn substantial sums: up to $10,000 per hour for high-value wallets, and more if attacks scale broadly.

Traditional marketplace trust signals (ratings, download counts, reviews) offer little protection here: they are easily manipulated via fake downloads and automated bot activity.

## Lessons for Developers and Security Teams

- **Rigorous Extension Vetting:** Be wary of new or newly popular extensions, especially those with suspiciously high install numbers or recently created accounts.
- **Monitor Marketplace Developments:** Stay updated on reported malicious packages. Security news from firms like Koi and affected developers (e.g., posts from Zak Cole) can provide early warnings.
- **Adopt Defense-in-Depth:** Use endpoint protection and MFA, and avoid storing sensitive credentials locally.
- **Promote Marketplace Reforms:** Advocate for stronger verification and reporting mechanisms within extension marketplaces.

## References

- [Koi Security Analysis](https://securelist.com/open-source-package-for-cursor-ai-turned-into-a-crypto-heist/116908/)
- [LummaStealer Details (Trend Micro)](https://www.trendmicro.com/en_us/research/25/g/lumma-stealer-returns.html)
- [Zak Cole Security Incident](https://x.com/0xzak/status/1955265807807545763)

---

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/whitecobra-targets-developers-with-dozens-of-malicious-extensions/)
