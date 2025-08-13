---
layout: "post"
title: "Announcing the Release Candidate of TypeScript 5.9: What's New and Improved"
description: "Daniel Rosenwasser announces the Release Candidate of TypeScript 5.9, highlighting key changes such as an updated tsc --init, import defer support, Node 20 module mode, performance optimizations, new DOM API documentation, expanded editor hovers, and notable behavioral and type inference changes."
author: "Daniel Rosenwasser"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-rc/"
viewing_mode: "external"
feed_name: "Microsoft TypeScript Blog"
feed_url: "https://devblogs.microsoft.com/typescript/feed/"
date: 2025-07-25 16:53:10 +00:00
permalink: "/2025-07-25-Announcing-the-Release-Candidate-of-TypeScript-59-Whats-New-and-Improved.html"
categories: ["Coding"]
tags: ["Coding", "Developer Tools", "DOM APIs", "Expandable Hovers", "Import Defer", "JavaScript", "Module Detection", "News", "Node 20", "npm", "Performance Optimization", "Tsc Init", "Tsconfig.json", "Type Inference", "TypeScript", "TypeScript 5.9"]
tags_normalized: ["coding", "developer tools", "dom apis", "expandable hovers", "import defer", "javascript", "module detection", "news", "node 20", "npm", "performance optimization", "tsc init", "tsconfig dot json", "type inference", "typescript", "typescript 5 dot 9"]
---

Daniel Rosenwasser details the latest features and improvements in the TypeScript 5.9 Release Candidate. This update introduces new configuration defaults, deferred module imports, enhanced editor integration, and improvements based on community feedback.<!--excerpt_end-->

# Announcing TypeScript 5.9 Release Candidate

_Composed by Daniel Rosenwasser_

## Introduction

TypeScript 5.9 Release Candidate (RC) has been announced, bringing a host of updates, optimizations, and new features designed to streamline the experience for TypeScript developers. The RC can be installed using npm:

```sh
npm install -D typescript@rc
```

Below, we detail the most important highlights of this new release.

---

## Major Updates in TypeScript 5.9

### 1. Minimal and Updated `tsc --init`

Previously, running `tsc --init` generated a comprehensive `tsconfig.json` filled with commented options to promote discoverability of compiler features. However, based on feedback and user behavior (often deleting these comments), the default config is now more minimal, focusing on practicality and current developer needs. The new `tsconfig.json` prescribes more relevant settings by default, such as using modules, targeting ESNext, and strict options tailored to modern app development.

Example of the new minimal `tsconfig.json`:

```json
{
  "compilerOptions": {
    "module": "nodenext",
    "target": "esnext",
    "types": [],
    "sourceMap": true,
    "declaration": true,
    "declarationMap": true,
    "noUncheckedIndexedAccess": true,
    "exactOptionalPropertyTypes": true,
    "strict": true,
    "jsx": "react-jsx",
    "verbatimModuleSyntax": true,
    "isolatedModules": true,
    "noUncheckedSideEffectImports": true,
    "moduleDetection": "force",
    "skipLibCheck": true
  }
}
```

Additional hints are provided as comments for Node developers and pointers to reference documentation.

For more information, see the [pull request](https://github.com/microsoft/TypeScript/pull/61813) and [discussion](https://github.com/microsoft/TypeScript/issues/58420).

---

### 2. Support for `import defer`

TypeScript 5.9 supports ECMAScript’s [deferred module evaluation proposal](https://github.com/tc39/proposal-defer-import-eval/), allowing developers to import modules using the `import defer` syntax. This defers module execution until the first access of the imported namespace:

```ts
import defer * as feature from "./some-feature.js";
// Module code runs only when a property is accessed
console.log(feature.specialConstant); // Side effects happen here
```

Restrictions: Only namespace imports are allowed with `import defer`. Named/default imports are not supported.

This feature improves performance by loading and executing code only when required, optimizing startup times and resource usage, especially for conditionally loaded or heavy modules. Note that it works only in environments that natively support this syntax or with appropriate bundler transformations.

---

### 3. Support for `--module node20`

A new stable module configuration, `--module node20`, is introduced to reflect Node.js v20’s ESM and CJS behavior. Compared to the existing `nodenext` mode, `node20` is meant to be stable and not subject to new future behaviors. When `--module node20` is used, `--target es2023` is implied unless overridden. See [implementation details](https://github.com/microsoft/TypeScript/pull/61805).

---

### 4. Summary Descriptions in DOM APIs

DOM API types now come with in-editor summary descriptions derived from MDN documentation, on top of simple MDN links, for better context and usability during development. See the updates [here](https://github.com/microsoft/TypeScript-DOM-lib-generator/pull/1993) and [here](https://github.com/microsoft/TypeScript-DOM-lib-generator/pull/1940).

---

### 5. Expandable Hovers (Preview)

TypeScript 5.9 introduces expandable hovers (aka quick info verbosity) in editors like VS Code. Instead of jumping to type definitions for details, developers can expand hover tooltips to reveal more type information interactively.

- Click `+` to expand more details
- Click `-` to collapse back

This enhancement aids in fast code exploration without excessive navigation. [Feature PR](https://github.com/microsoft/TypeScript/pull/59940)

---

### 6. Configurable Maximum Hover Length

Previously, lengthy quick info tooltips could be truncated, sometimes omitting critical information. Now, hover length can be configured via the `js/ts.hover.maximumLength` setting in VS Code. The default is also larger, so more info is generally shown by default. [Feature PR](https://github.com/microsoft/TypeScript/pull/61662), [VS Code change](https://github.com/microsoft/vscode/pull/248181)

---

### 7. Optimizations

#### a. Cache Instantiations on Mappers

Reusable type argument instantiations are now cached, reducing performance bottlenecks and unnecessary work in large and complex codebases. Notably relevant to projects using libraries such as Zod or tRPC. [Details](https://github.com/microsoft/TypeScript/pull/61505)

#### b. Avoiding Closure Creation

Optimizations remove unnecessary closure creation for file/directory checks, cited to improve certain code paths’ speed by about 11% ([see more](https://github.com/microsoft/TypeScript/pull/61822/)).

---

### 8. Notable Behavioral Changes

- Updates to DOM library types (`lib.d.ts`) may affect type-checking, especially around `ArrayBuffer` and related types. This can lead to new type errors involving `ArrayBufferLike` and `Buffer`.
- Advice: Use the latest `@types/node` package and specify buffer types more explicitly.
- Type argument inference now better avoids leaking type variables but may require updating code to provide type arguments explicitly.

[More details](https://github.com/microsoft/TypeScript/pull/61668).

---

## What's Next?

TypeScript’s team is concurrently working on a native port of TypeScript (eventually to be TypeScript 7), available in nightly previews. The full TypeScript 5.9 release is expected soon after this RC.

For more information on the native port and upcoming releases, see: [TypeScript native previews](https://devblogs.microsoft.com/typescript/announcing-typescript-native-previews/)

---

## Conclusion

Developers are encouraged to try the new RC and provide feedback to the TypeScript team. TypeScript 5.9 continues to evolve with practical defaults, cutting-edge language features, and tooling improvements for a better developer experience.

---

Happy Hacking!

– Daniel Rosenwasser and the TypeScript Team

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-rc/)
