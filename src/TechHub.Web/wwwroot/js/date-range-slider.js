// Client-side clamping for dual-handle date range slider.
// Prevents the from-handle from crossing past the to-handle and vice versa.
// Runs immediately in the browser during the capture phase, BEFORE Blazor's
// event handlers read e.target.value via SignalR. This eliminates visual
// glitches caused by the server round-trip delay.
// Server-side Math.Clamp in the C# code-behind serves as a fallback.

// Tracks the instance ID of the most recent initClamping call.
// Used by reset() to avoid clobbering a newer instance's ready state when
// Blazor Server delivers DisposeAsync's reset() after the new component's
// initClamping() due to concurrent async execution.
let _activeInstanceId = null;

/**
 * Called by DateRangeSlider.DisposeAsync to clear the ready flag.
 * Only resets when instanceId matches the active initClamping instance;
 * a stale reset from a disposed component is ignored if a newer one has
 * already initialized.
 *
 * @param {number} instanceId - The instance ID assigned in C# (passed via ElementReference)
 */
export function reset(instanceId) {
    if (_activeInstanceId === instanceId) {
        _activeInstanceId = null;
        window.__dateRangeSliderReady = false;
    }
}

/**
 * Initialize client-side clamping for a dual-handle slider.
 * Uses a capture-phase event listener on the container so it fires
 * BEFORE Blazor's bubble-phase event handlers on the input elements.
 * This ensures Blazor receives already-clamped values.
 *
 * @param {HTMLElement} container - The .slider-container element (passed via ElementReference)
 * @param {number} instanceId - Per-instance ID generated in C# to guard against stale reset() calls
 */
export function initClamping(container, instanceId) {
    // Record this instance as the active one before any work.
    // A subsequent reset(oldId) call for a disposed predecessor will be a no-op
    // because oldId !== instanceId.
    _activeInstanceId = instanceId;

    // Signal that initialization is in progress. Cleared on all exit paths so
    // markScriptsReady knows whether to wait for this component.
    window.__dateRangeSliderReady = false;

    if (!container) {
        console.warn('[DateRangeSlider] #date-range-slider element not available, skipping clamping init');
        window.__dateRangeSliderReady = true;
        return;
    }

    const fromSlider = container.querySelector('.slider-from');
    const toSlider = container.querySelector('.slider-to');
    const fill = container.querySelector('.slider-fill');

    if (!fromSlider || !toSlider || !fill) {
        console.warn('[DateRangeSlider] Could not find slider elements for clamping');
        window.__dateRangeSliderReady = true;
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

    window.__dateRangeSliderReady = true;
}
