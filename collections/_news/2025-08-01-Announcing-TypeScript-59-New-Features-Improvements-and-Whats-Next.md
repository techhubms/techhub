---
external_url: https://devblogs.microsoft.com/typescript/announcing-typescript-5-9/
title: 'Announcing TypeScript 5.9: New Features, Improvements, and What’s Next'
author: Daniel Rosenwasser
viewing_mode: external
feed_name: Microsoft TypeScript Blog
date: 2025-08-01 16:19:25 +00:00
tags:
- ECMAScript
- Expandable Hovers
- Import Defer
- JavaScript
- Node.js
- Performance Optimization
- Tsconfig
- Type Checking
- Type Inference
- TypeScript
- TypeScript 5.9
- VS Code
section_names:
- coding
---
Daniel Rosenwasser details the release of TypeScript 5.9, covering its new features, performance enhancements, and key behavior changes—essential reading for TypeScript developers.<!--excerpt_end-->

# Announcing TypeScript 5.9: New Features, Improvements, and What’s Next

*By Daniel Rosenwasser*

Today we are excited to announce the release of TypeScript 5.9!

If you’re not familiar with TypeScript, it’s a language that builds on JavaScript by adding syntax for types. TypeScript lets you check your code ahead of time to help avoid bugs. The TypeScript type-checker powers excellent editor tooling—such as code completions and go-to-definition—in editors like Visual Studio and Visual Studio Code. Learn more about TypeScript at [the official website](https://typescriptlang.org/).

If you’re ready, you can start using TypeScript 5.9 today:

```bash
npm install -D typescript
```

## What’s New in TypeScript 5.9

### 1. Minimal and Updated `tsc --init`

TypeScript’s compiler (`tsc`) has long supported an `--init` flag to generate a starter `tsconfig.json`. Previously, running `tsc --init` produced a large file with many commented-out settings for discoverability. However, feedback showed that most users deleted these lines and relied on editor features or the [tsconfig reference](https://www.typescriptlang.org/tsconfig/) to configure their project.

Now, `tsc --init` creates a more concise and prescriptive `tsconfig.json`, focusing on commonly used modern module features and best practices. Here’s an example of the new output:

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

For more details, see [this pull request](https://github.com/microsoft/TypeScript/pull/61813) and [the discussion issue](https://github.com/microsoft/TypeScript/issues/58420).

### 2. Support for `import defer`

TypeScript 5.9 adds support for ECMAScript’s [deferred module evaluation proposal](https://github.com/tc39/proposal-defer-import-eval/) via the `import defer` syntax. This allows importing a module without immediately executing it—useful for deferring side effects and improving startup performance. Only namespace imports are supported:

```typescript
import defer * as feature from "./some-feature.js";
```

The module and dependencies are loaded, but execution is deferred until you access an export:

```typescript
console.log(feature.specialConstant); // Module is evaluated at this moment
```

Named and default imports are not supported with `import defer`.

> _Note:_ `import defer` is not transformed by TypeScript and is meant for runtimes or bundlers that support this feature. It is available under `--module` modes `preserve` and `esnext`.

Implementation championed by [Nicolò Ribaudo](https://github.com/nicolo-ribaudo). [See the pull request.](https://github.com/microsoft/TypeScript/pull/60757)

### 3. Support for `--module node20`

TypeScript introduces a stable module option, `node20`, for compatibility with Node.js v20. Unlike `nodenext`, it will remain stable and defaults `--target` to `es2023` unless overridden. For implementation, [see here](https://github.com/microsoft/TypeScript/pull/61805).

### 4. Summary Descriptions in DOM APIs

TypeScript now includes summary descriptions for many DOM APIs (previously just links to MDN), thanks to [Adam Naji](https://github.com/Bashamega). These help developers understand APIs at-a-glance. See [DOM lib generator PRs](https://github.com/microsoft/TypeScript-DOM-lib-generator/pull/1993), [1993](https://github.com/microsoft/TypeScript-DOM-lib-generator/pull/1993), and [1940](https://github.com/microsoft/TypeScript-DOM-lib-generator/pull/1940).

### 5. Expandable Hovers (Preview)

Editor tooltips (quick info) are now expandable in VS Code, allowing you to view types in greater detail without navigating away. Clicking `+` within a hover expands type definitions. For details, see [the PR](https://github.com/microsoft/TypeScript/pull/59940).

### 6. Configurable Maximum Hover Length

Long hover tooltips can now be adjusted using the `js/ts.hover.maximumLength` setting in VS Code. TypeScript’s default maximum hover length is also increased, providing more information by default. Related PRs: [TypeScript](https://github.com/microsoft/TypeScript/pull/61662), [VS Code](https://github.com/microsoft/vscode/pull/248181).

### 7. Optimizations

#### a. Cache Instantiations on Mappers

Repeated type instantiations (common in libraries like Zod/tRPC) are now cached, reducing redundant computation and improving performance. [See the PR](https://github.com/microsoft/TypeScript/pull/61505) by [Mateusz Burzyński](https://github.com/Andarist).

#### b. Avoiding Closure Creation in `fileOrDirectoryExistsUsingSource`

Unnecessary closure allocation was found in file existence checks, leading to ~11% performance improvement in affected code paths. [See the PR](https://github.com/microsoft/TypeScript/pull/61822/) by [Vincent Bailly](https://github.com/VincentBailly).

## Notable Behavioral Changes

### Changes in `lib.d.ts`

- DOM type changes may impact type-checking.
- `ArrayBuffer` changes mean it is no longer a supertype of various TypedArray types (including Node.js’s `Buffer`).
- Errors such as the following may appear:

```text
error TS2345: Argument of type 'ArrayBufferLike' is not assignable to parameter of type 'BufferSource'.
error TS2322: Type 'ArrayBufferLike' is not assignable to type 'ArrayBuffer'.
error TS2322: Type 'Buffer' is not assignable to type 'Uint8Array<ArrayBufferLike>'.
```

- Use the latest `@types/node` and explicitly specify buffer types as needed, e.g., `Uint8Array<ArrayBuffer>`.
- If passing a TypedArray where an `ArrayBuffer` is expected, call `.buffer`:

```typescript
let data = new Uint8Array([0, 1, 2, 3, 4]);
someFunc(data.buffer);
```

### Type Argument Inference Changes

TypeScript 5.9 adjusts inference to fix variable leaks, which may introduce new type inference errors correctable via explicit type arguments. [See more](https://github.com/microsoft/TypeScript/pull/61668).

## What’s Next? TypeScript 6.0 and 7.0

- TypeScript 6.0 will be a transition release for preparing codebases for TypeScript 7.0, focusing on deprecations and minor behavior updates—expected to be largely API compatible with 5.9.
- Significant ongoing work is the [native port of TypeScript](https://devblogs.microsoft.com/typescript/typescript-native-port/) for TypeScript 7.0.
- Early previews of 7.0 can be [tried in VS Code](https://marketplace.visualstudio.com/items?itemName=TypeScriptTeam.native-preview).
- More information on these future releases is coming soon.

---

We hope TypeScript 5.9 improves your development experience. Happy hacking!

*Daniel Rosenwasser and the TypeScript Team*

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9/)
