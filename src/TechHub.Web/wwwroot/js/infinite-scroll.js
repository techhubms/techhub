// Infinite scroll using Intersection Observer API
// Module for detecting when users scroll near the bottom and triggering load

let observer = null;
let dotnetHelper = null;

export function observeScrollTrigger(helper, triggerId) {
    // Clean up previous observer if any
    dispose();

    dotnetHelper = helper;
    const trigger = document.getElementById(triggerId);

    if (!trigger) {
        console.warn('[InfiniteScroll] Trigger element not found:', triggerId);
        return;
    }

    observer = new IntersectionObserver(
        (entries) => {
            const entry = entries[0];
            if (entry?.isIntersecting && dotnetHelper) {
                console.debug('[InfiniteScroll] Loading next batch');
                dotnetHelper.invokeMethodAsync('LoadNextBatch');
            }
        },
        {
            rootMargin: '300px', // Trigger 300px before element is visible
            threshold: 0
        }
    );

    observer.observe(trigger);
    console.debug('[InfiniteScroll] Observer active for:', triggerId);
}

export function dispose() {
    if (observer) {
        observer.disconnect();
        observer = null;
    }
    dotnetHelper = null;
}
