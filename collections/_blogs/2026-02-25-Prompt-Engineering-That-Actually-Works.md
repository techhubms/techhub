---
layout: "post"
title: "Prompt Engineering That Actually Works"
description: "This writeup by Hidde de Smet expands on his talk at the Xebia Azure AI Winter Jam, presenting proven prompt engineering techniques and mental models for LLMs and AI agents. Core frameworks, such as the 5 Principles, Chain of Thought, context engineering, and the shift to agent instructions, are explored in depth for practitioners building with AI—including Microsoft tools like GitHub Copilot."
author: "Hidde de Smet"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://hiddedesmet.com/prompt-engineering-that-actually-works"
viewing_mode: "external"
feed_name: "Hidde de Smet's Blog"
feed_url: "https://hiddedesmet.com/feed.xml"
date: 2026-02-25 00:00:00 +00:00
permalink: "/2026-02-25-Prompt-Engineering-That-Actually-Works.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Agent Architectures", "Agents", "AI", "AI Agents", "Azure AI", "Best Practices", "Blogs", "Chain Of Thought", "Chatgpt", "Claude", "Context Engineering", "Copilot Instructions", "Development", "GitHub Copilot", "Large Language Models", "Llm", "Meta Prompting", "Prompt Engineering", "RAG", "Self Critique", "System Prompts", "Workflow Automation"]
tags_normalized: ["agent architectures", "agents", "ai", "ai agents", "azure ai", "best practices", "blogs", "chain of thought", "chatgpt", "claude", "context engineering", "copilot instructions", "development", "github copilot", "large language models", "llm", "meta prompting", "prompt engineering", "rag", "self critique", "system prompts", "workflow automation"]
---

Hidde de Smet explains practical frameworks and real-world techniques for effective prompt engineering and context engineering with LLMs and agent tools, including GitHub Copilot, helping AI practitioners move from vague queries to reliable, production-grade results.<!--excerpt_end-->

# Prompt Engineering That Actually Works

*By Hidde de Smet*

This comprehensive writeup builds on Hidde de Smet's Xebia Azure AI Winter Jam talk, offering a guide to getting consistent, powerful results out of LLM-based AI systems. Drawing from O’Reilly’s "Prompt Engineering for LLMs" and "Prompt Engineering for Generative AI," as well as the latest research, it synthesizes foundational knowledge and actionable advice for technical practitioners.

## The Realities of Prompt Engineering

- "Prompt wizardry" is outdated—today’s models need clarity, structure, and well-designed context, not incantations.
- Core to effective prompting: specificity, examples (few-shot), and iteration for refinement.
- LLMs are document completers: the start of your prompt predicts everything that follows.

## The 80/20 Techniques

1. **Be specific:** Define audience, format, constraints, expected outputs.
2. **Show, don’t tell:** Provide 2-3 concrete examples (few-shot learning) for pattern consistency.
3. **Iterate:** Prompt improvement comes from iterating—a single "perfect prompt" is unrealistic.

## How LLMs “Think”

- LLMs predict one token at a time, not reasoning holistically—so order and constraint positioning in prompts matters.
- Context windows are growing (now up to 10M tokens), but "context rot" and "lost in the middle" remain practical risks for accuracy.
- Always place critical details at the start or end of prompts.

## Core Frameworks and Models

### The Loop Framework

- Express the user problem in human terms
- Convert to model domain (assign role/context/examples)
- LLM completes task
- Transform/validate output

### Five Prompt Principles

1. **Give Direction** (assign role/persona)
2. **Specify Format** (bullets, tables, JSON, etc.)
3. **Provide Examples**
4. **Evaluate Quality** (criteria, rating, sources)
5. **Divide Labor** (chain tasks)

### Playwriting Analogy

- Treat prompts as scripts for an actor: set the stage, assign a role, provide sample lines, direct the scene.

## Advanced Patterns

- **Chain of Thought (CoT):** Ask for reasoning steps for complex tasks.
- **Prewarming:** Gather knowledge first, then prompt.
- **ReAct:** Alternate reasoning and actions for agents.
- **Prompt Chaining:** Break down into discrete, testable steps.
- **RAG (Retrieval-Augmented Generation):** Fetch external doc context before LLM reply.
- **Context Engineering:** Architect the full input context (system instructions, history, tools, memory, docs, task).
- **System Prompts:** Clearly separate identity, rules, format, guardrails for AI agents.

## From Chatbots to Agents

- The shift is from manual chat prompting to persistent agent instructions (e.g., `.github/copilot-instructions.md` for GitHub Copilot, `CLAUDE.md`, custom "SKILL.md").
- Context engineering is overtaking narrow prompt engineering—designing system-level inputs for workflows, enforcement, and scaling.
- Best practices are shared across Copilot, Claude, Azure AI, and similar frameworks.

## Practical Prompts and Evaluation

- Use prompt templates for common tasks (expert, extractor, analyst, creator)
- Follow clear rules: start with actions, specify roles, provide constraints, give examples, iterate
- Use frameworks like SOMA (Specificity, Objective, Multiple scenarios, Automation) for rigorous evaluation

## Key Takeaways for AI & Copilot Practitioners

- Mastering prompt and context engineering is crucial for deploying agents and LLM features at scale.
- The same skills transfer across tools: GitHub Copilot, Claude, Azure AI, etc.
- Clear, layered instructions—supported by structured examples and iterative improvement—yield the most reliable, valuable AI outputs in real work.

---

## Resources

- [Prompt Engineering for LLMs](https://www.oreilly.com/library/view/prompt-engineering-for/9781098156145/) – O’Reilly
- [Prompt Engineering for Generative AI](https://www.oreilly.com/library/view/prompt-engineering-for/9781098153427/) – O’Reilly
- [Anthropic Prompt Library](https://docs.anthropic.com/en/prompt-library/library)
- [Prompt Engineering Guide](https://www.promptingguide.ai/)
- [OpenAI Prompt Engineering](https://platform.openai.com/docs/guides/prompt-engineering)

---

*Author: [Hidde de Smet](https://linkedin.com/in/hiddedesmet)*

Azure Solution Architect focused on cloud-based AI solutions, DevOps, and technical leadership.

This post appeared first on "Hidde de Smet's Blog". [Read the entire article here](https://hiddedesmet.com/prompt-engineering-that-actually-works)
