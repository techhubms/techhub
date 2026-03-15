---
external_url: https://devclass.com/2026/01/28/typescript-inventor-anders-hejlsberg-ai-is-a-big-regurgitator-of-stuff-someone-has-done/
title: 'TypeScript inventor Anders Hejlsberg: AI’s Role in Language Porting and Development Tools'
author: Tim Anderson
primary_section: ai
feed_name: DevClass
date: 2026-01-28 17:44:39 +00:00
tags:
- AI
- AI in Development
- AI/ML
- Anders Hejlsberg
- Blogs
- C#
- Code Translation
- Development
- Development Tools
- Go
- IDEs
- Language Porting
- ML
- MCP
- Microsoft
- Native Compiler
- Programming Languages
- Semantic Services
- Typescript
- .NET
section_names:
- ai
- dotnet
---
In this piece, Tim Anderson interviews Anders Hejlsberg, Microsoft technical fellow, revealing insights on AI's use in compiler porting and the impact of machine learning on programming languages and developer tooling.<!--excerpt_end-->

# TypeScript inventor Anders Hejlsberg: AI’s Role in Language Porting and Development Tools

**Author:** Tim Anderson

## Overview

Anders Hejlsberg, Microsoft technical fellow and creator of both C# and TypeScript, shares his perspective on the role of AI in software development, programming language evolution, and the porting of the TypeScript compiler.

## Compiler Porting Project

Hejlsberg's team is working on the port of the TypeScript compiler from JavaScript (TypeScript) running on V8 to a native binary for performance reasons. This transition, targeted for TypeScript 7.0, is engineered for speed, leveraging shared memory concurrency in native code. The port preserves the original type checker's behavior to avoid subtle changes for existing users.

- The compiler is being ported function-by-function, rather than rewritten, to ensure exact semantic compatibility.
- Go was chosen as the language for the native compiler, instead of C# (Hejlsberg’s own creation) or Rust. The decision was driven by Go’s similarities to JavaScript and its support for required features like cyclic data structures and automatic garbage collection.

## AI’s Impact and Limitations

Hejlsberg reflects that AI, while powerful, presents challenges for deterministic tasks like direct code translation:

- **Attempted to use AI for porting TypeScript to Go**: The results were unreliable, as AI models can "hallucinate" code, requiring painstaking manual review for correctness.
- **AI excels as an assistant**: Hejlsberg suggests AI could generate programs that automate parts of the process, providing oversights and deterministic outcomes. AI is now helping his team manage porting of new pull requests from the JavaScript codebase to Go.

## Developer Tools and AI

- TypeScript's language service is being updated to leverage AI for better code checking and suggestion.
- AI will increasingly shift the development workflow, blending traditional IDE features with large language model (LLM) capabilities and semantic understanding.
- New architectures like MCP (Microsoft Copilot Platform) will allow tools and services to answer developer questions or automate refactoring with AI assistance.

## Programming Language Choices and Community Response

- The choice of Go over C# for the native compiler sparked controversy within the Microsoft developer community. Hejlsberg defends the decision as pragmatic, based on technical needs rather than language loyalty.
- He further addresses the technical tradeoffs and design philosophy behind TypeScript's evolution as a superset of JavaScript, emphasizing stability and continuity.

## Lessons and Future Perspective

- AI's role is shifting from mere code suggestion in IDEs to "supervision"—developers must oversee the models, ensuring correctness and reliability for critical workloads.
- Expect major changes not in the core languages, but in the tools and semantic services built around them, powered by AI.

**Related:** [Full interview with Anders Hejlsberg](https://github.blog/developer-skills/programming-languages-and-frameworks/7-learnings-from-anders-hejlsberg-the-architect-behind-c-and-typescript/)

---

## Key Points

- Porting large, complex codebases requires deterministic tools; current AI solutions are not yet reliable enough for full automation.
- AI is, however, highly useful for repetitive migration tasks and empowering semantic developer services.
- Pragmatic language/tool choices matter more than loyalty to a particular technology.
- The future of development will be shaped by the convergence of AI (LLMs), traditional language services, and new semantic/agentic architectures.

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2026/01/28/typescript-inventor-anders-hejlsberg-ai-is-a-big-regurgitator-of-stuff-someone-has-done/)
