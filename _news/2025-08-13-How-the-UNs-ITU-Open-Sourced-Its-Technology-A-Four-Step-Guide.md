---
layout: "post"
title: "How the UN's ITU Open Sourced Its Technology: A Four-Step Guide"
description: "This article details the process the International Telecommunication Union (ITU), a United Nations organization, followed to open source its technology with guidance from GitHub. It covers their transition from a private Azure DevOps setup to a public open-source repository, including research, mindset shift, code and licensing considerations, and community engagement. The piece is a practical guide for organizations seeking to make a similar move, outlining documentation, contribution workflows, repository security, and licensing, all grounded in hands-on collaboration between ITU, GitHub, and the open source community."
author: "Joshua Ku"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/open-source/social-impact/from-private-to-public-how-a-united-nations-organization-open-sourced-its-tech-in-four-steps/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-08-13 16:00:00 +00:00
permalink: "/2025-08-13-How-the-UNs-ITU-Open-Sourced-Its-Technology-A-Four-Step-Guide.html"
categories: ["DevOps"]
tags: ["Azure DevOps", "BSD 2 License", "Collaboration", "Community Engagement", "Continuous Integration", "CONTRIBUTING.md", "Contribution Guidelines", "DevOps", "Documentation", "GitHub", "Licensing", "Linting", "News", "Open Source", "Repository Management", "Social Impact", "Software Development", "Testing"]
tags_normalized: ["azure devops", "bsd 2 license", "collaboration", "community engagement", "continuous integration", "contributing dot md", "contribution guidelines", "devops", "documentation", "github", "licensing", "linting", "news", "open source", "repository management", "social impact", "software development", "testing"]
---

Joshua Ku, in collaboration with the ITU's BDT and GitHub, presents a step-by-step guide on how a United Nations agency transitioned its software from a private Azure DevOps environment to open source, offering actionable advice for organizations on the same journey.<!--excerpt_end-->

# How the UN's ITU Open Sourced Its Technology: A Four-Step Guide

**Author:** Joshua Ku

Helping teams adopt smarter technology practices and fostering open, collaborative communities is both rewarding and impactful. In this article, Joshua Ku describes how the International Telecommunication Union (ITU) Telecommunication Development Bureau (BDT)—a development arm of the United Nations specializing in digital technologies—worked with GitHub Skills-Based Volunteering to move from a closed, Azure DevOps-based environment to a fully open source model. This hands-on, six-month journey resulted in ITU enabling global partners to access, contribute to, and build upon their software.

## Why Open Source?

- UN organizations and nonprofits often have limited resources.
- Open sourcing their technology enables scaling impact and leveraging contributions from the wider community.

## The Four-Step Transition Process

### 1. Research Existing Open Source Best Practices

- Studied diverse open-source repositories to identify effective onboarding practices and community management approaches.
- Evaluated repository health via READMEs, contribution guidelines, and issue-tracking strategies.
- Used repositories like [Kubernetes](https://github.com/kubernetes/kubernetes), [Ersilia](https://github.com/ersilia-os), and [Terraform](https://github.com/hashicorp/terraform) as inspiration for vibrant, well-run open-source projects.

### 2. Shift Mindset and Prepare the Code Base

- Transition required moving from a closed to an open and public-first philosophy.
- Reviewed code and sanitized sensitive or proprietary information.
- Used sample data and created guides for input formatting.
- Developed onboarding documentation:
  - **Getting Started guide:** Prepares new contributors with environment setup instructions.
  - **CONTRIBUTING.md:** Lays out expectations and responsibilities for contributors.
- Implemented automated tests for code quality maintenance, leveraging [continuous integration best practices](https://github.com/resources/articles/devops/continuous-integration).

### 3. Decide on Licensing

- Selected a permissive open-source license compatible with desired outcomes (BSD-2 in ITU's case) after reviewing options via [choosealicense.com](https://choosealicense.com).
- Ensured compatibility with project dependencies and clear legal guidelines for contributors and users.

### 4. Engage the Open Source Community

- Identified “first issues” and tagged them with [good first issue](https://github.blog/open-source/social-impact/for-good-first-issue-introducing-a-new-way-to-contribute/) to lower the barrier for new contributors.
- Encouraged active participation and learning from external developers.

## Key Practices and Takeaways

- **Documentation:** Maintain up-to-date README and clear contribution guides.
- **Security:** Protect sensitive information; establish robust repository security practices.
- **Community Engagement:** Proactively welcome and onboard contributors; set up initial issues as easy wins.
- **Sustainability:** The ITU team continues to open source more products, benefiting from established workflows and community practices validated through collaboration with GitHub.

## How to Get Involved

If you are interested in contributing to open-source or learning about social impact projects, explore [For Good First Issue](https://forgoodfirstissue.github.com/). The open source movement thrives on wide participation, and your skills can support impactful organizations tackling global challenges.

## Conclusion

Transitioning from a private to a public open-source technology stack is an achievable goal with careful planning, research, and collaboration. ITU's experience, supported by GitHub, demonstrates how structured preparation, robust documentation, thoughtful licensing, and intentional community building make open source engagement both accessible and impactful.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/open-source/social-impact/from-private-to-public-how-a-united-nations-organization-open-sourced-its-tech-in-four-steps/)
