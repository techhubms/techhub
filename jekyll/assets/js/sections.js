function showSectionCollections() {
    const urlParams = new URLSearchParams(window.location.search);
    const urlSection = urlParams.get('section');

    // If no querystring parameter, preserve server-side state
    if (!urlSection) {
        return;
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
    const sectionDiv = document.getElementById(`section-nav-${urlSection}`);
    if (sectionDiv) {
        sectionDiv.classList.add('active');
    }

    // Show the selected section's subnavigation
    const collectionsDiv = document.getElementById(`section-collections-${urlSection}`);
    if (collectionsDiv) {
        collectionsDiv.classList.remove('hidden');
    }
}

if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
    module.exports = {
        showSectionCollections
    };
}
