---
title: The uphill climb of making diff lines performant
primary_section: devops
feed_name: GitHub Engineering Blog
author: Luke Ghenco
external_url: https://github.blog/engineering/architecture-optimization/the-uphill-climb-of-making-diff-lines-performant/
section_names:
- devops
date: 2026-04-03 16:00:00 +00:00
tags:
- Architecture & Optimization
- Code Review
- Component Architecture
- CSS Selectors
- Datadog Monitoring
- DevOps
- Diff Rendering
- Diffs
- DOM Optimization
- Engineering
- Event Delegation
- Frontend Performance
- GitHub
- GPU Transforms
- Hydration
- Interaction To Next Paint (inp)
- JavaScript Heap
- JavaScript Map
- Memoization
- Memory Optimization
- News
- Performance Engineering
- Progressive Loading
- Pull Requests
- React
- State Management
- TanStack Virtual
- Useeffect
- Virtualization
- Web Performance
---

Luke Ghenco explains how GitHub improved pull request “Files changed” performance by simplifying React diff-line components, reducing DOM/event handler overhead, and introducing virtualization for very large diffs.<!--excerpt_end-->

# The uphill climb of making diff lines performant

Pull requests are central to how engineers collaborate on GitHub, and at GitHub’s scale a PR can range from a one-line change to thousands of files and millions of lines. GitHub recently shipped a new React-based **Files changed** experience (now the default), with a focus on keeping review fast and responsive—especially for large pull requests.

In worst cases before these optimizations, GitHub observed:

- JavaScript heap exceeding **1 GB**
- DOM node counts surpassing **400,000**
- Page interactions becoming sluggish or unusable
- Poor **Interaction to Next Paint (INP)** scores (a key responsiveness metric)

GitHub had already referenced some improvements in a changelog, but this article explains what changed, why it mattered, and what the team measured.

## Performance improvements by pull request size and complexity

The team found there was no single fix that worked across all PR sizes:

- Preserving all features and browser-native behaviors can still hit a ceiling at extreme sizes.
- “Worst-case” mitigations can be the wrong tradeoff for typical reviews.

They pursued multiple targeted strategies:

- **Focused optimizations for diff-line components** to keep most PRs fast without losing expected behavior (like native find-in-page).
- **Graceful degradation with virtualization** to keep the largest PRs usable by limiting what’s rendered.
- **Foundational component and rendering improvements** that compound across all PR sizes.

## First steps: optimizing diff lines

The team set three primary goals:

1. Reduce memory and JavaScript heap size.
2. Reduce DOM node count.
3. Reduce average INP, and significantly improve p95/p99.

The guiding approach was simplification: less state, fewer elements, less JavaScript, fewer React components.

## What worked and what didn’t with v1

In v1, each diff line was expensive:

- Unified view: roughly **10 DOM elements** per line
- Split view: closer to **15 DOM elements** per line
- Syntax highlighting added many more `<span>` nodes

React structure was also heavy:

- Unified diffs: typically **8+ React components** per line
- Split diffs: **13+ components** per line
- Extra UI states (comments/hover/focus) added more

A major contributor was event handling:

- Many small components each attached multiple event handlers (often **5–6 per component**)
- A single diff line could carry **20+ event handlers**, multiplied across thousands of lines

This approach initially made sense when porting diff lines from the classic Rails view and leaning into small reusable components, but it proved unsustainable at large scale.

### v1 summary

For every v1 diff line:

- Minimum of **10–15** DOM elements
- Minimum of **8–13** React components
- Minimum of **20** React event handlers

Larger PRs consistently led to slower INP and higher heap usage.

## Small changes make a large impact: v2

Even tiny DOM reductions matter at scale. One example: removing unnecessary `<code>` tags from line-number cells.

- Dropping 2 DOM nodes per diff line
- Over 10,000 lines, that’s **20,000 fewer nodes**

Images in the original post illustrate the v1 vs v2 DOM and component structures:

- ![V1 Diff Components and HTML. We had 8 react components for a single diff line.](https://github.blog/wp-content/uploads/2026/04/3.png?resize=960%2C1340)
- ![V1 HTML DOM structure. It is a typical HTML table structure with <tr> elements and <td> elements.](https://github.blog/wp-content/uploads/2026/04/Screenshot-2026-04-02-at-4.12.35-PM.png?resize=720%2C899)
- ![V2 HTML DOM structure. It is a typical HTML table structure with <tr> elements and <td> elements. The difference between V1 and V2 is the lack of <code> tags in the diff line number elements.](https://github.blog/wp-content/uploads/2026/04/Screenshot-2026-04-02-at-4.12.47-PM.png?resize=704%2C874)
- ![V1 Diff Components and HTML. We had 8 react components for a single diff line.](https://github.blog/wp-content/uploads/2026/04/Screenshot-2026-04-02-at-4.13.00-PM.png?resize=681%2C1024)
- ![V2 Diff Components and HTML. We had 3 react components for a single diff line.](https://github.blog/wp-content/uploads/2026/04/Screenshot-2026-04-02-at-4.13.11-PM.png?resize=667%2C1024)

The team reduced per-line React components significantly (described as going from eight to two), mainly by removing thin wrapper components that supported both Split and Unified views at once.

Instead, v2 uses dedicated components per view. This duplicates some code, but is simpler and faster.

### Simplifying the component tree

Changes in v2 included:

- Removing deeply nested component trees
- Using dedicated components for split vs unified diff lines
- Accepting some code duplication to reduce indirection and complexity

### Centralizing event handling

Instead of per-line mouse handlers, v2 uses a single top-level handler with `data-attribute` values.

Example behavior described:

- When a user click-drags to select multiple diff lines, the top-level handler checks the event’s `data-attribute` to decide which lines to highlight.

### Moving complex state into conditionally rendered child components

The most impactful structural change was moving state for:

- commenting
- context menus

…into their own components.

Rationale: for PRs with thousands of lines, it’s inefficient for every line to carry complex state when only a small subset will ever be interacted with. This also aligns the main diff-line component more closely with the Single Responsibility Principle.

### O(1) data access and fewer `useEffect` hooks

Problems in v1:

- O(n) lookups across shared data stores/state
- Extra re-rendering due to `useEffect` hooks spread through the diff-line tree

v2 mitigations:

- Restrict `useEffect` usage to the top level of diff files
- Add lint rules to prevent `useEffect` hooks inside line-wrapping components
- Redesign global and diff state machines to use O(1) lookups with `Map`

An access example from the article:

```js
commentsMap[‘path/to/file.tsx’][‘L8’]
```

## Did it work?

GitHub reports large improvements on a test PR using split diff with **10,000** line changes.

| Metric | v2 | Improvement |
| --- | --- | --- |
| Total lines of code | 2,000 | 27% less |
| Total unique component types | 10 | 47% fewer |
| Total components rendered | ~50,004 | 74% fewer |
| Total DOM nodes | ~180,000 | 10% fewer |
| Total memory usage | ~80–120 MB | ~50% less |
| INP on a large PR (M1 MacBook Pro, 4x slowdown) | ~100 ms | ~78% faster |

## Virtualization for the largest pull requests

For p95+ PRs (over 10,000 diff lines plus context), even optimized components can’t keep responsiveness if tens of thousands of lines are rendered at once.

GitHub adopted **window virtualization**, rendering only what’s visible and swapping elements while scrolling.

They integrated **TanStack Virtual**:

- [TanStack Virtual](https://tanstack.com/virtual/latest)

Reported impact for p95+ PRs:

- **10x reduction** in JavaScript heap usage and DOM nodes
- INP reduced from **275–700+ ms** down to **40–80 ms**

## Further performance optimizations

Additional improvements mentioned:

- Reduce unnecessary React re-renders and improve state management
- Replace heavy CSS selectors (example: `:has(...)`)
- Re-engineer drag/resize handling using **GPU transforms** to avoid forced layouts
- Improve monitoring with interaction-level INP tracking, diff-size segmentation, and memory tagging
- Surface these metrics in a **Datadog** dashboard
- Server-side optimization to hydrate only visible diff lines (reducing time-to-interactive and memory)
- Progressive diff loading and background fetches so users can interact sooner

## Summary

GitHub’s “Files changed” performance work centered on making the diff-line architecture cheaper to render and easier to reason about:

- Fewer DOM nodes and a simpler component tree
- Centralized event handling
- Move complex state out of the baseline diff line
- Prefer O(1) lookups and limit `useEffect` to predictable places
- Use virtualization (TanStack Virtual) for extreme PR sizes

For readers who want to see the changes in practice, the post suggests checking open PRs:

- http://github.com/pulls


[Read the entire article](https://github.blog/engineering/architecture-optimization/the-uphill-climb-of-making-diff-lines-performant/)

