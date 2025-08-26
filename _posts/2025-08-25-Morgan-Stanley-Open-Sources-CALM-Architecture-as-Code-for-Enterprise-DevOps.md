---
layout: "post"
title: "Morgan Stanley Open Sources CALM: Architecture as Code for Enterprise DevOps"
description: "This article details Morgan Stanley's release of CALM (Common Architecture Language Model), an open-source Architecture-as-Code solution aimed at automating architectural governance, compliance, and design validation in enterprise DevOps. The piece explains CALM's structure, DevOps integration, impact on deployment cycles, and its potential benefits for organizations beyond financial services."
author: "Tom Smith"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops/?utm_source=rss&utm_medium=rss&utm_campaign=morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-08-25 22:03:46 +00:00
permalink: "/2025-08-25-Morgan-Stanley-Open-Sources-CALM-Architecture-as-Code-for-Enterprise-DevOps.html"
categories: ["DevOps"]
tags: ["Architectural Compliance", "Architecture as Code", "Automation", "CALM", "CI/CD", "Configuration as Code", "DevOps", "Enterprise", "Enterprise Software", "FINOS", "JSON Meta Schema", "Machine Readable Architecture", "Morgan Stanley", "Open Source", "Posts", "Security Approvals", "Social Facebook", "Social LinkedIn", "Social X", "Software Delivery"]
tags_normalized: ["architectural compliance", "architecture as code", "automation", "calm", "cislashcd", "configuration as code", "devops", "enterprise", "enterprise software", "finos", "json meta schema", "machine readable architecture", "morgan stanley", "open source", "posts", "security approvals", "social facebook", "social linkedin", "social x", "software delivery"]
---

Authored by Tom Smith, this article introduces CALM, Morgan Stanley's open-source Architecture-as-Code tool, and explores how it streamlines enterprise DevOps through automation, compliance, and architectural validation.<!--excerpt_end-->

# Morgan Stanley Open Sources CALM: Architecture as Code for Enterprise DevOps

**Author: Tom Smith**

## Introduction

Enterprise software development often suffers from a disconnect between architecture intent and real-world implementation. Static diagrams become outdated, compliance reviews introduce delays, and crucial architectural decisions are lost in unused documentation. Recognizing these issues first-hand, Morgan Stanley developed CALM to bridge the gap and accelerate delivery.

## What is CALM?

CALM (Common Architecture Language Model) is an open-source "Architecture as Code" platform released by Morgan Stanley through FINOS. The tool enables software architects to define, validate, and visualize system architectures in a standardized, machine-readable format. CALM is built on a JSON Meta Schema and transforms high-level and detailed architectural designs into executable specifications understood by both humans and machines.

## Why CALM Matters

- **Bridges intent and implementation:** Ensures architectural decisions are kept in sync with deployed solutions.
- **Automates compliance:** Embeds security and compliance rules, enabling faster, pattern-based approvals.
- **Accelerates deployment:** Used internally by Morgan Stanley to support over 1,400 deployments, reducing review bottlenecks from months to minutes.

## Key Features

- **Standardized Model:** Defines architecture using nodes (services, databases, networks), relationships, and metadata for constraint/context.
- **Continuous Validation:** Designs are version-controlled, testable, and validated through the CALM CLI in CI/CD pipelines.
- **Pattern-based Approval:** Enables reuse of pre-approved patterns, reducing risk and review friction.
- **DevOps Native:** Architectural definitions are treated as code, allowing integration with source control, automated checks, and real-time compliance feedback.

## Impact in the Real World

CALM has helped Morgan Stanley accelerate deployments and compliance cycles. The approach offers major benefits for:

- Automated and consistent architectural validation
- Faster approval and deployment of enterprise applications
- Simplification of security and regulatory reviews
- Reusable architecture patterns and organizational best practices

## Broader Implications

Although CALM was developed for the financial sector, any organization facing complex architectures, regulatory requirements, or labor-intensive compliance reviews can benefit. As DevOps pushes for greater automation, solutions like CALM provide essential links between enterprise intent and operational reality.

## How to Get Started

CALM is available as an open-source project on FINOS and comes with a CLI for generating, validating, and visualizing architecture specifications, with documentation and examples to bootstrap adoption. Organizations are encouraged to start by codifying known architectural patterns and progressively expanding their adoption.

## Conclusion

Morgan Stanley's CALM initiative demonstrates how Architecture as Code can streamline DevOps practices and compliance in large organizations, making architectural governance central to continuous delivery and agile transformations.

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops/?utm_source=rss&utm_medium=rss&utm_campaign=morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops)
