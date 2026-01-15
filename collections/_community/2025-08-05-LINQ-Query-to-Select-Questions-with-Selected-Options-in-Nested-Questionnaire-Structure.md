---
layout: post
title: LINQ Query to Select Questions with Selected Options in Nested Questionnaire Structure
author: GreatlyUnknown
canonical_url: https://www.reddit.com/r/csharp/comments/1mikvpz/im_sure_there_is_a_linq_query_for_this_but_i_just/
viewing_mode: external
feed_name: Reddit CSharp
feed_url: https://www.reddit.com/r/csharp/.rss
date: 2025-08-05 20:46:37 +00:00
permalink: /coding/community/LINQ-Query-to-Select-Questions-with-Selected-Options-in-Nested-Questionnaire-Structure
tags:
- .NET
- C#
- Coding
- Community
- Data Query
- Functional Programming
- LINQ
- Nested Collections
- Options Pattern
- Query Optimization
- Questionnaire
- Questions Filtering
- SelectMany
section_names:
- coding
---
GreatlyUnknown seeks advice on writing a LINQ query to retrieve questions with selected options from a nested questionnaire structure, sparking a discussion about effective usage of SelectMany in C#.<!--excerpt_end-->

# LINQ Query to Select Questions with Selected Options in Nested Questionnaire Structure

**Author:** GreatlyUnknown

## Problem Statement

A questionnaire has the following nested structure:

- **Categories**
  - **Sections**
    - **Questions**
      - **Options** (each can be selected or not)

The goal is to collect all `Question` objects with at least one selected `Option`, avoiding deeply nested foreach loops in favor of a LINQ-based approach.

## Example LINQ Solutions

Participants suggested using `SelectMany` and `Where`:

```csharp
var questionsWithAtLeastOneSelectedOption =
    categories
        .SelectMany(c => c.Sections)
        .SelectMany(s => s.Questions)
        .Where(q => q.Options.Any(o => o.IsSelected));
```

Or, more succinctly (if query syntax is preferred):

```csharp
var questionsWithSelected =
    from c in categories
    from s in c.Sections
    from q in s.Questions
    where q.Options.Any(o => o.IsSelected)
    select q;
```

### Key Notes

- **SelectMany** flattens nested collections, making it effective for traversing multi-level data.
- **Where** allows filtering to questions that meet the criteria (at least one option selected).

## Community Takeaways

- Use of `SelectMany` is standard for this kind of nested selection in C#.
- Remembering key LINQ operators helps keep code concise and readable.
- Some participants advocate using AI tools for such queries, though traditional community support is still valued.

## Summary

If working with a nested questionnaire structure in C#, use `SelectMany` and `Where` to efficiently filter for questions with selected options, avoiding cumbersome nested loops.

---

**References:**

- [LINQ SelectMany Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.selectmany)
- [LINQ Filtering Patterns](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/filtering)

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mikvpz/im_sure_there_is_a_linq_query_for_this_but_i_just/)
