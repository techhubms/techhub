---
section_names:
- ai
- devops
- github-copilot
- security
external_url: https://github.blog/security/application-security/how-exposed-is-your-code-find-out-in-minutes-for-free/
date: 2026-04-14 15:00:00 +00:00
feed_name: The GitHub Blog
tags:
- AI
- Application Security
- Code Scanning
- Code Security
- Code Security Risk Assessment
- CodeQL
- Copilot Autofix
- DevOps
- GitHub Actions
- GitHub Code Security
- GitHub Copilot
- GitHub Enterprise Cloud
- GitHub Team
- News
- Pull Requests
- Remediation
- Risk Assessment
- Risk Assessment Dashboard
- Secret Protection
- Secret Risk Assessment
- Secret Scanning
- Secure Development Lifecycle
- Security
- Security Alerts
- Security Posture
- Static Analysis
- Vulnerability Management
- Web Application Security
primary_section: github-copilot
title: How exposed is your code? Find out in minutes—for free
author: Dorothy Pearce
---

Dorothy Pearce introduces GitHub’s free Code Security Risk Assessment, a one-click scan that uses CodeQL to surface vulnerabilities across up to 20 active repositories, and explains how the results help teams prioritize remediation (including where Copilot Autofix may apply).<!--excerpt_end-->

# How exposed is your code? Find out in minutes—for free

Most security leaders suspect there are vulnerabilities in their codebase they don’t know about.

A common problem is that most code never gets a thorough security review. Vulnerabilities can build up quietly across repositories, languages, and teams—often unnoticed until something breaks.

GitHub is introducing the **Code Security Risk Assessment**: a **free, one-click scan** intended to reveal vulnerabilities across an organization’s code.

- Docs: [Code Security Risk Assessment](https://docs.github.com/en/code-security/concepts/code-scanning/code-security-risk-assessment)
- Entry point: [Run your free assessment](https://github.com/get_started?with=risk-assessment)

## Who it’s for

- Available to **GitHub organization admins** and **security managers**.
- Runs on:
  - **GitHub Enterprise Cloud**
  - **GitHub Team** plans

The post also aims to be useful for readers who can’t run the scan themselves, by describing what the assessment shows and why it’s worth doing.

## What the assessment scans and reports

The assessment:

- Scans up to **20** of your most active repositories
- Uses **CodeQL** (GitHub’s static analysis engine)
- Produces a dashboard that summarizes:
  - **Total vulnerabilities**, by severity (**critical**, **high**, **medium**, **low**)
  - Vulnerabilities by **language**
  - **Rules detected** (issue classes), including how many repos are affected and severity
  - **Most vulnerable repositories**, to help prioritize remediation
  - **Copilot Autofix eligibility** — how many findings could be auto-remediated using Copilot Autofix

## Cost and usage details

- The assessment is described as **completely free** (no license required).
- It also states that the **GitHub Actions minutes** used for scanning **don’t count against your quota**.

## Completing the security picture (secrets + vulnerabilities)

If you’ve already run GitHub’s [Secret Risk Assessment](https://docs.github.com/code-security/concepts/secret-security/about-secret-security-with-github#secret-risk-assessment), the Code Security Risk Assessment is positioned as the complementary view for source-code vulnerabilities.

The post highlights that:

- **Both assessments run together from a single entry point**.
- There’s a **tabbed interface** to switch between:
  - Secret exposure findings
  - Code vulnerability findings

It also cites GitHub’s 2025 stats for secret scanning outcomes (as reported in the post):

- Customers using [Secret Protection](https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning) scanned nearly **2 billion pushes**
- Blocked **19 million** secret exposures

## From found to fixed (Copilot Autofix + Code Security)

The post emphasizes that identifying issues is only step one; remediation is what reduces risk.

It positions **GitHub Code Security** and **Copilot Autofix** as a way to speed up fixes, and includes reported 2025 metrics across GitHub:

- **460,258 security alerts** were fixed using **Copilot Autofix**
- **50%** of vulnerability alerts were resolved directly in **pull requests**
- Mean time to remediation was nearly **twice as fast** with Copilot Autofix:
  - **0.66 hours** with Autofix
  - **1.29 hours** manually

Your assessment results will show how many of your findings are eligible for Copilot Autofix, and the post says you can enable **Code Security** directly from the results page.

## Links

- Run the assessment: [github.com/get_started?with=risk-assessment](https://github.com/get_started?with=risk-assessment)
- Docs: [Code Security Risk Assessment](https://docs.github.com/en/code-security/concepts/code-scanning/code-security-risk-assessment)
- Secret scanning overview: [About secret scanning](https://docs.github.com/en/code-security/secret-scanning/introduction/about-secret-scanning)
- Code scanning overview: [About code scanning](https://docs.github.com/en/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning)


[Read the entire article](https://github.blog/security/application-security/how-exposed-is-your-code-find-out-in-minutes-for-free/)

