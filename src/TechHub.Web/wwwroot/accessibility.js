// Accessibility enhancements for skip links and focus management

/**
 * Handle skip link clicks to move focus to the target element
 * This is required for proper keyboard navigation accessibility
 */
function initializeSkipLinks() {
    const skipLinks = document.querySelectorAll('a.skip-link[href^="#"]');
    
    skipLinks.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            
            // Get the target element from the href
            const targetId = link.getAttribute('href').substring(1);
            const targetElement = document.getElementById(targetId);
            
            if (targetElement) {
                // Move focus to the target element immediately
                targetElement.focus();
                
                // Scroll to the element (in case it's not visible)
                targetElement.scrollIntoView({ behavior: 'auto', block: 'start' });
            }
        });
    });
}

/**
 * Restore skip link focus after Blazor enhanced navigation
 * Enhanced navigation replaces content but doesn't restore focus properly
 */
function setupEnhancedNavigationFocusHandler() {
    // Listen for Blazor's enhanced navigation completion
    Blazor.addEventListener('enhancednavigationcomplete', () => {
        // Small delay to ensure DOM is updated
        setTimeout(() => {
            // Re-initialize skip links for new content
            initializeSkipLinks();
        }, 50);
    });
}

// Initialize on DOMContentLoaded
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        initializeSkipLinks();
        
        // Set up enhanced navigation handler when Blazor is ready
        if (window.Blazor) {
            setupEnhancedNavigationFocusHandler();
        } else {
            // Wait for Blazor to load
            const checkBlazor = setInterval(() => {
                if (window.Blazor) {
                    clearInterval(checkBlazor);
                    setupEnhancedNavigationFocusHandler();
                }
            }, 50);
        }
    });
} else {
    // DOM already loaded
    initializeSkipLinks();
    
    if (window.Blazor) {
        setupEnhancedNavigationFocusHandler();
    }
}
