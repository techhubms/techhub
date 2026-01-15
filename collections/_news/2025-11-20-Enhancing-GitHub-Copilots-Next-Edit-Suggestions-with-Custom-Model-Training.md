---
layout: post
title: Enhancing GitHub Copilot’s Next Edit Suggestions with Custom Model Training
author: Kevin Merchant
canonical_url: https://github.blog/ai-and-ml/github-copilot/evolving-github-copilots-next-edit-suggestions-through-custom-model-training/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-11-20 18:02:21 +00:00
permalink: /github-copilot/news/Enhancing-GitHub-Copilots-Next-Edit-Suggestions-with-Custom-Model-Training
tags:
- A/B Testing
- AI
- AI & ML
- AI Model Training
- Coding
- Coding Assistant
- Data Pipeline
- Developer Experience
- Developer Tools
- GitHub Copilot
- IDE Enhancements
- IDE Integration
- LLMs
- Machine Learning
- Model Evaluation
- Model Training
- NES
- News
- Next Edit Suggestions
- Next Edit Suggestions (nes)
- Prompt Engineering
- Reinforcement Learning
- Supervised Fine Tuning
- Synthetic Data
- VS Code
section_names:
- ai
- coding
- github-copilot
---
Kevin Merchant explores the evolution of GitHub Copilot’s Next Edit Suggestions, revealing how custom model training, reinforcement learning, and continuous developer feedback have dramatically improved real-time code recommendations.<!--excerpt_end-->

# Enhancing GitHub Copilot’s Next Edit Suggestions with Custom Model Training

GitHub Copilot's Next Edit Suggestions (NES) delivers rapid, context-aware code editing recommendations directly within Visual Studio Code. This news post provides a detailed look at how NES has been improved through custom model training and developer-driven iteration.

## Technical Challenges of Next Edit Suggestions

Rather than predicting the next token, NES must anticipate the next *edit*. This requires deep contextual awareness, inferencing user intent, and maintaining low-latency performance in-editor. Off-the-shelf models proved insufficient due to either slow response times or low-quality suggestions. To address this, the Copilot team engineered a bespoke model co-designed with the VS Code team for seamless integration.

## Custom Dataset Creation & Training

Initial attempts to use pull request diffs for model training fell short. Pull requests lacked the sequential, real-time editor context and provided limited negative samples. The team pivoted to collecting granular code editing sessions from internal volunteers, producing a dataset with higher fidelity and relevance. Supervised fine-tuning (SFT) on this data enabled NES to outperform generic LLM baselines.

## Reinforcement Learning & Out-of-Distribution Robustness

Recognizing supervised training's limitations, the team introduced reinforcement learning (RL) to further optimize NES using unlabeled data. RL refinement leveraged a grader designed to assess edit quality and readability in the UI, continually updated via qualitative analysis. This process expanded the training corpus and allowed explicit encoding of what constitutes undesirable edit suggestions, bolstering NES’s generalization and reliability.

## Model Improvements and Continuous Evaluation

Recent NES iterations involved prompt optimization (reducing context and markup to enhance speed), LLM-based data quality filtering, generation of synthetic data from larger models, and targeted hyperparameter tuning. The Copilot team regularly trains and tests dozens of candidates via offline benchmarks, internal use (“dogfooding”), and online A/B experimentation, capturing detailed acceptance and hide rates from real developer workflows.

Quality, speed, and precision have improved across major releases:

- **April release**: Increased suggestion quality and reduced token overhead for faster responses.
- **May release**: Refined model’s eagerness, showing fewer but more relevant suggestions.
- **November release**: Shortened prompts, increased caching, yielding both higher quality and lower latency.

Performance metrics demonstrate significant gains in acceptance rate and reductions in hidden/unwanted suggestions.

## Developer Feedback & Adaptive Experience

Ongoing community feedback has directly shaped model objectives, balancing assertive versus restrained suggestion behavior to suit varying developer preferences. NES continues to evolve, refining display and customization options. Future work targets adaptation to individual editing styles for greater personalization.

## Upcoming Enhancements

Next steps for NES include:

- *Edits at a distance*: Suggestions spanning multiple files.
- *Faster responses*: Further infrastructure and model latency reductions.
- *Smarter edits*: Enhanced cross-file context and dependency handling.

To try the latest NES, update VS Code and Copilot Chat extension, and confirm NES is enabled in settings.

---

**Acknowledgements**: COPILOT NES improvements are thanks to contributions across GitHub, Microsoft, and the wider developer community. Continuous engagement—including filing issues and direct feedback—helps drive ongoing innovation and improvements.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/evolving-github-copilots-next-edit-suggestions-through-custom-model-training/)
