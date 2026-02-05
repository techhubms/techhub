/**
 * Custom page interactivity for SDLC, DX Space, and other custom pages
 * Handles collapsible sections, cards, and interactive elements
 * 
 * ES Module - imported dynamically when pages have [data-collapsible] elements
 * @see {@link /workspaces/techhub/src/TechHub.Web/Configuration/JsFiles.cs} for loading configuration
 */

'use strict';

/**
 * Initialize collapsible cards and interactive elements
 * Call this function after import to set up event handlers
 */
export function initCollapsibleCards() {
    // SDLC phase blocks
    const phaseHeaders = document.querySelectorAll('.sdlc-phase-header');
    phaseHeaders.forEach(header => {
        // Skip if already initialized
        if (header.dataset.initialized) return;
        header.dataset.initialized = 'true';

        header.addEventListener('click', function () {
            const content = this.nextElementSibling;
            const toggle = this.querySelector('.sdlc-phase-toggle');
            const isExpanded = this.getAttribute('aria-expanded') === 'true';

            if (content && content.classList.contains('sdlc-phase-content')) {
                content.classList.toggle('expanded');
                this.setAttribute('aria-expanded', !isExpanded);
                if (toggle) {
                    toggle.classList.toggle('expanded');
                }
            }
        });
    });

    // SDLC card headers
    const cardHeaders = document.querySelectorAll('.sdlc-card-header');
    cardHeaders.forEach(header => {
        if (header.dataset.initialized) return;
        header.dataset.initialized = 'true';

        header.addEventListener('click', function () {
            const content = this.nextElementSibling;
            const icon = this.querySelector('.sdlc-card-icon');
            const isExpanded = this.getAttribute('aria-expanded') === 'true';

            if (content && content.classList.contains('sdlc-card-content')) {
                content.classList.toggle('expanded');
                this.setAttribute('aria-expanded', !isExpanded);
                if (icon) {
                    icon.classList.toggle('expanded');
                }
            }
        });
    });

    // DX Space card headers
    const dxCardHeaders = document.querySelectorAll('.dx-card-header');
    dxCardHeaders.forEach(header => {
        if (header.dataset.initialized) return;
        header.dataset.initialized = 'true';

        header.addEventListener('click', function () {
            const content = this.nextElementSibling;
            const icon = this.querySelector('.dx-card-icon');
            const isExpanded = this.getAttribute('aria-expanded') === 'true';

            if (content && content.classList.contains('dx-card-content')) {
                content.classList.toggle('expanded');
                this.setAttribute('aria-expanded', !isExpanded);
                if (icon) {
                    icon.classList.toggle('expanded');
                }
            }
        });
    });

    // Feature filters (GitHub Copilot Features page)
    initFeatureFilters();
}

/**
 * Initialize feature card filters
 */
function initFeatureFilters() {
    const filterContainer = document.querySelector('[data-feature-filters]');
    if (!filterContainer) return;

    const ghesFilter = document.getElementById('filter-ghes');
    const videosFilter = document.getElementById('filter-videos');
    const featureCards = document.querySelectorAll('.feature-card');

    if (!ghesFilter || !videosFilter || featureCards.length === 0) return;

    function applyFilters() {
        const showGhesOnly = ghesFilter.checked;
        const showVideosOnly = videosFilter.checked;

        featureCards.forEach(card => {
            const hasGhes = card.dataset.ghes === 'true';
            const hasVideo = card.dataset.hasVideo === 'true';

            let show = true;
            if (showGhesOnly && !hasGhes) show = false;
            if (showVideosOnly && !hasVideo) show = false;

            card.style.display = show ? '' : 'none';
        });
    }

    ghesFilter.addEventListener('change', applyFilters);
    videosFilter.addEventListener('change', applyFilters);
}

/**
 * Initialize expandable custom page badges on section cards
 * Handles "+X more" buttons that reveal and replace with custom pages
 */
export function initExpandableBadges() {
    const expandButtons = document.querySelectorAll('.badge-expandable[data-expand-target]');
    if (!expandButtons) return;

    expandButtons.forEach(button => {
        // Skip if already initialized
        if (button.dataset.initialized) return;
        button.dataset.initialized = 'true';
        
        button.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            
            const targetId = this.dataset.expandTarget;
            const targetElement = document.getElementById(targetId);
            
            if (!targetElement) return;
            
            // Show the hidden custom pages
            targetElement.hidden = false;
            
            // Remove the expand button (replace with badges permanently)
            this.remove();
        });
    });
}

// Auto-initialize on DOMContentLoaded if loaded as a regular script
// (backward compatibility, though we load via dynamic import now)
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        initCollapsibleCards();
        initExpandableBadges();
    });
} else {
    // DOM already loaded, initialize immediately
    initCollapsibleCards();
    initExpandableBadges();
}
