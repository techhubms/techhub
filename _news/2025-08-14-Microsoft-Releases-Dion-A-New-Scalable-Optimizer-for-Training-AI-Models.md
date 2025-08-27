---
layout: "post"
title: "Microsoft Releases Dion: A New Scalable Optimizer for Training AI Models"
description: "This news article introduces Dion, a distributed optimizer developed by Microsoft to address the scalability and efficiency challenges in training large AI models. Dion builds on orthonormal update techniques to enable faster and more effective model training while minimizing compute and communication costs. The article explains the concept of orthonormal updates, how Dion differs from predecessors like AdamW and Muon, empirical results highlighting Dion’s performance benefits at scale, and provides resources for further exploration, including the open-source Dion repository."
author: "stclarke"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/research/blog/dion-the-distributed-orthonormal-update-revolution-is-here/"
viewing_mode: "external"
feed_name: "Microsoft News"
feed_url: "https://news.microsoft.com/source/feed/"
date: 2025-08-14 15:28:24 +00:00
permalink: "/2025-08-14-Microsoft-Releases-Dion-A-New-Scalable-Optimizer-for-Training-AI-Models.html"
categories: ["AI"]
tags: ["AdamW", "AI", "AI Model Training", "Company News", "Deep Learning", "Dion Optimizer", "Distributed Training", "FSDP", "Large Language Models", "LLaMA 3", "Machine Learning", "Microsoft Research", "Muon Optimizer", "News", "Optimization Algorithms", "Orthonormal Updates", "PyTorch", "Scale Efficiency", "Tensor Parallelism"]
tags_normalized: ["adamw", "ai", "ai model training", "company news", "deep learning", "dion optimizer", "distributed training", "fsdp", "large language models", "llama 3", "machine learning", "microsoft research", "muon optimizer", "news", "optimization algorithms", "orthonormal updates", "pytorch", "scale efficiency", "tensor parallelism"]
---

This update from Microsoft Research explains Dion, a new distributed optimizer for efficient large-scale AI model training, and credits the work of the Dion team and Microsoft collaborators.<!--excerpt_end-->

# Microsoft Releases Dion: A New Scalable Optimizer for Training AI Models

Microsoft Research has introduced Dion, a distributed optimizer open-sourced to help accelerate the training of large AI models. Dion builds on newer optimization methods to address the limitations of popular approaches like AdamW and Muon, excelling at scale with minimal communication and compute costs.

## Background and Motivation

AI model training commonly relies on optimizers like Adam and AdamW. While effective, their performance at very large scales can be limiting due to inefficiencies in updating weight matrices, especially during distributed training. Recent developments such as the Muon optimizer showed strong improvements but required heavy matrix multiplication and inter-node communication, slowing training for billion-parameter models.

## What Makes Dion Different?

Dion introduces **orthonormal updates**, a method that equalizes how a weight matrix update affects model activations across all input directions. This is achieved by enforcing *orthonormality* on the update matrix, rather than relying on the most conservative learning rate. Whereas Muon required full orthonormalization (and associated computational cost), Dion orthonormalizes only the top _r_ singular vector space—*rank approximation*—which reduces overhead yet maintains performance. This rank can remain small relative to model size, enabling scalability.

### Technical Highlights

- **Amortized Power Iteration**: Duke exploits repeated matrix multiplications spread over optimization steps, reducing the per-step cost to just two multiplications.
- **QR Decomposition**: Used to extract an orthonormal basis covering the top singular directions, compatible with distributed implementations (e.g., FSDP and tensor parallelism).
- **Error Feedback**: Residual error from lower-rank approximation accumulates in the momentum matrix for later application, improving training accuracy over time.

## Empirical Results

Microsoft’s experiments show that while Dion offers negligible benefits at small scales, at the scale of billions of parameters (e.g., LLaMA-3 405B), Dion outperforms Muon and AdamW—especially with large batch sizes. Even with aggressive rank reduction (e.g., 1/16 or 1/64th of full rank), Dion retains its speed and effectiveness.

Performance illustrations:

- Wall-clock time per optimizer step is reduced by an order of magnitude for the largest models.
- Qualitative improvements extend to both pretraining and post-training phases.

## Open Source Availability

- Dion is available on GitHub: [Dion Optimizer](https://github.com/microsoft/dion/)
- The package includes FSDP2 + Tensor Parallel (TP) implementations for PyTorch, as well as Muon reference implementations.

## Further Resources

- [Microsoft Dion research blog](https://www.microsoft.com/en-us/research/blog/dion-the-distributed-orthonormal-update-revolution-is-here/)
- [Microsoft research copilot experience](https://aka.ms/research-copilot/?OCID=msr_researchforum_Copilot_MCR_Blog_Promo) – Explore more AI-powered tools.

## Acknowledgements

Thanks to Riashat Islam and Pratyusha Sharma for feedback on writing and presentation.

---

> *Dion is now open-sourced to enable the global AI research community to train models more efficiently, making large-scale AI model development faster and more accessible.*

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/research/blog/dion-the-distributed-orthonormal-update-revolution-is-here/)
