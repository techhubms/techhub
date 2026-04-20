---
author: carlottacaste
primary_section: github-copilot
feed_name: Microsoft Tech Community
title: 'Agents League Winner Spotlight: CertPrep Multi-Agent System on Microsoft Foundry'
section_names:
- ai
- azure
- github-copilot
date: 2026-04-17 07:00:00 +00:00
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-winner-spotlight-reasoning-agents-track/ba-p/4511211
tags:
- Agentic AI
- Agents League
- AI
- Azure
- Azure AI Foundry
- Azure Content Safety
- Certification Exam Prep
- Community
- Concurrent Execution
- Foundry Agent Service
- Foundry SDK
- GitHub Copilot
- GPT 4o
- Guardrails
- Human in The Loop
- JSON Mode
- Microsoft Foundry
- Microsoft Learn
- Mock Exams
- Multi Agent System
- Pydantic
- Readiness Scoring
- Reasoning Agents
- Streamlit
---

carlottacaste spotlights Athiq Ahmed’s winning Agents League Reasoning Agents project, CertPrep, detailing a Microsoft Foundry-based multi-agent pipeline that builds study plans, tracks readiness, generates assessments, and applies guardrails and human approval steps.<!--excerpt_end-->

## Overview

[Agents League](https://github.com/microsoft/agentsleague/tree/main) is a developer challenge intended to showcase what *agentic AI* looks like when you move beyond single prompts and build systems that can **plan, reason, verify, and collaborate**.

Participants had two weeks to ship real agents across three tracks:

- Creative Apps
- Reasoning Agents
- Enterprise Agents

This post spotlights the winning project for the **Reasoning Agents** track:

- **CertPrep Multi-Agent System — Personalised Microsoft Exam Preparation** (GitHub issue with full details): https://github.com/microsoft/agentsleague/issues/76
- Author: Athiq Ahmed (GitHub profile): https://github.com/athiq-ahmed

## The Reasoning Agents challenge scenario

The track asked teams to build a **multi-agent system** that can help learners prepare for **Microsoft certification exams** by:

- Understanding certification syllabi
- Generating personalized study plans
- Assessing learner readiness
- Adapting based on performance and feedback

The suggested reference architecture models a learning journey:

- Start from free-form student input
- Use a sequence of specialized reasoning agents to curate Microsoft Learn resources
- Produce a structured study plan with timelines and milestones
- Maintain engagement through reminders
- Shift to an assessment phase to evaluate readiness
- Recommend an exam or loop back into targeted remediation
- Emphasize reasoning, decision-making, and human-in-the-loop validation

Reference starter kit details:

- https://github.com/microsoft/agentsleague/tree/main/starter-kits/2-reasoning-agents

## The winning project: CertPrep Multi-Agent System

**CertPrep** is an AI solution for **personalized Microsoft certification exam preparation**, supporting **nine certification exam families**.

At a high level, it turns free-form learner input into:

- A structured certification plan
- Measurable progress signals
- Actionable recommendations

## Inside the multi-agent architecture

CertPrep is designed as a multi-agent pipeline with:

- Sequential reasoning
- Parallel execution
- Human-in-the-loop gates
- Traceability and responsible AI guardrails

### Specialized reasoning agents

The solution is composed of eight specialized reasoning agents (the post lists these explicitly):

- **LearnerProfilingAgent**: converts free-text background info into a structured learner profile using Microsoft Foundry SDK (with deterministic fallbacks)
- **StudyPlanAgent**: generates a week-by-week plan using a constrained allocation algorithm based on available time
- **LearningPathCuratorAgent**: maps exam domains to curated Microsoft Learn resources with trusted URLs and estimated effort
- **ProgressAgent**: computes a weighted readiness score based on domain coverage, time utilization, and practice performance
- **AssessmentAgent**: generates and evaluates domain-proportional mock exams
- **CertificationRecommendationAgent**: outputs a “GO / CONDITIONAL GO / NOT YET” decision plus remediation steps and next-cert suggestions

### Guardrails and human checks

- A **17-rule Guardrails Pipeline** performs validation checks at every agent boundary.
- There are **two explicit human-in-the-loop gates**, intended to prevent decisions being made without enough learner confirmation or data.

### Foundry and related tooling used

CertPrep leverages **Microsoft Foundry Agent Service**:

- Microsoft Foundry Agent Service overview: https://learn.microsoft.com/en-us/azure/foundry/agents/overview

Implementation details called out in the post:

- Managed agents via **Foundry SDK**
- Structured JSON outputs using **GPT-4o** (JSON mode) with conservative temperature settings
- Guardrails enforced through **Azure Content Safety**
- Parallel agent fan-out using concurrent execution
- Typed contracts with **Pydantic** for each agent boundary
- AI-assisted development with **GitHub Copilot** (code generation, refactoring, test scaffolding)

Performance note:

- The pipeline is designed to run **in under one second in mock mode** to support reliable demos without live credentials.

## User experience (Streamlit app)

The front end is built with **Streamlit** and organized as a **7-tab interactive interface**.

The learner flow described:

1. **Profile & Goals Input**
   - Learners describe background/experience/goals in natural language.
   - The UI shows the structured learner profile produced by the profiling agent.
2. **Learning Path & Study Plan Visualization**
   - Study plan shown with visuals like **Gantt-style timelines** and domain breakdowns.
3. **Progress Tracking & Readiness Scoring**
   - UI surfaces an **exam-weighted readiness score** (domain coverage, adherence, assessment performance).
4. **Assessments and Feedback**
   - Practice assessments generated dynamically.
   - Results include actionable feedback (not just scores).
5. **Transparent Recommendations**
   - Final recommendations are backed by reasoning traces and visual summaries.

The UI also includes:

- An **Admin Dashboard**
- Demo-friendly modes (switch between live and mock execution)
- Reasoning trace inspection for judges/reviewers/instructors

## Why the project stood out (as described)

The post highlights:

- Clear separation of reasoning roles (instead of a prompt-heavy monolith)
- Deterministic fallbacks and guardrails
- Observable, debuggable workflows aligned with Foundry’s production goals
- Explainable outputs surfaced in the UX

## Try it yourself

- Project details (GitHub issue): https://github.com/microsoft/agentsleague/issues/76
- Demo video (YouTube): https://www.youtube.com/watch?v=okWcFnQoBsE
- Live app (mock data): https://agentsleague.streamlit.app/

[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-winner-spotlight-reasoning-agents-track/ba-p/4511211)

