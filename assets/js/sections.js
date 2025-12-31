function showSectionCollections(section) {
    const urlParams = new URLSearchParams(window.location.search);
    const urlSection = urlParams.get('section');

    // If no URL parameter is provided and no section argument, preserve server-side state
    if (!section && !urlSection) {
        // Don't modify section collections - keep server-side rendering
        return 'default';
    }

    const currentSection = section || urlSection || 'default';

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

    // Show the selected section navigation
    const sectionDiv = document.getElementById(`section-nav-${currentSection}`);
    if (sectionDiv) {
        sectionDiv.classList.add('active');
    }

    // Show the selected section collections
    const collectionsDiv = document.getElementById(`section-collections-${currentSection}`);
    if (collectionsDiv) {
        collectionsDiv.classList.remove('hidden');
    }

    return currentSection;
}

if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
    module.exports = {
        showSectionCollections
    };
}
