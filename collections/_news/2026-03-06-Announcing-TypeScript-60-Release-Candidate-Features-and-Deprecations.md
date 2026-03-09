---
layout: "post"
title: "Announcing TypeScript 6.0 Release Candidate: Features and Deprecations"
description: "This official announcement introduces the TypeScript 6.0 Release Candidate, outlining new features, key language improvements, changes to module resolution, added and updated APIs, breaking changes, and deprecations. It also provides guidance for developers to migrate codebases in preparation for TypeScript 7.0's native port and stricter defaults."
author: "Daniel Rosenwasser"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-rc/"
viewing_mode: "external"
feed_name: "Microsoft TypeScript Blog"
feed_url: "https://devblogs.microsoft.com/typescript/feed/"
date: 2026-03-06 19:13:14 +00:00
permalink: "/2026-03-06-Announcing-TypeScript-60-Release-Candidate-Features-and-Deprecations.html"
categories: ["Coding"]
tags: ["Coding", "CommonJS", "Compiler Options", "Deprecations", "DOM Types", "ES2025", "ESM", "Getorinsert", "JavaScript", "Map", "Moduleresolution", "News", "Node", "npm", "RC", "RegExp.escape", "Strict Mode", "Temporal API", "Tsconfig.json", "TypeScript", "TypeScript 6.0", "TypeScript 7.0"]
tags_normalized: ["coding", "commonjs", "compiler options", "deprecations", "dom types", "es2025", "esm", "getorinsert", "javascript", "map", "moduleresolution", "news", "node", "npm", "rc", "regexpdotescape", "strict mode", "temporal api", "tsconfigdotjson", "typescript", "typescript 6dot0", "typescript 7dot0"]
---

Daniel Rosenwasser announces the Release Candidate of TypeScript 6.0, detailing major updates, feature highlights, and migration guidance for developers as the platform transitions to its next major version.<!--excerpt_end-->

# Announcing TypeScript 6.0 Release Candidate

**Author:** Daniel Rosenwasser and the TypeScript Team

## Overview

TypeScript 6.0 Release Candidate (RC) has been announced as a transitional release bridging the current JavaScript-based compiler and the upcoming native Go-based TypeScript 7.0. This update introduces a mixture of new features, deprecations, and modernized defaults, preparing developers for significant future changes.

---

## Highlights and Features

- **RC Installation:**

  ```bash
  npm install -D typescript@rc
  ```

- **Native Port Coming:** TypeScript 7.0 will rely on a rewritten compiler and language service in Go, leveraging native code speed and multithreading.
- **Transition-Focused:** TypeScript 6.0 contains changes and deprecations to align with TS 7.0.

### Notable Changes Since Beta

- Improved type-checking, especially for function expressions in generic calls and JSX.
- Further deprecation of import assertion syntax, replaced with import attributes.
- Updated DOM types and enhancements to Temporal APIs.

### Type Inference Improvements

- Less context-sensitivity for `this` in functions: functions that don’t use `this` are no longer treated as contextually sensitive; this increases inference accuracy and reduces related errors.

### Module Resolution and Imports

- Support for subpath imports starting with `#/` (aligning with recent Node.js enhancements).
- `--moduleResolution bundler` can be combined with `--module commonjs` (previously not allowed).

### Stable Type Ordering

- New `--stableTypeOrdering` flag helps match type output with the upcoming parallel checker in TypeScript 7.0, aiding migration and reducing diffs due to ordering discrepancies.

### ECMAScript 2025

- `es2025` option is now available for the `target` and `lib` compiler options. No new language features, but additional built-in types.

### Temporal API Types

- Built-in types for the `Temporal` proposal now available under `esnext` or specific `temporal.esnext` settings.

### Map/WeakMap `getOrInsert` Methods

- Typed support for `getOrInsert` and `getOrInsertComputed`, reflecting ECMAScript proposals. Now available under the updated libs.

### Regular Expression Escaping

- Native support for `RegExp.escape()` for safe regex construction.

### DOM Library Consolidation

- The main `dom` lib now fully includes `dom.iterable` and `dom.asynciterable`, simplifying config for most projects.

---

## Breaking Changes & Deprecations

- **Stricter Defaults:**
  - `strict` mode now `true` by default
  - `module` defaults to `esnext`
  - `target` defaults to most recent ECMAScript
- **Typed Imports:** `types` field in `tsconfig.json` now defaults to `[]`, requiring explicit listing (e.g., `["node"]`).
- **Deprecated Options Removed:**
  - `target: es5` and `--downlevelIteration`
  - Module formats: `amd`, `umd`, `systemjs`, `none`
  - `baseUrl` as a lookup root
  - Classic module resolution and outFile option
  - Old-style `module` for namespaces
  - Import assertions (replaced by import attributes)
  - Disabling interop and strict mode flags that don’t align with modern JavaScript

Migration may involve running codemods, changing config keys, and explicitly setting types.

See further documentation and migration advice with provided links to TypeScript’s dev blogs and pull requests.

---

## Preparing for TypeScript 7.0

TypeScript 6.0 provides warnings for deprecated features scheduled for removal in 7.0. Developers are encouraged to address them soon, as 7.0’s release is imminent and will complete the transition to a native core.

---

## Action Steps

- Install the 6.0 RC and test compatibility.
- Update `tsconfig.json` to accommodate stricter and modern defaults.
- Review deprecations, especially those around module resolution and typing configurations.
- Provide feedback to the TypeScript team if migration issues are discovered.

---

## Further Information & Resources

- [Release Announcement](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-rc/)
- [TypeScript Nightly Builds](https://www.typescriptlang.org/docs/handbook/nightly-builds.html)
- [Native 7.0 Preview](https://www.npmjs.com/package/@typescript/native-preview)
- [Full List of Pull Requests](https://github.com/microsoft/TypeScript/pulls)

---

Happy Hacking!

– Daniel Rosenwasser and the TypeScript Team

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-rc/)
