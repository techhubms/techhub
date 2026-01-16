---
layout: post
title: 'Building docfind: Fast Client-Side Search for VS Code Docs with Rust, WASM, and Copilot'
author: João Moreno
canonical_url: https://code.visualstudio.com/blogs/2026/01/15/docfind
viewing_mode: external
feed_name: Visual Studio Code Releases
feed_url: https://code.visualstudio.com/feed.xml
date: 2026-01-15 00:00:00 +00:00
permalink: /ai/news/Building-docfind-Fast-Client-Side-Search-for-VS-Code-Docs-with-Rust-WASM-and-Copilot
tags:
- AI
- Blog
- CLI Tools
- Client Side Search
- Coding
- DevOps
- Docfind
- Edge Computing
- FSST
- FST
- Levenshtein Automaton
- News
- Open Source
- RAKE
- Rust
- Search Indexing
- Static Site Search
- VS Code
- Wasm Bindgen
- WebAssembly
section_names:
- ai
- coding
- devops
---
João Moreno from the VS Code team recounts building docfind—a Rust and WebAssembly-based client-side search engine—detailing the engineering journey, technical challenges, and the critical role GitHub Copilot played in delivering a blazing fast, serverless search experience.<!--excerpt_end-->

# Building docfind: Fast Client-Side Search with Rust and WebAssembly

*By João Moreno*

This article explores the journey behind creating **docfind**, the fast client-side search powering the [VS Code documentation site](https://code.visualstudio.com/). The VS Code team sought to move away from traditional server-based search, aiming for instant, responsive queries directly in users' browsers without server roundtrips or maintenance overhead.

## The Search Challenge

- Traditional web search required a backend or third-party API (Algolia, TypeSense, Lunr.js, Stork, etc.), each imposing limitations on index size, hosting, dependencies, or speed.
- The team wanted a compact, efficient, totally client-side approach for thousands of documentation pages.

## Technical Inspirations

- **Finite State Transducers (FSTs):** Inspired by Andrew Gallant's work, used to store indexed keywords and provide rapid lookup.
- **Rust Libraries:** Used [fst](https://docs.rs/fst/latest/fst/) for FSTs, [RAKE](https://docs.rs/rake/latest/rake/) for keyword extraction, and [FSST](https://docs.rs/fsst-rs/latest/fsst/index.html) for compressing index strings.

## Implementation Approach

- **Rust CLI Tool:** Builds the search index from a JSON dump of documentation, extracting keywords and compressing document data.
- **WebAssembly (WASM) Module:** The index is embedded directly into the WASM module, loaded by end users’ browsers at search time—no extra server calls needed.
- **Technical Data Flow:**
    - 1. Gather docs data → extract/score keywords with RAKE
    - 2. Build FST mapping keywords to docs
    - 3. Compress strings with FSST
    - 4. Embed everything into a WASM module
    - 5. In-browser, WASM exposes search APIs leveraging Levenshtein automata for typo tolerance

```rust
pub struct Index {
    /// FST mapping keywords to document indices
    fst: Vec<u8>,
    /// FSST-compressed document strings
    document_strings: FsstStrVec,
    /// (keyword_index, score) → (doc_index, score) pairs
    keyword_to_documents: Vec<Vec<(usize, u8)>>,
}
```

- **Efficient Binary Packing:** Instead of statically including the index at Rust compile time (with `include_bytes!`), a reusable WASM "template" is patched at build, so only the data segment changes—a process requiring low-level WASM binary manipulation.
- **Search Logic:** Client-side search in JS loads the module, decompresses result data as needed, and displays results instantly as the user types.

## Developer Tools and GitHub Copilot

- **Copilot’s Role:** The author, not a daily Rust developer, used GitHub Copilot extensively for:
    - Learning Rust idioms and unfamiliar libraries
    - WASM toolchain setup and WASM-specific Rust patterns
    - Scaffold and documentation creation
    - Navigating and patching intricate WASM binaries
    - Copilot’s code suggestions, explanations, and even project structuring saved substantial time
- **Takeaway:** Copilot served as an AI mentor, unlocking productivity and enabling the author to build production-quality solutions outside his main tech stack.

## Results

- **Performance:** 50,000 documents, ~6MB index (2.7MB compressed), ~0.4ms search latency per typical query
- **No Server Needed:** Only a WASM module is served—no runtime backend or APIs
- **Open Source:** docfind is available for anyone to use, with guided installation and setup

## Try docfind

- **Install:** Linux/Mac via shell script, Windows via PowerShell
- **Usage:** Prepare docs as JSON, run the CLI, and serve generated JS+WASM. Bring your own UI for search results.
- **Demos/Docs:** [docfind repo](https://github.com/microsoft/docfind), [interactive demo](https://microsoft.github.io/docfind)

## Conclusion

docfind is an example of modern, engineering-driven tech solving real-world performance and experience challenges for developers—brought to life by the synergy of Rust, WASM, and AI-assisted development.

---

*For questions or feedback, visit the [docfind GitHub repository](https://github.com/microsoft/docfind/issues).*

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2026/01/15/docfind)
