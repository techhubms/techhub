---
layout: "post"
title: "Build vs. Buy: What it Really Takes to Harden Your Software Supply Chain"
description: "This article by Matt Moore dives into the trade-offs engineering teams face when deciding between building custom-hardened container images or purchasing a supply chain security solution. It discusses challenges around automation, continuous maintenance, testing, and secure delivery when protecting modern CI/CD pipelines and cloud-native applications from supply chain attacks."
author: "Matt Moore"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/build-vs-buy-what-it-really-takes-to-harden-your-software-supply-chain/"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-10-03 06:30:32 +00:00
permalink: "/2025-10-03-Build-vs-Buy-What-it-Really-Takes-to-Harden-Your-Software-Supply-Chain.html"
categories: ["DevOps", "Security"]
tags: ["Access Control", "Automation", "Best Practices", "Build Pipelines", "Business Of DevOps", "CI/CD", "CI/CD Security", "Container Registry", "Container Registry Security", "Container Security", "Container Vulnerabilities", "Contributed Content", "DevOps", "Devsecops", "DockerHub", "Hardened Images", "Integration Testing", "Kubernetes", "Kubernetes Security", "Open Source Vulnerabilities", "Posts", "Secure Build Pipelines", "Security", "Social Facebook", "Social LinkedIn", "Social X", "Software Distribution", "Software Maintenance", "Software Supply Chain Security", "SolarWinds", "Supply Chain Security", "Threat Mitigation"]
tags_normalized: ["access control", "automation", "best practices", "build pipelines", "business of devops", "cislashcd", "cislashcd security", "container registry", "container registry security", "container security", "container vulnerabilities", "contributed content", "devops", "devsecops", "dockerhub", "hardened images", "integration testing", "kubernetes", "kubernetes security", "open source vulnerabilities", "posts", "secure build pipelines", "security", "social facebook", "social linkedin", "social x", "software distribution", "software maintenance", "software supply chain security", "solarwinds", "supply chain security", "threat mitigation"]
---

Matt Moore explores what it takes to secure your software supply chain, covering the difficult questions facing engineering teams around building or buying hardened images, maintenance, automation pitfalls, and supply chain attack risks.<!--excerpt_end-->

# Build vs. Buy: What it Really Takes to Harden Your Software Supply Chain

**Author: Matt Moore**

Securing the software supply chain has become one of the top priorities for modern engineering teams, especially in the wake of high-profile attacks like the SolarWinds breach. This article examines the true costs and complexities behind the decision to either build your own hardened container images or purchase supply chain security products.

## Why Supply Chain Security Matters

- Software build pipelines are now prime targets for attackers seeking to compromise organizations at scale.
- Incidents such as the SolarWinds attack and a 2024 DockerHub breach have shown that tampering with build systems or image distribution channels can have wide-reaching and damaging effects.

## The DIY (Build) Approach: More Than Meets the Eye

At the outset, building your own images may sound straightforward:

- Start with a base image
- Patch vulnerabilities
- Add automation for ongoing updates

**Hidden Challenges:**

- Upstream distributions of images are often out of date, so teams end up applying superficial, last-mile fixes.
- Managing vulnerabilities, alerts, and regulatory requirements becomes an ongoing effort.
- Statistics show 90% of codebases contain open source components lagging many versions behind.
- Larger organizations may need whole teams dedicated to tracking security issues and maintaining compliance.

## The Ongoing Burden of Maintenance

- Developers on average spend ~6 hours/week just sourcing and updating open-source packages.
- Keeping images patched, tested, and validated is a persistent engineering duty, not a one-time event.
- Without dedicated ownership, even well-built images can become outdated liabilities.

## Automation: Necessary but Not Sufficient

- Automation tools can open pull requests and patch known vulnerabilities but can break if upstream changes introduce API or behavior shifts.
- Example: A change in Kubernetes' client-go library caused major refactoring for downstream projects when new context parameters were required.
- Teams need deep understanding of automation, build tooling, and upstream communities to prevent automated updates from causing issues downstream.

## Testing and Validation are Critical

- Beyond unit tests, hardened images require full integration testing, sometimes in local Kubernetes clusters or actual cloud deployments.
- Complex setups (e.g., images needing GPU resources) can slow down and complicate testing.
- Without thorough, real-world tests, subtle incompatibilities may be missed, introducing risks into production.

## Secure Distribution: The Last Mile

- Even the most secure image is a risk if it's distributed improperly.
- Case in point: A compromised DockerHub token led to the distribution of a malicious Kong Ingress Controller image, highlighting the need for auditable and revocable access controls.

## Conclusion: Hardening is a Practice, Not Just a Feature

- Building your own hardened images provides more control but comes with substantial, ongoing maintenance costs and responsibilities.
- Engineering organizations must honestly assess their capacity to support this practice over the long term.
- Supply chain security isn't something you can "set and forget"—it's a continuous engineering commitment, especially as threats become more sophisticated.

## Further Reading

- [Reflections on Trusting Trust (Ken Thompson)](https://www.cs.cmu.edu/~rdriley/487/papers/Thompson_1984_ReflectionsonTrustingTrust.pdf)
- [SolarWinds Hack Explained](https://www.techtarget.com/whatis/feature/SolarWinds-hack-explained-Everything-you-need-to-know)
- [Checkmarx Open Source Security Report](https://www.blackduck.com/content/dam/black-duck/en-us/reports/rep-ossra.pdf)
- [IDC Developer Security Report](https://media.jfrog.com/wp-content/uploads/2024/09/24210102/idc-report-asset.pdf)

---

Whether your organization chooses to build or buy, a clear-eyed understanding of the true scope of work is critical to effective supply chain security.

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/build-vs-buy-what-it-really-takes-to-harden-your-software-supply-chain/)
