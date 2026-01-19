---
layout: post
title: 'Securing the Open Source Supply Chain: Impact of the GitHub Secure Open Source Fund'
author: Kevin Crosby
canonical_url: https://github.blog/open-source/maintainers/securing-the-supply-chain-at-scale-starting-with-71-important-open-source-projects/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-08-11 16:00:00 +00:00
permalink: /ai/news/Securing-the-Open-Source-Supply-Chain-Impact-of-the-GitHub-Secure-Open-Source-Fund
tags:
- AI Security
- AutoGPT
- CodeQL
- Dependency Management
- DevOps Tooling
- Fuzz Testing
- GitHub Secure Open Source Fund
- GitHub Security Lab
- Incident Response
- Log4j
- Maintainers
- Microsoft
- Ollama
- Open Source
- Open Source Security
- Secret Scanning
- Secure Development
- Security Best Practices
- Supply Chain Security
- Threat Modeling
- Vulnerability Remediation
section_names:
- ai
- devops
- security
---
Authored by Kevin Crosby, this article highlights how 71 critical open source projects—supported by the GitHub Secure Open Source Fund and partners like Microsoft—improved their security posture with tangible results and shared strategies.<!--excerpt_end-->

# Securing the Supply Chain at Scale: Insights from the GitHub Secure Open Source Fund

*By Kevin Crosby*

## Introduction

When the Log4j zero day surfaced in December 2021, it underscored the fragility of today’s software supply chain—where small, under-resourced dependencies can expose millions of downstream users. With most cloud workloads relying on hundreds of open source packages, supporting and securing this ecosystem has become urgent.

GitHub’s Secure Open Source Fund, launched in November 2024, directly invests in maintainers with a structured three-week program offering security education, certifications, tooling, and community mentorship. The goal: measurably increase open source security by tying funding to real-world, programmatic security outcomes.

## Tangible Results and Early Outcomes

- **1,100+ vulnerabilities remediated using CodeQL**
- **More than 50 new CVEs created and published**
- **92 secrets prevented from leaking; 176 existing leaks resolved**
- **All participants reported actionable next steps for future security**
- **80% of projects activated three or more GitHub security features**
- **63% improved knowledge of AI and MCP (Microsoft Cloud Platform) security**

Participants leveraged AI—including GitHub Copilot—to run vulnerability scans, design fuzzing strategies, and automate security audits, accelerating improvements and sharing lessons within their communities.

## Category Spotlights

### AI and ML Frameworks / Edge LLM Tooling

Projects such as Ollama, AutoGPT/Gravitasml, scikit-learn, OpenCV, and others are now foundational to AI and LLM work. With millions of installations, these projects integrated advanced security— from threat modeling and pruning dependencies to continuous integration of CodeQL and secret scanning. Notably, maintainers used GitHub Copilot to set up fuzzing for libraries like shadcn/ui.

### Front-End, Full-Stack, and UI Libraries

Frameworks like Next.js, Svelte, and shadcn/ui drive modern user interfaces. Participants implemented formal security policies, enabled CodeQL scanning, and designed robust incident-response and reporting workflows, often using GitHub security automation.

### Web Servers, Networking, and Gateways

Projects such as Node.js, Express, and Fastify focused on revising threat models, automating code scanning with CodeQL, and strengthening release artifact signatures, benefiting millions of applications.

### DevOps, Build Systems, and Containers

Tools such as Turborepo and Flux emphasized supply chain resilience—enabling private vulnerability reporting, least-privilege workflows, and public threat models, supported by GitHub’s CI/CD infrastructure.

### Security Frameworks and Identity Tools

Libraries like Log4j, CycloneDX, OAuthlib, and Zitadel implemented new threat models, collaborated with security teams, and distributed reusable security auditing packs.

### Developer Utilities, CLI Tools, and Scientific Computing

Projects such as Oh My Zsh, nvm, JUnit, and Matplotlib documented incident response plans, enabled multi-factor authentication, and wired up CodeQL and fuzzing harnesses, making improvements that propagate downstream.

## Broad Patterns & Key Learnings

- **Timeboxing and focused curriculum** drove high engagement and impact.
- **Hands-on, interactive security themes** with mentors promoted practical adoption.
- **Community and funding amplify cultural change,** helping maintainers share forkable security playbooks and incident plans far beyond their own projects.
- **Ecosystem impact is magnified** by public commitments—over $1.38M and major backing from partners like Microsoft, Mozilla, OpenSSF, and others.

## Conclusion & Next Steps

The Secure Open Source Fund made immediate, broad improvements in critical projects’ security—protecting millions of builds each day from future supply chain threats. Microsoft’s involvement is specifically referenced as a supporting partner and advocate for programs like threat modeling, SBOM, and incident response.

**Maintainers, funders, and ecosystem partners are encouraged to join ongoing sessions, share their stories, and help secure our digital infrastructure for all.**

---

**For more details or to participate, see:** [GitHub Secure Open Source Fund](https://resources.github.com/github-secure-open-source-fund/)

*Funding and ecosystem partners listed: Microsoft for Startups, Mozilla, OpenSSF, and many others.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/open-source/maintainers/securing-the-supply-chain-at-scale-starting-with-71-important-open-source-projects/)
