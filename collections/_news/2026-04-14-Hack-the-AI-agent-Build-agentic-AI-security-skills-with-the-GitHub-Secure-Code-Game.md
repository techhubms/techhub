---
primary_section: ai
tags:
- Agentic AI
- Agentic AI Security
- AI
- AI & ML
- AI Security
- CLI
- Coding Training
- CVE
- Cybersecurity
- DevOps
- GitHub Codespaces
- GitHub Models
- GitHub Security Lab
- LLM Security
- MCP
- MCP Servers
- Memory Poisoning
- Multi Agent Workflows
- News
- OWASP Top 10 For Agentic Applications
- Prompt Injection
- Remote Code Execution
- Sandboxing
- Secure Code Game
- Secure Coding
- Secure Coding Training
- Security
- Tool Misuse
- Vulnerability Research
title: 'Hack the AI agent: Build agentic AI security skills with the GitHub Secure Code Game'
external_url: https://github.blog/security/hack-the-ai-agent-build-agentic-ai-security-skills-with-the-github-secure-code-game/
author: Joseph Katsioloudes
feed_name: The GitHub Blog
date: 2026-04-14 18:17:59 +00:00
section_names:
- ai
- devops
- security
---

Joseph Katsioloudes introduces Season 4 of GitHub’s Secure Code Game, a hands-on set of challenges where you exploit and fix vulnerabilities in an agentic AI assistant (ProdBot) to learn real-world AI-agent security risks like prompt-based tool misuse, memory poisoning, and sandbox escape.<!--excerpt_end-->

# Hack the AI agent: Build agentic AI security skills with the GitHub Secure Code Game

Season 4 of the **Secure Code Game** focuses on **agentic AI security**: AI systems that don’t just generate text, but can **browse the web, call tools/APIs, execute commands, store memory, and coordinate multiple agents**.

The game is free, open source, runs in-editor/in a dev environment, and is designed as “exploit and fix” training: players intentionally break vulnerable code and then harden it.

## What the Secure Code Game is

The **Secure Code Game** is a free, open source course where you:

- exploit intentionally vulnerable code
- fix it while keeping functionality intact
- progress through “seasons” of challenges

Background from prior seasons:

- **Season 1 (March 2023):** establish a “secure code mindset” by fixing vulnerabilities.
- **Season 2:** expanded into multi-stack challenges across **JavaScript, Python, Go, and GitHub Actions**, with community contributions.
- **Season 3:** focused on **LLM security** (craft malicious prompts, then defend against them).
- Across seasons: **10,000+ developers** have played.

## Why agentic AI security matters

The post argues that agentic systems have quickly moved from prototypes to production, while security readiness lags.

It references:

- **OWASP Top 10 for Agentic Applications 2026** (risks like goal hijacking, tool misuse, identity abuse, memory poisoning)
- A **Dark Reading** poll: **48%** of cybersecurity professionals believe agentic AI will be the top attack vector by end of 2026
- **Cisco’s State of AI Security 2026** report: **83%** planned to deploy agentic capabilities; only **29%** felt ready to deploy securely

The takeaway: the adoption/readiness gap is where vulnerabilities thrive, and practicing offensive thinking helps close it.

## Meet ProdBot (Season 4)

Season 4 puts you into **ProdBot**, a deliberately vulnerable “productivity bot” that behaves like an agentic assistant inside a terminal.

It’s inspired by tools like:

- [OpenClaw](https://openclaw.ai/)
- [GitHub Copilot CLI](https://github.com/features/copilot/cli)

ProdBot capabilities include:

- turning natural language into **bash commands**
- browsing a **simulated web**
- connecting to **MCP (Model Context Protocol) servers** (external tool providers)
- running **org-approved skills** (prebuilt automations)
- storing **persistent memory**
- orchestrating **multi-agent workflows**

### The objective

Across five levels, your goal is to get ProdBot to reveal a secret it should not expose:

- if you can read `password.txt`, you’ve found a vulnerability

No AI or coding background is required; interaction is through natural language prompts in the CLI.

## Five levels, five expanding attack surfaces

Each level adds capabilities and introduces a new class of risk:

- **Level 1:** ProdBot generates and executes bash commands in a sandbox.
  - Goal: try to **break out of the sandbox**.
- **Level 2:** adds web access to a simulated internet.
  - Focus: what happens when an agent reads **untrusted content**.
- **Level 3:** connects to **MCP servers** (external tools like stock quotes, browsing, cloud backup).
  - More tools means more ways for attackers to influence behavior.
- **Level 4:** adds **org-approved skills** and **persistent memory**.
  - Trust is layered; the question is whether it’s justified.
- **Level 5:** a combined environment with multiple components.
  - **Six specialized agents**, **three MCP servers**, **three skills**, and a simulated OSS project web.
  - The platform claims sandboxing and “pre-verified” data; you test whether that holds.

The post does not reveal exact solutions to avoid spoilers, but states the attack patterns map to real problems security teams are facing with autonomous agents.

## Real-world motivation: “ClawBleed”

An example given is **CVE-2026-25253** (CVSS 8.8 – High), nicknamed **“ClawBleed”**, described as a one-click **RCE** vulnerability that enabled token theft via a malicious link and full control of an OpenClaw instance.

## How to start (and what it costs)

Season 4 runs in **GitHub Codespaces**, so you don’t need local installation.

- Start in under ~2 minutes
- Free to play
- Codespaces offers up to **60 hours/month** of free usage

It also uses **GitHub Models** (with rate limits):

- Rate limits: https://docs.github.com/github-models/prototyping-with-ai-models#rate-limits
- Responsible use guidance: https://docs.github.com/en/github-models/responsible-use-of-github-models

Start here:

- https://securitylab.github.com/secure-code-game/

## FAQ (as stated)

- **Do I need AI or coding experience?** No; everything happens through natural language in the CLI.
- **Do I need previous seasons?** No; seasons are self-contained. Season 3 can help as a foundation.
- **How long does it take?** About two hours (varies).
- **Is it free?** Yes.
- **What about rate limits?** Season 4 uses GitHub Models; if you hit limits, wait for reset and resume.

## Credits

Special thanks:

- [Rahul Zhade](https://github.com/rzhade3)
- [Bartosz Gałek](https://github.com/bgalek)


[Read the entire article](https://github.blog/security/hack-the-ai-agent-build-agentic-ai-security-skills-with-the-github-secure-code-game/)

