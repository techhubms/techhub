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
        this.boundHandleScroll = this.handleScroll.bind(this);
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

        // Override anchor link scroll behavior to match detection line
        this.tocLinks.forEach((link, headingId) => {
            link.addEventListener('click', (e) => {
                e.preventDefault();
                const heading = document.getElementById(headingId);
                if (heading) {
                    // Calculate detection line position
                    const contentViewportHeight = window.innerHeight - STICKY_HEADER_OFFSET;
                    const detectionLineFromTop = STICKY_HEADER_OFFSET + (contentViewportHeight * DETECTION_LINE_PERCENT);
                    const headingTop = heading.getBoundingClientRect().top + window.scrollY;
                    const targetScroll = headingTop - detectionLineFromTop;

                    window.scrollTo({
                        top: targetScroll,
                        behavior: 'smooth'
                    });

                    // Update URL hash without triggering scroll, preserving current path
                    const newUrl = window.location.pathname + window.location.search + `#${headingId}`;
                    history.pushState(null, '', newUrl);
                }
            });
        });

        // Handle initial page load with hash
        const initialHash = window.location.hash.slice(1); // Remove # from hash
        if (initialHash && this.tocLinks.has(initialHash)) {
            const heading = document.getElementById(initialHash);
            if (heading) {
                // Immediately activate the linked heading
                this.setActive(initialHash);

                // Wait for page to fully render, then scroll
                setTimeout(() => {
                    // Calculate detection line position
                    const contentViewportHeight = window.innerHeight - STICKY_HEADER_OFFSET;
                    const detectionLineFromTop = STICKY_HEADER_OFFSET + (contentViewportHeight * DETECTION_LINE_PERCENT);
                    const headingTop = heading.getBoundingClientRect().top + window.scrollY;
                    const targetScroll = headingTop - detectionLineFromTop;

                    window.scrollTo({
                        top: targetScroll,
                        behavior: 'smooth'
                    });
                }, 100);
            }
        } else {
            // No hash, detect current position immediately on page load
            this.updateActiveHeading();
        }
    }

    /**
     * Handle scroll events with requestAnimationFrame throttling
     * This ensures updates happen at most once per frame (~60fps)
     */
    handleScroll() {
        if (!this.ticking) {
            window.requestAnimationFrame(() => {
                this.updateActiveHeading();
                this.ticking = false;
            });
            this.ticking = true;
        }
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
    setActive(headingId) {
        // Don't update if already active
        if (this.currentActiveId === headingId) {
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
            const newHeading = document.getElementById(headingId);
            if (newHeading) {
                newHeading.classList.add('toc-active-heading');
            }
        }
    }

    /**
     * Clean up event listeners
     */
    destroy() {
        window.removeEventListener('scroll', this.boundHandleScroll);
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
