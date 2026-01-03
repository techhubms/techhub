function setLogoVisibility() {
    // Get current hostname
    const hostname = window.location.hostname;

    // Check for testing overrides in query string
    const urlParams = new URLSearchParams(window.location.search);
    const xebiaOverride = urlParams.get('xebia') === 'true';

    // Only run if Xebia override is detected - otherwise server-side rendering is already correct
    if (xebiaOverride || hostname === 'xebia.ms' || hostname.endsWith('.xebia.ms')) {
        const xebiaLogo = document.getElementById('xebia-logo');
        const xebiaContact = document.getElementById('xebia-contact');
        const githubCopilotLogo = document.getElementById('github-copilot-logo');
        const techhubLogo = document.getElementById('techhub-logo');
        const techHubAbout = document.getElementById('techhub-about');

        // Hide TechHub logo first then show Xebia/GitHub logo's to avoid showing both at once briefly
        techhubLogo.style.display = 'none';
        xebiaLogo.style.display = 'inline-block';
        githubCopilotLogo.style.display = 'inline-block';

        // Then do the same for the contact button
        techHubAbout.style.display = 'none';
        xebiaContact.style.display = 'inline-block';
    }
}

// Export for testing if in Node.js environment
if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
    module.exports = {
        setLogoVisibility
    };
}
