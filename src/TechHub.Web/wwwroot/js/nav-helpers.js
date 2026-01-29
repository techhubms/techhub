/**
 * Navigation Helpers - Back to Top and Back to Previous Page
 * 
 * Provides sticky bottom buttons for:
 * - Back to top: Smooth scroll to top of page
 * - Back to previous page: Navigate to previous page in history
 * 
 * Buttons appear when user scrolls down (300px threshold) and are hidden at top.
 * 
 * Blazor Integration:
 * - Handles Blazor enhanced navigation via pageshow event
 * - Uses MutationObserver to detect when DOM changes remove the container
 * - Automatically recreates buttons after navigation or DOM updates
 * 
 * Browser Compatibility:
 * - Uses modern APIs (requestAnimationFrame, MutationObserver)
 * - Requires ES6+ support
 * 
 * @see {@link /workspaces/techhub/src/TechHub.Web/AGENTS.md} for architecture details
 */

(function () {
    'use strict';

    // Configuration
    const SCROLL_THRESHOLD = 300; // Show buttons after scrolling 300px
    const BUTTON_CONTAINER_ID = 'nav-helper-buttons';

    // Create and inject the button container
    function createButtonContainer() {
        const container = document.createElement('div');
        container.id = BUTTON_CONTAINER_ID;
        container.className = 'nav-helper-buttons';
        container.setAttribute('aria-label', 'Navigation helpers');

        // Back to previous page button (left arrow)
        const backToPrevBtn = document.createElement('button');
        backToPrevBtn.className = 'nav-helper-btn nav-helper-btn-prev';
        backToPrevBtn.setAttribute('aria-label', 'Back to previous page');
        backToPrevBtn.setAttribute('title', 'Back to previous page');
        backToPrevBtn.innerHTML = `
            <svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
                <path d="M12 4L6 10L12 16" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
        `;
        backToPrevBtn.addEventListener('click', goToPreviousPage);

        // Back to top button (up arrow)
        const backToTopBtn = document.createElement('button');
        backToTopBtn.className = 'nav-helper-btn nav-helper-btn-top';
        backToTopBtn.setAttribute('aria-label', 'Back to top');
        backToTopBtn.setAttribute('title', 'Back to top');
        backToTopBtn.innerHTML = `
            <svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
                <path d="M10 16L10 4M10 4L4 10M10 4L16 10" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
        `;
        backToTopBtn.addEventListener('click', scrollToTop);

        container.appendChild(backToPrevBtn);
        container.appendChild(backToTopBtn);
        document.body.appendChild(container);

        return container;
    }

    // Scroll to top smoothly and reset focus
    function scrollToTop() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });

        // Reset focus to body after scroll completes
        // This ensures the next Tab press will naturally focus the skip-link
        // with proper :focus-visible styling (programmatic focus doesn't trigger focus-visible)
        setTimeout(() => {
            document.body.focus();
            // If body isn't focusable, make it temporarily focusable
            if (document.activeElement !== document.body) {
                document.body.setAttribute('tabindex', '-1');
                document.body.focus();
                document.body.removeAttribute('tabindex');
            }
        }, 500); // Wait for smooth scroll to complete
    }

    /**
     * Navigate to previous page in browser history
     * 
     * Simple history.back() navigation since TOC scroll-spy uses replaceState
     * instead of pushState, preventing history pollution from scroll positions.
     */
    function goToPreviousPage() {
        // Check if there's history to go back to
        if (window.history.length > 1) {
            window.history.back();
        } else {
            // If no history, go to homepage
            window.location.href = '/';
        }
    }

    /**
     * Handle clicks on hash-only links (e.g., href="#section-heading")
     * 
     * Hash-only links don't work correctly with Blazor enhanced navigation because
     * the browser doesn't know the full page URL context. This handler intercepts
     * clicks on hash-only links and converts them to full URL navigation.
     * 
     * @param {Event} event - The click event
     */
    function handleHashLinkClick(event) {
        const link = event.target.closest('a[href^="#"]');
        if (!link) return;

        const href = link.getAttribute('href');
        
        // Only handle hash-only links (not full URLs with hashes like /page#section)
        if (!href || !href.startsWith('#') || href === '#') return;

        // Prevent default behavior
        event.preventDefault();

        // Navigate to full URL with hash
        // This triggers proper browser navigation instead of just updating the hash
        const fullUrl = window.location.pathname + window.location.search + href;
        window.location.href = fullUrl;
    }

    // Set up hash link click handler on article content
    function setupHashLinkHandler() {
        // Target article-body where rendered markdown content lives
        const articleBody = document.querySelector('.article-body');
        if (articleBody && !articleBody.dataset.hashLinksHandled) {
            articleBody.addEventListener('click', handleHashLinkClick);
            articleBody.dataset.hashLinksHandled = 'true';
        }
    }

    // Show/hide buttons based on scroll position
    function handleScroll() {
        const container = document.getElementById(BUTTON_CONTAINER_ID);
        if (!container) return;

        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        if (scrollTop > SCROLL_THRESHOLD) {
            container.classList.add('visible');
        } else {
            container.classList.remove('visible');
        }
    }

    // Scroll handler with debouncing
    let scrollTimeout;
    function onScroll() {
        if (scrollTimeout) {
            window.cancelAnimationFrame(scrollTimeout);
        }
        scrollTimeout = window.requestAnimationFrame(handleScroll);
    }

    // Initialize
    function init() {
        // Only create container if it doesn't exist (avoid duplicates)
        if (!document.getElementById(BUTTON_CONTAINER_ID)) {
            createButtonContainer();
        }

        // Set up hash link handler for article content
        setupHashLinkHandler();

        // Always check scroll position on init
        handleScroll();
    }

    // Listen for scroll events (set up once globally)
    window.addEventListener('scroll', onScroll, { passive: true });

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // Re-initialize after page shows (handles back/forward navigation)
    window.addEventListener('pageshow', init);

    // Re-initialize after Blazor enhanced navigation
    if (window.Blazor) {
        Blazor.addEventListener('enhancedload', init);
    }
})();
