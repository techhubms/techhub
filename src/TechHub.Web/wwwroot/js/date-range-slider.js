// Client-side clamping for dual-handle date range slider.
// Prevents the from-handle from crossing past the to-handle and vice versa.
// Runs immediately in the browser during the capture phase, BEFORE Blazor's
// event handlers read e.target.value via SignalR. This eliminates visual
// glitches caused by the server round-trip delay.
// Server-side Math.Clamp in the C# code-behind serves as a fallback.

/**
 * Initialize client-side clamping for a dual-handle slider.
 * Uses a capture-phase event listener on the container so it fires
 * BEFORE Blazor's bubble-phase event handlers on the input elements.
 * This ensures Blazor receives already-clamped values.
 *
 * @param {HTMLElement} container - The .slider-container element (passed via ElementReference)
 */
export function initClamping(container) {
    const fromSlider = container.querySelector('.slider-from');
    const toSlider = container.querySelector('.slider-to');
    const fill = container.querySelector('.slider-fill');

    if (!fromSlider || !toSlider || !fill) {
        console.warn('[DateRangeSlider] Could not find slider elements for clamping');
        return;
    }

    const max = parseInt(fromSlider.max, 10);

    function updateFill() {
        const fromVal = parseInt(fromSlider.value, 10);
        const toVal = parseInt(toSlider.value, 10);
        const left = (fromVal / max) * 100;
        const width = ((toVal - fromVal) / max) * 100;
        fill.style.left = left + '%';
        fill.style.width = width + '%';
    }

    // Capture phase on container fires BEFORE Blazor's listeners on the target inputs.
    // By clamping input.value here, Blazor reads the corrected value from e.target.value.
    container.addEventListener('input', (e) => {
        if (e.target === fromSlider) {
            const toVal = parseInt(toSlider.value, 10);
            if (parseInt(fromSlider.value, 10) > toVal) {
                fromSlider.value = toVal;
            }
        } else if (e.target === toSlider) {
            const fromVal = parseInt(fromSlider.value, 10);
            if (parseInt(toSlider.value, 10) < fromVal) {
                toSlider.value = fromVal;
            }
        }
        updateFill();
    }, { capture: true });

    console.debug('[DateRangeSlider] Client-side clamping initialized');
}
