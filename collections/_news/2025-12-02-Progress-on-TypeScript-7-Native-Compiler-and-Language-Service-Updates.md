---
external_url: https://devblogs.microsoft.com/typescript/progress-on-typescript-7-december-2025/
title: 'Progress on TypeScript 7: Native Compiler and Language Service Updates'
author: Daniel Rosenwasser
feed_name: Microsoft TypeScript Blog
date: 2025-12-02 17:31:32 +00:00
tags:
- Compiler
- Editor Integration
- JavaScript
- Language Service
- Migration
- Native Port
- Performance
- Project Corsa
- Tooling
- Tsc
- Tsgo
- Type Checking
- TypeScript
- TypeScript 7.0
- VS Code Extension
- News
- .NET
section_names:
- dotnet
primary_section: dotnet
---
Daniel Rosenwasser presents major progress on TypeScript 7, focusing on the transition to a native compiler and language service. The article explores new features, editor support, and guidance for developers migrating to or evaluating TypeScript 7.<!--excerpt_end-->

# Progress on TypeScript 7: Native Compiler and Language Service Updates

*By Daniel Rosenwasser*

Earlier this year, the TypeScript team announced a major initiative: porting the TypeScript compiler and language service to native code (Project Corsa, soon to be TypeScript 7.0) to leverage better performance, memory handling, and parallelism. This post provides updates on what’s ready, future plans, performance gains, and migration advice for developers.

## Overview: Why Native?

By moving to native code, TypeScript aims to deliver:

- Faster type-checking and command-line builds
- Reduced memory usage
- Better support for multi-threaded, parallel compilation and analysis

This migration is already benefiting day-to-day developer workflows, especially for large projects.

## Editor Support & Language Service

TypeScript’s native previews can now be tried directly in editors using a [VS Code extension on the Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=TypeScriptTeam.native-preview). The key features already implemented include:

- Code completions (including auto-imports)
- Go-to-Definition, Go-to-Implementation
- Find-All-References
- Rename, Quick Info, and Signature Help
- Formatting, Selection Ranges, Code Lenses, Call Hierarchy
- Quick Fixes (e.g., for missing imports)

Although the port isn't yet 100% feature-complete, the main development experience is robust and noticeably faster—especially for large or complex codebases. The new architecture is designed for reliability and leverages shared-memory parallelism.

You can easily toggle between the VS Code’s built-in TypeScript and the new native preview to compare experiences. The team encourages broader adoption and feedback.

## Compiler: Progress and Performance

The TypeScript 7 native compiler is available for preview via npm as [`@typescript/native-preview`](https://www.npmjs.com/package/@typescript/native-preview), providing the `tsgo` CLI. It achieves parity with the previous `tsc` in most features, including:

- Full type checking and compatibility with TypeScript 5.9 error output
- Incremental builds (`--incremental`), project references, and build mode support
- Dramatically improved build speeds (up to 10x faster in tests compared to 6.0 JavaScript-based compiler)

Performance benchmarks show major reductions in build times:

| Project   | tsc (6.0) | tsgo (7.0) | Speedup |
|-----------|-----------|------------|---------|
| sentry    | 133.08s   | 16.25s     | 8.19x   |
| vscode    | 89.11s    | 8.74s      | 10.2x   |
| typeorm   | 15.80s    | 1.06s      | 9.88x   |
| playwright| 9.30s     | 1.24s      | 7.51x   |

## Expected Differences and Migration Notes

Some differences and limitations to be aware of in TypeScript 7.0:

- Several compiler flags and APIs deprecated or removed (e.g., `--baseUrl`, `--target es5`)
- Default settings (such as `--strict`) changed for improved safety
- Some JavaScript and JSDoc type-checking rules refactored or dropped for maintainability
- Emit and `--watch` functionality still maturing; may require both 6.0 and 7.0 installed for some tooling
- Language service API (previously TSServer) now standard LSP; some IDE integrations may require adjustments

A migration tool, `ts5to6`, is being developed to help update `tsconfig.json` files automatically.

## TypeScript 6.0: The Bridge Release

TypeScript 6.0 will be the last JavaScript-based release (Strada). It is designed for maximum compatibility with TypeScript 7 and acts as a transition for projects and tooling. Patch releases after 6.0 will only address critical issues. Developers should plan to use 7.0 for performance and 6.0 for tools/APIs not yet available in the native line.

## Community Feedback and Next Steps

Developers are encouraged to:

- Install and test the [VS Code native preview extension](https://marketplace.visualstudio.com/items?itemName=TypeScriptTeam.native-preview)
- Try [`@typescript/native-preview`](https://www.npmjs.com/package/@typescript/native-preview) for builds
- File issues and feedback [on GitHub](https://github.com/microsoft/typescript-go/issues)

The team is focused on closing parity gaps and moving toward a full native compiler and service experience.

---

For more details, roadmap updates, and in-depth technical documentation, refer to official TypeScript DevBlogs and the linked resources.

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/progress-on-typescript-7-december-2025/)
