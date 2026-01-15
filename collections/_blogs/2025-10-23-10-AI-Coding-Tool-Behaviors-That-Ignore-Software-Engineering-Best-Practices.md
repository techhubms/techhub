---
layout: post
title: 10 AI Coding Tool Behaviors That Ignore Software Engineering Best Practices
author: Mike Vizard
canonical_url: https://devops.com/analysis-identifies-10-ai-coding-tool-behaviors-that-ignore-best-software-engineering-practices/
viewing_mode: external
feed_name: DevOps Blog
feed_url: https://devops.com/feed/
date: 2025-10-23 17:51:27 +00:00
permalink: /ai/blogs/10-AI-Coding-Tool-Behaviors-That-Ignore-Software-Engineering-Best-Practices
tags:
- AI
- AI Coding Tools
- Anti Patterns
- Application Security
- Automation
- Best Practices
- Blogs
- Code Quality
- Code Review
- Coding
- Continuous Integration
- DevOps
- Open Source
- OX Security
- Security
- Security Vulnerabilities
- Social Facebook
- Social LinkedIn
- Social X
- Software Engineering
- Unit Testing
section_names:
- ai
- coding
- devops
- security
---
Mike Vizard examines a report by Ox Security highlighting ten problematic behaviors exhibited by AI coding tools, focusing on their impact on software quality and DevOps security workflows.<!--excerpt_end-->

# 10 AI Coding Tool Behaviors That Ignore Software Engineering Best Practices

**Author:** Mike Vizard

An analysis by Ox Security of over 300 open source repositories uncovers ten recurring behaviors in AI-generated code that conflict with software engineering best practices. These patterns, identified through in-depth examination of AI coding tool outputs, raise significant concerns about code quality, maintainability, and security for DevOps teams.

## Key Behaviors Identified

1. **Comments Everywhere:** Overuse of inline comments, making code verbose and harder to review while increasing computational overhead.
2. **By-The-Book Fixation:** Rigid adherence to typical coding conventions, often missing chances for innovation or efficient solutions.
3. **Over-Specification:** Creation of highly specific, one-off solutions instead of reusable, modular code components.
4. **Avoidance of Refactors:** Failure to improve or refactor code, resulting in the persistence of suboptimal or outdated patterns.
5. **Bugs Déjà-Vu:** Repetition of identical bugs due to poor code reuse and lack of learning from previous mistakes.
6. **Worked on My Machine Syndrome:** Lack of awareness of deployment environments, leading to code that functions locally but fails in production.
7. **Return of Monoliths:** Tendency to develop tightly-coupled, monolithic architectures rather than favoring modern microservices.
8. **Fake Test Coverage:** Superficial unit tests that inflate coverage metrics without providing meaningful validation.
9. **Vanilla Style:** Preference for reinventing the wheel over leveraging established libraries or SDKs.
10. **Phantom Bugs:** Unnecessary over-engineering for rare edge cases, resulting in unwarranted complexity and resource waste.

The analysis notes that issues such as excessive commenting, by-the-book solutions, over-specification, and avoidance of refactoring appear in approximately 80% of reviewed code, whereas phantom bugs are less frequent.

## Implications for DevOps Teams

Eyal Paz, vice president of research at Ox Security, points out that while policies can help enforce best practices, DevOps teams must be vigilant in validating AI-generated code. The report warns that AI tools act much like junior developers, lacking contextual understanding and judgment and often generating code that requires senior developer review before release.

As more non-technical and junior developers use these tools, the risk of propagating poor coding practices increases. Unlike humans, AI coding tools do not naturally question instructions or review the validity of their output, sometimes producing flawed unit tests or inefficient solutions repeatedly.

## Security Considerations

While the report indicates that AI tools do not inherently create more security vulnerabilities compared to human developers, the volume of generated code can overwhelm review and remediation efforts, leaving gaps in security coverage. Ox Security recommends embedding security checks directly into AI-assisted coding workflows to tackle this scaling problem.

## Conclusion and Recommendations

DevOps and engineering teams should:

- Regularly review AI-generated code for anti-patterns
- Establish automated security and quality checks early in the pipeline
- Educate users of AI coding tools on best practices and common pitfalls
- Rely on experienced developers for code review and architectural oversight

The findings underscore the importance of combining AI-assisted development with strong human governance and integrated security practices.

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/analysis-identifies-10-ai-coding-tool-behaviors-that-ignore-best-software-engineering-practices/)
