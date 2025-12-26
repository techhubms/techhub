function showSectionCollections(section) {
    const urlParams = new URLSearchParams(window.location.search);
    const currentSection = section || urlParams.get('section') || 'default';

    // Hide all section collections first
    const allCollections = document.querySelectorAll('.section-collections');
    allCollections.forEach(div => {
        if (div.id !== 'section-collections-home') {
            div.classList.add('hidden');
        }
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
