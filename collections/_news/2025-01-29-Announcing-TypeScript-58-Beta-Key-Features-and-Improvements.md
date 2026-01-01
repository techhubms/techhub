---
layout: "post"
title: "Announcing TypeScript 5.8 Beta: Key Features and Improvements"
description: "Daniel Rosenwasser introduces the TypeScript 5.8 Beta, highlighting new features such as checked returns for conditional and indexed access types, improved CommonJS and ESM interoperability, new compiler flags for advanced workflows, and optimizations for program performance. The post guides developers through technical changes and how to leverage them."
author: "Daniel Rosenwasser"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/typescript/announcing-typescript-5-8-beta/"
viewing_mode: "external"
feed_name: "Microsoft TypeScript Blog"
feed_url: "https://devblogs.microsoft.com/typescript/feed/"
date: 2025-01-29 18:51:38 +00:00
permalink: "/2025-01-29-Announcing-TypeScript-58-Beta-Key-Features-and-Improvements.html"
categories: ["Coding"]
tags: ["Checked Return Types", "Coding", "CommonJS", "Compiler Flags", "Conditional Types", "Declaration Files", "Erasablesyntaxonly", "ESM", "Indexed Access Types", "JavaScript", "Libreplacement", "Module Node18", "Module Nodenext", "News", "Node.js", "Program Optimization", "TypeScript", "TypeScript 5.8"]
tags_normalized: ["checked return types", "coding", "commonjs", "compiler flags", "conditional types", "declaration files", "erasablesyntaxonly", "esm", "indexed access types", "javascript", "libreplacement", "module node18", "module nodenext", "news", "nodedotjs", "program optimization", "typescript", "typescript 5dot8"]
---

Daniel Rosenwasser and the TypeScript Team unveil TypeScript 5.8 Beta, outlining key new features for developers, including advanced typing improvements and module interoperability.<!--excerpt_end-->

# Announcing TypeScript 5.8 Beta: Key Features and Improvements

**Author: Daniel Rosenwasser and the TypeScript Team**

TypeScript 5.8 Beta is now available, bringing several advancements to the language and its tooling. This release introduces features and improvements aimed at enhancing type safety, module compatibility, and developer productivity.

## Getting Started

To install the beta version:

```bash
npm install -D typescript@beta
```

## Core Features in TypeScript 5.8

### 1. Checked Returns for Conditional and Indexed Access Types

- TypeScript 5.8 adds precise typing for functions returning conditional types.
- Example: refining the return type of a function like `showQuickPick` so that the returned value accurately reflects input parameters, avoiding unnecessary type assertions.
- The new type checking mechanism can distinguish between different branches in conditional and indexed access types, providing more accurate error detection and safety.
- This change also covers more ergonomic patterns using type maps or interfaces for return values.

### 2. Improved ECMAScript Modules and CommonJS Interoperability

- Node.js 22 allows `require()` calls from CommonJS modules to ESM modules (except for ESM files with top-level `await`).
- TypeScript 5.8's `--module nodenext` flag supports this Node.js 22 behavior by not issuing errors for such require calls.
- A new `--module node18` flag is added for stable targeting of Node.js 18 environments, separating it from `nodenext` behaviors.

### 3. The `--erasableSyntaxOnly` Compiler Option

- Ensures TypeScript code only uses constructs that can be removed ("erased") to produce runnable JavaScript, following restrictions in Node.js' direct TypeScript execution support.
- Disallowed: `enum` declarations, namespaces/modules with runtime code, class parameter properties, and some import forms.

### 4. The `--libReplacement` Flag

- Developers can now disable TypeScript's lookup and loading of custom lib files, improving build speed for those not using custom lib packages.
- The flag allows finer control over library file resolution.

### 5. Consistent Computed Property Names in Declaration Files

- TypeScript 5.8 consistently preserves computed property names in the generated `.d.ts` files, enhancing compatibility and predictability when using dynamic property keys.
- There may be rare compatibility considerations with older TypeScript versions.

### 6. Performance Optimizations

- Reductions in array allocations during path normalization for large codebases.
- Reuse of project option validation results to minimize full re-validations on minor edits, making editor and watch mode snappier.

### 7. Notable Behavioral Changes

- Updates to DOM typings via `lib.d.ts` may affect some codebases.
- Import assertions using `assert` are rejected in favor of `with`, following ECMAScript changes.

## Limitations and Notes

- New conditional type checks kick in for functions with a single generic parameter over a union, but not for those using options bags or more complex structures unless specifically handled.
- Direct execution of TypeScript in Node.js (`--experimental-strip-types`) is still limited in supported syntax.
- Some new options and behaviors may be subject to breaking changes in future versions as Node.js and TypeScript evolve.

## Additional Resources

- [TypeScript 5.8 Beta Announcement](https://devblogs.microsoft.com/typescript/announcing-typescript-5-8-beta/)
- [PR: Checked Returns in Conditional Types](https://github.com/microsoft/TypeScript/pull/56941)
- [PR: CommonJS–ESM Interop](https://github.com/microsoft/TypeScript/pull/60761)
- [Node.js TypeScript Support](https://nodejs.org/api/typescript.html#type-stripping)

## Conclusion

TypeScript 5.8 Beta is feature-stable, with planned focus on bug fixes and polish before the stable release. Developers are encouraged to try the beta or nightly builds to take advantage of new type safety improvements, workflow flags, and optimized performance ahead of the full release.

Happy Hacking!

— Daniel Rosenwasser and the TypeScript Team

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-5-8-beta/)
