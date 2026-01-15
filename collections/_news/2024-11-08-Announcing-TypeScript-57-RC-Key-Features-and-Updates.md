---
layout: post
title: 'Announcing TypeScript 5.7 RC: Key Features and Updates'
author: Daniel Rosenwasser
canonical_url: https://devblogs.microsoft.com/typescript/announcing-typescript-5-7-rc/
viewing_mode: external
feed_name: Microsoft TypeScript Blog
feed_url: https://devblogs.microsoft.com/typescript/feed/
date: 2024-11-08 21:01:44 +00:00
permalink: /coding/news/Announcing-TypeScript-57-RC-Key-Features-and-Updates
tags:
- Coding
- Composite Projects
- ES2024
- Implicit Any
- Index Signature
- JavaScript
- JSON Imports
- Lib.d.ts
- News
- Node.js
- Path Rewriting
- Project Configuration
- Relative Imports
- Release Candidate
- Rewriterelativeimportextensions
- Tsconfig
- TypedArray
- TypeScript
- TypeScript 5.7
- V8 Compile Caching
- Variable Initialization
section_names:
- coding
---
In this detailed announcement, Daniel Rosenwasser introduces the TypeScript 5.7 RC, outlining major feature enhancements and important changes for TypeScript developers.<!--excerpt_end-->

# Announcing TypeScript 5.7 RC

*By Daniel Rosenwasser and the TypeScript Team*

Today, we are announcing the availability of the release candidate (RC) for TypeScript 5.7. To try out the RC, simply run:

```sh
npm install -D typescript@rc
```

Below, we highlight key updates and new features in this release.

## Checks for Never-Initialized Variables

TypeScript now offers improved checks for variables that have never been initialized. While it already warned about possibly uninitialized variables, previous analysis had some blind spots, such as variables accessed from inner functions. TypeScript 5.7 can now catch more of these scenarios, improving reliability and preventing runtime errors. See [PR #55887](https://github.com/microsoft/TypeScript/pull/55887) for details.

**Example:**

```ts
function foo() {
  let result: number;
  // No assignment to 'result'
  function printResult() {
    console.log(result); // error: Variable 'result' is used before being assigned.
  }
}
```

## Path Rewriting for Relative Paths

Modern tools like ts-node, tsx, Deno, and Bun allow in-place execution of TypeScript files. To maximize compatibility, when importing via relative paths with TypeScript extensions (e.g., `.ts`, `.tsx`), developers often use imports like `./foo.ts`. While TypeScript previously supported this using `--allowImportingTsExtensions`, generating `.js` files from `.ts` files caused issues as paths weren't rewritten.

A new compiler option, `--rewriteRelativeImportExtensions`, addresses this. When enabled, relative imports ending in TypeScript extensions will be rewritten to their JavaScript equivalents on build.

**Example:**

```ts
// main.ts
import * as foo from "./foo.ts"; // Will be rewritten to ./foo.js
```

Limitations:

- Only relative paths are rewritten.
- Dynamic imports or imports involving custom resolution (like `baseUrl`, `paths`, or package exports) are not rewritten automatically.

See [PR #59767](https://github.com/microsoft/TypeScript/pull/59767) for more details.

## Support for `--target es2024` and `--lib es2024`

TypeScript 5.7 adds support for the ES2024 target, unlocking features such as `SharedArrayBuffer`, `ArrayBuffer` improvements, `Object.groupBy`, `Map.groupBy`, `Promise.withResolvers`, and more. There's also a change to `TypedArrays`, which are now generic over `ArrayBufferLike`. This may require updates to `@types/node` in some codebases.

**Example:**

```ts
interface Uint8Array<TArrayBuffer extends ArrayBufferLike = ArrayBufferLike> {
  // ...
}
```

Thanks to [Kenta Moriuchi](https://github.com/petamoriken) for contributions here. [See PR #59417](https://github.com/microsoft/TypeScript/pull/59417).

## Improved Project Configuration and Ownership File Search

TypeScript’s language server (TSServer) now extends its search for relevant `tsconfig.json` files up the directory tree, providing better support for monorepos and nested project configurations. Instead of stopping at the first `tsconfig.json` encountered, TSServer continues searching, aiding scenarios with multiple configuration files (e.g., test configs, workspace setups).

This enhances flexibility for project organization and configuration structure. Learn more in [PR #57196](https://github.com/microsoft/TypeScript/pull/57196) and [PR #59688](https://github.com/microsoft/TypeScript/pull/59688).

## Faster Editor Project Ownership Checks for Composite Projects

Performance for large, composite projects improves: when opening files, TypeScript can now quickly determine project ownership without fully parsing unrelated projects. This leverages the composite setting’s guarantee that root input files are known up-front. See [PR #59688](https://github.com/microsoft/TypeScript/pull/59688) for more information.

## Validated JSON Imports in `--module nodenext`

TypeScript now validates JSON imports when using `--module nodenext`. You must specify `type: "json"` as an import attribute for such imports.

**Example:**

```ts
import myConfig from "./myConfig.json" with { type: "json" }; // ✅
```

JSON imports no longer generate named exports, and only the default export is provided. For further details, see [PR #60019](https://github.com/microsoft/TypeScript/pull/60019).

## Node.js V8 Compile Caching Support

TypeScript 5.7 utilizes the new `module.enableCompileCache()` API in Node.js 22, enabling significant speed-ups for subsequent executions by caching compilation work (up to 2.5x faster in tests). See [PR #59720](https://github.com/microsoft/TypeScript/pull/59720).

## Notable Behavioral Changes

- **lib.d.ts Updates**: Changes to types generated for the DOM may affect type-checking. [See relevant issues](https://github.com/microsoft/TypeScript/pull/60061).
- **TypedArray Generics**: All `TypedArrays` are now generic; issues may require updating type declarations (see above).
- **Index Signatures for Symbol-Named Methods**: Methods declared with non-literal computed property names now correctly contribute to type information as index signatures (see [PR #59860](https://github.com/microsoft/TypeScript/pull/59860)).
- **Implicit 'any' Errors**: Improved detection of implicit `any` return types in functions returning `null` or `undefined` within generic signatures, especially under `noImplicitAny` (see [PR #59661](https://github.com/microsoft/TypeScript/pull/59661)).

## What’s Next?

TypeScript 5.7 is nearly complete, with the focus now shifting to bug fixes ahead of the stable release. To follow along or coordinate upgrades, check the [iteration plan](https://github.com/microsoft/TypeScript/issues/59905) for upcoming dates.

You can also try daily/nightly builds using [nightly npm packages](https://www.typescriptlang.org/docs/handbook/nightly-builds.html) or the VS Code [Nightly Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-next).

We encourage you to test this RC in your projects and provide feedback!

Happy hacking!

— Daniel Rosenwasser and the TypeScript Team

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-5-7-rc/)
