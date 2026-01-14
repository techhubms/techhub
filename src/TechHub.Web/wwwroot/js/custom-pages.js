// Custom page interactivity for SDLC, DX Space, and other custom pages
// Handles collapsible sections, cards, and interactive elements

(function () {
    'use strict';

    // Initialize collapsible cards
    function initCollapsibleCards() {
        // SDLC phase blocks
        const phaseHeaders = document.querySelectorAll('.sdlc-phase-header');
        phaseHeaders.forEach(header => {
            header.addEventListener('click', function () {
                const content = this.nextElementSibling;
                const toggle = this.querySelector('.sdlc-phase-toggle');

                if (content && content.classList.contains('sdlc-phase-content')) {
                    content.classList.toggle('expanded');
                    if (toggle) {
                        toggle.classList.toggle('expanded');
                    }
                }
            });
        });

        // SDLC card headers
        const cardHeaders = document.querySelectorAll('.sdlc-card-header');
        cardHeaders.forEach(header => {
            header.addEventListener('click', function () {
                const content = this.nextElementSibling;
                const icon = this.querySelector('.sdlc-card-icon');

                if (content && content.classList.contains('sdlc-card-content')) {
                    content.classList.toggle('expanded');
                    if (icon) {
                        icon.classList.toggle('expanded');
                    }
                }
            });
        });

        // DX Space card headers
        const dxCardHeaders = document.querySelectorAll('.dx-card-header');
        dxCardHeaders.forEach(header => {
            header.addEventListener('click', function () {
                const content = this.nextElementSibling;
                const icon = this.querySelector('.dx-card-icon');

                if (content && content.classList.contains('dx-card-content')) {
                    content.classList.toggle('expanded');
                    if (icon) {
                        icon.classList.toggle('expanded');
                    }
                }
            });
        });
    }

    // Initialize on page load
    document.addEventListener('DOMContentLoaded', initCollapsibleCards);

    // Re-initialize after Blazor enhanced navigation
    if (window.Blazor) {
        Blazor.addEventListener('enhancedload', initCollapsibleCards);
    }
})();
