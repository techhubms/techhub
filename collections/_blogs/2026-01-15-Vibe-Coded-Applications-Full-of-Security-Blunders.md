---
external_url: https://devclass.com/2026/01/15/vibe-coded-applications-full-of-security-blunders/
title: Vibe Coded Applications Full of Security Blunders
author: Tim Anderson
feed_name: DevClass
date: 2026-01-15 17:14:53 +00:00
tags:
- AI Agent
- AI Code Generation
- AI/ML
- Application Security
- Authorization Logic
- Business Logic Bugs
- Claude
- Claude Code
- Cursor
- Devin
- Open AI Codex
- OpenAI Codex
- Replit
- Secure Coding
- Security Best Practices
- Security Vulnerabilities
- Server Side Request Forgery
- Tenzai
- Vibe Coding
section_names:
- ai
- security
primary_section: ai
---
Tim Anderson reports on Tenzai's research led by Ori David, highlighting how applications built with 'vibe coding' using AI agents like Claude and Codex tend to be insecure due to common flaws and overlooked best practices.<!--excerpt_end-->

# Vibe Coded Applications Full of Security Blunders

Tim Anderson reviews recent research into the security pitfalls of 'vibe coding,' a style where developers rely on AI coding agents to write code based primarily on natural language prompts. Ori David from startup Tenzai conducted an experiment, using five prominent coding agents—Cursor, Claude Code, OpenAI Codex, Replit, and Devin—to build three applications from the same detailed prompts.

## Summary of Findings

- **Frequent Vulnerabilities:** Every agent produced similar numbers of vulnerabilities, with Claude, Devin, and Codex generating critical flaws.
- **Example Flaw:** A highlighted PHP code sample from Claude failed to enforce authentication reliably before deleting products, potentially allowing unauthorized actions.
- **Logic and Business Rule Weaknesses:** Agents struggled most with authorization logic and business logic bugs, such as permitting negative orders or prices in e-commerce scenarios.
- **Common Problems:** Issues included server-side request forgery (SSRF) vulnerabilities and a lack of best-practice security controls (like missing security headers).
- **Unskilled Developer Risk:** 'Vibe coding' lowers the technical barrier for application building, raising the risk that code will go live with significant security problems if not reviewed by experienced developers.

## Remediation and Human Oversight

- **Role of AI in Security:** While some vendors, like Tenzai, are developing AI agents to find and fix such vulnerabilities, the research notes that AI-reported gaps remain in what AI alone can catch.
- **Importance of Human Review:** Thorough code review processes by skilled developers are more likely to catch nuanced or business-specific security issues.
- **Organizational Pressures:** Tight deadlines or resource constraints may push developers to bypass best security practices, increasing risks especially when combined with heavy reliance on AI-generated code.

## Key Takeaways

- AI coding agents are inconsistent in generating secure code, especially regarding complex authorization and business rules.
- Human oversight remains crucial for robust security, regardless of AI involvement.
- As AI tools become more accessible, organizations must reinforce the need for strong review and secure coding practices to avoid introducing dangerous flaws.

---
**References:**

- [Tenzai Blog: Bad Vibes — Comparing the secure coding capabilities of popular coding agents](https://blog.tenzai.com/bad-vibes-comparing-the-secure-coding-capabilities-of-popular-coding-agents/)
- [Bad Vibes Prompts Dataset (GitHub)](https://github.com/TenzaiLabs/datasets/tree/main/bad_vibes_prompts)
- [MDN: Server Side Request Forgery](https://developer.mozilla.org/en-US/docs/Web/Security/Attacks/SSRF)

For further reading, the article also points to related news on security tooling, software agent ecosystems, and updates from other AI development platforms.

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2026/01/15/vibe-coded-applications-full-of-security-blunders/)
