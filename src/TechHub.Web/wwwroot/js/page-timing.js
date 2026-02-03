// Page timing using Performance API
// Logs accurate client-side page load metrics to server
// Supports both initial page load and Blazor enhanced navigation

(function() {
    let navigationStartTime = null;
    
    // Measure and send timing for initial page load
    const measureInitialLoad = () => {
        const perfData = performance.getEntriesByType('navigation')[0];
        
        if (perfData) {
            const metrics = {
                page: window.location.pathname,
                dns: Math.round(perfData.domainLookupEnd - perfData.domainLookupStart),
                tcp: Math.round(perfData.connectEnd - perfData.connectStart),
                request: Math.round(perfData.responseStart - perfData.requestStart),
                response: Math.round(perfData.responseEnd - perfData.responseStart),
                domParse: Math.round(perfData.domInteractive - perfData.responseEnd),
                domReady: Math.round(perfData.domContentLoadedEventEnd - perfData.fetchStart),
                totalLoad: Math.round(perfData.loadEventEnd - perfData.fetchStart),
                timeToInteractive: Math.round(perfData.domInteractive - perfData.fetchStart)
            };
            
            sendMetrics(metrics);
        }
    };
    
    // Measure and send timing for Blazor navigation
    const measureNavigation = (url) => {
        if (navigationStartTime) {
            const elapsed = Math.round(performance.now() - navigationStartTime);
            const metrics = {
                page: new URL(url).pathname,
                dns: 0,
                tcp: 0,
                request: 0,
                response: 0,
                domParse: 0,
                domReady: elapsed,
                totalLoad: elapsed,
                timeToInteractive: elapsed
            };
            
            sendMetrics(metrics);
        }
    };
    
    // Send metrics to server
    const sendMetrics = (metrics) => {
        fetch('/api/page-timing', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(metrics)
        }).catch(() => {}); // Silently fail if server unavailable
    };
    
    // Initial page load timing
    if (document.readyState === 'complete') {
        setTimeout(measureInitialLoad, 0);
    } else {
        window.addEventListener('load', () => setTimeout(measureInitialLoad, 0));
    }
    
    // Blazor enhanced navigation timing
    if (window.Blazor) {
        Blazor.addEventListener('enhancednavigationstart', (event) => {
            navigationStartTime = performance.now();
        });
        
        Blazor.addEventListener('enhancedload', (event) => {
            measureNavigation(window.location.href);
        });
    } else {
        // Wait for Blazor to initialize
        const checkBlazor = setInterval(() => {
            if (window.Blazor) {
                clearInterval(checkBlazor);
                
                Blazor.addEventListener('enhancednavigationstart', (event) => {
                    navigationStartTime = performance.now();
                });
                
                Blazor.addEventListener('enhancedload', (event) => {
                    measureNavigation(window.location.href);
                });
            }
        }, 100);
        
        // Stop checking after 5 seconds
        setTimeout(() => clearInterval(checkBlazor), 5000);
    }
})();



// Also log on enhanced navigation (Blazor page changes)
if (window.Blazor) {
    window.Blazor.addEventListener('enhancednavigationcompleted', () => {
        console.info(`📊 Page ${window.location.pathname} navigated (enhanced navigation)`);
    });
}
