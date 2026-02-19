---
layout: "post"
title: "TypeScript Inventor Anders Hejlsberg on AI, Native Compiler Port, and the Future of Development Tools"
description: "This article summarizes an interview with Microsoft Technical Fellow Anders Hejlsberg, focusing on his views about AI’s role in programming, TypeScript’s performance evolution, and challenges in language and tooling choices. Hejlsberg discusses difficulties using AI in critical code migrations, the impact of large language models on development workflows, and future directions for TypeScript and development environments."
author: "DevClass.com"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.devclass.com/ai-ml/2026/01/28/typescript-inventor-anders-hejlsberg-ai-is-a-big-regurgitator-of-stuff-someone-has-done/4079582"
viewing_mode: "external"
feed_name: "DevClass"
feed_url: "https://devclass.com/feed/"
date: 2026-01-28 17:44:39 +00:00
permalink: "/2026-01-28-TypeScript-Inventor-Anders-Hejlsberg-on-AI-Native-Compiler-Port-and-the-Future-of-Development-Tools.html"
categories: ["AI", "Coding"]
tags: ["AI", "Anders Hejlsberg", "Blogs", "C#", "Code Migration", "Coding", "Development Tools", "Go", "IDE", "Language Services", "Large Language Models", "Microsoft", "Native Compilation", "Programming Languages", "Semantic Analysis", "TypeScript", "TypeScript Compiler"]
tags_normalized: ["ai", "anders hejlsberg", "blogs", "csharp", "code migration", "coding", "development tools", "go", "ide", "language services", "large language models", "microsoft", "native compilation", "programming languages", "semantic analysis", "typescript", "typescript compiler"]
---

DevClass.com presents insights from their interview with Anders Hejlsberg, focusing on the intersection of AI, programming languages, and the evolution of TypeScript tooling.<!--excerpt_end-->

# TypeScript Inventor Anders Hejlsberg on AI, Native Compiler Port, and the Future of Development Tools

**Author:** DevClass.com

## Introduction

Microsoft technical fellow Anders Hejlsberg, the creator of C# and TypeScript, discusses major shifts in programming and development influenced by AI, the evolution of TypeScript, and language tooling decisions.

## AI and Programming Languages

Hejlsberg characterizes AI as "a big regurgitator of stuff someone has done, with some extrapolation on top" and observes that established programming languages are best suited for AI-driven coding because they provide rich training data. He reflects on his team’s experience trying to use AI for porting code from TypeScript to Go—finding that AI failed to deliver the determinism required for such a critical migration task, with hallucinations in generated code necessitating extensive manual review.

Hejlsberg suggests that AI is better employed to create specialized migration utilities (which are then run deterministically) than to directly convert code between languages.

## Evolving the TypeScript Compiler

The TypeScript team is transitioning its compiler to native code for substantial performance gains, moving away from the original TypeScript/V8 implementation with TypeScript 7.0. Hejlsberg states the conservative function-by-function port strategy preserves semantics and minimizes surprises during migration.

### Language Choice: Why Not C# or Rust?

Choices for the new compiler language included Rust and C#. Rust was ruled out due to its stricter data structure model and lack of automatic garbage collection. Although C# was considered, the ultimate decision was to use Go, which offered closer alignment to JavaScript’s programming style and supported the shared memory concurrency needed for the project.

Hejlsberg acknowledges criticism from the C# community but emphasizes selecting the best tool for the job.

## AI’s Future Role in Development Tools

AI’s role is growing beyond being a simple assistant in the integrated development environment (IDE). According to Hejlsberg, AI is advancing towards supervising and automating programming tasks, relying less on traditional IDE patterns. This includes the integration of language services with emerging Microsoft development platforms (referred to as MCP), moving toward AI-driven code understanding, refactoring, and semantic queries.

Hejlsberg notes successful use of AI for post-port code migrations between repositories, particularly for incoming pull requests, indicating progress in using AI supervisors for incremental development tasks.

## Outlook: TypeScript’s Future

Future changes to TypeScript will likely track JavaScript's ongoing standardization, with gradual enhancements of its type system rather than disruptive changes. Hejlsberg expects tooling—especially leveraging AI and connected language services—to undergo more radical transformation than the language itself.

## Key Takeaways

- AI is impactful but best applied to deterministic tasks via generated migration helpers or for incremental maintenance.
- The TypeScript compiler’s move to Go reflects a pragmatic approach to language tooling.
- AI will fundamentally reshape development toolchains, shifting away from the classic IDE workflow.

## References

- [Full Interview with Anders Hejlsberg](https://github.blog/developer-skills/programming-languages-and-frameworks/7-learnings-from-anders-hejlsberg-the-architect-behind-c-and-typescript/)
- Discussion on [TypeScript Compiler Native Port](https://devclass.com/2025/03/12/typescript-compiler-ported-to-native-code-c-faithful-ask-why-go-was-used/)
- Related analysis by industry experts ([Redmonk](https://redmonk.com/sogrady/2025/06/18/language-rankings-1-25/), etc.)

This post appeared first on "DevClass". [Read the entire article here](https://www.devclass.com/ai-ml/2026/01/28/typescript-inventor-anders-hejlsberg-ai-is-a-big-regurgitator-of-stuff-someone-has-done/4079582)
