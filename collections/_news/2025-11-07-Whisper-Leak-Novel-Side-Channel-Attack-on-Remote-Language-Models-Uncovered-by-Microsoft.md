---
layout: post
title: 'Whisper Leak: Novel Side-Channel Attack on Remote Language Models Uncovered by Microsoft'
author: Microsoft Defender Security Research Team, Jonathan Bar Or and Geoff McDonald
canonical_url: https://www.microsoft.com/en-us/security/blog/2025/11/07/whisper-leak-a-novel-side-channel-cyberattack-on-remote-language-models/
viewing_mode: external
feed_name: Microsoft Security Blog
feed_url: https://www.microsoft.com/en-us/security/blog/feed/
date: 2025-11-07 17:00:00 +00:00
permalink: /ai/news/Whisper-Leak-Novel-Side-Channel-Attack-on-Remote-Language-Models-Uncovered-by-Microsoft
tags:
- AI Security
- BERT
- Cyberattack Mitigation
- Data Confidentiality
- Defender Security Research
- Encryption
- Language Models
- LightGBM
- LSTM
- Machine Learning
- Microsoft Azure
- Network Security
- Obfuscation
- OpenAI
- Packet Size Analysis
- Privacy
- Side Channel Attack
- Streaming Model
- TLS
- Whisper Leak
section_names:
- ai
- security
---
Microsoft Defender Security Research Team, with Jonathan Bar Or and Geoff McDonald, unveils Whisper Leak: a side-channel attack that reveals AI language model conversation topics by analyzing encrypted traffic, and covers new mitigation measures in response.<!--excerpt_end-->

# Whisper Leak: Novel Side-Channel Attack on Remote Language Models Uncovered by Microsoft

## Executive Summary

Microsoft's Defender Security Research Team has uncovered a new threat—Whisper Leak—a side-channel attack on remote language models. This cyberattack can expose which topics users discuss with AI chatbots, even though their communications are protected by end-to-end encryption (TLS). The article details the attack methodology, the risks it introduces, and how industry partners are responding.

## What Is the Attack?

The Whisper Leak attack leverages patterns in encrypted network traffic—specifically, packet sizes and timings—to infer the subject matter of conversations with AI-powered chatbots. Attackers positioned to monitor network traffic (e.g., ISPs, local network participants, or compromised Wi-Fi routers) can apply machine learning to analyze these patterns and classify topics, potentially undermining privacy for users, including those in sensitive environments.

## Background on Language Model Communications

- **AI chatbots increasingly handle sensitive data.**
- **Communication is encrypted using HTTPS (TLS).**
- Language models stream tokens individually for immediate feedback. This streaming nature results in distinctive packet and timing sequences, even after encryption.

## The Cryptographic Context

- TLS uses both symmetric and asymmetric cryptography (e.g., RSA, ECDH, AES, ChaCha20).
- The ciphertext length mostly matches plaintext length, except for headers and authentication codes. Thus, traffic analysis can reveal information despite encryption.

## Prior Side-Channel Attacks

Whisper Leak builds upon recent and concurrent works, including:

- Token length leaks (USENIX Security 2024): Inferring output content based on packet size.
- Timing attacks (Carlini & Nasr, 2024): Using token production timing to identify topics.
- Token count and cache sharing attacks (Zhang et al., Zheng et al., 2024).

## The Whisper Leak Experiment

- Binary classifiers (LightGBM, Bi-LSTM, BERT-based) trained to distinguish conversations on a sensitive topic (“money laundering”) from random queries.
- Used 100 prompt variants (positive) and 11,716 unrelated questions (negative).
- Measured Area Under Precision-Recall Curve (AUPRC) to evaluate success.
- Many tested models achieved over 98% accuracy in this controlled setup, indicating real risk to privacy.

### Real-World Implications

- In monitoring scenarios with heavy class imbalance (10,000 conversations, 1 sensitive topic), attackers continued to achieve high precision and recall.
- This means surveillance entities could reliably identify users asking about banned or sensitive topics even with encrypted messaging.

### Risk Evolves with Data

- Effectiveness of the attack grows as adversaries collect more traffic for training.
- Sophisticated targeting (multi-turn, multi-session) only increases the risk.

## Mitigation and Industry Response

- Responsible disclosure led to OpenAI, Microsoft (Azure), and others deploying mitigations.
- Microsoft Azure streaming API now includes an 'obfuscation' field—randomly varying text in each response—to mask real output lengths and timings.
- Other vendors (Mistral, xAI) added similar protections (e.g., parameter "p").
- These mitigations drastically reduced the attack's effectiveness.

## What Can Users Do?

- Avoid sensitive queries via AI chatbots on untrusted networks.
- Utilize VPNs to further obfuscate network activity.
- Prefer providers who have implemented mitigation measures.
- Choose non-streaming model endpoints if privacy is paramount.
- Regularly update on provider security measures.

## Access Source Code and Technical Report

- Whisper Leak models and collection code: [Whisper Leak repository](http://github.com/yo-yo-yo-jbo/whisper_leak)
- Full technical whitepaper: [arXiv report](https://arxiv.org/abs/2511.03675)

## Learn More & Further Reading

- Stay updated via [Microsoft Security blog](https://www.microsoft.com/security/blog/), LinkedIn, and X.
- Join Microsoft Ignite for latest AI security sessions: [Register here](https://aka.ms/MSIgnite_Blog_Security_Generic)

---
**Authors**: Microsoft Defender Security Research Team (Jonathan Bar Or, Geoff McDonald)

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/11/07/whisper-leak-a-novel-side-channel-cyberattack-on-remote-language-models/)
