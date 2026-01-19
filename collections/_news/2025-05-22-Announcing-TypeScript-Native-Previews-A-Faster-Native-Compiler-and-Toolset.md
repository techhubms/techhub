---
layout: post
title: 'Announcing TypeScript Native Previews: A Faster, Native Compiler and Toolset'
author: Daniel Rosenwasser
canonical_url: https://devblogs.microsoft.com/typescript/announcing-typescript-native-previews/
viewing_mode: external
feed_name: Microsoft TypeScript Blog
feed_url: https://devblogs.microsoft.com/typescript/feed/
date: 2025-05-22 15:04:21 +00:00
permalink: /coding/news/Announcing-TypeScript-Native-Previews-A-Faster-Native-Compiler-and-Toolset
tags:
- API
- Go
- JavaScript Type Checking
- JSX
- Language Server
- LSP
- Native Compiler
- Node.js
- Open Source
- Performance
- Project Corsa
- Project Strada
- TypeScript
- TypeScript Native
- VS Code Extension
section_names:
- coding
---
In this detailed announcement, Daniel Rosenwasser unveils the TypeScript Native Previews—offering a natively compiled TypeScript compiler and toolset in Go. The article covers installation, new features, ongoing limitations, and an active roadmap for developers.<!--excerpt_end-->

## Announcing TypeScript Native Previews

**By Daniel Rosenwasser**

This past March, the TypeScript team unveiled efforts to port the entire TypeScript compiler and toolset to native code, primarily in Go. This port has led to substantial improvements: most projects now experience over a 10x speed-up, not solely due to native code, but also thanks to shared memory parallelism and concurrency.

### Public Availability & How to Try It

**TypeScript Native Previews** are now broadly available. Developers can preview the native TypeScript compiler through npm:

```bash
npm install -D @typescript/native-preview
```

This provides the `tsgo` executable, which is used similarly to `tsc`:

```bash
npx tsgo --project ./src/tsconfig.json
```

Currently, `tsgo` is separate from `tsc` for testing and experimentation, though future releases will streamline this.

#### Editor Support: Visual Studio Code

A preview VS Code extension, "TypeScript (Native Preview)", is available via the Visual Studio Marketplace. After installing, enable it through the Command Palette ("TypeScript Native Preview: Enable (Experimental)") or by toggling the relevant setting (`typescript.experimental.useTsgo`) in either the Settings UI or `settings.json`.

### Updates, Release Cadence, and Roadmap

- The native preview is the precursor to TypeScript 7, published nightly.
- Extension users will receive automatic updates.
- Issues or disruptions can be reported on [GitHub](https://github.com/microsoft/typescript-go/issues), and the new language service can be disabled if needed.

**Note:** Native previews currently lack some stable TypeScript features, including `--build`, `--declaration` emits, downlevel targets, and some advanced editor integrations (e.g., auto-imports, find-all-references, and rename).

### What’s New: Progress Since Initial Announcement

#### Project Names

- **Corsa**: Codename for the native (TS7) codebase.
- **Strada**: The JS-based TypeScript 5.8 codebase.

#### Type-Checking Support

- **Type-checker port**: Most of the type-checker is ported; most projects should see the same errors (with some intentional changes and possible lib.d.ts differences).
- File issues for any divergences found.
- **New features added:**
  - JSX type-checking support (was previously missing).
  - Enhanced JavaScript + JSDoc checking.

##### JSX Checking Example (Sentry codebase)

- With TypeScript 5.8 (Strada): about 73 seconds to type-check ~1.1M lines.
- With `tsgo` (Corsa): ~6.7 seconds for similar workload (10x+ speedup).
- Results may vary, but large projects with JSX benefit significantly from the new compilation model.
- Further details on JSX implementation can be found in linked [GitHub PRs](https://github.com/microsoft/typescript-go/pull/762) and [factory imports](https://github.com/microsoft/typescript-go/pull/780).

#### JavaScript Checking

- Native preview supports parsing and type-checking of JavaScript via JSDoc.
- Implementation for JS support was rewritten, leading to potential changes for some legacy JS patterns—feedback is sought for needed adjustments.

#### Editor Support & LSP Progress

- Transitioning from TSServer protocol (used by Strada) to LSP for wider compatibility.
- Basic LSP features available: errors/diagnostics, go-to-definition, and hover.
- Completion support recently enabled (full auto-imports/advanced features pending).
- Ongoing work on language server tests and advanced editor features: find-all-references, rename, and signature help.

#### API Progress

- Early API layer implemented, enabling communication over IPC with TypeScript processes regardless of consuming language.
- Node.js integration uses a new native Rust module ([libsyncrpc](https://github.com/microsoft/libsyncrpc)) for synchronous process communication.
- Further API development and design input are welcomed ([details](https://github.com/microsoft/typescript-go/pull/711)).

### Known and Notable Differences

- Some default settings and deprecations differ from older versions, such as `node`/`node10` resolutions (replaced by `node16`, `nodenext`, `bundler`, etc.).
- Projects using deprecated settings may see module or type declaration errors; recommended is to switch configurations or use auto-imports to resolve issues.
- Current limitations:
  - Limited support for downlevel emit
  - JSX emit: preserves code, not transforms
  - Declaration emit unsupported
  - `--build` mode not available (individual projects can still be built)
  - Project reference language service features incomplete

### Roadmap & What’s Next

- Aim for more comprehensive features (including `--build`) and complete language service/editor capabilities by later this year
- Nightly previews and periodic major updates planned; developers are encouraged to try the native previews and provide feedback

---

**Happy Hacking!**

— Daniel Rosenwasser and the TypeScript Team

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-native-previews/)
