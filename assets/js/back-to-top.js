/**
 * Back to Top Button
 * Shows a floating button when user scrolls down, smoothly scrolls to top on click
 */
(function () {
  'use strict';

  const button = document.querySelector('.back-to-top');
  if (!button) return;

  const SCROLL_THRESHOLD = 50;

  function updateVisibility() {
    const shouldShow = window.scrollY > SCROLL_THRESHOLD;
    button.classList.toggle('visible', shouldShow);
  }

  function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  // Throttle scroll events for performance
  let ticking = false;
  window.addEventListener('scroll', function () {
    if (!ticking) {
      window.requestAnimationFrame(function () {
        updateVisibility();
        ticking = false;
      });
      ticking = true;
    }
  }, { passive: true });

  button.addEventListener('click', scrollToTop);

  // Initial state
  updateVisibility();
})();
