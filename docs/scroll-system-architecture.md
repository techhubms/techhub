# Scroll System Architecture

This document explains the full scroll system: position save/restore, the
`navigating` lifecycle, how infinite scroll and TOC scroll-spy tie into it,
and the known bugs and edge cases with their test coverage.

## Table of Contents

- [Components](#components)
- [The `navigating` Flag](#the-navigating-flag)
- [Scroll Position Save/Restore](#scroll-position-saverestore)
  - [Browser Native Restore Disabled](#browser-native-restore-disabled)
  - [Key Concepts](#key-concepts)
  - [Save Timing](#save-timing)
  - [The `markScriptsReady` Bridge](#the-markscriptsready-bridge)
  - [`allComponentsReady` Gate](#allcomponentsready-gate)
  - [Restore Retry Mechanism](#restore-retry-mechanism)
  - [Fixed Bug: `beforeenhancedload` Timing](#fixed-bug-beforeenhancedload-timing)
- [Forward Navigation Lifecycle](#forward-navigation-lifecycle)
- [Back/Forward Navigation Lifecycle](#backforward-navigation-lifecycle)
- [Infinite Scroll: Normal Flow](#infinite-scroll-normal-flow)
- [TOC Scroll-Spy: How It Ties In](#toc-scroll-spy-how-it-ties-in)
- [The Cascade Bug](#the-cascade-bug)
- [Why requestAnimationFrame (not setTimeout)](#why-requestanimationframe-not-settimeout)
- [Why the Circuit Cache Matters](#why-the-circuit-cache-matters)
- [Why Not Just Disconnect the IO During Navigation?](#why-not-just-disconnect-the-io-during-navigation)
- [Known Edge Cases](#known-edge-cases)
- [Test Coverage](#test-coverage)
- [Related Documentation](#related-documentation)

## Components

| Layer | File | Responsibility |
|-------|------|----------------|
| JavaScript | `src/TechHub.Web/wwwroot/js/scroll-manager.js` | Scroll events, position save/restore, IO, TOC spy, navigation lifecycle |
| Blazor component | `src/TechHub.Web/Components/ContentItemsGrid.razor` | Batch loading, IO setup/teardown, DOM rendering |
| Circuit cache | `src/TechHub.Web/Services/ContentGridStateCache.cs` | Preserves all loaded grid items across enhanced navigations |
| TOC component | `src/TechHub.Web/Components/SidebarToc.razor` | Renders heading links, emits `[data-toc-scroll-spy]` attribute |
| Page scripts | `src/TechHub.Web/wwwroot/js/page-scripts.js` | Calls `initTocScrollSpy()` when `[data-toc-scroll-spy]` exists |

## The `navigating` Flag

A single module-level variable gates all scroll work:

```text
navigating = false | 'forward' | 'traverse'
```

| Value | Set by | Cleared by | Meaning |
|-------|--------|------------|---------|
| `false` | `finishNavigation()` | — | Normal state: scroll events processed |
| `'forward'` | `beforeenhancedload` or `pushState` | `finishNavigation()` via `enhancedload` | User clicked a link, navigating to new page |
| `'traverse'` | `popstate` handler | `finishNavigation()` via `restoreScrollPosition` | Back/forward navigation |

While `navigating` is truthy:

- `onScroll()` → returns immediately (no button updates, no TOC — see [TOC Scroll-Spy](#toc-scroll-spy-how-it-ties-in))
- `onScrollEnd()` → returns immediately (no `lastSettledScrollY` update — prevents stale saves during programmatic scrollTo)
- IO callback → records `pendingIntersect` instead of firing `LoadNextBatch` (see [The Cascade Bug](#the-cascade-bug))

## Scroll Position Save/Restore

### Browser Native Restore Disabled

`history.scrollRestoration` is set to `'manual'` in the Blazor lib module
(`TechHub.Web.lib.module.js`). The browser's async restore races with our
synchronous `restoreScrollPosition()` and randomly clobbers the position with 0.
We own scroll restoration entirely.

### Key Concepts

- **`lastSettledScrollY`**: Updated only in `onScrollEnd` (scrollend event or 150ms
  debounce fallback). Represents the user's true resting position, immune to
  programmatic `scrollIntoView` shifts.
- **`savedPositions`**: Map of `pathname+search` → Y position. Hash is excluded so
  TOC `replaceState` doesn't create conflicting entries.
- **`__scrollSaveLock`**: Test mechanism that overrides `lastSettledScrollY` for one
  save cycle. Set by `ScrollToPositionAsync` in E2E tests because Playwright's
  `scrollIntoViewIfNeeded` fires scroll events before `.click()`, corrupting
  `lastSettledScrollY`.

### Save Timing

Positions are saved in exactly two places:

1. **`pushState` interceptor** (forward nav): `saveScrollPosition()` fires
   synchronously before `originalPushState.apply()` changes the URL.
2. **`popstate` handler** (back/forward): saves `lastSettledScrollY` under the
   page we're LEAVING (`lastPathname + lastSearch`).

### The `markScriptsReady` Bridge

Blazor pages call `markScriptsReady()` at the end of `OnAfterRenderAsync`. This
signals that the DOM is in its final state. `scroll-manager.js` patches this
function to also call `restoreScrollPosition()`. The lifecycle:

1. `enhancedload` fires → lib module sets `__scriptsReady = false`
2. Page renders → `OnAfterRenderAsync` → `markScriptsReady()`
3. Patched `markScriptsReady` → `hideNavSpinner()` + calls original
4. Original polls `allComponentsReady()` at 10ms intervals
5. Once ready: sets `__scriptsReady = true` + invokes `__restoreScrollPosition()`
6. `restoreScrollPosition()` fires → `scrollTo(savedY)` → `rAF(finishNavigation)`

Every page **must** call `markScriptsReady` — enforced by a convention test.

### `allComponentsReady` Gate

`markScriptsReady` doesn't restore immediately — it waits for all layout-affecting
components to finish rendering. This prevents scroll jumps caused by late DOM
changes adding height above the restored position.

`allComponentsReady()` checks three flags (only when the corresponding element exists):

| Flag | Set by | Checked when |
|------|--------|--------------|
| `window.__scrollListenerReady['scroll-trigger']` | `observeScrollTrigger()` in scroll-manager.js | `#scroll-trigger` exists |
| `window.__dateRangeSliderReady` | `date-range-slider.js` init | `#date-range-slider` exists |
| `window.__mermaidReady` | `initMermaid()` in page-scripts.js | Unprocessed `.mermaid` elements exist |

If the element isn't on the page, the flag is skipped — pages without infinite
scroll or mermaid diagrams restore immediately after `markScriptsReady` is called.

### Restore Retry Mechanism

When the page isn't tall enough for the saved Y position (content still loading or
lazy-rendering), `restoreScrollPosition` doesn't give up. It sets up:

- **ResizeObserver** on `<html>` — detects document height changes
- **MutationObserver** on `<body>` — detects DOM additions (childList + subtree)
- **150ms debounce** — coalesces rapid layout shifts into a single scroll attempt
- **30s deadline** — hard timeout to prevent infinite waiting

Once `document.scrollHeight - innerHeight >= savedY`, it scrolls and calls
`finishNavigation()`. If the deadline expires first, it scrolls anyway (best
effort). A `restoreKey` check ensures we abort if the user navigated away.

### Fixed Bug: `beforeenhancedload` Timing

Previously, `beforeenhancedload` would scroll to top (zeroing `lastSettledScrollY`)
before `pushState` saved the position — causing the save to record 0. This was
fixed by moving `saveScrollPosition()` into `beforeenhancedload` (before
`scrollTo(0)`) and having the `pushState` interceptor skip saving when
`navigating='forward'` (meaning `beforeenhancedload` already handled it).

Current (correct) event order:

```text
beforeenhancedload → saveScrollPosition() ✓ → scrollTo(0) → navigating='forward'
    ... circuit update ...
pushState → navigating='forward' → SKIP save (already done)
```

```mermaid
sequenceDiagram
    participant U as User
    participant B as Blazor
    participant SM as scroll-manager.js
    participant H as History API

    U->>B: Click link
    B->>SM: beforeenhancedload event
    SM->>SM: saveScrollPosition() ✓ (correct lastSettledScrollY)
    SM->>SM: scrollTo(0), lastSettledScrollY = 0
    SM->>SM: navigating = 'forward'
    Note over B: Circuit update + DOM patch
    B->>H: history.pushState(newUrl)
    H->>SM: pushState interceptor
    SM->>SM: navigating='forward' → skip save
```

**`__scrollSaveLock`** remains as a test utility for E2E tests where Playwright's
`scrollIntoViewIfNeeded` fires scroll events that corrupt `lastSettledScrollY`
before the test's programmatic click triggers navigation.

## Forward Navigation Lifecycle

```mermaid
sequenceDiagram
    participant U as User
    participant SM as scroll-manager.js
    participant BZ as Blazor

    U->>BZ: Click enhanced link
    BZ->>SM: beforeenhancedload
    SM->>SM: saveScrollPosition() (correct value)
    SM->>SM: scrollTo(0), navigating='forward', show spinner

    Note over BZ: Fetch + render new page

    BZ->>SM: pushState (URL update)
    SM->>SM: navigating='forward' → skip save
    BZ->>SM: enhancedload
    SM->>SM: hideSpinner, focus body
    SM->>SM: finishNavigation() → navigating=false
```

## Back/Forward Navigation Lifecycle

```mermaid
sequenceDiagram
    participant U as User
    participant SM as scroll-manager.js
    participant BZ as Blazor
    participant IO as IntersectionObserver
    participant Cache as GridStateCache

    U->>SM: Press Back → popstate
    SM->>SM: Save leaving page position
    SM->>SM: navigating = 'traverse'

    Note over BZ: Re-render page from cache
    Cache-->>BZ: Return all cached batches (e.g. 1-3)
    BZ->>SM: observeScrollTrigger() → new IO created

    IO->>SM: Fires (trigger visible at Y=0)
    SM->>SM: navigating='traverse' → pendingIntersect=true

    BZ->>SM: markScriptsReady → restoreScrollPosition
    SM->>SM: scrollTo(savedY)

    Note over SM: rAF fires AFTER IO in same frame
    SM->>SM: finishNavigation()
    SM->>SM: Check: is trigger visible at savedY?

    alt Trigger in viewport (user was at bottom)
        SM->>BZ: LoadNextBatch (correct)
    else Trigger not visible (user was mid-page)
        SM->>SM: Clear pendingIntersect (no load)
    end
```

## Infinite Scroll: Normal Flow

```mermaid
sequenceDiagram
    participant U as User
    participant IO as IntersectionObserver
    participant SM as scroll-manager.js
    participant BZ as Blazor (ContentItemsGrid)

    BZ->>SM: observeScrollTrigger(helper, "scroll-trigger")
    SM->>IO: new IO with 300px rootMargin

    U->>U: Scrolls down
    IO->>SM: Trigger enters viewport+300px
    SM->>SM: navigating=false → proceed
    SM->>IO: disconnect() (one-shot)
    SM->>BZ: helper.invokeMethodAsync('LoadNextBatch')

    BZ->>BZ: Load batch N+1, render
    BZ->>SM: observeScrollTrigger() again (fresh IO)
    Note over SM: Cycle repeats until hasMoreContent=false
```

The 300px `rootMargin` triggers loading *before* the user sees the end of content.
The one-shot disconnect + re-attach architecture makes cascade impossible during
normal scrolling: the trigger must leave and re-enter the zone for another batch.

## TOC Scroll-Spy: How It Ties In

The TOC uses the same `navigating` flag and scroll events:

```mermaid
flowchart TD
    A[scroll event fires] --> B{navigating?}
    B -->|yes| C[return early]
    B -->|no| D[updateButtonVisibility]
    D --> E{tocState && !rafPending?}
    E -->|yes| F[RAF → updateTocHighlight]
    E -->|no| G[skip]

    H[scrollend fires] --> I{navigating?}
    I -->|yes| J[return early]
    I -->|no| K[lastSettledScrollY = scrollY]
    K --> L[updateTocHighlight - final pass]
    L --> M[replaceState with active heading hash]
```

Key integration points:

- **During navigation**: TOC updates are suppressed (no stale highlights from
  intermediate scroll positions during restore).
- **`finishNavigation`** calls `onScrollEnd()` once → triggers a TOC highlight
  pass at the final restored position.
- **`pageKey` guard**: `setTocActive` only calls `replaceState` if we're still on
  the page where TOC was initialized — prevents hash from leaking to the next page
  if a late scrollend fires after navigation starts.
- **Click handling**: TOC clicks use `replaceState` (not `pushState`) so they don't
  pollute history. The `beforeenhancedload` handler has `if (navigating) return` to
  avoid resetting scroll on same-page hash navigation.

## The Cascade Bug

**Problem**: On back-nav, the page initially renders at scroll-Y=0. The IO trigger
sits near the top of the (short) page. If the IO callback calls `LoadNextBatch`
immediately, it creates a chain: batch 4 loads → trigger still visible → fires
again → batch 5 → … → all content loads.

**Root cause**: IO fires at the restored scroll position (or at Y=0 while the
circuit cache is still being restored) before the user has deliberately scrolled
to request more content.

**Fix**: `skipFirst` — when `startObserving` is called after a traverse navigation
(either from `finishNavigation` for the deferred case, or from `observeScrollTrigger`
when `postNavPending=true`), a `skipFirst=true` flag is passed. The IO closure keeps
a `firstSkipped` boolean: the very first intersecting entry is silently ignored and
the observer stays connected. Only subsequent intersection entries (triggered by
actual user scrolling away and back) load the next batch.

This is strictly safer than the previous "allow 1 batch then gate" approach:

- No `postNavBatchFlushed` flag that a stray scroll event could clear before Blazor
  re-attaches, accidentally unblocking the cascade.
- Trade-off: if the trigger is visible on return, the user must scroll away (trigger
  exits zone) and back (re-enters zone) before more content loads. This is acceptable
  because in the happy path all needed content is already cached.

**`postNavPending` race guard**: In rare cases `finishNavigation` can run *before*
`observeScrollTrigger` is called (e.g. `markScriptsReady` fires before Blazor's
`OnAfterRenderAsync`):

- `finishNavigation` sees `infiniteScrollState?.deferred` is null → sets
  `postNavPending = true`.
- It also immediately starts an early observer with `skipFirst=true` using
  `lastHelper`/`lastTriggerElementId` (saved from the most recent
  `observeScrollTrigger` call), so any user scroll during the race window is caught.
- When `observeScrollTrigger` is later called, `dispose()` stops the early observer
  and restarts with `skipFirst=true` (because `postNavPending` is still set).
- If the early observer fires a real load before `observeScrollTrigger` arrives,
  it clears `postNavPending` so the re-attach does not double-skip.

`postNavPending` is also cleared in `beforeenhancedload` so a subsequent forward
navigation cannot accidentally inherit the flag.

## Why requestAnimationFrame (not setTimeout)

The HTML spec rendering pipeline (step 13 "update the rendering"):

- Step 13.10: Run IntersectionObserver callbacks
- Step 13.14: Run rAF callbacks

Within a single frame after `scrollTo`:

1. IO processes new geometry → fires callback (sees `navigating='traverse'`)
2. rAF fires → `finishNavigation()` resets `navigating = false`

`setTimeout(fn, 0)` races with IO — it's a task, not a rendering step. It can
fire before or after IO depending on the browser's task queue.

## Why the Circuit Cache Matters

Without `ContentGridStateCache`, back-navigation starts with batch 1 only. The page
is short, trigger is visible, and cascade occurs. The cache preserves ALL loaded
items per filter-key, yielding a tall page that pushes the trigger below viewport
at the saved scroll position.

## Why Not Just Disconnect the IO During Navigation?

1. **Blazor creates a NEW observer during navigation.** `OnAfterRenderAsync` calls
   `observeScrollTrigger()` — disconnecting the old one doesn't prevent the new one.
2. **Can't skip observing during navigation.** After `finishNavigation`, no observer
   would exist and Blazor won't re-call `observeScrollTrigger`.
3. **Re-observing a visible element fires immediately.** IO spec delivers the first
   intersection entry synchronously. The `skipFirst` mechanism handles this correctly.

## Known Edge Cases

| Edge Case | Behavior | Covered By |
|-----------|----------|------------|
| Trigger visible at restored position | First IO skipped, observer stays live; user must scroll away and back to load | Unit: "should NOT call LoadNextBatch on first intersection after back-nav" |
| Trigger NOT visible at restored position | First IO fires when user scrolls to trigger → skipped; second fire loads | Unit: "should call LoadNextBatch on second intersection after back-nav" |
| `postNavPending` race guard | finishNavigation ran before observeScrollTrigger; early observer started immediately with `skipFirst=true`; `observeScrollTrigger` reconnects cleanly | Unit: "should apply skipFirst to re-attach when postNavPending is set" / "should start early observer…" |
| Forward nav (trigger far away) | No issue — scrollTo(0) pushes trigger off-screen | IO guard: only `'traverse'` sets skipFirst |
| Trigger scrolled past (negative top) | `top < innerHeight + 300` → true → loads | Correct: past trigger means content is needed |
| Page not tall enough for restore | ResizeObserver + MutationObserver retry (150ms debounce, 30s deadline) | Unit: scroll retry tests |
| `beforeenhancedload` save timing | Save fires before scrollTo(0) — correct position captured | [Fixed Bug](#fixed-bug-beforeenhancedload-timing) |
| TOC replaceState during scroll | pageKey guard prevents hash leaking to new page | Unit: TOC page-key tests |

## Test Coverage

### Unit Tests (`tests/javascript/scroll-manager.test.js`)

**Infinite scroll:**

- `should call LoadNextBatch when trigger enters intersection margin`
- `should NOT call LoadNextBatch when trigger exits intersection margin`
- `should disconnect observer after LoadNextBatch is called (prevents cascade)`
- `should NOT call LoadNextBatch during navigation`
- `should NOT call LoadNextBatch on first intersection after back-nav (skipFirst gate)`
- `should call LoadNextBatch on second intersection after back-nav (after first is skipped)`
- `should apply skipFirst to re-attach when postNavPending is set (finishNavigation before observeScrollTrigger)`
- `should start early observer immediately in postNavPending race if lastHelper is saved`
- `should clear postNavPending when early observer fires real load, so re-attach does not double-skip`
- `should create a new observer on re-attach after batch load`

**Scroll position:**

- Scroll position save/restore on pushState and popstate
- `lastSettledScrollY` update only on scrollend
- Hash-only navigation does not trigger save/restore

**TOC:**

- Highlight updates on scroll (RAF-throttled)
- Final highlight pass on scrollend
- `pageKey` guard prevents cross-page replaceState
- replaceState (not pushState) for hash updates

### E2E Tests (`tests/TechHub.E2E.Tests/Web/`)

- `InfiniteScrollBackNavigationTests.BackNavigation_AfterInfiniteScroll_RestoresScrollPosition` — full infinite scroll + back-nav + position check
- `InfiniteScrollBackNavigationTests.BackNavigation_AfterInfiniteScroll_DoesNotTriggerCascade` — no cascade on back-nav
- `InfiniteScrollBackNavigationTests.BackNavigation_CascadingLoad_ScrollDoesNotReachEnd` — scroll position stays near saved Y, page doesn't reach end
- `ScrollRestorationTests.BackNavigation_OnLongContentPage_RestoresScrollPosition` — isolates restore on a page WITHOUT infinite scroll

### E2E Test Helpers (`tests/TechHub.E2E.Tests/Helpers/BlazorHelpers.cs`)

- `ScrollToLoadMoreAsync` — scroll until item count increases, waits for `__scrollListenerReady`
- `ScrollToEndOfContentAsync` — scroll until all content loaded
- `ScrollToPositionAsync` — scroll to Y + set `__scrollSaveLock` (prevents Playwright race condition)

## Related Documentation

- [docs/javascript.md](javascript.md) — JavaScript architecture, scroll manager overview
- [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) — Web project conventions
- [tests/TechHub.E2E.Tests/AGENTS.md](../tests/TechHub.E2E.Tests/AGENTS.md) — E2E patterns for infinite scroll
