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

    // If already loaded, just re-highlight new elements
    if (loaded.highlightJs) {
        try {
            hljs.highlightAll();
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
        // Load library only once
        if (!loaded.mermaid) {
            await loadScript(CDN.mermaid.cdnUrl);
            loaded.mermaid = true;

            const config = {
                startOnLoad: false,
                theme: 'dark',
                useMaxWidth: true,
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
                    clusterBkg: '#16162a',
                    clusterBorder: '#e0e0e0',
                    defaultLinkColor: '#e0e0e0',
                    activationBorderColor: '#e0e0e0',
                    activationBkgColor: '#2d2d4a',
                    sequenceNumberColor: '#1a1a2e'
                },
                flowchart: {
                    curve: 'basis',
                    padding: 20,
                    nodeSpacing: 50,
                    rankSpacing: 80
                },
                sequence: {
                    actorMargin: 50,
                    boxMargin: 10,
                    boxTextMargin: 5,
                    noteMargin: 10,
                    messageMargin: 35
                },
                gantt: {
                    leftPadding: 75,
                    gridLineStartPadding: 35
                },
                class: {
                    padding: 20
                },
                state: {
                    padding: 20
                }
            };

            mermaid.initialize(config);
        }

        // Render diagrams every time (for Blazor navigation)
        try {
            await mermaid.run({ nodes: mermaidElements });

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

    const svg = clonedDiagram.querySelector('svg');
    if (svg) {
        svg.removeAttribute('width');
        svg.removeAttribute('height');
        svg.style.width = 'min(1200px, 85vw)';
        svg.style.height = 'auto';
        svg.style.maxHeight = '75vh';
    }

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
        const module = await import('./toc-scroll-spy.js');
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
    const expandableElements = document.querySelector('[data-expand-target]');
    if (!collapsibleElements && !expandableElements) return;

    try {
        const module = await import('./custom-pages.js');

        if (collapsibleElements) {
            module.initCollapsibleCards();
        }
        if (expandableElements) {
            module.initExpandableBadges();
        }
    } catch (error) {
        console.error('Failed to load custom pages:', error);
    }
}

// ─── Script Loading Helper ───────────────────────────────────────────────────

/**
 * Load an external script by URL. Returns a promise that resolves when loaded.
 */
function loadScript(src, integrity = null) {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = src;
        script.defer = true;
        script.crossOrigin = 'anonymous';
        script.referrerPolicy = 'no-referrer';
        if (integrity) {
            script.integrity = integrity;
        }
        script.onload = resolve;
        script.onerror = reject;
        document.body.appendChild(script);
    });
}

// ─── Global Exposure ─────────────────────────────────────────────────────────
// Expose init functions globally so Blazor components can call them via JS interop.
// Each function is idempotent and only processes new/unprocessed DOM elements.

window.initHighlighting = initHighlighting;
window.initMermaid = initMermaid;
window.initTocScrollSpy = initTocScrollSpy;
window.initCustomPages = initCustomPages;

// ─── E2E Test Flags ──────────────────────────────────────────────────────────
// E2E tests poll for __scriptsReady to know when page scripts are done.
// Components call markScriptsLoading() before and markScriptsReady() after their
// init calls, so E2E tests can wait for a stable state.

window.markScriptsLoading = function() {
    window.__scriptsReady = false;
    window.__scriptsLoading = true;
};

window.markScriptsReady = function() {
    window.__scriptsReady = true;
    window.__scriptsLoading = false;
};
