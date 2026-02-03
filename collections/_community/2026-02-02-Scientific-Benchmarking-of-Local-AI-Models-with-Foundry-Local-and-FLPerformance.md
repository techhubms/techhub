---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/benchmarking-local-ai-models/ba-p/4490780
title: Scientific Benchmarking of Local AI Models with Foundry Local and FLPerformance
author: Lee_Stott
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-02 08:00:00 +00:00
tags:
- AI
- Benchmarking
- Community
- Concurrency
- FLPerformance
- Latency
- Microsoft Foundry Local
- Model Evaluation
- Node.js
- OpenAI
- Performance Testing
- Prompt Engineering
- React
- SDK Integration
- Statistical Analysis
- Tokens Per Second
- TTFT
- Visualization
section_names:
- ai
---
Lee Stott presents a practical walkthrough for developers on scientifically benchmarking local AI models using Microsoft Foundry Local, FLPerformance, and modern JavaScript frameworks. The article guides you from architecture to results interpretation.<!--excerpt_end-->

# Scientific Performance Measurement with Foundry Local

## Introduction

Choosing the right AI model for your application means more than just comparing benchmarks—real-world needs like latency, hardware constraints, and output quality must be tested on your actual systems. This guide walks through building a comprehensive benchmarking platform using [FLPerformance](https://github.com/leestott/FLPerformance), Node.js, React, and Microsoft Foundry Local, enabling scientific measurement and informed model selection.

## Why Model Benchmarking Requires Purpose-Built Tools

Manual or ad hoc testing doesn’t cut it for real-world reliability. True benchmarking involves:

- Controlled, repeatable test conditions
- Measuring latency, throughput, quality, and hardware impact across multiple sessions and loads
- Statistical analysis over many iterations (p50, p95, p99 percentiles)
- Testing real workloads, not just academic prompts

The article details why this depth is necessary for meaningful comparisons and highlights common benchmarking pitfalls.

## Architecture Overview: The Three-Layer Platform

The platform separates concerns for scalability and clarity:

- **Frontend (React):** Model management, benchmark configuration, test execution, and rich results visualizations (tables, charts, recommendations). Models are loaded directly from the [Foundry Local catalog](https://foundrylocal.ai).
- **Backend (Node.js/Express):** Orchestrates test runs, loads/unloads models, manages concurrency, and persists results. FLPerformance acts as the main orchestrator, handling metrics and passing data to the frontend via WebSockets for real-time updates.
- **SDK (Foundry Local):** Handles service management, model lifecycle, and provides OpenAI-compatible APIs, ensuring consistent and comparable measurements for all models.

## Scientific Measurement Infrastructure

Code examples demonstrate how to:

- Measure **Time to First Token (TTFT)** for perceived responsiveness
- Track **Time Per Output Token (TPOT)** for throughput
- Compute **total latency** and **tokens per second**
- Capture errors and calculate statistical distributions (mean, p95, etc.)
- Minimize measurement overhead to avoid skewing results

The provided `BenchmarkExecutor` code shows real-world patterns for collecting, storing, and inspecting results.

## Designing Custom Benchmark Suites

Generic prompts don’t reflect production requirements. The FLPerformance platform allows custom suite design:

- Mix short-form, reasoning, code generation, creative, and factual prompts
- Add workload-specific suites (e.g., customer-support)
- Specify prompt categories and expected token counts for meaningful throughput comparisons

Tailoring suites to your real application scenarios gives data you can trust when making tradeoffs.

## Data Visualization and Recommendations

Insight comes from visualizing data across models:

- **Comparison tables** enumerate TTFT, TPOT, throughput, error rates, and p95 latencies side-by-side
- **Latency charts** make outliers and variance obvious
- **Recommendations engine** synthesizes results, proposing 'fastest', 'best throughput', and 'most consistent' models for various priorities

Sample React/Chart.js snippets illustrate how to display benchmark data for easy interpretation.

## Best Practices and Takeaways

The article distills key practices:

- Always test on target hardware
- Use multi-dimensional, statistically robust metrics
- Create application-specific benchmarks
- Warm up models before measurement
- Scale tests for concurrency to match production scenarios
- Document all test methodologies for repeatability

## Resources

- [FLPerformance GitHub](https://github.com/leestott/FLPerformance)
- [Foundry Local documentation](https://foundrylocal.ai)
- Quick Start, Architecture, and Benchmarking guides linked in the article

## Conclusion

With FLPerformance and Microsoft Foundry Local, rigorous model performance analysis is within any developer team's reach—with code and methodology ready to adapt to any AI deployment scenario.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/benchmarking-local-ai-models/ba-p/4490780)
