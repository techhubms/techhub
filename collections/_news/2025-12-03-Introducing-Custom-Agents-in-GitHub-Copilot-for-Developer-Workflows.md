---
layout: "post"
title: "Introducing Custom Agents in GitHub Copilot for Developer Workflows"
description: "This article presents GitHub Copilot’s new custom agents, which let development teams automate, debug, and secure engineering workflows across terminals, editors, and GitHub.com using partner-built and user-defined agents. It covers agent functionality, integration points, practical examples, and partnering companies. Guidance is provided on using and creating custom agents for observability, infrastructure-as-code, security, and more."
author: "Griffin Ashe"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/news-insights/product-news/your-stack-your-rules-introducing-custom-agents-in-github-copilot-for-observability-iac-and-security/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-12-03 17:00:45 +00:00
permalink: "/news/2025-12-03-Introducing-Custom-Agents-in-GitHub-Copilot-for-Developer-Workflows.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot", "Security"]
tags: ["Agentic AI", "AI", "AI & ML", "API Integration", "Automation", "CI/CD", "Coding", "Custom Agents", "Database Migrations", "DevOps", "DevOps Workflows", "Generative AI", "GitHub Copilot", "IaC", "Incident Response", "JFrog Security", "News", "News & Insights", "Observability", "PagerDuty", "Product", "Security", "Security Automation", "Terraform", "VS Code"]
tags_normalized: ["agentic ai", "ai", "ai and ml", "api integration", "automation", "cislashcd", "coding", "custom agents", "database migrations", "devops", "devops workflows", "generative ai", "github copilot", "iac", "incident response", "jfrog security", "news", "news and insights", "observability", "pagerduty", "product", "security", "security automation", "terraform", "vs code"]
---

Griffin Ashe introduces GitHub Copilot’s partner-built and custom agents, describing how these AI-powered tools help developers automate, secure, and optimize workflows across observability, infrastructure, and security domains.<!--excerpt_end-->

# Introducing Custom Agents in GitHub Copilot for Developer Workflows

GitHub Copilot has expanded beyond just code completion. Now, with custom agents, development teams can automate, debug, and secure workflows across the entire software development lifecycle—directly from their editor, terminal, or github.com.

## What Are Custom Agents?

Custom agents are Markdown-defined domain experts that extend GitHub Copilot’s capabilities to cover specialized workflows. These agents act like zero-maintenance teammates: for example, a JFrog security analyst who enforces compliance, a PagerDuty responder for incidents, or a MongoDB performance specialist.

Custom agents integrate anywhere Copilot is available:

- **Terminal (Copilot CLI)**
- **VS Code (Copilot Chat)**
- **GitHub.com (Copilot panel)**

Agents are easy to set up. Simply add a `.md` agent file to your repository or organization’s agent directory (e.g., `.github/agents/agent-name.md`).

### Agent Example

```markdown
---
name: readme-specialist
description: Expert at creating and maintaining high-quality README documentation
---
You are a documentation specialist focused on README files... [truncated for brevity]
```

When included in the repository, your agent becomes available to your workflow instantly.

## Real-World Examples From Partners

Custom agents solve a variety of engineering challenges:

### PagerDuty Incident Responder

- Contextualizes incidents, summarizes status, and proposes next steps
- Drafts team updates based on live PagerDuty alerts
- Command example:

  ```
  copilot --agent=pagerduty-incident-responder --prompt "Summarize active incidents and propose the next investigation steps."
  ```

### JFrog Security Agent

- Scans for vulnerable dependencies and suggests safe upgrades
- Directly patches dependency files and drafts security-aware PRs

### Neon Migration Specialist

- Reviews database schema migrations for safety and best practices
- Validates and tunes migrations automatically

### Amplitude Experiment Implementation

- Integrates A/B testing scaffolds
- Ensures proper analytics event tracking in product logic

### More Provided Agents

- **Dynatrace Observability and Security Expert:** Monitoring setup and optimization
- **Elasticsearch Remediation Agent:** Query, config, and observability support
- **MongoDB Performance Advisor:** Query optimization and analysis
- **Terraform Infrastructure Agent:** Review and refine IaC modules
- **LaunchDarkly Flag Cleanup:** Safe identification and removal of obsolete feature flags
- **Octopus Release Notes Expert:** Streamlines deployment documentation

## Benefits of Custom Agents

- Automate and enforce team-specific patterns and rules (like Terraform conventions)
- Share subject matter expertise across the team
- Integrate directly with DevOps, security, and observability systems
- Reuse definitions at repository or organizational scale

## Getting Started

Run any partner agent with:

```
copilot --agent=<agent-name> --prompt "<task>"
```

- Browse all agents in the [`awesome-copilot` repo](https://github.com/github/awesome-copilot)
- Learn to [build your own custom agent](https://docs.github.com/en/copilot/concepts/agents/coding-agent/about-custom-agents)
- [Give feedback](https://github.com/orgs/community/discussions/180853) to help shape the ecosystem

Custom agents move GitHub Copilot from just assisting code, to empowering teams to apply their unique workflows and standards automatically across the full software lifecycle.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/product-news/your-stack-your-rules-introducing-custom-agents-in-github-copilot-for-observability-iac-and-security/)
