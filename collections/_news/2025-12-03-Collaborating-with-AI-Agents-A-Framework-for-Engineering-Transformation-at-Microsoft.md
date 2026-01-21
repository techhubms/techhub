---
external_url: https://devblogs.microsoft.com/engineering-at-microsoft/the-interaction-changes-everything-treating-ai-agents-as-collaborators-not-automation/
title: 'Collaborating with AI Agents: A Framework for Engineering Transformation at Microsoft'
author: Jenny Ferries
feed_name: Microsoft Engineering Blog
date: 2025-12-03 01:46:50 +00:00
tags:
- AI Agent
- AI Agents
- AI Transformation
- Code Migration
- Collaboration
- Developer Productivity
- Developer Workflow
- Engineering Productivity
- Engineering@Microsoft
- Entra SDK
- Human AI Partnership
- Microsoft Entra
- Migration Framework
- Prompt Engineering
- Prompt Template
- Security Boundaries
section_names:
- ai
---
Jenny Ferries shares insights from Microsoft's journey towards treating AI agents as collaborators, outlining a practical framework for boosting engineering outcomes through human-AI partnership in complex projects.<!--excerpt_end-->

# Collaborating with AI Agents: A Framework for Engineering Transformation at Microsoft

*By Jenny Ferries*

Microsoft has reimagined the role of AI in engineering—not as automation, but as a true partner in solving complex technical tasks. In this article, Jenny Ferries details the experience leading a large-scale Entra SDK migration and how the team's most significant breakthrough was shifting from automation thinking to collaborative partnership with AI.

## From Automation to Collaboration

When upgrading the Entra SDK across hundreds of repositories, traditional automation approaches failed due to the requirement for nuanced judgment—especially where security and custom logic were involved. Rather than scripting every edge case, Ferries’s team reframed the AI’s role: the AI agent became a co-creative engineer embedded within the team’s mission and trusted to exercise independent judgment.

## Core Components of the Collaborative Framework

The framework for integrating AI as a collaborator has eight key elements:

### 1. Identity and Mission Statement

Assign the AI a defined project role and context (“You are part of the Entra SDK v2 Migration Team”). Clarify why the work matters and the agent's responsibility to use judgment, not just follow instructions.

### 2. Purpose and Intent

State the project values and outcomes: prioritize primary goals (security, maintainability), provide structured steps, allow for thoughtful autonomy, and encourage escalation when in doubt.

### 3. Key Goals

List and prioritize concrete objectives, including quality and collaboration alongside technical completion.

### 4. Step-by-Step Framework with Judgment Guidance

Structure detailed migration steps and supply examples, while embedding conditions and triggers for escalation to human reviewers.

### 5. Security and Boundaries

Explicitly declare what must never be violated (security/authentication logic), when to ask for help, and how to document or pause for human review.

### 6. Validation and Quality Control

Require self-validation, documentation of changes, and preparation for human code review with flagged uncertainties.

### 7. Escalation Guidance

Provide concrete criteria for when the agent should stop work and request human input, reframing this as professional judgment rather than failure.

### 8. Recognition and Closing

Acknowledge the AI agent’s contributions and restate the values of quality, care, and improvement of the system.

[![Flow chart describing the 8 step process](https://devblogs.microsoft.com/engineering-at-microsoft/wp-content/uploads/sites/72/2025/12/2025-12-entra-sdk-ai-jenny-ferries-300x290.webp)](https://devblogs.microsoft.com/engineering-at-microsoft/wp-content/uploads/sites/72/2025/12/2025-12-entra-sdk-ai-jenny-ferries.webp)

## Key Learnings and Impact

- **Escalation normalized:** Agents flagged ambiguous cases more often, preventing hidden flaws.
- **Judgment improved:** AI’s ability to make trade-offs increased with context and role framing.
- **Better documentation:** AI-generated PR summaries aided human understanding and review.
- **Failure became graceful:** Agents would pause and seek help rather than generating broken solutions.
- **Reusable structure:** The framework translated well to other domains, including security reviews and documentation.

## Why It Matters

By switching to a partnership model, Microsoft teams unlocked higher accuracy, trust, and scale in technical work requiring human-like judgment. The article encourages technical leaders to adopt this identity and escalation-focused framework for any complex task where automation alone is insufficient.

## How to Apply the Framework

1. Start with a judgment-intensive technical task (e.g., migrations, security reviews).
2. Adapt the template to your project’s context.
3. Assign the AI an identity/role, clarify mission and escalation paths.
4. Encourage safe questioning and learning from outcomes.

## Conclusion

The future of AI in engineering is not about replacement but augmentation—where AI augments human judgment through structured collaboration. This framework by Jenny Ferries provides practical steps for transforming both engineering results and team culture by building a relationship with AI based on trust, recognition, and partnership.

*If you experiment with this approach, share your feedback to help everyone build better human-AI collaborations.*

This post appeared first on "Microsoft Engineering Blog". [Read the entire article here](https://devblogs.microsoft.com/engineering-at-microsoft/the-interaction-changes-everything-treating-ai-agents-as-collaborators-not-automation/)
