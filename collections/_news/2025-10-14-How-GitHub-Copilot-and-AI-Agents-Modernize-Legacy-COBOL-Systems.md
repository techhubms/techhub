---
external_url: https://github.blog/ai-and-ml/github-copilot/how-github-copilot-and-ai-agents-are-saving-legacy-systems/
title: How GitHub Copilot and AI Agents Modernize Legacy COBOL Systems
author: Andrea Griffiths
feed_name: The GitHub Blog
date: 2025-10-14 16:00:00 +00:00
tags:
- AI & ML
- AI Agents
- Azure OpenAI
- Business Logic Extraction
- COBOL
- Code Analysis
- Enterprise Development
- Java Migration
- Legacy Modernization
- Mainframe Migration
- Mainframe Modernization
- Markdown Documentation
- Mermaid Diagrams
- Microsoft Global Black Belt
- Prompt Engineering
- Semantic Kernel
- Test Automation
section_names:
- ai
- azure
- coding
- github-copilot
primary_section: github-copilot
---
Andrea Griffiths outlines how Julia Kordick and Microsoft’s team leverage GitHub Copilot, AI agents, and Semantic Kernel to modernize COBOL mainframe systems, with a practical, developer-focused framework.<!--excerpt_end-->

# How GitHub Copilot and AI Agents Modernize Legacy COBOL Systems

## Overview

Modernizing legacy mainframe systems is a daunting challenge for today’s developers, especially those unfamiliar with aging languages like COBOL. In this article, Andrea Griffiths details how Microsoft’s Julia Kordick and her team overcame these barriers using GitHub Copilot, AI agents, and the Microsoft Semantic Kernel framework. The article provides actionable strategies and open-source resources for developers facing similar modernization tasks.

## The Challenge

Mainframe systems running COBOL continue to underpin critical financial and government operations, but the scarcity of experienced COBOL developers has turned legacy modernization into a pressing problem. Organizations face massive technical debt and a looming talent shortage as experts retire and COBOL codebases persist.

## AI-Powered Approach to Legacy Modernization

Julia Kordick, Microsoft Global Black Belt, demonstrates a systematic, AI-augmented strategy that does not require new developers to master COBOL. Instead, the process combines domain expert knowledge with modern AI tools — primarily GitHub Copilot and multiple autonomous agents orchestrated through Semantic Kernel — to analyze, document, and migrate legacy codebases.

### The Three-Step Modernization Framework

**1. Code Preparation (Reverse Engineering):**

- Use GitHub Copilot to extract business logic, generate markdown documentation, and map call chains and dependencies from COBOL files.
- AI speeds up what would have taken months of manual code analysis. However, human review of AI-generated analysis remains critical for business context.

**2. Enrichment (AI-Digestible Structuring):**

- Translate non-English comments and ensure code context is suitable for AI processing.
- Leverage COBOL’s structured identifiers (divisions) and prompt Copilot to outline and explain each section — even without COBOL expertise.
- Save AI-generated documentation as source-of-truth markdown files.

**3. Automation Aids (Scaling with Agents):**

- Transition from interactive Copilot sessions to orchestrated agent workflows using the Microsoft Semantic Kernel platform.
- Specialized agents automate call chain mapping, generate validation tests, suggest dependency updates, and even convert COBOL logic to modern languages such as Java.
- Visualizations (e.g., Mermaid diagrams) help clarify complex dependencies and guide modernization efforts.

## Sample Workflow: Azure-Supported Modernization

Julia’s team has open-sourced a reference framework built with Semantic Kernel and Azure OpenAI, supporting:

- Modular agents for specific analysis and modernization tasks.
- Cost tracking for each AI operation.
- Built-in checkpoints for human review.
- Quick setup scripts for developer productivity.

**Quick Start:**

```bash
git clone https://github.com/Azure-Samples/Legacy-Modernization-Agents
cd Legacy-Modernization-Agents
./doctor.sh setup
./doctor.sh run
```

Open-source resources are available at [aka.ms/cobol](https://aka.ms/cobol) and [aka.ms/cobol-blog](https://devblogs.microsoft.com/all-things-azure/how-we-use-ai-agents-for-cobol-migration-and-mainframe-modernization/).

## Honest View on Limitations

The article stresses that AI is not a complete replacement for human judgment—each mainframe system remains unique, and AI-driven automation still requires human validation and domain expertise. Full end-to-end automation of COBOL migration may be years away, but these tools offer dramatic improvements over previous consultant-led, code-generation approaches.

## Getting Started: Practical Recommendations

- Start with small, manageable legacy codebases.
- Use Copilot interactively to generate business logic documentation on a file-by-file basis.
- Familiarize yourself with prompt engineering for effective code analysis.
- Apply the Azure Samples framework to grow your modernization toolkit.
- Plan for architectural upgrades — aim to move core business logic from the monolithic mainframe to maintainable cloud-native services.
- Share results with your team and broader community.

## Key Insights

> AI amplifies, rather than replaces, developer expertise. Combining domain knowledge, modern architecture skills, and AI’s analysis capabilities makes even decades-old COBOL codebases approachable for today’s developers.

---

**Special thanks to Julia Kordick of Microsoft for sharing strategic and technical insights. For more, connect with [Julia on LinkedIn](https://www.linkedin.com/in/julia-kordick/).**

With intelligent automation and open-source frameworks, the barrier to legacy modernization is lower than ever. The best time to start is now.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/how-github-copilot-and-ai-agents-are-saving-legacy-systems/)
