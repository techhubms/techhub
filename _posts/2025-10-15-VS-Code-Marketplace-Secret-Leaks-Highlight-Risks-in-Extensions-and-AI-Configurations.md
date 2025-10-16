---
layout: "post"
title: "VS Code Marketplace Secret Leaks Highlight Risks in Extensions and AI Configurations"
description: "This deep dive examines the security vulnerabilities uncovered by Wiz researchers in the Visual Studio Code and OpenVSX marketplaces. Over 500 extensions were found to contain leaked secrets, including access tokens for Microsoft Azure DevOps and AI providers. The article details the investigation process, the implications for the developer supply chain, and the collaborative response from Microsoft and security experts, as well as recommendations for extension publishers and users on minimizing risk."
author: "Jeff Burt"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/massive-vs-code-secrets-leak-puts-focus-on-extensions-ai-wiz/"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-10-15 18:35:27 +00:00
permalink: "/2025-10-15-VS-Code-Marketplace-Secret-Leaks-Highlight-Risks-in-Extensions-and-AI-Configurations.html"
categories: ["AI", "Azure", "DevOps", "Security"]
tags: ["Access Tokens", "AI", "AI Development", "Azure", "Azure DevOps", "Code Editor", "Developer Tools", "DevOps", "DevOps And Open Technologies", "Exposed Secrets", "Extension Security", "Extensions Leak", "Identity Access Management", "Identity And Access Management", "Microsoft Marketplace", "Microsoft Visual Studio", "OpenVSX", "Plugin Vulnerabilities", "Posts", "Secrets Management", "Security", "Social Facebook", "Social LinkedIn", "Social X", "Supply Chain Security", "VS Code", "Wiz"]
tags_normalized: ["access tokens", "ai", "ai development", "azure", "azure devops", "code editor", "developer tools", "devops", "devops and open technologies", "exposed secrets", "extension security", "extensions leak", "identity access management", "identity and access management", "microsoft marketplace", "microsoft visual studio", "openvsx", "plugin vulnerabilities", "posts", "secrets management", "security", "social facebook", "social linkedin", "social x", "supply chain security", "vs code", "wiz"]
---

Jeff Burt provides an in-depth report on Wiz’s investigation into a major secrets leak in VS Code extension marketplaces, exposing critical risks for Microsoft developers and AI tool users.<!--excerpt_end-->

# VS Code Marketplace Secret Leaks Highlight Risks in Extensions and AI Configurations

**Author: Jeff Burt**

Researchers from cybersecurity firm Wiz recently uncovered a major supply chain risk involving leaked secrets in Visual Studio Code and OpenVSX extension marketplaces. The investigation, conducted in cooperation with Microsoft, found that over 500 VS Code extensions exposed sensitive secrets, including API keys, passwords, and most notably, Azure DevOps Personal Access Tokens (PATs).

## Key Findings

- **More than 100 VS Code extension publishers leaked access tokens** that could have been used to distribute malicious extensions to over 150,000 users.
- **Over 550 unique secrets** were discovered across 500+ extensions, exposing credentials for services like OpenAI, AWS, GitHub, MongoDB, and Azure DevOps.
- Leaks were attributed primarily to publisher errors, such as bundling hidden dotfiles or failing to sanitize hardcoded secrets from packages.
- The Open VSX Marketplace, used by AI-powered VS Code forks, was also affected – with open-vsx.org access tokens leaked for extensions installed over 100,000 times.

## Security Impact for Developers and Organizations

- Attackers could exploit leaked tokens to push malware via extension updates.
- Even theme extensions, typically considered safe, contributed to the vulnerable install base as they are not prevented from including malicious content.
- The investigation found sensitive access tokens belonging not only to individuals but also to large enterprises and vendors, creating organizational risk.

## Microsoft and Wiz’s Response

- Upon notification from Wiz, Microsoft worked for six months to implement improved security in the VS Code Marketplace, including pre-publication scanning for secrets and notifying publishers.
- Users are advised to minimize installed extensions, carefully review extension trust criteria, and be cautious with auto-updating settings that could propagate compromised updates.
- Corporate security teams should maintain an inventory of allowed IDE extensions and favor the VS Code Marketplace, which now has stronger controls than OpenVSX.

## Role of AI and ‘Vibe Coding’ in Security Leaks

- The leak investigation highlighted an increase in AI-related secrets being exposed, a trend linked to the popularity of AI-assisted coding tools and practices like 'vibe coding.'
- Developers integrating AI APIs, such as OpenAI or Gemini, were found to leak credentials in code published to public repositories or extension packages.
- Researchers warn that as AI tooling becomes more accessible, both developers and non-developers risk security missteps if diligence is not maintained.

## Recommendations for the Developer Community

- **Extension Publishers:** Scan extensions for secrets before publication, especially for hidden config files and hardcoded credentials.
- **Developers:** Limit installed extensions to only those necessary; evaluate update policies; trust only reputable sources.
- **Security Teams:** Enforce allowlists and inventories of approved extensions; prioritize VS Code Marketplace over less-regulated sources.

## Conclusion

The findings emphasize that even widely trusted extension marketplaces can be a vector for supply chain attacks, especially as developers increasingly adopt AI-powered development. Continued vigilance, improved marketplace controls, and security best practices are essential to protecting developer and enterprise environments.

For the full technical breakdown and ongoing updates, read the [Wiz research blog](https://www.wiz.io/blog/supply-chain-risk-in-vscode-extension-marketplaces).

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/massive-vs-code-secrets-leak-puts-focus-on-extensions-ai-wiz/)
