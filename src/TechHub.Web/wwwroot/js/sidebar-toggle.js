/**
 * Sidebar Toggle - Desktop sidebar collapse/expand functionality.
 *
 * State is persisted via a cookie so the server can read it during SSR
 * and apply the 'sidebar-collapsed' class on <html> before first paint.
 * The JS only needs to toggle the class and update the cookie on click.
 *
 * @see {@link /workspaces/techhub/src/TechHub.Web/Components/App.razor} for server-side class rendering
 * @see {@link /workspaces/techhub/src/TechHub.Web/wwwroot/css/page-container.css} for grid layout
 * @see {@link /workspaces/techhub/src/TechHub.Web/wwwroot/css/sidebar.css} for collapsed hiding rules
 */

'use strict';

const COOKIE_NAME = 'sidebar-collapsed';

/**
 * Check if the sidebar is currently collapsed.
 * @returns {boolean}
 */
function isCollapsed() {
    return document.documentElement.classList.contains('sidebar-collapsed');
}

/**
 * Set a cookie with SameSite=Lax and 1-year expiry.
 * @param {string} value - 'true' or 'false'
 */
function setCookie(value) {
    const maxAge = 365 * 24 * 60 * 60; // 1 year
    document.cookie = `${COOKIE_NAME}=${value};path=/;max-age=${maxAge};SameSite=Lax`;
}

/**
 * Toggle the sidebar collapsed state.
 * Toggles the class on <html> and updates the cookie.
 */
function toggle() {
    const collapsed = document.documentElement.classList.toggle('sidebar-collapsed');
    setCookie(collapsed ? 'true' : 'false');
}

// Expose API for Blazor JS interop
window.TechHub = window.TechHub || {};
window.TechHub.sidebar = {
    isCollapsed,
    toggle
};
