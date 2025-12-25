/**
 * Show section collections subnavigation
 * - Hides all section-collections divs
 * - Shows only the requested section's subnavigation
 * - Highlights the corresponding section in main navigation
 * 
 * @param {string} section - Section identifier (e.g., 'ai', 'github-copilot'). 
 *                          If not provided, reads from ?section= URL parameter.
 * @returns {string} The section that was activated
 */
function showSectionCollections(section) {
    const urlParams = new URLSearchParams(window.location.search);
    const targetSection = section || urlParams.get('section');
    
    // If no section specified and no URL param, do nothing
    if (!targetSection) {
        return null;
    }

    // Hide ALL section-collections divs first
    const allCollectionDivs = document.querySelectorAll('.section-collections');
    allCollectionDivs.forEach(div => {
        div.classList.add('hidden');
    });

    // Show only the target section's collections
    const targetCollectionsDiv = document.getElementById(`section-collections-${targetSection}`);
    if (targetCollectionsDiv) {
        targetCollectionsDiv.classList.remove('hidden');
    }

    // Update main navigation active state
    const allSectionNavLinks = document.querySelectorAll('[id^="section-nav-"]');
    allSectionNavLinks.forEach(link => {
        link.classList.remove('active');
    });

    const targetSectionNav = document.getElementById(`section-nav-${targetSection}`);
    if (targetSectionNav) {
        targetSectionNav.classList.add('active');
    }

    return targetSection;
}

if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
    module.exports = {
        showSectionCollections
    };
}

