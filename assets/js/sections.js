function showSectionCollections(section) {
    const urlParams = new URLSearchParams(window.location.search);
    const currentSection = section || urlParams.get('section') || 'default';

    // Show the selected section
    const sectionDiv = document.getElementById(`section-nav-${currentSection}`);
    if (sectionDiv) {
        sectionDiv.classList.add('active');
    }

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
