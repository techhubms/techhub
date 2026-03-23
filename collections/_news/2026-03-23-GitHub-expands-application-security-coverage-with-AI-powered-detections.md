---
tags:
- AI
- AI Powered Detections
- Application Security
- Bash
- Code Scanning
- Code Security
- CodeQL
- Copilot Autofix
- DevOps
- Dockerfile
- GitHub Advanced Security
- GitHub Code Security
- HCL
- Insecure Cryptographic Algorithms
- News
- PHP
- Policy Enforcement
- Pull Requests
- Remediation
- Security
- Shell
- SQL Injection
- Static Analysis
- Terraform
- Vulnerability Detection
date: 2026-03-23 16:00:00 +00:00
primary_section: ai
external_url: https://github.blog/security/application-security/github-expands-application-security-coverage-with-ai-powered-detections/
title: GitHub expands application security coverage with AI-powered detections
author: Marcelo Oliveira
section_names:
- ai
- devops
- security
feed_name: The GitHub Blog
---

Marcelo Oliveira explains how GitHub Code Security is combining CodeQL static analysis with AI-powered detections to find more vulnerabilities across languages and frameworks, and how results (and Copilot Autofix suggestions) show up directly in pull requests.<!--excerpt_end-->

# GitHub expands application security coverage with AI-powered detections

AI is speeding up software delivery and increasing the mix of languages and frameworks inside modern repos. That creates a gap for security teams: traditional static analysis is strong for supported languages, but real-world codebases also contain scripts, infrastructure-as-code, and components in ecosystems that are harder to cover.

GitHub is introducing **AI-powered security detections** in **GitHub Code Security** (public preview planned for **early Q2**) to expand coverage across more languages and frameworks. These detections are designed to **complement CodeQL**, not replace it.

## Expanding application security coverage with static analysis and AI

- **CodeQL** remains the foundation for deep semantic static analysis in supported languages.
- GitHub is adding a **hybrid detection model**:
  - CodeQL for static analysis where it’s strongest
  - AI-powered detections to surface potential vulnerabilities in areas that are difficult to support with static analysis alone
- Findings (and suggested fixes) are intended to show up **directly in the pull request workflow**.

### Early results GitHub shared

- In internal testing, GitHub processed **170,000+ findings** over **30 days**.
- GitHub reports **80%+ positive developer feedback**.
- Ecosystems highlighted as newly supported via AI-powered detections include:
  - **Shell/Bash**
  - **Dockerfiles**
  - **Terraform configurations (HCL)**
  - **PHP**

GitHub positions this work as part of a broader **agentic detection platform** that also targets security, code quality, and code review across the developer workflow.

## Bringing expanded security coverage into pull requests

GitHub is focusing on pull requests as the best place to surface issues early:

- When a pull request is opened, **GitHub Code Security analyzes changes** using:
  - **CodeQL static analysis**, or
  - **AI-powered security detections**, depending on what’s most appropriate
- Results appear **in the pull request**, alongside other code scanning findings.

Examples of risks GitHub says can be surfaced:

- Unsafe, string-built **SQL queries or commands**
- **Insecure cryptographic algorithms**
- **Infrastructure configurations** that expose sensitive resources

## Turning detections into fixes with Copilot Autofix

GitHub connects detection to remediation using **Copilot Autofix**, which can suggest fixes developers can:

- Review
- Test
- Apply as part of normal code review

GitHub’s reported usage stats for 2025:

- **460,000+ security alerts fixed** using Autofix
- Average time to resolution:
  - **0.66 hours** with Autofix
  - **1.29 hours** without Autofix

## Enforce security outcomes at the point of merge

GitHub argues that, because it sits at the merge point of the workflow, teams can enforce security outcomes in the same place code is reviewed and approved:

- Detection in PRs
- Remediation suggestions (Autofix)
- Policy enforcement tied to merges

GitHub also notes it will preview these AI-powered detections at RSAC.

## Source

- GitHub Blog: GitHub expands application security coverage with AI-powered detections (Marcelo Oliveira)
- RSAC link mentioned in the post: https://github.com/resources/events/github-rsac2026


[Read the entire article](https://github.blog/security/application-security/github-expands-application-security-coverage-with-ai-powered-detections/)

