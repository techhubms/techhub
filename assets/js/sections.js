function showSectionCollections(section) {
    if (!section) {
        const urlParams = new URLSearchParams(window.location.search);
        section = urlParams.get('section');
    }

    // If no section parameter, preserve server-side state
    if (!section) {
        return 'default';
    }

    // Hide all section collections first
    const allCollections = document.querySelectorAll('.section-collections');
    allCollections.forEach(div => {
        div.classList.add('hidden');
    });

    // Remove active class from all section navigation items first
    const allSectionNavs = document.querySelectorAll('.site-nav .page-link.regular');
    allSectionNavs.forEach(nav => {
        nav.classList.remove('active');
    });

    // Highlight the selected section in main navigation
    const sectionDiv = document.getElementById(`section-nav-${section}`);
    if (sectionDiv) {
        sectionDiv.classList.add('active');
    }

    // Show the selected section's subnavigation
    const collectionsDiv = document.getElementById(`section-collections-${section}`);
    if (collectionsDiv) {
        collectionsDiv.classList.remove('hidden');
    }

    return section;
}

if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
    module.exports = {
        showSectionCollections
    };
}
