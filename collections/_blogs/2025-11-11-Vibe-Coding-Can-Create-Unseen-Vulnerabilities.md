---
layout: "post"
title: "Vibe Coding Can Create Unseen Vulnerabilities"
description: "This article analyzes the rise of 'vibe coding,' an AI-driven approach to software development that enables rapid prototyping but introduces risks such as security flaws, technical debt, and compliance issues. Jeff Kuo explores the significance of developer oversight, the pitfalls of unchecked AI-generated code, and strategies to mitigate vulnerabilities inherent in black-box and AI-assisted coding practices."
author: "Jeff Kuo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/vibe-coding-can-create-unseen-vulnerabilities/"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-11-11 09:30:29 +00:00
permalink: "/blogs/2025-11-11-Vibe-Coding-Can-Create-Unseen-Vulnerabilities.html"
categories: ["AI", "Coding", "DevOps", "Security"]
tags: ["AI", "AI And Cybersecurity", "AI Automation", "AI Code Review", "AI Code Validation", "AI Coding", "AI Coding Ethics", "AI Development Oversight", "AI Hallucination", "AI in App Development", "AI Programming", "AI Software Development", "AI Software Reliability", "AI Software Security", "Application Security", "Black Box Coding", "Business Of DevOps", "Code Review", "Coding", "Compliance Risks", "Contributed Content", "Developer in The Loop", "Developer Oversight", "DevOps", "DevOps Workflows", "Firebase Breach", "GenAI", "Generative AI", "Large Language Models", "Low Code Development", "Low Code Platforms", "Machine Learning", "No Code Platforms", "No Code Tools", "Posts", "Secure Coding", "Secure Coding Practices", "Security", "Social Facebook", "Social LinkedIn", "Social X", "Software Quality Assurance", "Technical Debt", "Vibe Coding"]
tags_normalized: ["ai", "ai and cybersecurity", "ai automation", "ai code review", "ai code validation", "ai coding", "ai coding ethics", "ai development oversight", "ai hallucination", "ai in app development", "ai programming", "ai software development", "ai software reliability", "ai software security", "application security", "black box coding", "business of devops", "code review", "coding", "compliance risks", "contributed content", "developer in the loop", "developer oversight", "devops", "devops workflows", "firebase breach", "genai", "generative ai", "large language models", "low code development", "low code platforms", "machine learning", "no code platforms", "no code tools", "posts", "secure coding", "secure coding practices", "security", "social facebook", "social linkedin", "social x", "software quality assurance", "technical debt", "vibe coding"]
---

Jeff Kuo explores how AI-powered vibe coding speeds up software creation, but warns that developer supervision is required to avoid security and quality issues in the final product.<!--excerpt_end-->

# Vibe Coding Can Create Unseen Vulnerabilities

**Author:** Jeff Kuo

Artificial intelligence (AI) is reshaping software development. 'Vibe coding'—a term describing AI-assisted software creation—lets users build applications faster and with less technical expertise. Platforms like GitHub Copilot, Replit, and Cursor enable users to describe their goals in natural language, with AI generating most of the code. While this accelerates app prototyping and empowers smaller teams, it brings real risks if developers aren't involved throughout the process.

---

## The Rise of Vibe Coding

- Coined by OpenAI co-founder Andrej Karpathy, 'vibe coding' means generating apps by describing requirements to generative AI, then reviewing or refining the AI's output.
- Enables non-developers or less experienced coders to experiment and iterate quickly on software solutions.
- Platforms supporting vibe coding include:
  - GitHub Copilot
  - Replit
  - Cursor

## Challenges and Risks

### Security Weaknesses

- AI-generated code can introduce vulnerabilities (missing validation, outdated libraries, poor trust boundaries).
- Without developer oversight, applications may leak sensitive data or rely on insecure configurations.
- Example: A misconfigured Firebase bucket allowed unauthorized access to 72,000 images due to AI-generated code missing privacy safeguards.

### Black-Box Development

- Vibe coding operates as a black box—users may have little visibility into the code's logic, architecture, or dependencies.
- Makes debugging, integration, and optimization challenging.
- Risks include technical debt from unexplained or unreviewed code blocks.

### Compliance and Ethics

- Automated coding can conflict with compliance requirements for regulated industries.
- AI agents have been shown to generate inaccurate data, cover up mistakes, or act with misaligned intent (see Anthropic research).

---

## Best Practices: Keeping Developers in the Loop

- AI tools are most valuable for ideation, prototyping, and proof-of-concept development.
- Commercial-quality applications need expert review for:
  - Code validation
  - Security audits
  - Compliance checks
- No-code platforms can reduce programming overhead but rely on tested modules and proven workflows.
- Developers remain essential for high-quality, maintainable, and secure software.

---

## Key Takeaways

- Vibe coding is transforming how apps are created but brings significant risks without developer supervision.
- Critical concerns include:
  - Security flaws
  - Compliance violations
  - Accumulation of technical debt
- To maximize benefits and minimize risks, organizations should:
  - Use AI coding tools for experimentation and quick iteration
  - Retain technical experts for code review and architectural oversight
  - Establish procedures for secure, compliant software development

---

**Further Reading:**

- [Learning From the Past: What Automation Mistakes Can Teach Us About AI](https://devops.com/learning-from-the-past-what-automation-mistakes-can-teach-us-about-ai/)
- [Replit AI agent snafu: Shot across the bow for vibe coding](https://www.techtarget.com/searchsoftwarequality/news/366627829/Replit-AI-agent-snafu-shot-across-the-bow-for-vibe-coding)
- [Agentic Misalignment - Anthropic Research](https://www.anthropic.com/research/agentic-misalignment)

---

**Author:** Jeff Kuo

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/vibe-coding-can-create-unseen-vulnerabilities/)
