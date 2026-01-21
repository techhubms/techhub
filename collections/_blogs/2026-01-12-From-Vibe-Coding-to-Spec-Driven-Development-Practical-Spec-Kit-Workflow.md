---
external_url: https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development-part2
title: 'From Vibe Coding to Spec-Driven Development: Practical Spec-Kit Workflow'
author: Hidde de Smet
viewing_mode: external
feed_name: Hidde de Smet's Blog
date: 2026-01-12 00:00:00 +00:00
tags:
- .NET 9
- AI Assisted Development
- ASP.NET Core Identity
- Blazor Server
- C# 13
- Claude
- Coding Best Practices
- Copilot
- Development
- Docker
- Edge Case Handling
- EF Core
- GitHub
- MVP
- PostgreSQL
- Quality Assurance
- Series
- SignalR
- Software Architecture
- Spec Kit
- Specification Driven Development
- SQL Server
- Task Management
- Technical Planning
- Test Automation
- User Stories
- Vibe Coding
- Workflow Automation
section_names:
- ai
- coding
- devops
- github-copilot
---
Hidde de Smet continues his AI-assisted development series by demonstrating the full Spec-Kit workflow—detailing how to move from requirements to production-ready code using .NET 9, Blazor, and GitHub Copilot. A must-read for software engineers adopting modern, spec-driven workflows.<!--excerpt_end-->

# From Vibe Coding to Spec-Driven Development: Part 2 - The Spec-Kit Workflow

**Author:** Hidde de Smet

Part 2 of a series on mastering AI-assisted development, this guide transitions readers from ad-hoc coding habits ("vibe coding") to a rigorous, specification-driven approach using Spec-Kit, GitHub Copilot, and the .NET/Blazor stack. The post makes the process concrete by building a real-world team task manager app.

## Series Overview

1. Part 1: The problem and the solution
2. **Part 2: The Spec-Kit workflow (this post)**
3. Best practices and troubleshooting
4. Team collaboration and advanced patterns
5. Case studies and lessons learned

## What You'll Build

- Multi-user authentication
- Team workspaces
- Task assignment and tracking
- Real-time updates (SignalR)
- Responsive UI

## Prerequisites

- Install `specify-cli` from GitHub
- Any AI coding assistant (e.g., GitHub Copilot, Claude, Cursor, Windsurf)
- Familiarity with .NET and Blazor

## The Spec-Kit Workflow in Practice

### 1. Initialize Your Project

Create your folder and initialize with Spec-Kit:

```sh
specify init team-task-manager
cd team-task-manager
```

Project directories for agents, prompts, specs, and configuration are auto-created for structure and reproducibility.

### 2. Create the Constitution

The 'constitution' encodes project foundations and non-negotiables:

- Project purpose and business case
- Core technical principles (simplicity, UX, security, performance, mobile-first)
- Explicit technologies: .NET 9, Blazor Server, SQL Server/PostgreSQL, ASP.NET Core Identity, Docker
- Constraints and non-goals (avoid microservices, no SPA frameworks, no feature creep)
- Quality standards (coverage, testing, code style, security)
- Governance and change management (versioning, review, enforcement)

A detailed example is provided, setting boundaries for the rest of the workflow. This upfront rigor helps the AI assistant (like Copilot) stay on track and prevents architecture drift.

### 3. Specification Phase

Create `.speckit/spec.md` covering:

- User stories and clear acceptance criteria (authentication, team management, task management, etc.)
- UI requirements (including responsive design)
- Data models and key entities
- Edge case handling and error conditions
- Explicit performance and security requirements
- Success metrics for engagement, system health, and user satisfaction

The blog gives practical checklists to ensure no "NEEDS CLARIFICATION" holes and keeps implementation details out of specs.

### 4. Generate the Plan

Run `/speckit.plan` to generate:

- Functional requirements (`requirements.md`)
- Architecture and tech stack decisions (`plan.md`)
- Entity/data models (EF Core, C#)
- REST API contract definitions and integration points
- Checklists to validate readiness

The post shares detailed samples, including typical code for API design, authentication policy, and DB schema, showing how the AI translates high-level intent into actionable implementation guidance.

### 5. Break Into Tasks

The `/speckit.tasks` phase splits the entire job into 130+ small, orderable, testable tasks grouped by user story and feature boundaries. This phase encodes dependencies, parallelization opportunities, and acceptance criteria for each step—enabling both developer and AI auto-implementation.

### 6. Implementation with AI Assistants

- Apply plan and tasks using Copilot (or other supported tool)
- Generate code and config following the specified standards
- Focus on incremental and test-driven delivery: run, test, review after each phase
- Handle change by updating the spec and re-running plan/tasks as project scope evolves

### 7. Best Practices & Quality

The post closes with advice:

- Always test increments, not just final delivery
- Be explicit up front ("specs must be exhaustive")
- Review and adapt plans critically at each phase
- Never skip acceptance, edge cases, or architectural review
- Validate Copilot or AI tool output with human context—catching hallucinations, outdated API calls, or missed requirements

### 8. Resources

- Spec-Kit GitHub: <https://github.com/github/spec-kit>
- Example projects: <https://github.com/hiddedesmet/spec-kit-task-manager>
- .NET, Blazor, Entity Framework, and SignalR official docs

---

By working through these phases, readers can shift from chaotic, intuition-based coding to a methodical, spec-driven and AI-assisted workflow that improves delivery speed, reduces ambiguity, and raises overall code quality.

---

*Looking forward:* The next parts of the series will cover advanced spec techniques, real-world troubleshooting, and lessons learned in production.

---

*For questions, feedback, or to follow the series, connect with Hidde de Smet on LinkedIn or visit [hiddedesmet.com](https://hiddedesmet.com).*

This post appeared first on "Hidde de Smet's Blog". [Read the entire article here](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development-part2)
