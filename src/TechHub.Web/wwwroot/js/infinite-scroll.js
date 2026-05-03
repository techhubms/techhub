// Infinite scroll using scroll event detection
// Module for detecting when users scroll near the bottom and triggering load
// Uses the same pattern as toc-scroll-spy.js: window scroll events + getBoundingClientRect

let boundHandleScroll = null;
let activeTriggerId = null;
let initialPagePath = null; // pathname+search when listener attached; detects page changes
let suppressNextTriggerCheck = false; // Anti-cascade: skip trigger check after scroll restore

const TRIGGER_MARGIN_PX = 300; // Load when trigger is within 300px of viewport bottom

export function observeScrollTrigger(helper, triggerElementId, suppressOnAttach = false) {
    // Clean up previous listener if any
    dispose();

    // Set the suppress flag atomically with listener attachment when restoring from
    // circuit cache (back-navigation). This prevents a race where markScriptsReady()
    // fires a scroll-restore event between the old separate setSuppressNextTriggerCheck()
    // call and this observeScrollTrigger() call — a window in which handleScroll would
    // run without suppression and could trigger a cascade of batch loads.
    // Passing suppressOnAttach = true eliminates that two-call window: the flag is set
    // inside this single JS execution, atomically with listener setup.
    //
    // Second-layer defence: detect back/forward navigation via the Navigation API
    // (Chromium 102+). When navigationType === 'traverse', markScriptsReady() will fire
    // a scroll-restoration event regardless of whether the C# layer detected a cache hit.
    // For example, if PersistentComponentState (prerender) is used instead of the circuit
    // cache, suppressOnAttach arrives as false but a traverse scroll still fires. Without
    // this guard the trigger check would run unsuppressed and cascade-load extra batches.
    const isTraversal = window.navigation?.currentEntry?.navigationType === 'traverse';
    if (suppressOnAttach || isTraversal) {
        suppressNextTriggerCheck = true;
    }

    const trigger = document.getElementById(triggerElementId);
    if (!trigger) {
        console.warn('[InfiniteScroll] Trigger element not found:', triggerElementId);
        return;
    }

    // Record the page path so we can detect when the user navigates away during
    // enhanced navigation. If the URL changes before our listener is removed,
    // handleScroll must stop immediately to avoid triggering loads on the wrong page.
    // Same pattern as toc-scroll-spy.js's initialPagePath guard.
    initialPagePath = window.location.pathname + window.location.search;

    // Capture helper and triggerElementId via closure — no module-level state needed.
    // No rAF throttling needed here — unlike TOC scroll-spy (which updates UI on every frame),
    // infinite scroll just checks position and calls LoadNextBatch which is already
    // debounced by the Blazor component's isLoadingMore flag.
    // Avoiding rAF also ensures this works in headless Chrome with --disable-gpu
    // where requestAnimationFrame callbacks are never delivered.
    function handleScroll() {
        // During Blazor enhanced navigation the URL changes (pushState) before the old
        // component is disposed. nav-helpers.js resetPagePosition() scrolls to top,
        // firing a scroll event while this listener is still active. Without this guard
        // we'd interact with stale state.
        if (initialPagePath !== null &&
            window.location.pathname + window.location.search !== initialPagePath) {
            dispose();
            return;
        }

        const el = document.getElementById(triggerElementId);
        if (!el) return;

        // After scroll restoration, skip the trigger check to prevent a cascade.
        // Layout differences (fonts, images, async CSS) can place the trigger just
        // inside the margin. The flag is set via suppressOnAttach (or the legacy
        // setSuppressNextTriggerCheck) and cleared here so only the first scroll
        // event after back-navigation is suppressed. Subsequent scroll events proceed
        // normally.
        if (suppressNextTriggerCheck) {
            suppressNextTriggerCheck = false;
            return;
        }

        // Trigger when the element is within TRIGGER_MARGIN_PX of the viewport bottom
        // Same concept as IO rootMargin: '300px' but using scroll events
        if (el.getBoundingClientRect().top <= window.innerHeight + TRIGGER_MARGIN_PX) {
            console.debug('[InfiniteScroll] Loading next batch');
            helper.invokeMethodAsync('LoadNextBatch');
        }
    }

    boundHandleScroll = handleScroll;
    activeTriggerId = triggerElementId;
    window.addEventListener('scroll', boundHandleScroll, { passive: true });

    // Signal that the scroll listener for this specific trigger is attached and ready.
    // Scoped by triggerId so multiple concurrent listeners don't interfere.
    // E2E tests wait for this before scrolling to ensure timing is correct.
    window.__scrollListenerReady ??= {};
    window.__scrollListenerReady[triggerElementId] = true;

    // Increment a version counter so E2E tests can detect a FRESH listener attachment
    // vs a stale one. After a tag filter re-render, dispose() resets the readiness flag
    // to false, but a point-in-time check might see the previous true before dispose()
    // runs. The version counter lets tests wait for "version > previousVersion" which
    // is immune to this race because the counter only increases on fresh attachments.
    window.__scrollListenerVersion ??= {};
    const currentVersion = window.__scrollListenerVersion[triggerElementId] || 0;
    window.__scrollListenerVersion[triggerElementId] = currentVersion + 1;

    if (typeof window.__e2eSignal === 'function') window.__e2eSignal('scroll-listener:' + triggerElementId);

    console.debug('[InfiniteScroll] Scroll listener active for:', triggerElementId);

    // Check immediately in case trigger is already visible (e.g. short content).
    // Skip when suppression is active: suppressOnAttach was set for a back-navigation
    // restore. The suppress flag must remain set to handle the first scroll event fired
    // by markScriptsReady's window.scrollTo() — consuming it here would allow the
    // restore scroll to trigger LoadNextBatch and cascade.
    if (!suppressNextTriggerCheck) {
        handleScroll();
    }
}

export function dispose() {
    if (boundHandleScroll) {
        window.removeEventListener('scroll', boundHandleScroll);
        boundHandleScroll = null;
    }
    if (activeTriggerId) {
        // Set to false instead of deleting — this ensures E2E tests that poll for
        // __scrollListenerReady[triggerId] === true will correctly see the transition
        // from true → false → true when the listener is re-attached after a filter change.
        window.__scrollListenerReady ??= {};
        window.__scrollListenerReady[activeTriggerId] = false;
        if (typeof window.__e2eSignal === 'function') window.__e2eSignal('scroll-disposed:' + activeTriggerId);
        activeTriggerId = null;
    }
    initialPagePath = null;
    // NOTE: suppressNextTriggerCheck is NOT reset here. It survives through the
    // dispose() → re-attach cycle that happens in observeScrollTrigger().
    // It's cleared inside handleScroll().
}

// Suppress the next trigger check in handleScroll to prevent a cascade of batch
// loads on back-navigation. Layout differences between the original and restored
// page can place the trigger just inside the TRIGGER_MARGIN_PX threshold.
// Prefer passing suppressOnAttach=true to observeScrollTrigger (atomic, no race).
// This function is kept for any standalone callers that set the flag separately.
export function setSuppressNextTriggerCheck() {
    suppressNextTriggerCheck = true;
}
