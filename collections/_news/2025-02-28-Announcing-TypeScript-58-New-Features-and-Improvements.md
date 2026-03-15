---
external_url: https://devblogs.microsoft.com/typescript/announcing-typescript-5-8/
title: 'Announcing TypeScript 5.8: New Features and Improvements'
author: Daniel Rosenwasser
feed_name: Microsoft TypeScript Blog
date: 2025-02-28 19:35:56 +00:00
tags:
- Compiler Options
- Declaration Files
- ECMAScript Modules
- JavaScript
- Node.js
- Performance Optimization
- Return Type Checking
- Type System
- TypeScript
- VS Code
- News
- .NET
section_names:
- dotnet
primary_section: dotnet
---
Daniel Rosenwasser announces TypeScript 5.8, detailing new type system features, improved Node.js module interop, and important optimizations. This post guides TypeScript users through the latest enhancements and key behavioral updates.<!--excerpt_end-->

# Announcing TypeScript 5.8

*By Daniel Rosenwasser and the TypeScript Team*

Today we’re excited to announce the release of **TypeScript 5.8**! For those unfamiliar, TypeScript is a superset of JavaScript that introduces static types, enabling developers to express their intent and benefit from tooling that can catch errors like typos or incorrect use of
`null` and `undefined`. Types also power the sophisticated editor tooling available in Visual Studio and VS Code, enhancing completion, navigation, and refactoring for JavaScript and TypeScript alike.

To start using TypeScript, simply install it via npm:

```sh
npm install -D typescript
```

Let’s explore the key updates in TypeScript 5.8.

## What’s New Since the Beta and Release Candidate?

Following its beta release, TypeScript 5.8 underwent refinement regarding functions with conditional return types. While some changes were deferred to TypeScript 5.9 due to complexity, **TypeScript 5.8** introduces more granular checks for branches within return expressions.

No other major changes were added since the release candidate.

## Granular Checks for Branches in Return Expressions

Previously, TypeScript's handling of conditional return expressions like `cond ? trueBranch : falseBranch` sometimes led to loss of type information—especially when one branch was of type `any`. This could prevent the compiler from catching bugs where a value didn't match the intended return type.

**TypeScript 5.8** now checks each branch of a conditional directly inside a `return` statement against the declared return type, allowing bugs to be caught even when union types are involved. For example:

```typescript
declare const untypedCache: Map<any, any>;

function getUrlObject(urlString: string): URL {
  return untypedCache.has(urlString)
    ? untypedCache.get(urlString)
    : urlString; // Error: Type 'string' is not assignable to type 'URL'.
}
```

This change is part of ongoing improvements to stricter conditional type checking.

[See implementation PR](https://github.com/microsoft/TypeScript/pull/56941).

## Support for `require()` of ECMAScript Modules in `--module nodenext`

Historically, Node.js allowed ESM files to import CommonJS files but not the reverse. Node.js 22 now permits `require("esm")` from CommonJS modules (with exceptions such as top-level `await`). This relaxes dual-publishing requirements for library authors.

TypeScript 5.8 supports this interoperability via the `--module nodenext` flag, suppressing errors for these use cases. Until a stable node version enables this by default, its use is recommended for Node.js 22+ environments; use `--module node16` or `node18` otherwise.

[More details](https://github.com/microsoft/TypeScript/pull/60761).

## `--module node18` Compiler Flag

A new `--module node18` flag is added, providing a stable config point for targeting Node.js 18. It differs from `nodenext` in notable ways:

- Disallows `require()` for ESM files
- Allows import assertions (now deprecated in favor of import attributes)

[See pull request](https://github.com/microsoft/TypeScript/pull/60722)

## The `--erasableSyntaxOnly` Compiler Option

With Node.js 23.6's experimental ability to strip types at runtime, only syntax that can be easily erased (leaving valid JavaScript) is supported—excluding constructs like enums, namespaces with runtime code, parameter properties (
`constructor(public x: number)`), and `import =` aliases. Tools like [ts-blank-space](https://github.com/bloomberg/ts-blank-space) and [Amaro](https://github.com/nodejs/amaro) share these constraints.

TypeScript 5.8's new `--erasableSyntaxOnly` flag will report errors if such constructs are encountered. It is often used with `--verbatimModuleSyntax` for precise module import handling.

[Details on implementation](https://github.com/microsoft/TypeScript/pull/61011)

## The `--libReplacement` Flag

Since TypeScript 4.5, custom `lib` files (via packages like `@typescript/lib-*`) could override standard libraries. TypeScript 5.8 adds a `--libReplacement` flag (defaulting to enabled), allowing users to disable this if desired. In the future, explicit enabling may be required.

[See discussion](https://github.com/microsoft/TypeScript/issues/61023)

## Preserved Computed Property Names in Declaration Files

When generating `.d.ts` files, computed property names in classes are more faithfully preserved, e.g.:

```typescript
export let propName = "theAnswer";
export class MyClass {
  [propName] = 42;
}
// Declares: [propName]: number; in .d.ts
```

In previous TypeScript versions, these would sometimes be downgraded to string index signatures. Note: Statistically named properties are not created, and errors still apply under `--isolatedDeclarations`.

[Implementing PR](https://github.com/microsoft/TypeScript/pull/60052)

## Optimizations on Program Loads and Updates

TypeScript 5.8 brings significant internal optimizations:

- **Faster Path Normalization**: Reduces memory allocations for projects with many files
- **Quicker Re-validation**: Avoids redundant project option checks on minor edits, improving responsiveness in large codebases

[Path normalization PR](https://github.com/microsoft/TypeScript/pull/60812)  |  [Option reuse PR](https://github.com/microsoft/TypeScript/pull/60754)

## Notable Behavioral Changes

- **DOM Type Updates**: Changes to `lib.d.ts` may affect type-checking, especially for DOM-related code. [Related issues](https://github.com/microsoft/TypeScript/issues/60985)
- **Restriction on Import Assertions**: With `--module nodenext`, import assertions (now replaced by import attributes) will cause errors. Example:

  ```typescript
  // Not future-compatible
  import data from "./data.json" assert { type: "json" };
  // Future-proof
  import data from "./data.json" with { type: "json" };
  ```

[Change details](https://github.com/microsoft/TypeScript/pull/60761)

## What’s Next?

The next release will be **TypeScript 5.9**. Follow the [iteration plan](https://github.com/microsoft/TypeScript/issues) for upcoming features and dates. Try nightly builds with `npm install typescript@next` or the [VS Code TypeScript Nightly extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-next).

**TypeScript 5.8** is available now, and we hope it improves your coding experience. Happy hacking!

— Daniel Rosenwasser and the TypeScript Team

This post appeared first on "Microsoft TypeScript Blog". [Read the entire article here](https://devblogs.microsoft.com/typescript/announcing-typescript-5-8/)
