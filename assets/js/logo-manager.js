/**
 * Logo Manager - Co    } else if (hostname === 'localhost' || hostname === '127.0.0.1') {
        // Show localhost logo for local development
        const localhostLogo = document.getElementById('localhost-logo');
        if (localhostLogo) localhostLogo.style.display = 'inline-block';
    } else if (techhubOverride) {
        // Show Tech Hub logo for testing override
        const techhubLogo = document.getElementById('techhub-logo');
        if (techhubLogo) techhubLogo.style.display = 'inline-block';
    } else {s which logo is displayed based on current URL
 * This script runs on page load to show the appropriate logo
 */

function setLogoVisibility() {
    // Get current hostname
    const hostname = window.location.hostname;

    // Check for testing overrides in query string
    const urlParams = new URLSearchParams(window.location.search);
    const xebiaOverride = urlParams.get('xebia') === 'true';

    const xebiaLogo = document.getElementById('xebia-logo');
    const xebiaContact = document.getElementById('xebia-contact');
    const githubCopilotLogo = document.getElementById('github-copilot-logo');
    const techhubLogo = document.getElementById('techhub-logo');
    const techHubAbout = document.getElementById('techhub-about');

    // Determine which logos to show based on hostname or testing overrides
    if (xebiaOverride || hostname.endsWith('.xebia.ms')) {
        xebiaLogo.style.display = 'inline-block';
        githubCopilotLogo.style.display = 'inline-block';
        xebiaContact.style.display = 'inline-block';
    } else {
        techhubLogo.style.display = 'inline-block';
        techHubAbout.style.display = 'inline-block';
    }
}

// Export for testing if in Node.js environment
if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
    module.exports = {
        setLogoVisibility
    };
}
