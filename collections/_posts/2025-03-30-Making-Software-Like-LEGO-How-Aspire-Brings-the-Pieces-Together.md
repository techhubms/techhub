---
layout: "post"
title: "Making Software Like LEGO: How Aspire Brings the Pieces Together"
description: "David Fowler explores the challenges of modern web app development, emphasizing integration and orchestration over traditional coding. He introduces Aspire, a composable, contract-based system designed to streamline application lifecycle management, promote extensibility, and enable both human and AI stakeholders to collaborate more effectively."
author: "David Fowler"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://medium.com/@davidfowl/making-software-like-lego-how-aspire-brings-the-pieces-together-d6a99c2c4cde?source=rss-8163234c98f0------2"
viewing_mode: "external"
feed_name: "David Fowler's Blog"
feed_url: "https://medium.com/feed/@davidfowl"
date: 2025-03-30 18:44:09 +00:00
permalink: "/posts/2025-03-30-Making-Software-Like-LEGO-How-Aspire-Brings-the-Pieces-Together.html"
categories: ["Coding", "DevOps"]
tags: ["AI", "AI Workflows", "Application Lifecycle", "Aspire", "CI/CD", "Cloud Computing", "Coding", "Composability", "Configuration", "Developer Experience", "DevOps", "Infrastructure", "Integration", "Orchestration", "Platform Engineering", "Posts", "Software Development", "System Modeling", "Vertical Platforms", "Web Development"]
tags_normalized: ["ai", "ai workflows", "application lifecycle", "aspire", "cislashcd", "cloud computing", "coding", "composability", "configuration", "developer experience", "devops", "infrastructure", "integration", "orchestration", "platform engineering", "posts", "software development", "system modeling", "vertical platforms", "web development"]
---

In this post by David Fowler, the author discusses how Aspire transforms software development by making systems composable and robust, shifting focus from raw code to orchestration, resources, and extensible platform models for both developers and AI.<!--excerpt_end-->

# Making Software Like LEGO: How Aspire Brings the Pieces Together

**Author:** David Fowler

## Introduction

Modern web app development has shifted away from simply writing code—it now revolves around configuring, integrating, and orchestrating myriad services and components. David Fowler discusses this evolution, referencing Andrej Karpathy's insight that the bulk of development effort involves “plumbing” rather than programming.

> “It’s not even code, it’s… configurations, plumbing, orchestration, workflows, best practices.”

## The Overwhelming Complexity of Building Web Apps

Building even simple applications today requires combining:

- Frontend and backend frameworks
- Hosting (CDN, HTTPS, domains)
- Databases
- Authentication
- Blob storage
- Email services
- Payment gateways
- Background jobs
- Monitoring
- CI/CD pipelines
- Secret management

Every point of integration is a potential failure and a source of hidden complexity—config files, CLI flags, environment variables, and scattered documentation make orchestration and maintenance challenging.

## Limitations of Vertical Platforms

Many companies attempt to abstract this complexity by providing **vertical platforms** (e.g., Firebase, Heroku, Vercel). These platforms prioritize convenience and fast onboarding, but at the cost of flexibility—when you need to do something outside their intended workflows, extensibility becomes a challenge. This compromises composability and customizability, key requirements for larger organizations and customized solutions.

## The Value of Composability: Lessons from Kubernetes

Kubernetes became the dominant container orchestration platform not because it was the simplest, but due to its composable and extensible architecture. Teams could adapt and build on top of Kubernetes, integrating with their unique stacks, which was crucial for widespread adoption.

## Aspire’s Approach: A System for Composition, Not Just Control

Aspire brings composability to the entire application lifecycle. Rather than focusing solely on code, Aspire models:

- Services
- Dependencies
- Infrastructure

### Resources as Building Blocks

Aspire's core abstraction is the **resource**. Each resource represents a system component: executables, containers, cloud services, queues, databases, and more. These resources expose contracts detailing configuration, connectivity, execution, and deployment.

### Connecting Resources

Rather than manual wiring through environment variables and documentation, Aspire formalizes relationships:

```csharp
builder.AddProject("web")
       .WithReference(postgres);
```

References are contractual—the system knows how components relate and automates connection details such as secrets, connection strings, port mappings, volumes, and platform-specific requirements.

## Interlocking System Instead of Vertical Stack

Aspire is best understood not as a closed vertical stack, but as a **set of interlocking pieces**:

- Define custom Redis hosting integrations
- Model email sending with observability and retries as reusable resources
- Write custom publishers for proprietary database provisioning

Aspire provides **extension points**: developers can add new integrations and model complex flows in a consistent and maintainable way. Complexity is made composable rather than hidden or duct-taped.

## A Platform for Developers and AI

Aspire's model is accessible both to human developers and to AI systems. As Andrej Karpathy states:

> “A lot of glory will go to whoever figures out how to make it accessible and ‘just work’ out of the box, for both humans and, increasingly and especially, AIs.”

Aspire's strict contracts and discoverability facilitate both automated reasoning (for AIs) and human collaboration:

- AI can query application models for defined resources or suggest new integrations
- Developers benefit from reusable, inspectable building blocks

This leads to a fundamentally new development experience focused on design, not just connection.

## Moving Beyond Fragile Integrations

Many years have been devoted to improving code-level component models. Aspire extends that concept to infrastructure, configuration, and orchestration—key sources of complexity in today’s software. The goal is platforms that are truly composable, extensible, and adaptable to ever-growing needs, not brittle stacks glued together by scripts and guesswork.

## Conclusion

Aspire represents a paradigm shift in building and scaling modern software. By modeling complexity rather than masking it, Aspire provides a robust, extensible system where applications are assembled from interlocking resources that fit together—reducing fragility and increasing flexibility for both human and AI stakeholders.

This post appeared first on "David Fowler's Blog". [Read the entire article here](https://medium.com/@davidfowl/making-software-like-lego-how-aspire-brings-the-pieces-together-d6a99c2c4cde?source=rss-8163234c98f0------2)
