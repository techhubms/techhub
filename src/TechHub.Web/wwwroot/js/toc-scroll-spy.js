/**
 * Table of Contents Scroll Spy
 * 
 * Automatically highlights the TOC link for the currently visible heading.
 * Uses scrollend event for clean, reliable detection when scrolling stops.
 */

// Configuration: Detection line position as percentage of content viewport (0.0 to 1.0)
// Lower values = headings activate higher on screen (e.g., 0.30 = 30% from top of content area)
const DETECTION_LINE_PERCENT = 0.30;

// Sticky header offset in pixels (main nav + subnav)
const STICKY_HEADER_OFFSET = 106;

export class TocScrollSpy {
    constructor(tocElement, contentElement) {
        this.tocElement = tocElement;
        this.contentElement = contentElement;
        this.headings = [];
        this.tocLinks = new Map(); // headingId -> tocLink element
        this.currentActiveId = null;
        this.currentActiveH2Id = null; // Track active h2 for collapse/expand
        this.isProgrammaticScroll = false; // Flag to prevent scroll spy during navigation
        this.lastUserScrollTime = 0; // Track when user last scrolled manually
        this.currentScrollEndHandler = null; // Track current scrollend handler to cancel it
        this.currentScrollTimeout = null; // Track current timeout to cancel it
        this.boundHandleScroll = this.handleScroll.bind(this);
        this.boundHandleWheel = this.handleWheel.bind(this);
        this.boundHandleTouch = this.handleTouch.bind(this);
        this.ticking = false; // RAF throttle flag
    }

    /**
     * Initialize the scroll spy
     */
    init() {
        // Find all headings in content with IDs
        this.headings = Array.from(
            this.contentElement.querySelectorAll('h2[id], h3[id], h4[id], h5[id], h6[id]')
        );

        if (this.headings.length === 0) {
            return;
        }

        // Build map of heading IDs to TOC links
        // Support both hash-only links (#id) and full URLs (http://...#id)
        this.headings.forEach(heading => {
            const id = heading.getAttribute('id');
            const tocLink = this.tocElement.querySelector(`a[href="#${id}"], a[href$="#${id}"]`);
            if (tocLink) {
                this.tocLinks.set(id, tocLink);
            }
        });

        // Listen to scroll event with RAF throttling for real-time updates
        window.addEventListener('scroll', this.boundHandleScroll, { passive: true });

        // Listen for user-initiated scroll events (wheel, touch, keyboard)
        window.addEventListener('wheel', this.boundHandleWheel, { passive: true });
        window.addEventListener('touchstart', this.boundHandleTouch, { passive: true });
        window.addEventListener('keydown', this.boundHandleWheel, { passive: true }); // Arrow keys, Page Up/Down, etc.

        // Override ALL anchor links (in sidebar AND in main content) to use custom scroll behavior
        // This ensures clicking links in the main content TOC also scrolls to the detection line
        document.addEventListener('click', (e) => {
            const link = e.target.closest('a[href*="#"]');
            if (!link) return;

            // Extract the hash from the link
            const href = link.getAttribute('href');
            const hashMatch = href.match(/#(.+)$/);
            if (!hashMatch) return;

            const headingId = hashMatch[1];
            const heading = document.getElementById(headingId);

            // Only handle if this is one of our tracked headings
            if (!heading || !this.headings.includes(heading)) return;

            e.preventDefault();
            this.scrollToHeading(headingId);
        });

        // Handle initial page load with hash
        const initialHash = window.location.hash.slice(1); // Remove # from hash
        if (initialHash && this.tocLinks.has(initialHash)) {
            const heading = document.getElementById(initialHash);
            if (heading) {
                // Immediately activate the linked heading and expand its section
                this.setActive(initialHash);

                // Wait for page to fully render, then scroll
                setTimeout(() => {
                    this.scrollToHeading(initialHash, false); // Don't use smooth scroll on initial load
                }, 100);
            }
        } else {
            // No hash, detect current position immediately on page load and expand
            this.updateActiveHeading();
        }
    }

    /**
     * Scroll to a heading using the detection line position
     * @param {string} headingId - The ID of the heading to scroll to
     * @param {boolean} smooth - Whether to use smooth scrolling (default: true)
     */
    scrollToHeading(headingId, smooth = true) {
        const heading = document.getElementById(headingId);
        if (!heading) return;

        // Expand the target section first if it's in a different h2
        const headingLevel = parseInt(heading.tagName.substring(1));
        let targetH2Id = headingId;

        if (headingLevel > 2) {
            // Find parent h2 for this heading
            const headingIndex = this.headings.indexOf(heading);
            for (let i = headingIndex - 1; i >= 0; i--) {
                const h = this.headings[i];
                const level = parseInt(h.tagName.substring(1));
                if (level === 2) {
                    targetH2Id = h.getAttribute('id');
                    break;
                }
            }
        }

        // Cancel any previous scroll handlers (in case of rapid clicking)
        if (this.currentScrollEndHandler) {
            window.removeEventListener('scrollend', this.currentScrollEndHandler);
            this.currentScrollEndHandler = null;
        }
        if (this.currentScrollTimeout) {
            clearTimeout(this.currentScrollTimeout);
            this.currentScrollTimeout = null;
        }

        // Disable scroll spy during navigation
        this.isProgrammaticScroll = true;
        const scrollStartTime = Date.now();

        // Collapse/expand sections immediately before scrolling
        if (targetH2Id !== this.currentActiveH2Id) {
            this.currentActiveH2Id = targetH2Id;
            const h2Items = this.tocElement.querySelectorAll('.toc-depth-0');
            h2Items.forEach(item => {
                item.classList.remove('expanded');
            });
            const targetTocLink = this.tocLinks.get(targetH2Id);
            if (targetTocLink) {
                const targetItem = targetTocLink.closest('.toc-depth-0');
                if (targetItem) {
                    targetItem.classList.add('expanded');
                }
            }
        }

        // Set the target as active immediately
        this.setActive(headingId, true);

        // Calculate detection line position
        const contentViewportHeight = window.innerHeight - STICKY_HEADER_OFFSET;
        const detectionLineFromTop = STICKY_HEADER_OFFSET + (contentViewportHeight * DETECTION_LINE_PERCENT);
        const headingTop = heading.getBoundingClientRect().top + window.scrollY;
        const targetScroll = headingTop - detectionLineFromTop;

        window.scrollTo({
            top: targetScroll,
            behavior: smooth ? 'smooth' : 'auto'
        });

        // Re-enable scroll spy when scrolling actually stops
        const onScrollEnd = () => {
            // Only re-enable if enough time has passed (minimum 300ms)
            const elapsed = Date.now() - scrollStartTime;
            if (elapsed >= 300) {
                this.isProgrammaticScroll = false;
                this.currentScrollEndHandler = null;
            } else {
                // Not enough time - wait for next scrollend or timeout
                this.currentScrollTimeout = setTimeout(() => {
                    this.isProgrammaticScroll = false;
                    this.currentScrollEndHandler = null;
                    this.currentScrollTimeout = null;
                }, 300 - elapsed);
            }
        };

        // Store reference to handler so we can cancel it if user clicks again quickly
        this.currentScrollEndHandler = onScrollEnd;

        // Use scrollend event (modern browsers)
        window.addEventListener('scrollend', onScrollEnd, { once: true });

        // Fallback: also set a maximum timeout in case scrollend doesn't fire
        this.currentScrollTimeout = setTimeout(() => {
            if (this.isProgrammaticScroll) {
                this.isProgrammaticScroll = false;
                if (this.currentScrollEndHandler) {
                    window.removeEventListener('scrollend', this.currentScrollEndHandler);
                    this.currentScrollEndHandler = null;
                }
                this.currentScrollTimeout = null;
            }
        }, 2000);

        // Update URL hash without triggering scroll, preserving current path
        const newUrl = window.location.pathname + window.location.search + `#${headingId}`;
        history.pushState(null, '', newUrl);
    }

    /**
     * Handle scroll events with requestAnimationFrame throttling
     * This ensures updates happen at most once per frame (~60fps)
     */
    handleScroll() {
        // If user scrolled manually recently (within 100ms), cancel programmatic scroll flag
        const timeSinceUserScroll = Date.now() - this.lastUserScrollTime;
        if (timeSinceUserScroll < 100 && this.isProgrammaticScroll) {
            this.isProgrammaticScroll = false;
        }

        // Skip updates during programmatic scrolling (clicking links)
        if (this.isProgrammaticScroll) {
            return;
        }

        if (!this.ticking) {
            window.requestAnimationFrame(() => {
                this.updateActiveHeading();
                this.ticking = false;
            });
            this.ticking = true;
        }
    }

    /**
     * Handle user-initiated scroll events (wheel, touch, keyboard)
     */
    handleWheel() {
        this.lastUserScrollTime = Date.now();
    }

    /**
     * Handle touch events (mobile scrolling)
     */
    handleTouch() {
        this.lastUserScrollTime = Date.now();
    }

    /**
     * Update which heading should be active based on scroll position
     */
    updateActiveHeading() {
        // Calculate detection line position using constants
        const contentViewportHeight = window.innerHeight - STICKY_HEADER_OFFSET;
        const detectionLineFromTop = STICKY_HEADER_OFFSET + (contentViewportHeight * DETECTION_LINE_PERCENT);

        const scrollTop = window.scrollY || window.pageYOffset;
        const viewportBottom = scrollTop + window.innerHeight;
        const documentHeight = document.documentElement.scrollHeight;

        // Special case: If we're near bottom of page (within 50px), activate the last heading
        if (viewportBottom >= documentHeight - 50) {
            const lastHeading = this.headings[this.headings.length - 1];
            if (lastHeading) {
                const lastId = lastHeading.getAttribute('id');
                this.setActive(lastId);
                return;
            }
        }

        // Special case: Very top of page (scrollTop < 300px), activate first heading
        if (scrollTop < 300) {
            if (this.headings.length > 0) {
                const firstId = this.headings[0].getAttribute('id');
                this.setActive(firstId);
                return;
            }
        }

        // Find the heading CLOSEST to our detection line (40% from top of content area)
        // Pick the heading that's above the line and closest to it
        let targetId = null;
        let closestDistance = Infinity;
        const tolerance = 5; // Allow 5px tolerance for r

        for (let i = 0; i < this.headings.length; i++) {
            const heading = this.headings[i];
            const rect = heading.getBoundingClientRect();
            const headingTopFromViewport = rect.top;

            // Only consider headings that are at or above the detection line (with tolerance)
            if (headingTopFromViewport <= detectionLineFromTop + tolerance) {
                const distance = Math.abs(detectionLineFromTop - headingTopFromViewport);

                // Pick the heading closest to the detection line (smallest distance)
                if (distance < closestDistance) {
                    closestDistance = distance;
                    targetId = heading.getAttribute('id');
                }
            } else {
                // This heading is below the line, so stop
                break;
            }
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

        // Defensive check: ensure the heading exists and is in our content
        const newHeading = document.getElementById(headingId);
        if (!newHeading || !this.contentElement.contains(newHeading)) {
            return;
        }

        // Remove active from current TOC link and heading
        if (this.currentActiveId !== null) {
            const currentLink = this.tocLinks.get(this.currentActiveId);
            if (currentLink) {
                currentLink.classList.remove('active');
            }

            // Remove active class from previous heading
            const currentHeading = document.getElementById(this.currentActiveId);
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
        // Find which h2 section this heading belongs to
        const activeHeading = document.getElementById(activeHeadingId);
        if (!activeHeading) return;

        // Defensive check: ensure this heading is actually in our content element
        if (!this.contentElement.contains(activeHeading)) return;

        // Get the heading level
        const headingLevel = parseInt(activeHeading.tagName.substring(1)); // h2 -> 2, h3 -> 3, etc.

        let targetH2Id = activeHeadingId;

        // If this is not an h2, find the parent h2
        if (headingLevel > 2) {
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

        // Only update if we've changed h2 sections
        if (this.currentActiveH2Id === targetH2Id) {
            return;
        }
        this.currentActiveH2Id = targetH2Id;

        // Verify the target TOC link exists before updating
        const activeTocLink = this.tocLinks.get(targetH2Id);
        if (!activeTocLink) return;

        // Collapse all h2 sections (just remove expanded class, CSS default is collapsed)
        const h2Items = this.tocElement.querySelectorAll('.toc-depth-0');
        h2Items.forEach(item => {
            item.classList.remove('expanded');
        });

        // Expand the active h2 section
        const activeItem = activeTocLink.closest('.toc-depth-0');
        if (activeItem) {
            activeItem.classList.add('expanded');
        }
    }

    /**
     * Clean up event listeners
     */
    destroy() {
        window.removeEventListener('scroll', this.boundHandleScroll);
        window.removeEventListener('wheel', this.boundHandleWheel);
        window.removeEventListener('touchstart', this.boundHandleTouch);
        window.removeEventListener('keydown', this.boundHandleWheel);
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

        // Create and initialize the scroll spy
        const scrollSpy = new TocScrollSpy(tocElement, contentElement);
        scrollSpy.init();

        // Store instance for cleanup if needed
        tocElement._tocScrollSpy = scrollSpy;
    });
}
