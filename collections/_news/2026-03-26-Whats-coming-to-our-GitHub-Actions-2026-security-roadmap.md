---
external_url: https://github.blog/news-insights/product-news/whats-coming-to-our-github-actions-2026-security-roadmap/
section_names:
- azure
- devops
- security
date: 2026-03-26 16:49:14 +00:00
primary_section: azure
title: What’s coming to our GitHub Actions 2026 security roadmap
feed_name: The GitHub Blog
author: Greg Ose
tags:
- Actions Data Stream
- Amazon S3
- Azure
- Azure Data Explorer
- Azure Event Hubs
- CI/CD
- Commit SHA
- Dependency Pinning
- DevOps
- Egress Firewall
- GitHub Actions
- GitHub Hosted Runners
- GitHub Rulesets
- Immutable Releases
- Least Privilege
- News
- News & Insights
- Policy Driven Execution
- Product
- Pull Request Target
- Reusable Workflows
- Scoped Secrets
- Secret Governance
- Security
- Software Supply Chain Security
- Supply Chain Security
- Telemetry
- Workflow Dependency Locking
- Workflow Triggers
---

Greg Ose outlines GitHub Actions’ 2026 security roadmap, covering dependency locking for reproducible workflows, centralized policy controls for workflow execution and secrets, plus new runner telemetry and egress controls to reduce CI/CD supply chain risk.<!--excerpt_end-->

## Why this matters right now

Software supply chain attacks are increasingly targeting CI/CD automation itself (examples cited: **tj-actions/changed-files**, **Nx**, **trivy-action**). The described pattern:

- Vulnerabilities allow untrusted code execution
- Malicious workflows run without enough observability or control
- Compromised dependencies spread across many repositories
- Over-permissioned credentials get exfiltrated via unrestricted network access

GitHub’s stated goal is to close the gap between how easy these issues are to introduce and how hard they are to detect.

## What GitHub is building for 2026

The roadmap focuses on securing GitHub Actions across three layers:

1. **Ecosystem**: deterministic dependencies and more secure publishing
2. **Attack surface**: policies, secure defaults, and scoped credentials
3. **Infrastructure**: real-time observability and enforceable network boundaries for CI/CD runners

## 1) Building a more secure Actions ecosystem

### The current challenge

Action dependencies are not deterministic because workflow dependencies are resolved at runtime. Workflows can reference actions using mutable refs (tags/branches), so what runs in CI is not always fixed or auditable.

Using immutable commit SHAs helps, but is hard to manage at scale, and transitive dependencies can remain opaque.

### What’s changing: workflow-level dependency locking

GitHub plans to introduce a `dependencies:` section in workflow YAML to lock **direct and transitive** dependencies using commit SHAs and cryptographic hashes.

The post compares the approach to Go’s `go.mod + go.sum`, aiming for reproducibility and auditability.

![Example workflow YAML showing the dependencies section with cryptographic hashes commit SHAs for each action](https://github.blog/wp-content/uploads/2026/03/Screenshot-2026-03-26-at-12.34.47-PM.png?resize=1070%2C611)

**What this changes in practice**

- **Deterministic runs:** Every workflow executes exactly what was reviewed.
- **Reviewable updates:** Dependency changes show up as diffs in pull requests.
- **Fail-fast verification:** Hash mismatches stop execution before jobs run.
- **Full visibility:** Composite actions no longer hide nested dependencies.

**Expected workflow author experience**

- Resolve dependencies via GitHub CLI
- Commit the generated lock data into your workflow
- Update by re-running resolution and reviewing diffs

**Milestones**

| Phase | Target |
| --- | --- |
| Public preview | 3-6 months |
| General availability | 6 months |

### Future: hardened publishing with immutable releases

On the publishing side, GitHub describes moving toward [immutable releases](https://docs.github.com/en/code-security/concepts/supply-chain-security/immutable-releases) with stricter release requirements.

Stated goals:

- Make it clearer how and when code enters the Actions ecosystem
- Create a central enforcement point for detecting and blocking malicious code

## 2) Reducing attack surface with secure defaults

### The current challenge

GitHub Actions workflows can run:

- In response to many events
- Triggered by various actors
- With varying permissions

At scale, GitHub highlights that this flexibility can lead to over-permissioned workflows, unclear trust boundaries, and configurations that are easy to get wrong. The post cites the [Pwn Requests](https://securitylab.github.com/resources/github-actions-preventing-pwn-requests/) class of issues as an example.

### What’s changing: policy-driven execution

GitHub plans workflow execution protections built on GitHub’s [ruleset framework](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/about-rulesets).

Instead of reasoning per YAML file, organizations would define centralized policies controlling:

- Who can trigger workflows
- Which events are allowed

**Core policy dimensions**

- **Actor rules**: who can trigger workflows (users, roles like repo admins, or trusted automation like GitHub Apps, GitHub Copilot, or Dependabot)
- **Event rules**: which events are permitted (for example `push`, `pull_request`, `workflow_dispatch`, etc.)

**Examples given**

- Restrict `workflow_dispatch` to maintainers for sensitive deployment/release workflows.
- Prohibit `pull_request_target` and only allow `pull_request` so workflows triggered by external contributions run without access to repo secrets or write permissions.

### Safe rollout: evaluate mode

Workflow execution rules will support an “evaluate mode” where rules are not enforced, but runs that would have been blocked are surfaced in policy insights.

**Milestones**

| Phase | Target |
| --- | --- |
| Public preview | 3-6 months |
| General availability | 6 months |

## Scoped secrets and improved secret governance

### The current challenge

Secrets are currently scoped at repo/org level, which can be risky with reusable workflows where credentials can flow broadly.

### What’s changing: scoped secrets

Scoped secrets are intended to bind credentials to explicit execution contexts. Secrets can be scoped to:

- Specific repositories or organizations
- Branches or environments
- Workflow identities or paths
- Trusted reusable workflows (without callers passing secrets explicitly)

**What this changes**

- Secrets are no longer implicitly inherited
- Access requires matching an explicit execution context
- Modified or unexpected workflows won’t receive credentials

### Reusable workflow secret inheritance

With scoped secrets:

- Secrets are bound directly to trusted workflows
- Callers don’t automatically pass credentials
- Trust boundaries are explicit

### Permission model changes for Action Secrets

GitHub describes separating code contributions from credential management:

- Write access will no longer grant secret management permissions.
- Secret management will be available via a dedicated custom role and remain part of admin roles.

This is positioned as moving toward least privilege by default.

**Milestones**

| Capability | Phase | Target |
| --- | --- | --- |
| [Scoped secrets & reusable workflow inheritance](https://github.com/github/releases/issues/8150) | Public preview | 3-6 months |
| Scoped secrets & reusable workflow inheritance | GA | 6 months |
| [Secrets permission](https://github.com/github/releases/issues/8153) | GA | 3-6 months |

### Longer-term goal: a unified policy-first security model

GitHub’s stated direction is fewer implicit behaviors, fewer per-workflow configurations, and more centralized, enforceable policy—expanding coverage, adding richer approval/attestation gates, and consolidating controls.

## 3) Endpoint monitoring and control for CI/CD infrastructure

### The current challenge

GitHub Actions runners execute untrusted code, handle sensitive credentials, and interact with external systems, but historically:

- Visibility is limited
- Controls are minimal
- Investigation is reactive

### What’s changing: Actions Data Stream (visibility) and native egress firewall (control)

#### Increased visibility with Actions Data Stream

The Actions Data Stream is intended to provide:

- Near real-time execution telemetry
- Centralized delivery to existing systems

**Supported destinations**

- Amazon S3
- Azure Event Hub / Azure Data Explorer

Events are delivered in batches with at-least-once delivery guarantees and a common schema for indexing/correlation.

**What you can observe**

- Workflow and job execution details across repos/orgs
- Dependency resolution and action usage patterns
- (Future) Network activity and policy enforcement outcomes

**Milestones**

| Phase | Target |
| --- | --- |
| Public preview | 3-6 months |
| General availability | 6-9 months |

#### Native egress firewall for GitHub-hosted runners

GitHub-hosted runners currently allow unrestricted outbound network access, which enables easy exfiltration and unclear distinctions between expected vs unexpected traffic.

GitHub plans a native egress firewall operating outside the runner VM at **Layer 7**, intended to remain immutable even if an attacker gains root inside the runner.

Organizations would define egress policies including:

- Allowed domains and IP ranges
- Permitted HTTP methods
- TLS and protocol requirements

**Two modes**

1. **Monitor:** audit and correlate outbound requests to workflow run/job/step/command.
2. **Enforce:** block traffic not explicitly permitted.

**Milestones**

| Phase | Target |
| --- | --- |
| Public preview | 6-9 months |

### Future goal: runners as protected endpoints

GitHub describes expanding toward:

- Process-level visibility
- File system monitoring
- Richer execution signals
- Near real-time enforcement

## Summary of intended outcomes

The post summarizes the direction as “secure-by-default, verifiable automation,” aiming for:

- Deterministic and reviewable workflows
- Explicitly scoped secrets (no broad inheritance)
- Policy-governed execution (not YAML alone)
- Observable and controllable runners

## Get started / discuss

- Join the discussion in the GitHub community: https://github.com/orgs/community/discussions/190621


[Read the entire article](https://github.blog/news-insights/product-news/whats-coming-to-our-github-actions-2026-security-roadmap/)

