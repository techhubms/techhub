---
layout: "post"
title: "Building Long-Distance Next Edit Suggestions in GitHub Copilot"
description: "A technical deep dive into how GitHub Copilot's Next Edit Suggestions (NES) were expanded to work throughout the entire file in VS Code. The authors explain new machine learning models, evaluation frameworks, a multi-stage training process, and UX design considerations for surfacing distant code suggestions without disrupting developer workflow."
author: "Vikram Duvvur, Gaurav Mittal, Benjamin Simmonds"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/blogs/2026/02/26/long-distance-nes"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2026-02-26 00:00:00 +00:00
permalink: "/2026-02-26-Building-Long-Distance-Next-Edit-Suggestions-in-GitHub-Copilot.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Powered Suggestions", "Blog", "Code Refactoring", "Coding", "Developer Tools", "Editor Extensions", "Evaluation Framework", "Extended Range", "GitHub Copilot", "Machine Learning", "Model Training", "News", "Next Edit Suggestions", "Reinforcement Learning", "Software Engineering", "Supervised Finetuning", "User Experience", "VS Code"]
tags_normalized: ["ai", "ai powered suggestions", "blog", "code refactoring", "coding", "developer tools", "editor extensions", "evaluation framework", "extended range", "github copilot", "machine learning", "model training", "news", "next edit suggestions", "reinforcement learning", "software engineering", "supervised finetuning", "user experience", "vs code"]
---

Vikram Duvvur, Gaurav Mittal, and Benjamin Simmonds detail the engineering behind long-distance Next Edit Suggestions for GitHub Copilot, highlighting advances in ML modeling, evaluation, and UX for VS Code users.<!--excerpt_end-->

# Building Long-Distance Next Edit Suggestions in GitHub Copilot

*By Vikram Duvvur, Gaurav Mittal, and Benjamin Simmonds*

## Overview

This article explains how GitHub Copilot's Next Edit Suggestions (NES) were expanded to surface suggested code edits anywhere in your file, not just near the cursor. The work described includes novel ML modeling approaches, practical evaluation methods, model training details, and carefully considered UX changes in VS Code.

## Motivation and Challenge

The original NES feature inserted suggestions close to the cursor, which meant any necessary changes far from your location were missed. However, developers often edit or refactor code that impacts lines scattered throughout a file. To address real editing workflows, GitHub's team worked on predicting and suggesting next edits at any location in your code, not just the immediate vicinity.

## Technical Approach

- **Multi-Model Design:**
  - Introduced a dedicated *location model* to predict likely target lines for edits.
  - The standard NES model continues to generate the actual edit once the location is predicted.
  - This separation allows for specialization and independent iterations for accuracy and quality.

- **Evaluation Framework:**
  - Structured three-step process: identifying real developer workflows, constructing relevant jump examples, and measuring both jump and no-jump decision accuracy.
  - Ensures that model improvements reflect what developers actually need and do.

- **Dataset Construction:**
  - Evaluation dataset: hand-built from observed multi-edit scenarios.
  - Training dataset: scaled up from developer cursor trajectories, transformed into supervised training samples.

- **Supervised Finetuning (SFT):**
  - Utilized a structured hyperparameter grid search built on previous NES model tuning knowledge.
  - Also trialed Bayesian Optimization, but grid search yielded better results for this application.

## UX and User Trust

- Distant suggestions present usability and trust problems, since they're less discoverable and potentially more disruptive.
- To address this, a compact widget offers a diff-style preview and adapts to the code editor layout, occupying whitespace when possible. Developers can quickly judge relevance before choosing to inspect or apply the suggestion.

## Validation and Iterative Improvement

- Internal 'dogfooding' found that initial models jumped too aggressively. Dataset was rebalanced to include more 'no jump' samples and retrained, improving both intention and user experience.

- A/B testing against standard NES revealed a 23% increase in code written via NES, though distant suggestions were sometimes rejected more often, highlighting the importance of model restraint.

## Reinforcement Learning Enhancement

- Introduced Reinforcement Learning with Verified Rewards (RLVR): model is rewarded when its predictions match real editing behavior and penalized otherwise. This improved the precision of suggestions and user trust.

## Availability

- Long-distance NES is now live for VS Code users with GitHub Copilot subscriptions. To try it, enable extended NES range settings in VS Code and use it during multi-location editing tasks like refactoring.

## Looking Forward

- The team plans cross-file suggestions and unified models for location/content prediction to further improve the developer workflow.

## Acknowledgements

Thanks to the developer community and the engineers, researchers, designers, and product managers at GitHub and Microsoft who contributed to dataset creation, evaluation, modeling, and user experience.

---

**References:**

- [GitHub Copilot – Next Edit Suggestions](https://github.com/features/copilot)
- [VS Code Blog: Long-distance NES](https://code.visualstudio.com/blogs/2026/02/26/long-distance-nes)

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2026/02/26/long-distance-nes)
