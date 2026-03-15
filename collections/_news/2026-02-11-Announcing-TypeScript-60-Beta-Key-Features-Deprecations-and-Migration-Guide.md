---
external_url: https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-beta/
title: 'Announcing TypeScript 6.0 Beta: Key Features, Deprecations, and Migration Guide'
author: Daniel Rosenwasser
primary_section: dotnet
feed_name: Microsoft TypeScript Blog
date: 2026-02-11 18:50:26 +00:00
tags:
- Bundler
- Compiler
- Deprecation
- ESM
- Go
- JavaScript
- Language Service
- Migration Guide
- Module Resolution
- News
- Node.js
- Strict Mode
- TSConfig
- Type Inference
- TypeScript
- TypeScript 6.0
- TypeScript 7.0
- .NET
section_names:
- dotnet
---
Daniel Rosenwasser and the TypeScript Team announce the TypeScript 6.0 Beta, detailing key new features, major deprecations, and migration tips to prepare developers for TypeScript 7.0.<!--excerpt_end-->

# Announcing TypeScript 6.0 Beta

Daniel Rosenwasser and the TypeScript Team have introduced the beta release of **TypeScript 6.0**. This release is notably the last based on the current JavaScript codebase. A native Go-based compiler and language service is in development, set to arrive in TypeScript 7.0.

## Key Highlights

- **Transition Release:** TypeScript 6.0 acts as a bridge between 5.9 and the upcoming native port (7.0), providing migration support and tooling alignment.
- **Getting Started:** Install the beta via npm:

  ```sh
  npm install -D typescript@beta
  ```

## Major Features and Changes

### 1. Improved Context Sensitivity on `this`-less Functions

TypeScript 6.0 refines type inference for functions that do not use `this`. This means better inference for method and arrow functions, regardless of property order or explicit parameter typing.

### 2. Subpath Imports with `#/`

Node.js 20 introduced support for subpath imports starting with `#/`; TypeScript 6.0 supports these in relevant module resolution modes. This enables cleaner, more flexible import path mapping.

### 3. Enhanced Module Resolution Options

You can now combine `--moduleResolution bundler` with `--module commonjs`—a practical migration path with the deprecation of `node` and `node10` modes.

### 4. The `--stableTypeOrdering` Flag

Facilitates 6.0-to-7.0 migrations by making type ordering deterministic, matching 7.0 behavior. This aids comparison of declaration file outputs, but may slow down type checking.

### 5. DOM Iterable Improvements

The `lib.dom.d.ts` library now includes `dom.iterable` and `dom.asynciterable`, so explicit `lib` references are no longer required for iteration over DOM collections.

## Breaking Changes and Deprecations

- **`strict` mode is now true by default.**
- **Default `module` is now `esnext`, default `target` is floating current-year ES version.**
- **`types` field now defaults to an empty array**, requiring explicit inclusion like `"types": ["node"]`.
- **Deprecation of legacy output targets:**
    - `es5` as a target is deprecated (move to ES2015 or newer).
    - `amd`, `umd`, `systemjs` module kinds are no longer supported.
- **`baseUrl`**, `--downlevelIteration`, `--moduleResolution classic`, and several other older options are deprecated.
- **`esModuleInterop` and `allowSyntheticDefaultImports` can't be set `false`.**
- **Support for namespace declared with the `module` keyword is dropped; use `namespace` instead.**
- **Syntax for import assertions with `asserts` is removed; use `with` attributes.**
- **The `--outFile` option and the `no-default-lib` directive are removed in favor of bundlers and modern config.**
- **Command-line file specification is now an error when a `tsconfig.json` exists unless `--ignoreConfig` is used.**

## Preparing for TypeScript 7.0

All deprecations become removals in 7.0. Use `"ignoreDeprecations": "6.0"` (in `tsconfig.json`) to maintain compatibility for now, but projects should migrate soon. TypeScript 7.0 (native Go port) prioritizes build speed, deterministic output, and stricter typing.

## Migration Tips and Tools

- **Explicitly define `types`, `rootDir`, and other newly explicit config.**
- Consider using the [`ts5to6` migration tool](https://github.com/andrewbranch/ts5to6) to update your codebase.
- Review and refactor deprecated settings in `tsconfig.json`.
- Try out nightly builds of TypeScript 6.0 and 7.0 ([npm](https://www.npmjs.com/package/@typescript/native-preview), [VS Code Extension](https://marketplace.visualstudio.com/items?itemName=TypeScriptTeam.native-preview)).

## Further Reading and References

- [TypeScript 6.0 Announcement Blog](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-beta/)
- [Progress on TypeScript 7.0](https://devblogs.microsoft.com/typescript/progress-on-typescript-7-december-2025/)
- [Native Port Updates](https://devblogs.microsoft.com/typescript/typescript-native-port/)
- [Nightly Builds Info](https://www.typescriptlang.org/docs/handbook/nightly-builds.html)

---

TypeScript 6.0 beta serves as a pivotal transition, modernizing development defaults, simplifying configurations, and setting a launchpad for the upcoming native Go-based compiler in 7.0. Consider testing this release and prepare early for impending changes!

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-beta/)
