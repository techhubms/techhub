(function initSdlcModule(root) {
    'use strict';

    function getRequiredElement(selector, doc) {
        if (!doc) {
            return null;
        }

        return doc.querySelector(selector);
    }

    function toggleCardHeader(header) {
        if (!header) {
            return false;
        }

        const content = header.nextElementSibling;
        if (!content) {
            return false;
        }

        const icon = header.querySelector('.sdlc-card-icon');

        const isNowExpanded = !content.classList.contains('expanded');

        content.classList.toggle('expanded', isNowExpanded);
        if (icon) {
            icon.classList.toggle('expanded', isNowExpanded);
        }

        return isNowExpanded;
    }

    function togglePhaseHeader(header) {
        if (!header) {
            return false;
        }

        const content = header.nextElementSibling;
        if (!content) {
            return false;
        }

        const toggle = header.querySelector('.sdlc-phase-toggle');

        const isNowExpanded = !content.classList.contains('expanded');

        content.classList.toggle('expanded', isNowExpanded);
        if (toggle) {
            toggle.classList.toggle('expanded', isNowExpanded);
        }

        return isNowExpanded;
    }

    function setAllCardsExpanded(doc, expanded) {
        if (!doc) {
            return;
        }

        const allContents = doc.querySelectorAll('.sdlc-card-content');
        const allIcons = doc.querySelectorAll('.sdlc-card-icon');

        allContents.forEach((content) => content.classList.toggle('expanded', expanded));
        allIcons.forEach((icon) => icon.classList.toggle('expanded', expanded));
    }

    function toggleAll(doc) {
        if (!doc) {
            return false;
        }

        const allContents = doc.querySelectorAll('.sdlc-card-content');
        const isAnyExpanded = Array.from(allContents).some((content) => content.classList.contains('expanded'));

        setAllCardsExpanded(doc, !isAnyExpanded);

        return !isAnyExpanded;
    }

    function openCard(cardId, doc) {
        if (!doc || !cardId) {
            return false;
        }

        const card = doc.getElementById(`sdlc-card-${cardId}`);
        if (!card) {
            return false;
        }

        const content = card.querySelector('.sdlc-card-content');
        const icon = card.querySelector('.sdlc-card-icon');

        if (!content) {
            return false;
        }

        const shouldOpen = true;
        content.classList.toggle('expanded', shouldOpen);
        if (icon) {
            icon.classList.toggle('expanded', shouldOpen);
        }

        if (typeof card.scrollIntoView === 'function') {
            card.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        }

        card.style.boxShadow = '0 0 20px rgba(88, 166, 255, 0.5)';
        root.setTimeout(() => {
            card.style.boxShadow = '';
        }, 2000);

        return true;
    }

    function initializeMermaid(doc) {
        if (!doc || !root.mermaid) {
            return;
        }

        const diagram = getRequiredElement('.sdlc-mermaid .mermaid', doc);
        if (!diagram) {
            return;
        }

        try {
            root.mermaid.initialize({
                startOnLoad: false,
                theme: 'dark',
                securityLevel: 'loose',
                flowchart: {
                    htmlLabels: true,
                    curve: 'basis'
                }
            });

            // mermaid.run returns a promise in newer versions.
            const result = root.mermaid.run({ querySelector: '.sdlc-mermaid .mermaid' });
            if (result && typeof result.catch === 'function') {
                result.catch(() => {
                    // Ignore rendering failures to keep the page functional.
                });
            }
        } catch {
            // Ignore Mermaid initialization errors to keep the page functional.
        }
    }

    function wireMermaidClickHandlers(doc) {
        if (!doc) {
            return;
        }

        root.sdlcOpenCardPlanning = function sdlcOpenCardPlanning() {
            openCard('planning', doc);
        };
        root.sdlcOpenCardDesign = function sdlcOpenCardDesign() {
            openCard('design', doc);
        };
        root.sdlcOpenCardImplementation = function sdlcOpenCardImplementation() {
            openCard('implementation', doc);
        };
        root.sdlcOpenCardTesting = function sdlcOpenCardTesting() {
            openCard('testing', doc);
        };
        root.sdlcOpenCardDeployment = function sdlcOpenCardDeployment() {
            openCard('deployment', doc);
        };
        root.sdlcOpenCardMaintenance = function sdlcOpenCardMaintenance() {
            openCard('maintenance', doc);
        };
    }

    function init(doc) {
        if (!doc) {
            return;
        }

        if (!doc.querySelector('.sdlc-container')) {
            return;
        }

        wireMermaidClickHandlers(doc);
        initializeMermaid(doc);

        const expandAllButton = doc.querySelector('.sdlc-expand-all-btn');
        if (expandAllButton) {
            expandAllButton.addEventListener('click', () => toggleAll(doc));
        }

        doc.addEventListener('click', (event) => {
            const cardHeader = event.target.closest('.sdlc-card-header');
            if (cardHeader) {
                toggleCardHeader(cardHeader);
                return;
            }

            const phaseHeader = event.target.closest('.sdlc-phase-header');
            if (phaseHeader) {
                togglePhaseHeader(phaseHeader);
                return;
            }
        });
    }

    const api = {
        init,
        openCard,
        toggleAll,
        toggleCardHeader,
        togglePhaseHeader
    };

    if (typeof module !== 'undefined' && module.exports) {
        module.exports = api;
        return;
    }

    root.SDLC = api;
    init(root.document);
})(typeof window !== 'undefined' ? window : globalThis);
