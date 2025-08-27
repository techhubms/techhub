---
layout: "post"
title: "How GitHub Engineers Solve Platform Problems: Best Practices and Lessons from Infrastructure"
description: "Fabian Aguilar Gomez outlines best practices for platform engineering at GitHub, focusing on problem identification, resolution, and prevention at scale. The article covers domain understanding, platform skills, the importance of knowledge sharing, assessing change impacts, careful testing, and building resilient, scalable systems."
author: "Fabian Aguilar Gomez"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/engineering/infrastructure/how-github-engineers-tackle-platform-problems/"
viewing_mode: "external"
feed_name: "GitHub Engineering Blog"
feed_url: "https://github.blog/engineering/feed/"
date: 2025-06-10 16:00:00 +00:00
permalink: "/2025-06-10-How-GitHub-Engineers-Solve-Platform-Problems-Best-Practices-and-Lessons-from-Infrastructure.html"
categories: ["DevOps"]
tags: ["Ansible", "Collaboration", "DevOps", "Distributed Systems", "Engineering", "GitHub", "IaC", "Incident Response", "Infrastructure", "Knowledge Sharing", "Monitoring", "Network Fundamentals", "News", "Platform Engineering", "Problem Solving", "System Reliability", "Terraform", "Testing"]
tags_normalized: ["ansible", "collaboration", "devops", "distributed systems", "engineering", "github", "iac", "incident response", "infrastructure", "knowledge sharing", "monitoring", "network fundamentals", "news", "platform engineering", "problem solving", "system reliability", "terraform", "testing"]
---

Fabian Aguilar Gomez shares GitHub's approach to tackling platform problems, offering insights into best practices for engineers in large-scale infrastructure and platform teams.<!--excerpt_end-->

# How GitHub Engineers Solve Platform Problems: Best Practices from Infrastructure

*By Fabian Aguilar Gomez*

Platform engineering at scale is both challenging and rewarding. In this post, Fabian Aguilar Gomez draws a parallel between building Gundam model kits and the role of platform engineers at GitHub—where the focus is on supplying the tools and building blocks for product teams, rather than assembling the final product directly.

## Key Differences Between Product and Platform Engineering

- **Product engineers** build the user-facing features, akin to assembling the finished Gundam models.
- **Platform engineers** provide internal tools, infrastructure, and environments—the clippers, files, and displays needed to build and showcase the models—helping product teams deliver smoothly at scale.

A move to GitHub's infrastructure organization meant a shift in focus from external customer needs to internal engineering customers, requiring a revised approach to testing and problem-solving.

---

## Best Practices for Tackling Platform Problems

### Understanding Your Domain

Before addressing platform problems, deep domain knowledge is essential:

- **Talk to neighbors:** Learn from previous teams or stakeholders through handover meetings to clarify terminology and understand challenges.
- **Investigate old issues:** Reviewing backlogs can reveal recurring pain points and limitations to improve upon.
- **Read documentation:** Comprehensive documentation offers vital insight into system workings.

### Bridging Concepts to Platform-Specific Skills

Platform teams require more specialized technical skills beyond general engineering knowledge:

- **Networks:** Know the basics (TCP, UDP, L4 load balancing) and tools (e.g., dig) to comprehend traffic flow and potential bottlenecks.
- **Operating systems and hardware:** Selecting the right target for your workloads is vital for scalability, cost, and security.
- **Infrastructure as Code (IaC):** Automation via tools like Terraform and Ansible reduces human error in provisioning and updating infrastructure.
- **Distributed systems:** Accept that failures happen; proactive failover and recovery planning is crucial for resilience and user experience.

### Knowledge Sharing

Encouraging engineers to share lessons and experiences leads to innovation and better outcomes:

- **Fosters teamwork:** Accelerates problem-solving and catalyzes new solutions.
- **Prevents knowledge loss:** Institutional memory isn’t lost when engineers leave or are unavailable.
- **Improves customer outcomes:** Sharing insights enables the development of more reliable, scalable, and secure platforms.

---

## Assessing Impact: The "Impact Radius"

Any change to the fundamental platform—like DNS—can have widespread effects across many products (e.g., GitHub Pages, GitHub Copilot).

- **Understand dependencies:** Direct communication with dependent teams exposes how changes may cascade through the system.
- **Conduct postmortems:** Review incidents to learn about incident impacts, involved changes, and remediation steps.
- **Centralized monitoring and telemetry:** Health dashboards (such as a Single Availability Metric) streamline diagnosis and issue resolution.

---

## Testing Changes in Distributed Systems

Testing and validation are challenging in a distributed environment:

- **Test with Infrastructure as Code:** Verify provisioning/deprovisioning, and ensure re-provisioning doesn't risk accidental deletion.
- **End-to-end (E2E) testing:** Gradually direct real traffic to new or changed services for validation.
- **Self-healing:** Stress-test the system’s ability to recover from load spikes and to identify bottlenecks before they escalate.

Adopt rolling changes host-by-host to enable fine-grained rollback and minimize blast radius.

---

## Final Thoughts

Platform engineering underpins the developer experience at GitHub—requiring a blend of technical expertise, solid processes, and collaborative culture. The complexity is high, but so is the impact: smooth platforms empower the whole company to ship features more reliably and at greater speed.

**For more in-depth insights, visit [GitHub’s infrastructure blog](https://github.blog/engineering/infrastructure/).**

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/engineering/infrastructure/how-github-engineers-tackle-platform-problems/)
