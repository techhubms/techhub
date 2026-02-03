// Infinite scroll using Intersection Observer API
// Observes when scroll trigger element becomes visible and notifies Blazor component

export function observeScrollTrigger(dotnetHelper, triggerId) {
    const trigger = document.getElementById(triggerId);
    if (!trigger) {
        console.warn(`[InfiniteScroll] Trigger element #${triggerId} not found`);
        return null;
    }

    console.log('[InfiniteScroll] Setting up Intersection Observer for #' + triggerId);

    const observer = new IntersectionObserver(
        (entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    console.log('[InfiniteScroll] Trigger visible, loading next batch');
                    dotnetHelper.invokeMethodAsync('LoadNextBatch');
                }
            });
        },
        {
            rootMargin: '300px' // Trigger 300px before element becomes visible
        }
    );

    observer.observe(trigger);
    console.log('[InfiniteScroll] Observer active');

    return {
        dispose: () => {
            console.log('[InfiniteScroll] Disposing observer');
            observer.disconnect();
        }
    };
}
