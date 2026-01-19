---
layout: post
title: Announcing the Release Candidate (RC) of TypeScript 5.8
author: Daniel Rosenwasser
canonical_url: https://devblogs.microsoft.com/typescript/announcing-typescript-5-8-rc/
viewing_mode: external
feed_name: Microsoft TypeScript Blog
feed_url: https://devblogs.microsoft.com/typescript/feed/
date: 2025-02-13 22:27:05 +00:00
permalink: /coding/news/Announcing-the-Release-Candidate-RC-of-TypeScript-58
tags:
- CommonJS
- Compiler Flags
- Computed Properties
- Declaration Files
- ECMAScript Modules
- Import Assertions
- JavaScript
- Library Replacement
- Module Resolution
- Nightly Builds
- Node.js
- Performance Optimization
- Release Candidate
- TypeScript
section_names:
- coding
---
In this article, Daniel Rosenwasser details the new features and changes in the Release Candidate of TypeScript 5.8, aimed at improving type checking, module interoperability, build performance, and compatibility with modern JavaScript.<!--excerpt_end-->

## Announcing TypeScript 5.8 Release Candidate (RC)

*By Daniel Rosenwasser and the TypeScript Team*

Today, the TypeScript team is excited to announce the Release Candidate (RC) for TypeScript 5.8. This milestone brings several new features, improvements, and optimizations. Developers are encouraged to try out the RC using npm:

```sh
npm install -D typescript@rc
```

### What’s New Since the Beta?

- **Conditional Return Type Checking:**
  - TypeScript 5.8 improves checking of conditional return expressions within functions, enabling better error detection when return branches do not match the function's declared return type. For example, returning a string where a `URL` object is expected now results in an error.
- **Example:**

  ```typescript
  declare const untypedCache: Map<any, any>;
  function getUrlObject(urlString: string): URL {
    return untypedCache.has(urlString) ? untypedCache.get(urlString) : urlString; // Error: Type 'string' is not assignable to type 'URL'.
  }
  ```

  The [relevant pull request](https://github.com/microsoft/TypeScript/pull/56941) details this enhancement.

### Enhanced Module Interoperability

- **Support for `require()` of ECMAScript Modules in `--module nodenext`:**
  - Node.js 22 now allows CommonJS files to `require()` ESM files (except those with top-level `await`). TypeScript 5.8 supports this if you use the `--module nodenext` flag, making it easier for library authors to support ESM without dual-publishing.
  - [More information on implementation](https://github.com/microsoft/TypeScript/pull/60761).

- **`--module node18` Flag:**
  - A new stable flag for projects targeting Node.js 18, locking in behaviors appropriate for that environment. For example, `require()` of ESM is not allowed and import assertions remain supported.
  - Related changes detailed [here](https://github.com/microsoft/TypeScript/pull/60722) and [here](https://github.com/microsoft/TypeScript/pull/60761).

### New Compiler Flags & Compatibility Options

- **`--erasableSyntaxOnly`:**
  - Node.js 23.6 unflags support for running TypeScript files directly, but only permits easily erasable syntax. This flag makes TypeScript error for any syntax that can’t be simply stripped out (e.g., namespaces with runtime code, parameter properties, enums, `import =` aliases). Combining this flag with `--verbatimModuleSyntax` is recommended for stricter enforcement.
  - [See implementation](https://github.com/microsoft/TypeScript/pull/61011).

- **`--libReplacement`:**
  - TypeScript 4.5 introduced custom library file resolution. The new flag allows you to explicitly disable or enable this behavior to avoid unnecessary lookups and overhead if not needed.
  - [Discussion on the change](https://github.com/microsoft/TypeScript/issues/61023).

### Declaration File Generation Improvements

- **Computed Property Names in Declarations:**
  - TypeScript 5.8 now more predictably preserves computed property names when emitting declaration files, aligning the declaration output more closely with source code. This also reduces errors in previous versions related to declaration emit.
  - However, under the `--isolatedDeclarations` flag, certain computed property naming remains an error.
  - [Implementation details here](https://github.com/microsoft/TypeScript/pull/60052).

### Performance & Behavioral Enhancements

- **Program Load and Update Optimizations:**
  - TypeScript now avoids unnecessary allocations during path normalization and refrains from redundant project option validations, making builds and watch mode more responsive in large projects.
  - [Pull request](https://github.com/microsoft/TypeScript/pull/60812) for array allocation avoidance.
  - [Project option check improvements](https://github.com/microsoft/TypeScript/pull/60754).

- **Notable Behavioral Changes:**
  - DOM type changes in `lib.d.ts` may affect type checking. See [related issues](https://github.com/microsoft/TypeScript/issues/60985).
  - Import assertions have changed: Node.js 22 and TypeScript 5.8 (with `--module nodenext`) now enforce use of `import attributes` with the `with` keyword instead of `assert`.
  - Example:

    ```typescript
    // Deprecated import assertion
    import data from "./data.json" assert { type: "json" }; // Error in TypeScript 5.8
    // Preferred import attribute
    import data from "./data.json" with { type: "json" };
    ```

  - [Change details](https://github.com/microsoft/TypeScript/pull/60761)

### Looking Ahead

- Only critical or minor fixes are planned before the final release.
- The TypeScript team will release the stable version and update [the iteration plan](https://github.com/microsoft/TypeScript/issues/61023) for scheduling and coordination.
- Users are encouraged to test the RC and submit feedback on [GitHub issues](https://github.com/microsoft/TypeScript/issues).
- For those who want the latest, [nightly builds](https://www.typescriptlang.org/docs/handbook/nightly-builds.html) are available via npm; there is also a [Visual Studio Code extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-next) for these builds.

---

**Happy Hacking!**

— Daniel Rosenwasser and the TypeScript Team

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-5-8-rc/)
