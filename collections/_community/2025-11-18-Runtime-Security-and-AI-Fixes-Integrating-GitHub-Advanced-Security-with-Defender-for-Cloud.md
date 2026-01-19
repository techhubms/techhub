---
layout: post
title: 'Runtime Security and AI Fixes: Integrating GitHub Advanced Security with Defender for Cloud'
author: AndrewMFlick
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/security-where-it-matters-runtime-context-and-ai-fixes-now/ba-p/4470794
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 16:04:11 +00:00
permalink: /github-copilot/community/Runtime-Security-and-AI-Fixes-Integrating-GitHub-Advanced-Security-with-Defender-for-Cloud
tags:
- Agentic Remediation
- AI Suggested Fix
- Alert Fatigue
- Application Security
- Artifact Metadata
- Azure Portal
- Cloud Native Apps
- Code Scanning
- Copilot Autofix
- Dependabot
- DevSecOps
- GitHub Advanced Security
- Microsoft Defender For Cloud
- PR Automation
- Pull Request Automation
- Runtime Security
- Security Campaigns
- Security Integration
- Virtual Registry
- Vulnerability Prioritization
section_names:
- ai
- azure
- devops
- github-copilot
- security
---
AndrewMFlick introduces Microsoft’s integration of GitHub Advanced Security with Defender for Cloud, helping DevSecOps teams accelerate vulnerability remediation using runtime context, AI-suggested fixes, and code-to-cloud security integration.<!--excerpt_end-->

# Runtime Security and AI Fixes: Integrating GitHub Advanced Security with Defender for Cloud

## Introduction

Security teams and developers are overwhelmed by alert fatigue and slow remediation cycles. With attacks occurring every three minutes, teams need to prioritize critical vulnerabilities quickly. Microsoft addresses this with a new integration connecting GitHub Advanced Security's developer-first tools and Microsoft Defender for Cloud’s runtime protection, now enhanced with agentic remediation and available in public preview.

## DevSecOps Challenges

Despite better detection and collaboration in the industry, vulnerability counts continue rising, with slow mean times to remediation (~116 days for critical issues). Teams are distracted by noisy alerts, unclear priorities, and siloed tools, impeding response to real risks and slowing collaboration.

### Core Issues

- Alert fatigue for security teams; distinguishing real risks from noise is difficult.
- Developers lack prioritization, leading to wasted effort on non-exploitable issues.
- Separate toolsets for development and security slow collaboration and leave blind spots.

## The Solution: Context-Aware, AI-Driven Security Integration

### Workflow Overview

- Defender for Cloud detects vulnerabilities in live, internet-exposed APIs.
- GitHub campaign filters, using runtime context, prompt developers to prioritize urgent issues.
- Copilot Autofix suggests AI-powered code changes; developers apply fixes in minutes.
- Bulk assignment of autofixes is supported, with GitHub Copilot coding agents creating draft PRs for multi-merge fixes, ready for human review.

### Virtual Registry: Code-to-Runtime Mapping

- Virtual Registry in GitHub stores artifact metadata.
- Integration with Defender for Cloud enables precise mapping of code vulnerabilities to running production workloads.
- Teams use runtime and repository context to triage alerts, respond to incidents, and answer questions like “Is this vulnerability active in production?”
- New filters for Code Scanning and Security Campaigns leverage artifact metadata, improving prioritization and response.

### Agentic Remediation and PR Automation

- Copilot Autofix provides AI-powered remediation suggestions.
- Developers assign fixes to the Copilot coding agent for easy PR creation and bulk resolution.
- Automation reduces the time spent sorting through alerts and accelerates remediation.

## Key Benefits

- **Reduction in alert fatigue:** Teams focus on exploitable, production risks rather than noise.
- **Faster, AI-powered fixes:** Copilot Autofix resolves up to 50% of alerts in PRs, reducing remediation times by 70%.
- **Enhanced DevSecOps collaboration:** Connected context and automation result in more seamless teamwork. Up to 68% of alerts remediated via security campaigns.
- **Streamlined onboarding:** Setup is straightforward in both Defender for Cloud and GitHub; initial scan results and recommended fixes are available immediately.

## Getting Started

- **Microsoft Defender for Cloud:** Add or update a GitHub connector, grant code scanning consent. Setup is one click if using GitHub.
- **GitHub:** Filter by runtime context, receive AI-driven fixes.
- **Purchasing:** GitHub Advanced Security available as Code Security SKU ($30/committer/mo, April 2025), GHAS Bundle ($49/committer/mo, available now), GitHub Enterprise Cloud, GitHub Copilot. Defender CSPM costs $5/resource/mo; both available via Azure Portal.

## References

- [Software Under Siege | AppSec Threat Report 2025 | Contrast Security](https://www.contrastsecurity.com/software-under-siege-2025-report)
- [Edgescan | Vulnerability Statistics Report 2025](https://www.edgescan.com/stats-report/)
- GitHub Internal Data

---

*Author: AndrewMFlick (Microsoft), posted on Apps on Azure Blog, Nov 18, 2025.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/security-where-it-matters-runtime-context-and-ai-fixes-now/ba-p/4470794)
