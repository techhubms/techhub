---
layout: "post"
title: "The Right Kind of AI for Infrastructure as Code"
description: "This article by Ian Amit discusses the evolving landscape of AI in cloud infrastructure security, focusing on the distinction between AI-powered tools that simply identify issues and those that provide automated, standards-aligned resolution. Amit breaks down the 'AI spectrum' for Infrastructure as Code (IaC), analyzing where current AI solutions excel, why many fall short, and what engineering teams should look for in remediation tools that fit real-world DevOps workflows."
author: "Ian Amit"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/the-right-kind-of-ai-for-infrastructure-as-code/?utm_source=rss&utm_medium=rss&utm_campaign=the-right-kind-of-ai-for-infrastructure-as-code"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-08-13 16:33:28 +00:00
permalink: "/posts/2025-08-13-The-Right-Kind-of-AI-for-Infrastructure-as-Code.html"
categories: ["AI", "DevOps", "Security"]
tags: ["Agentic AI", "AI", "AI in DevOps", "Alert Fatigue", "CIS", "Cloud", "Cloud Compliance", "Cloud Security", "Contributed Content", "DevOps", "DevOps Automation", "IaC", "Merge Requests", "NIST", "Platform Engineering", "Policy as Code", "Posts", "Remediation Tools", "Security", "Security Automation", "Security Remediation", "Social Facebook", "Social LinkedIn", "Social X"]
tags_normalized: ["agentic ai", "ai", "ai in devops", "alert fatigue", "cis", "cloud", "cloud compliance", "cloud security", "contributed content", "devops", "devops automation", "iac", "merge requests", "nist", "platform engineering", "policy as code", "posts", "remediation tools", "security", "security automation", "security remediation", "social facebook", "social linkedin", "social x"]
---

Ian Amit explores how AI can be effectively applied to Infrastructure as Code, highlighting crucial gaps in current cloud security tools and explaining what attributes make AI truly valuable for DevOps and security teams.<!--excerpt_end-->

# The Right Kind of AI for Infrastructure as Code

*By Ian Amit*

AI is heavily marketed in cloud security, promising everything from automated copilots to advanced chatbots aimed at assisting teams with issue interpretation. But as Ian Amit points out, for most platform and DevOps teams, surfacing problems isn't the bottleneck—efficient, safe, and automated resolution is.

## The Challenge of Resolution in Cloud Security

Modern cloud environments are dynamic, leading to a persistent backlog of security findings. While tools increasingly alert teams to risks and policy violations, the core challenge becomes closing the gap between detection and resolution. Platform teams need AI that can move beyond summarization to deliver actionable code-level remediation that fits their workflow.

## Spectrum of AI in Infrastructure Security

Amit outlines a range of AI approaches being adopted in infrastructure:

- **Traditional Automation:** Relies on scripted rules and policy-as-code frameworks, effective for enforcing guardrails but still needs manual intervention when environments drift or policies change.
- **Copilot-style LLMs:** AI-powered assistants that generate code from descriptions or explain misconfigurations. Useful for prototyping, but tend to create security risks, inaccuracies, and lack truly production-ready fixes.
- **Agentic AI:** Takes limited automated actions—like isolating a resource or tagging an issue—but often lacks sufficient context to remediate issues safely at scale.
- **Fix Engines:** The most advanced, these tools interpret infrastructure, align changes with organizational policy (CIS, NIST), and generate merge-ready pull requests that are reviewable and auditable. These help teams actually ship code that is both secure and standards-compliant.

## Why Most AI Falls Short for Infrastructure

Amit argues that many AI tools lack context awareness and fail to understand architectural intent, leading to unsafe or non-compliant fixes. Reliance on public datasets, probabilistic outputs, and lack of traceability means that engineers are often left reviewing, revising, or ignoring AI suggestions instead of saving time.

Key issues include:

- Insufficient alignment with compliance frameworks (CIS, NIST)
- Fixes that are not context- or architecture-aware
- Lack of explainability and transparency

## What Effective AI Looks Like for Cloud Teams

The right AI approach should be:

- **Context-Aware:** Understand resource dependencies and configuration intent
- **Standards-Aligned:** Directly encode compliance policies into fixes
- **Merge-Ready:** Provide actionable, auditable pull requests
- **Transparent:** Ensure every change is traceable and explainable
- **Non-Disruptive:** Integrate with existing development workflows

Meeting these criteria, AI can support rapid, secure delivery pipelines without requiring engineers to change how they work or compromise on trust.

## Closing the Loop: From Alert to Fix

The article stresses that true value comes from AI that closes the remediation loop—not just alerting on issues but fixing them safely inside the dev pipeline. AI should move issues from dashboards to deployment quickly and transparently, reducing manual review cycles and ensuring security doesn't slow down software delivery.

## Shipping Security with AI

The most valuable AI tools will not be those with the most sophisticated explanations, but those that close gaps between detection and resolution—helping teams ship secure, standards-compliant code without sacrificing delivery speed or reliability.

---

*Read more from Ian Amit or visit DevOps.com for related articles on AI and cloud infrastructure security.*

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/the-right-kind-of-ai-for-infrastructure-as-code/?utm_source=rss&utm_medium=rss&utm_campaign=the-right-kind-of-ai-for-infrastructure-as-code)
