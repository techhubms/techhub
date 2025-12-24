---
layout: "post"
title: "Bridging the Gap Between ClickOps and Infrastructure-as-Code in DevOps"
description: "This article by Marcin Wyszynski explores the challenges in balancing rapid, informal infrastructure changes (ClickOps) with the rigor and governance of Infrastructure-as-Code (IaC) and GitOps methodologies. It proposes intent-driven, policy-aware tooling—often powered by AI and LLMs—as a solution for filling the gap between speed and safety, outlining how guardrails and automation can prevent shadow IT while maintaining developer velocity."
author: "Marcin Wyszynski"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/clickops-iac-and-the-excluded-avocado-middle/"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-11-13 14:53:34 +00:00
permalink: "/posts/2025-11-13-Bridging-the-Gap-Between-ClickOps-and-Infrastructure-as-Code-in-DevOps.html"
categories: ["AI", "DevOps"]
tags: ["AI", "Approval Workflows", "Audit Trails", "Automation", "ClickOps", "Cloud Operations", "DevOps", "Drift Detection", "Experimentation", "GitOps", "Governance", "IaC", "Intent Driven Infrastructure", "LLMs", "MCP", "OpenTofu", "Policy as Code", "Posts", "Pulumi", "Session Locking", "Shadow IT", "Social Facebook", "Social LinkedIn", "Social X", "Techstrong Council", "Terraform"]
tags_normalized: ["ai", "approval workflows", "audit trails", "automation", "clickops", "cloud operations", "devops", "drift detection", "experimentation", "gitops", "governance", "iac", "intent driven infrastructure", "llms", "mcp", "opentofu", "policy as code", "posts", "pulumi", "session locking", "shadow it", "social facebook", "social linkedin", "social x", "techstrong council", "terraform"]
---

Marcin Wyszynski discusses the gap between ClickOps and Infrastructure-as-Code, highlighting how intent-driven, AI-powered approaches can balance developer speed with operational safety in modern DevOps practices.<!--excerpt_end-->

# Bridging the Gap Between ClickOps and Infrastructure-as-Code in DevOps

## Introduction

Marcin Wyszynski explores the evolving landscape of infrastructure management in modern DevOps, focusing on the tension between ClickOps—a fast, informal cloud provisioning style—and the disciplined approaches of Infrastructure-as-Code (IaC) and GitOps.

## ClickOps Versus IaC: The Problem

- **ClickOps**: Quick, manual changes in cloud consoles; fast but untracked, ungoverned, and risky.
- **Infrastructure-as-Code (IaC)**: Automated, policy-enforced deployments using tools like Terraform, Pulumi, and OpenTofu; offers version control, repeatability, and audit trails.
- **GitOps**: Extends IaC with automation and governance through source control pipelines and approvals.

While IaC and GitOps have brought necessary order, their rigor can slow experimentation, driving developers back to ClickOps, resulting in governance gaps, technical debt, and increased security risks.

## The Deployment Gap

Wyszynski identifies the "deployment gap"—the space between efficient but unmanaged ClickOps and rigorous but slow IaC/GitOps—for temporary or experimental work. This gap leads to 'shadow IT' and orphaned resources, making it harder to balance rapid iteration with operational discipline.

## Towards Intent-Driven Infrastructure

**Intent-driven infrastructure** leverages AI (like large language models, LLMs) to translate natural language intent into safe, auditable cloud changes. Instead of writing complex Terraform configurations for a short-lived dev environment, developers "say" what they need, and the system auto-generates the setup while retaining audit, policy, and state.

### Example Workflow

1. **Prototype**: Request environment with plain language (e.g., "t3.micro instance with Ubuntu").
2. **Experiment**: Iterative changes tracked, policy enforced.
3. **Promote**: Export to IaC when ready for production, integrating with GitOps CI/CD.
4. **Clean Up**: Automatic or manual destruction of ephemeral resources.

## Guardrails versus Gates

Intent-driven systems use guardrails—not rigid gates—to balance speed and safety:

- Policy enforcement (e.g., no public EC2s or open S3 buckets)
- Session locking to avoid conflict
- Approval prompts for destructive actions
- Drift detection and audit logs

These measures prevent risky changes without hampering developer flow.

## Drivers for Change

- **AI/LLM advancements**: Accelerating feedback loops in development and now infrastructure.
- **Organizational need**: Pressure to balance agility and compliance, as security teams seek control without halting experimentation.

## Reality Check and Conclusion

Not every environment is production; most require speed and flexibility. Heavy processes push engineers toward ClickOps. IaC isn't the final form—intent-driven models can automate and govern without demanding complex config, reducing the reasons to "write more YAML." The future is more human-centric, collaborative, and adaptive.

## Key Takeaways

- Balance speed and safety with policy-aware automation.
- Use intent-driven infrastructure for ephemeral needs, IaC for durable systems.
- AI helps bridge the deployment gap and aligns infrastructure management with modern development velocity.

> "Infrastructure-as-Code gave us order. Intent-driven models will give us balance." —Marcin Wyszynski

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/clickops-iac-and-the-excluded-avocado-middle/)
