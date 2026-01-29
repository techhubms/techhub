---
external_url: https://www.reddit.com/r/MachineLearning/comments/1mj3t3r/d_gspo_qwen3s_sequencelevel_rlhf_method_vs_grpo/
title: 'GSPO vs. GRPO: Sequence-Level Reinforcement Learning for LLM Fine-Tuning'
author: MarketingNetMind
feed_name: Reddit Machine Learning
date: 2025-08-06 12:50:47 +00:00
tags:
- AIME’24
- CodeForces
- DeepSeek
- Gradient Stability
- GRPO
- GSPO
- LiveCodeBench
- LLM Fine Tuning
- MachineLearning
- Mixture Of Experts
- Model Convergence
- Policy Optimization
- PPO
- Qwen3
- Reinforcement Learning
- RFT
- RLHF
- Routing Replay
- Scaling Analysis
- Sequence Level Sampling
- Token Level Sampling
- Variance Reduction
- AI
- Machine Learning
- Community
section_names:
- ai
- ml
primary_section: ai
---
MarketingNetMind compares GSPO and GRPO, two reinforcement learning approaches for LLM fine-tuning, examining their variance, scalability, and real-world results in Mixture-of-Experts models.<!--excerpt_end-->

# GSPO vs. GRPO: Sequence-Level Reinforcement Learning for LLM Fine-Tuning

**Author:** MarketingNetMind

The Qwen team's recent proposal, Group Sequence Policy Optimization (GSPO), offers a new approach to reinforcement learning for post-training large language models (LLMs). GSPO is positioned as an alternative to Group Relative Policy Optimization (GRPO), which is used in DeepSeek, and aims to address the instability of token-level importance sampling in existing RL pipelines.

## Background

- **RLHF Methods:** Popular techniques like Proximal Policy Optimization (PPO) optimize LLMs using reward signals, often based on human feedback or verifiable metrics.
- **GRPO Extension:** DeepSeek's GRPO extends RLHF approaches by introducing sample-level value estimations but uses importance sampling at the token level.

## Issues with GRPO

- **Token-Level Sampling:** GRPO applies importance sampling to each token, resulting in high variance across long sequences.
- **MoE Instability:** In Mixture-of-Experts (MoE) models, token-level routing causes instability and can lead to model collapse unless mitigated with complex adjustments like Routing Replay.

### Key Concerns

- High variance due to per-token multiplication of importance weights
- Increased difficulty stabilizing training for MoE architectures
- Additional engineering requirements (e.g., Routing Replay) to prevent collapse

## GSPO’s Contributions

- **Sequence-Level Sampling:** GSPO instead uses sequence-level importance sampling, normalizing weights by the length of the sequence.
- **Variance Reduction:** This dramatically reduces variance in the optimization signal, reducing reliance on routing hacks.
- **Stable Convergence:** Qwen reports improved stability and scalability for MoE models, with faster convergence.

## Experimental Findings

- **Benchmarks:** On datasets such as AIME’24, LiveCodeBench, and CodeForces, GSPO shows stronger rewards curves than GRPO.
- **Scaling:** GSPO converges faster with scaled-up compute and presents smoother scaling trends.
- **Comparison:** While GRPO requires Routing Replay for performance, GSPO eliminates that dependence.

## Discussion Points

- Community members are invited to share experiences regarding instability with token-level importance sampling or GRPO.
- Sequence-level weighting like GSPO is proposed as a more robust alternative but further validation on multiple models is advised.

**Corrections and Definitions:**

- GSPO is a reinforcement fine-tuning (RFT) method, not strictly RLHF or RLVR unless specific types of rewards are used.
- RLHF involves rewards imitating human feedback; RLVR uses verifiable rewards; most current post-training adopts RLVR for scalability.

## Takeaways

- GSPO offers a promising alternative to existing RLHF/RFT approaches, especially for MoE models facing gradient instability.
- Stability and reduced variance may result from sequence-level evaluation, which addresses noise accumulation from tokenization.

Further reading, performance charts, and mathematical formulations can be found in the [Qwen Team's blog post](https://blog.netmind.ai/article/Qwen_Team_Proposes_GSPO_for_Qwen3%2C_Claims_DeepSeek).

> "Great analysis, the stability gains here are compelling... It makes me wonder if token-level instability in GRPO is a symptom of information loss in tokenization... GSPO's sequence-level approach feels more robust."

The discussion concludes with suggestions that more empirical results on diverse models are needed for wider adoption.

This post appeared first on "Reddit Machine Learning". [Read the entire article here](https://www.reddit.com/r/MachineLearning/comments/1mj3t3r/d_gspo_qwen3s_sequencelevel_rlhf_method_vs_grpo/)
