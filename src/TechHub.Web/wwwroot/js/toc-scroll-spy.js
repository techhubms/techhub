/**
 * Table of Contents Scroll Spy
 * 
 * Automatically highlights the TOC link for the currently visible heading.
 * Uses scrollend event for clean, reliable detection when scrolling stops.
 * 
 * CRITICAL: Uses history.replaceState() instead of pushState() to update URL hash.
 * This prevents polluting browser history with scroll positions - only actual
 * TOC link clicks create history entries, enabling clean back button navigation.
 * 
 * @see {@link /workspaces/techhub/src/TechHub.Web/AGENTS.md} for integration patterns
 */

// Hardcoded constants - must match CSS scroll-margin-top value
const STICKY_HEADER_OFFSET = 106; // px - height of sticky header
const DETECTION_LINE_PERCENT = 0.30; // 30% from top of content area

export class TocScrollSpy {
    constructor(tocElement, contentElement) {
        this.tocElement = tocElement;
        this.contentElement = contentElement;
        this.headings = [];
        this.tocLinks = new Map(); // headingId -> tocLink element
        this.headingElements = new Map(); // headingId -> heading element (cached)
        this.h2Items = null; // Cached TOC h2 items (.toc-depth-0)
        this.currentActiveId = null;
        this.currentActiveH2Id = null; // Track active h2 for collapse/expand
        this.boundHandleScroll = this.handleScroll.bind(this);
        this.boundHandleResize = this.handleResize.bind(this);
        this.boundHandleTocFocus = this.handleTocFocus.bind(this);
        this.ticking = false; // RAF throttle flag
        this.debugOverlay = null; // Visual debug line
        this.debugEnabled = false;
        this.cachedDetectionLine = 0; // Cache detection line position
        this.initialScrollEndHandler = null; // One-time scrollend handler
        this.initialScrollTimeout = null; // Fallback timeout for browsers without scrollend
        this.initialized = false; // Track initialization state to prevent duplicate listeners

        // Expose toggle function globally for console access
        window.toggleTocDebug = this.toggleDebug.bind(this);
    }

    /**
     * Toggle debug visualization (call from console: toggleTocDebug())
     */
    toggleDebug() {
        this.debugEnabled = !this.debugEnabled;

        if (this.debugEnabled) {
            if (!this.debugOverlay) {
                this.debugOverlay = document.createElement('div');
                // Use maximum 32-bit signed integer for z-index to ensure overlay is always on top
                this.debugOverlay.style.cssText = `
                    position: fixed;
                    left: 0;
                    right: 0;
                    height: 3px;
                    background: rgba(255, 0, 0, 0.8);
                    z-index: 2147483647;
                    pointer-events: none;
                    box-shadow: 0 0 10px rgba(255, 0, 0, 0.5);
                `;
                document.body.appendChild(this.debugOverlay);
            }
            this.debugOverlay.style.display = 'block';
            console.log('TOC Debug Mode: ENABLED - Red line shows detection position (matches CSS --scroll-margin-top)');
            console.log('Call toggleTocDebug() again to disable');
            // Update position immediately
            this.updateActiveHeading();
        } else {
            if (this.debugOverlay) {
                this.debugOverlay.style.display = 'none';
            }
            console.log('TOC Debug Mode: DISABLED');
        }
    }

    /**
     * Initialize the scroll spy
     */
    init() {
        // Guard against duplicate initialization
        if (this.initialized) {
            console.warn('TocScrollSpy already initialized. Call destroy() first if re-initialization is needed.');
            return;
        }

        // Find all H2 and H3 headings in content with IDs
        // H2 and H3 are always in the TOC, H4+ are not tracked
        this.headings = Array.from(
            this.contentElement.querySelectorAll('h2[id], h3[id]')
        );

        if (this.headings.length === 0) {
            return;
        }

        // Build map of heading IDs to TOC links and cache heading elements
        // Support both hash-only links (#id) and full URLs (http://...#id)
        this.headings.forEach(heading => {
            const id = heading.getAttribute('id');
            const tocLink = this.tocElement.querySelector(`a[href="#${id}"], a[href$="#${id}"]`);
            if (tocLink) {
                this.tocLinks.set(id, tocLink);
                this.headingElements.set(id, heading); // Cache heading element
            }
        });

        // Cache h2 TOC items to avoid repeated querySelectorAll
        this.h2Items = Array.from(this.tocElement.querySelectorAll('.toc-depth-0'));

        // Calculate and cache detection line position
        this.updateDetectionLine();

        // Detect if browser is ACTUALLY scrolling (not just if there's a hash)
        const initialScrollY = window.scrollY;

        this.updateActiveHeading();
        this.attachEventListeners();

        // Handle case where browser is already scrolling when we initialize
        // Set up one-time handler that auto-cancels when regular scroll/resize handlers trigger
        if ('onscrollend' in window) {
            this.initialScrollEndHandler = () => {
                this.updateActiveHeading();
                this.cleanupInitialScrollHandlers();
            };
            window.addEventListener('scrollend', this.initialScrollEndHandler, { once: true });
        } else {
            // Fallback for browsers without scrollend support
            this.initialScrollTimeout = setTimeout(() => {
                this.updateActiveHeading();
                this.cleanupInitialScrollHandlers();
            }, 200);
        }

        this.initialized = true;
    }

    /**
     * Attach event listeners for scroll tracking
     * Called AFTER initial heading update to avoid race conditions
     */
    attachEventListeners() {
        // Listen to scroll event with RAF throttling for real-time updates
        window.addEventListener('scroll', this.boundHandleScroll, { passive: true });

        // Recalculate detection line on window resize
        window.addEventListener('resize', this.boundHandleResize, { passive: true });

        // Listen for focus on TOC links to expand sections during keyboard navigation
        this.tocElement.addEventListener('focusin', this.boundHandleTocFocus);
    }

    /**
     * Handle scroll events with requestAnimationFrame throttling
     * This ensures updates happen at most once per frame (~60fps)
     */
    handleScroll() {
        this.cleanupInitialScrollHandlers(); // Cancel initial scroll handlers
        if (!this.ticking) {
            window.requestAnimationFrame(() => {
                this.updateActiveHeading();
                this.ticking = false;
            });
            this.ticking = true;
        }
    }

    /**
     * Handle window resize - recalculate detection line position
     */
    handleResize() {
        this.cleanupInitialScrollHandlers(); // Cancel initial scroll handlers
        this.updateDetectionLine();
        if (this.debugOverlay) {
            this.debugOverlay.style.top = `${this.cachedDetectionLine}px`;
        }
    }

    /**
     * Handle focus events on TOC links
     * Expands the appropriate H2 section when a TOC link receives focus (keyboard navigation)
     */
    handleTocFocus(event) {
        const link = event.target.closest('.toc-link');
        if (!link) return;

        // Get the heading ID from the link's href
        const href = link.getAttribute('href');
        if (!href) return;

        const headingId = href.split('#').pop();
        if (!headingId) return;

        // Check if this heading is in our tracked headings
        if (!this.tocLinks.has(headingId)) return;

        // Update collapse state to expand the section containing this link
        this.updateCollapseState(headingId);
    }

    /**
     * Calculate and cache detection line position
     * Uses hardcoded constants that MUST match CSS scroll-margin-top value
     */
    updateDetectionLine() {
        const contentViewportHeight = window.innerHeight - STICKY_HEADER_OFFSET;
        this.cachedDetectionLine = STICKY_HEADER_OFFSET + (contentViewportHeight * DETECTION_LINE_PERCENT);
    }

    /**
     * Update which heading should be active based on scroll position
     */
    updateActiveHeading() {
        // Use cached detection line position
        const detectionLineFromTop = this.cachedDetectionLine;
        const tolerance = 5; // Allow 5px tolerance

        // Find the heading CLOSEST to our detection line (30% from top of content area)
        // Pick the heading that's above the line and closest to it
        let targetId = null;
        let closestDistance = Infinity;

        // CRITICAL: Only check headings that are actually in the TOC (have a link in tocLinks)
        // This prevents H5/H6 or other unlisted headings from triggering highlights
        for (const [headingId, heading] of this.headingElements) {
            const rect = heading.getBoundingClientRect();
            const headingTopFromViewport = rect.top;

            // Only consider headings that are at or above the detection line (with tolerance)
            if (headingTopFromViewport <= detectionLineFromTop + tolerance) {
                const distance = Math.abs(detectionLineFromTop - headingTopFromViewport);

                // Pick the heading closest to the detection line (smallest distance)
                if (distance < closestDistance) {
                    closestDistance = distance;
                    targetId = headingId;
                }
            }
        }

        // Fallback: If no heading is above the detection line (e.g., page just loaded),
        // activate the first heading in the TOC to provide visual feedback
        if (!targetId && this.tocLinks.size > 0) {
            targetId = this.tocLinks.keys().next().value;
        }

        if (targetId) {
            this.setActive(targetId);
        }
    }

    /**
     * Set a TOC link as active
     */
    setActive(headingId, skipCollapseUpdate = false) {
        // Don't update if already active
        if (this.currentActiveId === headingId) {
            return;
        }

        // Use cached heading element
        const newHeading = this.headingElements.get(headingId);
        if (!newHeading) {
            return;
        }

        // Remove active from current TOC link and heading
        if (this.currentActiveId !== null) {
            const currentLink = this.tocLinks.get(this.currentActiveId);
            if (currentLink) {
                currentLink.classList.remove('active');
            }

            // Remove active class from previous heading (use cached element)
            const currentHeading = this.headingElements.get(this.currentActiveId);
            if (currentHeading) {
                currentHeading.classList.remove('toc-active-heading');
            }
        }

        // Add active to new TOC link and heading
        const newLink = this.tocLinks.get(headingId);
        if (newLink) {
            newLink.classList.add('active');
            this.currentActiveId = headingId;

            // Add active class to current heading
            newHeading.classList.add('toc-active-heading');

            // Update collapse/expand state based on which h2 section we're in (unless skipped)
            if (!skipCollapseUpdate) {
                this.updateCollapseState(headingId);
            }
        }
    }

    /**
     * Update which h2 section is expanded based on active heading
     */
    updateCollapseState(activeHeadingId) {
        // Use cached heading element
        const activeHeading = this.headingElements.get(activeHeadingId);
        if (!activeHeading) return;

        // Get the heading level
        const headingLevel = parseInt(activeHeading.tagName.substring(1)); // h2 -> 2, h3 -> 3

        let targetH2Id = activeHeadingId;

        // If this is an h3, find the parent h2
        if (headingLevel === 3) {
            // Walk backwards through headings to find the parent h2
            const activeIndex = this.headings.indexOf(activeHeading);
            if (activeIndex === -1) return; // Heading not in our list, abort

            for (let i = activeIndex - 1; i >= 0; i--) {
                const heading = this.headings[i];
                const level = parseInt(heading.tagName.substring(1));
                if (level === 2) {
                    targetH2Id = heading.getAttribute('id');
                    break;
                }
            }
        }

        // Verify the target H2 has a TOC link before proceeding
        // If not, we can't expand any section, so just return early
        const activeTocLink = this.tocLinks.get(targetH2Id);
        if (!activeTocLink) return;

        // Only update if we've changed h2 sections
        if (this.currentActiveH2Id === targetH2Id) {
            return;
        }
        this.currentActiveH2Id = targetH2Id;

        // Use cached h2Items instead of querySelectorAll
        this.h2Items.forEach(item => {
            item.classList.remove('expanded');
        });

        // Expand the active h2 section
        const activeItem = activeTocLink.closest('.toc-depth-0');
        if (activeItem) {
            activeItem.classList.add('expanded');
        }
    }

    /**
     * Clean up initial scroll handlers
     * Called automatically when regular scroll/resize handlers trigger
     */
    cleanupInitialScrollHandlers() {
        if (this.initialScrollEndHandler) {
            window.removeEventListener('scrollend', this.initialScrollEndHandler);
            this.initialScrollEndHandler = null;
        }
        if (this.initialScrollTimeout) {
            clearTimeout(this.initialScrollTimeout);
            this.initialScrollTimeout = null;
        }
    }

    /**
     * Clean up event listeners
     */
    destroy() {
        this.cleanupInitialScrollHandlers();
        window.removeEventListener('scroll', this.boundHandleScroll);
        window.removeEventListener('resize', this.boundHandleResize);
        this.tocElement.removeEventListener('focusin', this.boundHandleTocFocus);

        this.initialized = false;

        // Clean up debug overlay
        if (this.debugOverlay && this.debugOverlay.parentNode) {
            this.debugOverlay.parentNode.removeChild(this.debugOverlay);
        }

        // Clean up global function
        if (window.toggleTocDebug === this.toggleDebug) {
            delete window.toggleTocDebug;
        }
    }
}

/**
 * Initialize all TOC scroll spies on the page
 */
export function initTocScrollSpy() {
    const tocElements = document.querySelectorAll('[data-toc-scroll-spy]');

    tocElements.forEach(tocElement => {
        const contentSelector = tocElement.getAttribute('data-content-selector');
        if (!contentSelector) {
            console.warn('TOC element missing data-content-selector attribute');
            return;
        }

        const contentElement = document.querySelector(contentSelector);
        if (!contentElement) {
            console.warn(`Content element not found: ${contentSelector}`);
            return;
        }

        // Clean up existing instance if it exists
        if (tocElement._tocScrollSpy) {
            tocElement._tocScrollSpy.destroy();
            tocElement._tocScrollSpy = null;
        }

        // Create and initialize the scroll spy
        const scrollSpy = new TocScrollSpy(tocElement, contentElement);
        scrollSpy.init();

        // Store instance for cleanup if needed
        tocElement._tocScrollSpy = scrollSpy;
    });
}

/**
 * Clean up all TOC scroll spy instances
 */
export function cleanupAllTocScrollSpies() {
    const tocElements = document.querySelectorAll('[data-toc-scroll-spy]');

    tocElements.forEach(tocElement => {
        if (tocElement._tocScrollSpy) {
            tocElement._tocScrollSpy.destroy();
            tocElement._tocScrollSpy = null;
        }
    });
}
