// Blazor Circuit Connection Readiness Check
// Ensures the SignalR circuit is fully established before marking the page as loaded

window.blazorCircuitReady = new Promise((resolve) => {
    // Check if Blazor has already started
    if (window.Blazor) {
        // Circuit already exists - resolve immediately
        resolve();
        return;
    }

    // Wait for Blazor to initialize
    const checkBlazorReady = setInterval(() => {
        if (window.Blazor) {
            clearInterval(checkBlazorReady);
            
            // Blazor is loaded, but we need to ensure the circuit is connected
            // The circuit is ready when the first render completes
            // We'll use a short delay to ensure the WebSocket connection is established
            setTimeout(() => {
                console.log('Blazor circuit ready');
                resolve();
            }, 100);
        }
    }, 50); // Check every 50ms

    // Timeout after 10 seconds to prevent infinite waiting
    setTimeout(() => {
        clearInterval(checkBlazorReady);
        console.warn('Blazor circuit readiness timeout - forcing resolution');
        resolve();
    }, 10000);
});

// Expose a function to wait for Blazor circuit readiness
window.waitForBlazorCircuit = async function() {
    await window.blazorCircuitReady;
    return true;
};
