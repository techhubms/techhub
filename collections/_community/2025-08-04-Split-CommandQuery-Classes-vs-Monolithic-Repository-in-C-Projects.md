---
external_url: https://www.reddit.com/r/csharp/comments/1mhdqbc/split_commandquery_classes_vs_monolithic/
title: Split Command/Query Classes vs Monolithic Repository in C# Projects
author: Square_Potato6312
feed_name: Reddit CSharp
date: 2025-08-04 13:39:41 +00:00
tags:
- Backend Development
- C#
- Command Query Segregation
- CQRS
- Design Patterns
- File Organization
- Maintainability
- Project Structure
- Repository Pattern
- Software Practices
section_names:
- coding
primary_section: coding
---
In this community post, Square_Potato6312 discusses their recent experiences with CQRS and monolithic repository patterns in C# backend development, exploring team perspectives and best practices.<!--excerpt_end-->

## Split Command/Query Classes vs Monolithic Repository in C# Projects

**Author:** Square_Potato6312

### Background

Recently, during job interviews, the author was frequently asked about knowledge of CQRS (Command Query Responsibility Segregation). In a C#/Angular project taken over from another developer, a CQRS pattern was implemented in the C# backend. This involved defining individual classes such as `GetStuffQuery` and `UpdateStuffCommand` for each distinct operation.

### Observations from CQRS

The author found value in the clear separation and organized structure this approach brought, even if it increased the number of files compared to a single, monolithic repository that mixed multiple operations. Despite occasional feelings that this structure was perhaps excessive ("overkill"), the clarity and maintainability of separating concerns into individual classes were appreciated.

### Subsequent Project and Team Preferences

In a following project, the author started building a small library to consume an API, instinctively adopting a similar split-class structure for commands and queries. Additionally, a simple facade class was introduced for ease of use.

However, colleagues reacted negatively, expressing a preference for a single, monolithic file that combined all methods together. They also argued that naming a class after an action (e.g., `UpdateStuffCommand`) was poor practice. The author notes that past experience aligned with this monolithic approach, but the recent positive experiences with CQRS prompted reconsideration of such traditions.

### Personal Reflection

While not committed to defending either approach, the author expresses a desire for clean and maintainable code over following trends. Nonetheless, they anticipate that team pressure may necessitate reverting to the monolithic style, although it feels counterproductive after experiencing the benefits of CQRS.

### Open Question

The post concludes by asking the community for perspectives on modern best practices regarding code organization within this context—whether splitting command/query logic into dedicated classes is now preferable, or if monolithic repositories are still considered viable or, perhaps, outdated.

---

**Original Post:** [Reddit link](https://www.reddit.com/r/csharp/comments/1mhdqbc/split_commandquery_classes_vs_monolithic/)

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mhdqbc/split_commandquery_classes_vs_monolithic/)
