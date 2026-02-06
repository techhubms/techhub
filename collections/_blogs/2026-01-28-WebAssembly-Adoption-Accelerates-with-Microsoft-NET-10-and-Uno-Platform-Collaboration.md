---
external_url: https://devclass.com/2026/01/28/webassembly-gaining-adoption-behind-the-scenes-as-technology-advances/
title: WebAssembly Adoption Accelerates with Microsoft .NET 10 and Uno Platform Collaboration
author: Tim Anderson
primary_section: dotnet
feed_name: DevClass
date: 2026-01-28 08:44:04 +00:00
tags:
- .NET 10
- AOT Compilation
- Blogs
- Cross Browser Support
- Dart
- Deno
- Development
- Edge Computing
- Embedded Devices
- Java
- Kotlin
- Microsoft
- Multi Threading
- Node.js
- OCami
- OCaml
- Scala
- Scheme
- Server Runtimes
- Uno Platform
- Wasm
- Wasmtime
- WebAssembly
- .NET
section_names:
- dotnet
---
Tim Anderson summarizes Gerard Gallant’s WebAssembly adoption report, examining how .NET 10, Uno Platform, and recent browser innovations expand WebAssembly’s reach for developers.<!--excerpt_end-->

# WebAssembly Adoption Accelerates with Microsoft .NET 10 and Uno Platform Collaboration

**Author:** Tim Anderson

A recent report by Gerard Gallant, CIO at Dovico Software and author of "WebAssembly in Action," highlights WebAssembly’s (Wasm) growing use “behind the scenes” for application portability, performance, and security across web and server platforms.

## Rise in Wasm Usage

Data from Google’s Chrome Platform Status shows WebAssembly use on websites rising from 4.5% to 5.5% year-on-year as of 2025. Most adoption is not directly visible to end-users, as Wasm powers features embedded within broader applications.

## Improved Browser Support

- **Google Chrome** leads with the strongest Wasm support.
- **Mozilla Firefox** offers solid but not complete compatibility.
- **Apple Safari** has made progress in 2025, gaining exception handling, better JavaScript string integrations, and support for Wasm modules even when JIT (just-in-time) compilation is disabled. Safari now includes a new in-place interpreter.
- The release of Wasm 3.0 (spec finalized September 2025) enables cross-browser features such as native garbage collection for memory management, expanding the pool of programming languages that compile to Wasm, including Java, OCaml, Scala, Kotlin, Scheme, and Dart.

## Wasm on the Server

Wasm is not limited to browsers. Runtimes such as **Node.js**, **Deno**, and **Wasmtime** allow Wasm use in server environments, embedded devices, and edge computing platforms (e.g., Cloudflare, Akamai). Notably, some projects have compiled the PHP runtime to Wasm, enabling WordPress in Wasm sandboxes.

## Microsoft’s .NET 10 and Uno Platform Integration

The Microsoft .NET 10 release in November 2025 introduced improved ahead-of-time (AOT) compilation for Wasm, offering smaller downloads and quicker app startup. Uno Platform, a cross-platform .NET solution, leverages Wasm for client-side execution and is actively working with Microsoft to introduce multi-threading support to .NET Wasm workloads.

## Ongoing Challenges and Roadmap

- Work continues to make integrating Wasm modules with ECMAScript (JavaScript) modules as seamless as using standard JavaScript `import` statements.
- New intermediate steps such as "source phase imports"—already being considered by Chrome and Firefox—will gradually reduce the required JavaScript glue code.
- Feature proposals and ongoing developments are openly available via the [WebAssembly ESM-integration GitHub repository](https://github.com/WebAssembly/esm-integration/tree/main/proposals/esm-integration).

## The Subtle Reach of WebAssembly

Despite technical advances and growing adoption, Wasm’s profile remains low outside specialist circles. It is a foundational technology for application portability and cross-platform development, but not overtly visible.

## References

- [State of WebAssembly – Uno Platform](https://platform.uno/blog/the-state-of-webassembly-2025-2026/)
- [Chrome Platform Status metrics](https://chromestatus.com/metrics/feature/timeline/popularity/2237)
- [WebAssembly official features](https://webassembly.org/features/)

---

WebAssembly’s quiet but steady progress is poised to further enhance cross-platform development, especially as .NET, Uno Platform, and browser vendors expand their support and integration efforts.

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2026/01/28/webassembly-gaining-adoption-behind-the-scenes-as-technology-advances/)
