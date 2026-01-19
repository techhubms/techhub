---
external_url: https://devops.com/sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code
title: SonarSource Highlights Security Risks and Code Quality Issues in LLM-Generated Code
author: Mike Vizard
viewing_mode: external
feed_name: DevOps Blog
date: 2025-08-13 13:43:07 +00:00
tags:
- AI Coding Tools
- Analysis
- Business Of DevOps
- Claude Sonnet 4
- Code Quality
- Code Review
- Code Smells
- DevSecOps
- GPT 4o
- Hard Coded Credentials
- Java
- Large Language Models
- Llama 3.2
- LLMs
- Messy Code
- Path Traversal
- Security Vulnerabilities
- Social Facebook
- Social LinkedIn
- Social X
- SonarSource
- Technical Debt
section_names:
- ai
- devops
- security
---
Mike Vizard summarizes SonarSource's analysis of LLM-generated code, revealing frequent security vulnerabilities and long-term code quality issues. The article urges DevOps teams to be vigilant when leveraging AI coding tools.<!--excerpt_end-->

# SonarSource Highlights Security Risks and Code Quality Issues in LLM-Generated Code

**Author:** Mike Vizard

SonarSource has released a comprehensive analysis of code produced by various large language models (LLMs) such as OpenAI’s GPT-4o, Anthropic’s Claude Sonnet 4 and 3.7, Meta’s Llama-3.2-vision:90b, and OpenCoder-8B. The study evaluated more than 4,400 Java programming assignments using the company's proprietary analysis framework.

## Key Findings

- **High Functionality, High Risk:** LLMs like Claude Sonnet 4 achieved up to a 95.57% success rate on HumanEval, reflecting their advanced ability to generate executable, correct code for a range of problems.
- **Frequent Security Flaws:** Despite their capabilities, all models consistently produced code with significant vulnerabilities, such as hard-coded credentials and path traversal issues. For example, more than 70% of vulnerabilities in Llama-3.2-vision:90b output were rated 'blocker' severity; GPT-4o showed 62.5%, and Claude Sonnet 4 nearly 60% with similar severity.
- **Trade-offs Between Accuracy and Risk:** Improved functional benchmarks (e.g., performance gains in Claude Sonnet 4 over its predecessor) often led to increased rates of severe security bugs—up 93% in some cases.
- **Poor Maintainability:** Over 90% of identified issues were 'code smells'—indicators of poor structure, redundancy, or low maintainability, contributing to long-term technical debt.
- **Model Personality and Bias:** Each LLM exhibited a distinct 'coding personality.' Developers are urged to understand and account for the biases and verbosity tendencies embedded in each model.

## DevOps and Security Implications

Prasenjit Sarkar of SonarSource stressed the need for DevOps teams to thoroughly review LLM-generated code and to recognize both the strengths and weaknesses inherent in these AI tools. He notes that messy or verbose code is not only difficult to debug and fix, but may also require separate AI agents to assist in reviewing and maintaining code quality.

## Trust and Productivity

While LLM coding tools can boost developer productivity, the article cautions that these short-term gains may lead to hidden costs in future security incidents or maintenance challenges. It remains uncertain to what extent DevOps teams trust and widely adopt these AI-based coding solutions.

## Recommendations

- Always review, test, and secure code output from LLMs before deployment.
- Educate teams about risks of hard-coded secrets and code smells in generated code.
- Balance productivity benefits with long-term quality and security considerations.

_Sources: [SonarSource report](https://www.sonarsource.com/company/press-releases/the-coding-personalities-of-leading-llms/) | [Original Article](https://devops.com/sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/)_

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code)
