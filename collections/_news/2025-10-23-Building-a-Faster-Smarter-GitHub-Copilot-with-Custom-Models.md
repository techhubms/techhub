---
external_url: https://github.blog/ai-and-ml/github-copilot/the-road-to-better-completions-building-a-faster-smarter-github-copilot-with-a-new-custom-model/
title: Building a Faster, Smarter GitHub Copilot with Custom Models
author: Shengyu Fu
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-10-23 18:31:12 +00:00
tags:
- AI & ML
- AI Agents
- AI Coding Tools
- AI Powered Development
- Code Completion
- Custom Models
- Developer Tools
- Editor Integration
- Fine Tuning
- HumanEval
- LLM
- LLM Judge
- Machine Learning Evaluation
- Model Training
- Offline Evaluation
- Real Time Code Suggestions
- Reinforcement Learning
- Unit Testing
section_names:
- ai
- github-copilot
---
Shengyu Fu shares an in-depth perspective on the engineering journey behind GitHub Copilot’s enhanced completions, revealing technical model training strategies and real-world impact for developers.<!--excerpt_end-->

# Building a Faster, Smarter GitHub Copilot with Custom Models

**Author: Shengyu Fu**

GitHub Copilot remains a staple development tool, primarily for its powerful code completion feature. The core engineering team has iterated on Copilot’s custom models—driven by developer feedback and direct telemetry—to enable smarter, faster, and more relevant code suggestions. This article breaks down how recent model advancements have measurably improved Copilot’s user experience.

## Key Performance Improvements

- **20% more accepted and retained characters**: More suggested code is actually useful, less is deleted.
- **12% higher suggestion acceptance rate**: Developers find completions helpful more often.
- **3x higher token-per-second throughput** and **35% lower latency**: Faster, more responsive coding, even at scale.

These upgrades apply across all Copilot-supported editors and environments, aiming for smoother developer workflow and less editing overhead.

## Evaluation Methodologies

Copilot’s model improvements undergo a thorough, multi-stage evaluation:

### 1. Offline Evaluations

- **Execution-based Benchmarks**: Models are tested against public and internal code repositories, spanning all major languages. Unit tests and scenario coverage priorities functional correctness (e.g., suggestions must lead to code that compiles and passes tests).
- **LLM-judge Scoring**: An independent language model evaluates completion quality, relevance to context, and helpfulness—going beyond simple compilation to support developer-centric preferences.

### 2. Pre-Production

- **Qualitative Dogfooding**: Internal GitHub and Microsoft developers use preview models in real-world coding, offering structured feedback on readability, trust, and stylistic quality. Language experts help refine completion style and correctness.

### 3. Production A/B Testing

- Ships only after statistically significant improvements are proven in actual developer workflows, measured through metrics like acceptance rates, time-to-first token, latency, and more.

## Behind the Model Training Pipeline

- **Mid-training**: Building on a broad code base (almost 10 million repositories and over 600 languages), the model is trained to understand new APIs and idioms, not just language syntax.
- **Supervised Fine-Tuning**: The model is further specialized for fill-in-the-middle (FIM) scenarios, improving its ability to insert code precisely without duplications or format errors. Benchmarks like HumanEval Infilling show substantial gains.
- **Reinforcement Learning**: Custom RL algorithms reward the model for useful, context-aware, and concise suggestions, avoiding common pitfalls like excessive comments (reward hacking) or unwanted prefix duplication.

## Lessons Learned

1. **Careful Reward Balancing**: Overemphasis on certain metrics (e.g., longer completion length) led to excessive commenting, requiring added guardrails.
2. **Holistic Metrics Matter**: Optimizing for a single metric like acceptance rate may inflate numbers but harm genuine developer satisfaction. Multiple ways to measure impact are necessary.
3. **Train for Real Usage**: Synthetic fine-tuning data is aligned with real use cases, letting the team identify and correct unwanted patterns.

## Looking Forward

The GitHub Copilot team is actively:

- Expanding model training to address domain-specific needs (e.g., game dev, finance, ERP).
- Refining reward functions for semantic value and API modernity.
- Continuously driving performance, quality, and cost improvements for developers.

## Acknowledgments

Special thanks are extended to the developer community, and the cross-functional engineering, research, and product teams at GitHub and Microsoft who contributed to the success of these advancements.

Developers can experience these improvements directly by using GitHub Copilot in modern code editors like Visual Studio Code.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/the-road-to-better-completions-building-a-faster-smarter-github-copilot-with-a-new-custom-model/)
