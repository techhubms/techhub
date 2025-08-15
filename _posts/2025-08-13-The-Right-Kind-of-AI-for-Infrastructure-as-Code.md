---
layout: "post"
title: "The Right Kind of AI for Infrastructure as Code"
description: "This article by Ian Amit explores the practical applications of AI in cloud security, with a focus on Infrastructure as Code (IaC) environments. It discusses the spectrum of AI tools in DevOps, the challenges of alert overload, and the need for AI systems that deliver policy-aligned, actionable, and merge-ready fixes in security and platform engineering workflows."
author: "Ian Amit"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/the-right-kind-of-ai-for-infrastructure-as-code/?utm_source=rss&utm_medium=rss&utm_campaign=the-right-kind-of-ai-for-infrastructure-as-code"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-08-13 16:33:28 +00:00
permalink: "/2025-08-13-The-Right-Kind-of-AI-for-Infrastructure-as-Code.html"
categories: ["AI", "DevOps", "Security"]
tags: ["Agentic AI", "AI", "AI Powered Security", "Automation", "CIS", "Cloud", "Cloud Security", "Contributed Content", "DevOps", "IaC", "Large Language Models", "NIST", "Platform Engineering", "Policy as Code", "Posts", "Pull Requests", "Remediation", "Security", "Security Alerts", "Security Automation", "Security Compliance", "Social Facebook", "Social LinkedIn", "Social X", "Standards Alignment"]
tags_normalized: ["agentic ai", "ai", "ai powered security", "automation", "cis", "cloud", "cloud security", "contributed content", "devops", "iac", "large language models", "nist", "platform engineering", "policy as code", "posts", "pull requests", "remediation", "security", "security alerts", "security automation", "security compliance", "social facebook", "social linkedin", "social x", "standards alignment"]
---

Ian Amit examines how teams can leverage AI to move beyond alert management toward automated, standards-aligned remediation in infrastructure as code and cloud security contexts.<!--excerpt_end-->

# The Right Kind of AI for Infrastructure as Code

*By Ian Amit*

AI is rapidly permeating cloud security, with many tools claiming to be “AI-powered,” and copilots or chatbots offering to interpret a flood of alerts. However, as Ian Amit argues, real value lies not in understanding the problem but in reliably resolving it. For most platform teams, the true bottleneck is remediation—resolving security findings as fast as they arise in fast-changing cloud environments.

## Where AI Fits in Infrastructure Security

There’s a spectrum of approaches to integrating AI into infrastructure security:

- **Automation tools** like scripts, rules, policy-as-code, and guardrails address misconfigurations but still need manual oversight and intervention.
- **AI copilots** (based on large language models) assist with summarization, code generation (“vibe coding”), and explanations, but often fail to produce actionable or production-ready fixes and may introduce new security gaps.
- **Agentic AI systems** do lightweight remediation tasks—such as isolating a resource or updating tags—but lack context for safe, scalable actions.
- **Fix engines** are built to apply trusted, standards-aligned solutions (such as generating merge-ready pull requests) that engineers can review and deploy with confidence, closing the loop between detection and resolution.

## AI's Limitations in Infrastructure Contexts

Most current AI tooling in IaC security falls short because:

- Lacks architectural intent awareness, potentially breaking downstream resources.
- Often ignores compliance with frameworks like CIS or NIST.
- Outputs may contain hallucinations or insecure fixes, increasing the manual burden on engineering teams.
- Lacks determinism and traceability, shifting workloads rather than streamlining them.

## What Makes Effective AI in Cloud Security

The ideal AI system for cloud security and IaC should:

- Integrate smoothly into existing pipelines without disrupting workflows.
- Be context-aware, grasping both code syntax and the resource relationships within infrastructure.
- Apply policy and compliance standards (e.g., CIS, NIST) directly and generate fixes that can withstand audits.
- Deliver merge-ready, reviewable pull requests instead of just dashboard suggestions.
- Offer transparency, explaining changes and mapping them to organizational policy.

## Closing the Remediation Gap

Current security tools excel at surfacing issues—alerting to misconfigurations, risks, and policy violations. However, this often leaves engineering teams mired in alerts and manual ticket resolution.

AI’s greatest benefit arrives when it acts directly in the development workflow to automate well-scoped remediation: taking action where code is reviewed, tested, and shipped—automatically, safely, and in a way that remains reviewable.

## What Matters Most: Security That Ships

The complexity of an AI model, or its ability to explain issues, is secondary to whether it can help resolve them efficiently and safely. Effective AI empowers engineers to ship secure code faster, integrating with proven practices and automating wherever possible without sacrificing control or visibility.

---

*For further reading:* [Sonar Surfaces Multiple Caveats When Relying on LLMs to Write Code](https://devops.com/sonar-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/)

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/the-right-kind-of-ai-for-infrastructure-as-code/?utm_source=rss&utm_medium=rss&utm_campaign=the-right-kind-of-ai-for-infrastructure-as-code)
