---
layout: "post"
title: "Announcing TypeScript 5.9 Beta: New Features, Improvements, and Optimizations"
description: "Daniel Rosenwasser announces the release of TypeScript 5.9 Beta, detailing enhancements such as a minimal tsc --init, import defer support, the node20 module option, expanded hovers, configurable tooltip lengths, and performance improvements. The post encourages early use and feedback from the developer community."
author: "Daniel Rosenwasser"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-beta/"
viewing_mode: "external"
feed_name: "Microsoft TypeScript Blog"
feed_url: "https://devblogs.microsoft.com/typescript/feed/"
date: 2025-07-08 17:38:54 +00:00
permalink: "/2025-07-08-Announcing-TypeScript-59-Beta-New-Features-Improvements-and-Optimizations.html"
categories: ["Coding"]
tags: ["Coding", "Compiler", "Developer Tools", "ECMAScript", "Import Defer", "JavaScript", "Language Server", "News", "Node.js", "Open Source", "Performance Optimization", "Quick Info", "Tsconfig.json", "TypeScript", "TypeScript 5.9", "Visual Studio Code"]
tags_normalized: ["coding", "compiler", "developer tools", "ecmascript", "import defer", "javascript", "language server", "news", "node dot js", "open source", "performance optimization", "quick info", "tsconfig dot json", "typescript", "typescript 5 dot 9", "visual studio code"]
---

In this post, Daniel Rosenwasser introduces TypeScript 5.9 Beta, highlighting important compiler and tooling updates. Developers are invited to try out new features and contribute feedback as TypeScript continues to evolve.<!--excerpt_end-->

# Announcing TypeScript 5.9 Beta

*By Daniel Rosenwasser*

Today, we are excited to announce the availability of **TypeScript 5.9 Beta.** Developers can install the beta via npm using:

```shell
npm install -D typescript@beta
```

Let’s explore what’s new in TypeScript 5.9:

## Major Highlights

- **Minimal and Updated `tsc --init`**
- **Support for `import defer`**
- **Support for `--module node20`**
- **Summary Descriptions in DOM APIs**
- **Expandable Hovers (Preview)**
- **Configurable Maximum Hover Length**
- **Optimizations**

---

## Minimal and Updated `tsc --init`

Historically, running `tsc --init` produced a verbose `tsconfig.json` including many commented-out options for discoverability. However, based on user and team feedback, most developers immediately removed much of this content and relied on editor auto-complete or [the tsconfig reference](https://www.typescriptlang.org/tsconfig/) for details.

Recognizing these patterns, TypeScript 5.9 now generates a much simpler `tsconfig.json` with additional prescriptive defaults aimed at reducing friction, such as:

- Setting `moduleDetection` to treat every implementation file as a module
- Using `target` as `esnext` for modern ECMAScript features
- Specifying an empty `types` array by default
- Streamlining settings for JSX users

Example generated `tsconfig.json` in TypeScript 5.9:

```json
{
  // Visit https://aka.ms/tsconfig to read more about this file
  "compilerOptions": {
    // File Layout
    // "rootDir": "./src",
    // "outDir": "./dist",

    // Environment Settings
    // See also https://aka.ms/tsconfig_modules
    "module": "nodenext",
    "target": "esnext",
    "types": [],
    // For nodejs:
    // "lib": ["esnext"],
    // "types": ["node"],
    // and npm install -D @types/node

    // Other Outputs
    "sourceMap": true,
    "declaration": true,
    "declarationMap": true,

    // Stricter Typechecking Options
    "noUncheckedIndexedAccess": true,
    "exactOptionalPropertyTypes": true,

    // Style Options
    // "noImplicitReturns": true,
    // "noImplicitOverride": true,
    // "noUnusedLocals": true,
    // "noUnusedParameters": true,
    // "noFallthroughCasesInSwitch": true,
    // "noPropertyAccessFromIndexSignature": true,

    // Recommended Options
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

- More information: [Implementing Pull Request](https://github.com/microsoft/TypeScript/pull/61813), [Discussion Issue](https://github.com/microsoft/TypeScript/issues/58420)

---

## Support for `import defer`

TypeScript 5.9 introduces support for the [ECMAScript deferred module evaluation proposal](https://github.com/tc39/proposal-defer-import-eval/) via the new `import defer` syntax, allowing modules to be loaded without immediate execution. This grants more precise control for modules with expensive initializations or platform-specific side-effects, potentially improving startup performance.

Example usage:

```typescript
import defer * as feature from "./some-feature.js";

// No side effects have occurred yet
// Later, upon usage:
console.log(feature.specialConstant);
// Side effects from the module now occur
```

**Key behavior:**

- Only supports namespace imports (`import defer * as ...`)
- Does not support named or default imports
- The module is loaded immediately, but executed only when accessed
- Supported in `--module` modes `preserve` and `esnext`
- Not transformed/downleveled by TypeScript; the runtime or tooling must support it

Thanks to [Nicolò Ribaudo](https://github.com/nicolo-ribaudo) for championing and implementing this feature. [Implementation Details](https://github.com/microsoft/TypeScript/pull/60757)

---

## Support for `--module node20`

The new `--module node20` provides stable, Node.js v20-aligned module behavior within TypeScript. Compared to `nodenext`, `node20`:

- Is less likely to introduce new behaviors in the future
- Implies `--target es2023` by default
- Retains alignment with Node.js v20 module logic

Find out more: [Implementation PR](https://github.com/microsoft/TypeScript/pull/61805)

---

## Summary Descriptions in DOM APIs

Historically, TypeScript’s DOM API typings only linked to MDN–now, thanks to changes from [Adam Naji](https://github.com/Bashamega), many DOM API entries feature automatically imported summary descriptions. This lets developers quickly see what a DOM API does right in their editor, improving discoverability and reducing context switching.

[See the changes here](https://github.com/microsoft/TypeScript-DOM-lib-generator/pull/1993) and [here](https://github.com/microsoft/TypeScript-DOM-lib-generator/pull/1940).

---

## Expandable Hovers (Preview)

TypeScript 5.9 adds preview support for **expandable hovers** (quick info verbosity) in editors like Visual Studio Code. Instead of jumping to type definitions to learn more, users can now expand tooltips in-place with `+`/`-` buttons, drilling into type details as needed.

![Expandable hover example in VS Code](https://devblogs.microsoft.com/typescript/wp-content/uploads/sites/11/2025/06/bare-hover-5.8-01.png)

- Feedback is encouraged on this feature from both TypeScript users and partner editors.
- [Expandable Hovers PR](https://github.com/microsoft/TypeScript/pull/59940)

---

## Configurable Maximum Hover Length

Quick info (hover tooltips) are now more configurable: TypeScript 5.9 supports setting a maximum hover length in the language server, exposed in VS Code via the `js/ts.hover.maximumLength` option. The new default is higher, showing more detail before truncation.

See: [TypeScript PR](https://github.com/microsoft/TypeScript/pull/61662), [VS Code PR](https://github.com/microsoft/vscode/pull/248181)

---

## Optimizations

### 1. Cache Instantiations on Mappers

Redundant type instantiations are now avoided by caching intermediate instantiations, especially benefiting complex libraries like Zod and tRPC. This reduces excessive work and lowers the risk of exceeding instantiation depth limits.

- Thanks to [Mateusz Burzyński](https://github.com/Andarist)
- [Implementation](https://github.com/microsoft/TypeScript/pull/61505)

### 2. Avoiding Closure Creation in `fileOrDirectoryExistsUsingSource`

Performance improvements in file existence checks eliminate unnecessary function allocations, providing measurable speed-ups (up to 11% in cited cases).

- [Vincent Bailly](https://github.com/VincentBailly)
- [Implementation](https://github.com/microsoft/TypeScript/pull/61822/)

---

## What’s Next?

The team continues focusing on the [native port of TypeScript](https://devblogs.microsoft.com/typescript/typescript-native-port/), which will ultimately become TypeScript 7. You can try nightly builds of the native port now: [native previews info](https://devblogs.microsoft.com/typescript/announcing-typescript-native-previews/).

TypeScript 5.9 remains under active development. Feedback is welcome, and nightly releases are available for the latest features. Try the [JavaScript and TypeScript Nightly extension for VS Code](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-next) for early access.

Happy hacking!

— Daniel Rosenwasser and the TypeScript Team

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-beta/)
