/* JavaScript for features page filter toggle buttons */
document.addEventListener('DOMContentLoaded', function() {
    const filterSections = document.querySelectorAll('.section-filter');

    filterSections.forEach(function(section) {
        const ghesToggleBtn = section.querySelector('.ghes-toggle-btn');
        const videoToggleBtn = section.querySelector('.video-toggle-btn');

        // Error handling: ensure buttons exist
        if (!ghesToggleBtn || !videoToggleBtn) {
            return;
        }

        let ghesOnly = false;
        let videoOnly = false;

        // Find the next videos-grid after this section
        const nextVideosGrid = section.nextElementSibling?.querySelector('.videos-grid');
        if (!nextVideosGrid) {
            return;
        }

        const videoCards = nextVideosGrid.querySelectorAll('.video-card, .video-card-link');
        if (!videoCards.length) {
            return;
        }

        function applyFilters() {
            videoCards.forEach(card => {
                const videoCard = card.classList.contains('video-card') ? card : card.querySelector('.video-card');
                if (!videoCard) {
                    return;
                }

                const ghesSupport = videoCard.getAttribute('data-ghes') === 'true';
                const hasVideo = videoCard.querySelector('.play-icon') !== null;

                let shouldShow = true;

                if (ghesOnly && !ghesSupport) {
                    shouldShow = false;
                }

                if (videoOnly && !hasVideo) {
                    shouldShow = false;
                }

                card.style.display = shouldShow ? '' : 'none';
            });
        }

        ghesToggleBtn.addEventListener('click', function() {
            ghesOnly = !ghesOnly;
            ghesToggleBtn.classList.toggle('active');

            if (ghesOnly) {
                ghesToggleBtn.textContent = 'Show all features';
            } else {
                ghesToggleBtn.textContent = 'Show features with GHES support';
            }

            applyFilters();
        });

        videoToggleBtn.addEventListener('click', function() {
            videoOnly = !videoOnly;
            videoToggleBtn.classList.toggle('active');

            if (videoOnly) {
                videoToggleBtn.textContent = 'Show all features';
            } else {
                videoToggleBtn.textContent = 'Show features with videos';
            }

            applyFilters();
        });
    });
});
