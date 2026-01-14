/**
 * Header Scroll Behavior
 * Collapses the section banner when scrolling down
 */

export function initHeaderScroll() {
    const header = document.querySelector('.site-header');
    if (!header) return;

    let lastScrollY = window.scrollY;
    const scrollThreshold = 100; // Collapse after scrolling 100px

    function updateHeader() {
        const currentScrollY = window.scrollY;

        if (currentScrollY > scrollThreshold) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }

        lastScrollY = currentScrollY;
    }

    // Throttle scroll events with requestAnimationFrame
    let ticking = false;
    function handleScroll() {
        if (!ticking) {
            window.requestAnimationFrame(() => {
                updateHeader();
                ticking = false;
            });
            ticking = true;
        }
    }

    window.addEventListener('scroll', handleScroll, { passive: true });

    // Initial check
    updateHeader();
}
