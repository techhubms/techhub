/**
 * Hero Banner - Collapse/expand functionality with cookie persistence.
 *
 * Stores two cookies:
 *   - hero-banner-collapsed: 'true' or 'false'
 *   - hero-banner-hash:      hash of currently-active card titles
 *
 * When the hash stored in the cookie differs from the current hash,
 * the Blazor component auto-expands the banner and calls setHash() to
 * update the cookie so the banner stays closed on the next visit.
 *
 * @see {@link /workspaces/techhub/src/TechHub.Web/Components/HeroBanner.razor} for server-side rendering
 */

'use strict';

const COLLAPSED_COOKIE = 'hero-banner-collapsed';
const HASH_COOKIE = 'hero-banner-hash';

/**
 * Set a cookie with SameSite=Lax and 1-year expiry.
 * @param {string} name  - Cookie name
 * @param {string} value - Cookie value
 */
function setCookie(name, value) {
    const maxAge = 365 * 24 * 60 * 60; // 1 year
    document.cookie = `${name}=${encodeURIComponent(value)};path=/;max-age=${maxAge};SameSite=Lax`;
}

/**
 * Persist whether the banner is collapsed.
 * @param {boolean} collapsed
 */
function setCollapsed(collapsed) {
    setCookie(COLLAPSED_COOKIE, collapsed ? 'true' : 'false');
}

/**
 * Persist the hash of currently-visible card titles.
 * @param {string} hash
 */
function setHash(hash) {
    setCookie(HASH_COOKIE, hash);
}

// Expose API for Blazor JS interop
window.TechHub = window.TechHub || {};
window.TechHub.heroBanner = {
    setCollapsed,
    setHash
};
