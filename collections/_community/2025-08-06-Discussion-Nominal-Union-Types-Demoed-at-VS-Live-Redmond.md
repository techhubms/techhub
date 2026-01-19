---
layout: post
title: 'Discussion: Nominal Union Types Demoed at VS Live, Redmond'
author: ThinksAboutTooMuch
canonical_url: https://www.reddit.com/r/csharp/comments/1mj96yf/nominal_union_types_were_demoted_at_vs_live_at/
viewing_mode: external
feed_name: Reddit CSharp
feed_url: https://www.reddit.com/r/csharp/.rss
date: 2025-08-06 16:22:18 +00:00
permalink: /coding/community/Discussion-Nominal-Union-Types-Demoed-at-VS-Live-Redmond
tags:
- .NET
- .NET 11
- C#
- C# 15
- Discriminated Unions
- Language Design
- Language Features
- Mads Torgerson
- Nominal Union Types
- TypeScript
- Union Types
- VS Live
section_names:
- coding
---
ThinksAboutTooMuch shares insights from a VS Live Redmond session where Mads Torgerson demoed nominal union types for C#, exploring upcoming language features and community reactions.<!--excerpt_end-->

# Recap: Demo of Nominal Union Types at VS Live Redmond

**Posted by: ThinksAboutTooMuch**

Recently at VS Live in Redmond, Mads Torgerson (C# Program Manager at Microsoft) demoed a preview of 'nominal union types' coming to the language. According to the session recap, these types are described as "somewhere between TypeScript unions and discriminated unions," showing Microsoft's ongoing exploration of stronger type support in C#.

### Key Points from the Session

- **Feature Preview:** Demo of 'nominal union types,' a potential new language feature for C# allowing types that can represent one of several named types more safely and expressively.
- **Design Comparison:** The feature sits in the middle ground between TypeScript-style unions (which are open and flexible) and discriminated unions (which are more rigid and type-safe).
- **Proposal Status:** The official proposal documentation was referenced ([link](https://github.com/dotnet/csharplang/blob/main/meetings%2Fworking-groups%2Fdiscriminated-unions%2Funion-proposals-overview.md)), but is currently unavailable (404).
- **Timeline:** Based on the talk, work on this feature might only proceed for C# 15 / .NET 11 because most contributors are tied up with VS monthly releases.
- **Community Impressions:** The post and subsequent comments highlight both desire for more flexible unions (like those in TypeScript) and skepticism about whether C# will achieve the same brevity or elegance. There's a common sentiment that .NET features may sometimes solve only part of the use case.

### Miscellaneous Reactions

- Some playfulness about the language's direction and feature-naming ('demoted' vs 'demoed', 'succ types', 'factorial types'), reflecting the post's light tone.
- Sharing of some images from the presentation ([Imgur link](https://imgur.com/a/igTEpuG)), as the talk wasn't live-streamed.
- Speculation over when the new union type features might land and discussion of their necessity, citing comparison with features already present in TypeScript and handling of legacy features like `Nullable<T>`.

### Links

- [VS Live Conference Overview](https://vslive.com/)
- [Dotnet C# Language Proposals (for updates)](https://github.com/dotnet/csharplang)
- [Discriminated Unions Working Group (if available)](https://github.com/dotnet/csharplang/tree/main/meetings/working-groups/discriminated-unions)

---

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mj96yf/nominal_union_types_were_demoted_at_vs_live_at/)
