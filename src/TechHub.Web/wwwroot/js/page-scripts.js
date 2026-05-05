/**
 * Page Script Initializers
 *
 * Provides individual initialization functions for page-specific JavaScript libraries.
 * Each function is idempotent — safe to call multiple times (only processes new elements).
 *
 * Individual Blazor page components call these functions from their OnAfterRenderAsync,
 * ensuring scripts run on the final DOM after Blazor has finished rendering.
 *
 * CDN configuration is injected by App.razor into window.TechHubCDN before this module loads.
 *
 * @see {@link /workspaces/techhub/src/TechHub.Web/Configuration/CdnLibraries.cs} for version management
 * @see {@link /workspaces/techhub/src/TechHub.Web/Configuration/JsFiles.cs} for loading configuration
 */

'use strict';

// CDN configuration injected by App.razor (C# → JS bridge)
const CDN = window.TechHubCDN;

// Track what's already been loaded to avoid duplicate CDN fetches
const loaded = {
    highlightJs: false,
    mermaid: false
};

// ─── Highlight.js ────────────────────────────────────────────────────────────

/**
 * Initialize syntax highlighting on the page.
 * Loads Highlight.js from CDN on first call, then highlights all code blocks.
 * Subsequent calls only re-highlight (for new DOM content after navigation).
 */
export async function initHighlighting() {
    const codeElements = document.querySelector('pre code');
    if (!codeElements) return;

    // If already loaded, just highlight new (unprocessed) elements
    if (loaded.highlightJs) {
        try {
            document.querySelectorAll('pre code:not([data-highlighted])').forEach(el => hljs.highlightElement(el));
        } catch (e) {
            console.warn('Failed to re-highlight:', e);
        }
        return;
    }

    loaded.highlightJs = true;

    try {
        // Inject CSS theme
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = `${CDN.highlightJs.cdnUrl}/${CDN.highlightJs.themeFile}`;
        link.crossOrigin = 'anonymous';
        link.referrerPolicy = 'no-referrer';
        document.head.appendChild(link);

        // Load core library
        await loadScript(`${CDN.highlightJs.cdnUrl}/highlight.min.js`);

        // Load all core language files in parallel
        await Promise.all(
            CDN.highlightJs.languages.map(lang =>
                loadScript(`${CDN.highlightJs.cdnUrl}/languages/${lang}.min.js`).catch(err => {
                    console.warn(`Failed to load language: ${lang}`, err);
                })
            )
        );

        // Load third-party language plugins (Terraform for IaC content)
        await Promise.all(
            Object.values(CDN.highlightJs.thirdPartyLanguages).map(url =>
                loadScript(url).catch(err => {
                    console.warn('Failed to load third-party language plugin:', err);
                })
            )
        );

        // Register third-party languages with hljs
        if (typeof window.hljsDefineTerraform === 'function') {
            hljs.registerLanguage('terraform', window.hljsDefineTerraform);
            hljs.registerLanguage('tf', window.hljsDefineTerraform);
            hljs.registerLanguage('hcl', window.hljsDefineTerraform);
        }

        hljs.highlightAll();
    } catch (error) {
        console.error('Failed to load Highlight.js:', error);
    }
}

// ─── Mermaid ─────────────────────────────────────────────────────────────────

/**
 * Initialize mermaid diagrams on the page.
 * Loads Mermaid from CDN on first call, then renders unprocessed diagrams.
 * Subsequent calls only render new diagrams (for navigation to new pages).
 */
export async function initMermaid() {
    const mermaidElements = document.querySelectorAll('.mermaid:not([data-processed="true"])');
    if (mermaidElements.length === 0) return;

    try {
        // Load library only once. loadScript handles per-URL retries with exponential
        // backoff and transparently falls back to the next CDN if the primary fails.
        if (!loaded.mermaid) {
            await loadScript(CDN.mermaid.cdnUrls ?? CDN.mermaid.cdnUrl);
            loaded.mermaid = true;

            const config = {
                startOnLoad: false,
                theme: 'dark',
                themeVariables: {
                    primaryColor: '#2d2d4a',
                    primaryTextColor: '#e0e0e0',
                    primaryBorderColor: '#e0e0e0',
                    lineColor: '#e0e0e0',
                    secondaryColor: '#1a1a2e',
                    tertiaryColor: '#16162a',
                    background: '#1a1a2e',
                    mainBkg: '#2d2d4a',
                    secondBkg: '#1a1a2e',
                    labelBackground: '#2d2d4a',
                    labelColor: '#e0e0e0',
                    nodeTextColor: '#e0e0e0',
                    textColor: '#e0e0e0',
                    titleColor: '#e0e0e0',
                    edgeLabelBackground: '#2d2d4a',
                    clusterBkg: '#161b22',
                    clusterBorder: '#e0e0e0',
                    defaultLinkColor: '#e0e0e0',
                    activationBorderColor: '#e0e0e0',
                    activationBkgColor: '#2d2d4a',
                    sequenceNumberColor: '#1a1a2e'
                }
            };

            mermaid.initialize(config);
        }

        // Render diagrams every time (for Blazor navigation)
        try {
            await mermaid.run({ nodes: mermaidElements });
            if (typeof window.__e2eSignal === 'function') window.__e2eSignal('mermaid-rendered');

            // Add expand buttons to each diagram
            mermaidElements.forEach(element => {
                if (element.querySelector('.mermaid-expand-btn')) return; // Already added

                const expandBtn = document.createElement('button');
                expandBtn.className = 'mermaid-expand-btn';
                expandBtn.innerHTML = '<svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor"><path d="M1.5 1a.5.5 0 0 0-.5.5v4a.5.5 0 0 1-1 0v-4A1.5 1.5 0 0 1 1.5 0h4a.5.5 0 0 1 0 1h-4zM10 .5a.5.5 0 0 1 .5-.5h4A1.5 1.5 0 0 1 16 1.5v4a.5.5 0 0 1-1 0v-4a.5.5 0 0 0-.5-.5h-4a.5.5 0 0 1-.5-.5zM.5 10a.5.5 0 0 1 .5.5v4a.5.5 0 0 0 .5.5h4a.5.5 0 0 1 0 1h-4A1.5 1.5 0 0 1 0 14.5v-4a.5.5 0 0 1 .5-.5zm15 0a.5.5 0 0 1 .5.5v4a1.5 1.5 0 0 1-1.5 1.5h-4a.5.5 0 0 1 0-1h4a.5.5 0 0 0 .5-.5v-4a.5.5 0 0 1 .5-.5z"/></svg> Expand';
                expandBtn.title = 'Expand diagram to fullscreen';
                expandBtn.setAttribute('aria-label', 'Expand diagram to fullscreen');
                expandBtn.onclick = () => showMermaidModal(element);
                element.appendChild(expandBtn);
            });

            // Re-scroll to hash target after mermaid rendering.
            // Mermaid diagrams change <pre> to <svg> which shifts layout.
            // The browser's initial hash-scroll happens before mermaid runs,
            // so the scroll position becomes stale after the layout shift.
            // Use scrollTo with scroll-margin-top offset so the heading lands
            // at the same position as native anchor navigation.
            const hash = window.location.hash;
            if (hash) {
                const target = document.querySelector(hash);
                if (target) {
                    const scrollMarginTop = parseFloat(getComputedStyle(target).scrollMarginTop) || 0;
                    const targetTop = target.getBoundingClientRect().top + window.scrollY;
                    window.scrollTo({ top: targetTop - scrollMarginTop, behavior: 'instant' });
                }
            }
        } catch (error) {
            console.error('Mermaid rendering failed:', error);
        }
    } catch (error) {
        console.error('Failed to load Mermaid:', error);
    }
}

// ─── Mermaid Modal ───────────────────────────────────────────────────────────

function showMermaidModal(diagramElement) {
    let modal = document.getElementById('mermaid-modal');
    if (!modal) {
        modal = document.createElement('div');
        modal.id = 'mermaid-modal';
        modal.className = 'mermaid-modal';
        modal.innerHTML = `
            <div class="mermaid-modal-backdrop"></div>
            <div class="mermaid-modal-content">
                <div class="mermaid-modal-diagram"></div>
            </div>
        `;
        document.body.appendChild(modal);

        modal.querySelector('.mermaid-modal-backdrop').onclick = () => closeMermaidModal();

        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && modal.classList.contains('active')) {
                closeMermaidModal();
            }
        });
    }

    const modalDiagram = modal.querySelector('.mermaid-modal-diagram');
    modalDiagram.innerHTML = '';

    // Remove any existing close button from modal
    const existingClose = modal.querySelector('.mermaid-modal-close');
    if (existingClose) existingClose.remove();

    const clonedDiagram = diagramElement.cloneNode(true);
    const clonedButton = clonedDiagram.querySelector('.mermaid-expand-btn');
    if (clonedButton) {
        clonedButton.remove();
    }

    const closeBtn = document.createElement('button');
    closeBtn.className = 'mermaid-modal-close';
    closeBtn.innerHTML = '<svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor"><path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z"/></svg> Close';
    closeBtn.setAttribute('aria-label', 'Close fullscreen diagram');
    closeBtn.onclick = (e) => {
        e.stopPropagation();
        closeMermaidModal();
    };
    modal.appendChild(closeBtn);

    modalDiagram.appendChild(clonedDiagram);
    modal.classList.add('active');
    document.body.style.overflow = 'hidden';
}

function closeMermaidModal() {
    const modal = document.getElementById('mermaid-modal');
    if (modal) {
        modal.classList.remove('active');
        document.body.style.overflow = '';
    }
}

// ─── TOC Scroll Spy ──────────────────────────────────────────────────────────

/**
 * Initialize TOC scroll spy on the page.
 * Only activates if a [data-toc-scroll-spy] element exists.
 */
export async function initTocScrollSpy() {
    const tocElement = document.querySelector('[data-toc-scroll-spy]');
    if (!tocElement) return;

    try {
        const module = await import('./scroll-manager.js');
        module.initTocScrollSpy();
    } catch (error) {
        console.error('Failed to load TOC scroll spy:', error);
    }
}

// ─── Custom Pages ────────────────────────────────────────────────────────────

/**
 * Initialize custom page interactivity (collapsible cards, expandable badges).
 * Only activates if [data-collapsible] or [data-expand-target] elements exist.
 */
export async function initCustomPages() {
    const collapsibleElements = document.querySelector('[data-collapsible]');
    const featureSections = document.querySelector('.features-video-section');

    // Only detect expandable badges that need JS handling — Blazor-managed
    // badges (in SectionCard) use @onclick and are skipped by initExpandableBadges
    // via the data-initialized check. If ALL badges are pre-initialized, skip
    // the import entirely to avoid loading custom-pages.js unnecessarily.
    const expandableElements = document.querySelector('[data-expand-target]');
    const uninitializedExpand = expandableElements &&
        document.querySelector('[data-expand-target]:not([data-initialized])');

    if (!collapsibleElements && !uninitializedExpand && !featureSections) return;

    try {
        const module = await import('./custom-pages.js');

        if (collapsibleElements) {
            module.initCollapsibleCards();
        }
        if (uninitializedExpand) {
            module.initExpandableBadges();
        }
        if (featureSections) {
            module.initFeatureFilters();
        }
    } catch (error) {
        console.error('Failed to load custom pages:', error);
    }
}

// ─── Script Loading Helper ───────────────────────────────────────────────────

/**
 * Load an external script. Resilient against transient CDN failures:
 *   - Accepts either a single URL string or an array of fallback URLs (tried in order).
 *   - Retries each URL with exponential backoff before moving on to the next.
 *
 * @param {string|string[]} sources   - URL, or ordered list of URLs to try.
 * @param {object} [opts]
 * @param {string} [opts.integrity]   - Optional SRI hash (only applied when a single URL is given;
 *                                      fallback URLs usually have different hashes).
 * @param {number} [opts.timeoutMs=10000] - Per-attempt load timeout.
 * @param {number} [opts.retries=2]   - Retries per URL (on top of the initial attempt).
 * @param {number} [opts.backoffMs=500] - Initial backoff; doubles each retry.
 */
function loadScript(sources, opts = {}) {
    const urls = Array.isArray(sources) ? sources : [sources];
    const { integrity = null, timeoutMs = 10_000, retries = 2, backoffMs = 500 } = opts;
    const applyIntegrity = integrity && urls.length === 1;

    function attempt(url) {
        return new Promise((resolve, reject) => {
            const script = document.createElement('script');
            script.src = url;
            script.defer = true;
            script.crossOrigin = 'anonymous';
            script.referrerPolicy = 'no-referrer';
            if (applyIntegrity) {
                script.integrity = integrity;
            }
            const timer = setTimeout(() => {
                script.remove();
                reject(new Error(`Script load timeout: ${url}`));
            }, timeoutMs);
            script.onload = () => { clearTimeout(timer); resolve(); };
            script.onerror = (err) => { clearTimeout(timer); script.remove(); reject(err); };
            document.body.appendChild(script);
        });
    }

    return (async () => {
        let lastError;
        for (const url of urls) {
            for (let i = 0; i <= retries; i++) {
                try {
                    await attempt(url);
                    return;
                } catch (err) {
                    lastError = err;
                    if (i < retries) {
                        await new Promise(r => setTimeout(r, backoffMs * (2 ** i)));
                    }
                }
            }
            console.warn(`All attempts failed for ${url}, trying next source if available.`);
        }
        throw lastError ?? new Error('loadScript: no sources provided');
    })();
}

// ─── Global Exposure ─────────────────────────────────────────────────────────
// Expose init functions globally so Blazor components can call them via JS interop.
// Each function is idempotent and only processes new/unprocessed DOM elements.

window.initHighlighting = initHighlighting;
window.initMermaid = initMermaid;
window.initTocScrollSpy = initTocScrollSpy;
window.initCustomPages = initCustomPages;

// ─── Script Lifecycle Flag ────────────────────────────────────────────────────
// Single flag: window.__scriptsReady
//   false     = page scripts are loading (blocks WaitForBlazorReadyAsync)
//   true      = page scripts complete (WaitForBlazorReadyAsync passes)
//   undefined = initial page load / no scripts on page (passes through)
//
// Components call markScriptsLoading() before and markScriptsReady() after their
// init calls. markScriptsReady also triggers scroll position restoration.

window.markScriptsLoading = function() {
    window.__scriptsReady = false;
    if (typeof window.__e2eSignal === 'function') window.__e2eSignal('scripts-loading');
};

window.markScriptsReady = function() {
    // Guard: only process once per navigation. Multiple components may call this
    // (page component + layout fallback) — the first call does the work.
    if (window.__scriptsReady === true) {
        // Still attempt scroll restore — restoreScrollPosition is safe
        // to call multiple times (only restores on traverse navigation).
        if (typeof window.__restoreScrollPosition === 'function') {
            window.__restoreScrollPosition();
        }
        return;
    }

    window.__scriptsReady = true;
    if (typeof window.__e2eSignal === 'function') window.__e2eSignal('scripts-ready');

    // Restore scroll position on back/forward navigation.
    // This is the single synchronization point: all rendering is complete,
    // DOM is at its final height, so scrollTo won't be clamped.
    if (typeof window.__restoreScrollPosition === 'function') {
        window.__restoreScrollPosition();
    }
};
