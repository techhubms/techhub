// Infinite scroll using scroll event detection
// Module for detecting when users scroll near the bottom and triggering load
// Uses the same pattern as toc-scroll-spy.js: window scroll events + getBoundingClientRect

let boundHandleScroll = null;
let activeTriggerId = null;

const TRIGGER_MARGIN_PX = 300; // Load when trigger is within 300px of viewport bottom

export function observeScrollTrigger(helper, triggerElementId) {
    // Clean up previous listener if any
    dispose();

    const trigger = document.getElementById(triggerElementId);
    if (!trigger) {
        console.warn('[InfiniteScroll] Trigger element not found:', triggerElementId);
        return;
    }

    // Capture helper and triggerElementId via closure — no module-level state needed.
    // No rAF throttling needed here — unlike TOC scroll-spy (which updates UI on every frame),
    // infinite scroll just checks position and calls LoadNextBatch which is already
    // debounced by the Blazor component's isLoadingMore flag.
    // Avoiding rAF also ensures this works in headless Chrome with --disable-gpu
    // where requestAnimationFrame callbacks are never delivered.
    function handleScroll() {
        const el = document.getElementById(triggerElementId);
        if (!el) return;

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

    // Check immediately in case trigger is already visible (e.g. short content)
    handleScroll();
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
}
