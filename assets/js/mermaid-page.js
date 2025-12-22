async function renderMermaidDiagrams() {
  const codeBlocks = Array.from(document.querySelectorAll('pre > code'))
    .filter((code) => {
      const classes = (code.className || '').split(/\s+/).filter(Boolean);
      return classes.includes('language-mermaid') || classes.includes('mermaid');
    });

  if (codeBlocks.length === 0) {
    return;
  }

  const mermaidNodes = [];

  for (const code of codeBlocks) {
    const pre = code.parentElement;
    if (!pre) {
      continue;
    }

    const diagramSource = (code.textContent || '').trim();
    if (!diagramSource) {
      continue;
    }

    const container = document.createElement('div');
    container.className = 'mermaid';
    container.textContent = diagramSource;

    pre.replaceWith(container);
    mermaidNodes.push(container);
  }

  if (mermaidNodes.length === 0) {
    return;
  }

  try {
    const mermaidModule = await import('https://cdn.jsdelivr.net/npm/mermaid@11/dist/mermaid.esm.min.mjs');
    const mermaid = mermaidModule.default ?? mermaidModule;

    if (!mermaid || typeof mermaid.initialize !== 'function') {
      console.warn('Mermaid module loaded but API was unexpected.');
      return;
    }

    mermaid.initialize({
      startOnLoad: false,
      securityLevel: 'strict',
      theme: 'base',
      themeVariables: {
        // Background and primary colors
        primaryColor: '#f9a825',
        primaryTextColor: '#1a1a2e',
        primaryBorderColor: '#f9a825',
        // Secondary colors
        secondaryColor: '#1976d2',
        secondaryTextColor: '#ffffff',
        secondaryBorderColor: '#1976d2',
        // Tertiary colors
        tertiaryColor: '#2d2d4a',
        tertiaryTextColor: '#e0e0e0',
        tertiaryBorderColor: '#4a4a6a',
        // Background
        background: '#1a1a2e',
        mainBkg: '#2d2d4a',
        // Text colors
        textColor: '#e0e0e0',
        lineColor: '#8892b0',
        // Node colors
        nodeBorder: '#f9a825',
        nodeTextColor: '#1a1a2e',
        // Sequence diagram specific
        actorBkg: '#f9a825',
        actorBorder: '#f9a825',
        actorTextColor: '#1a1a2e',
        actorLineColor: '#8892b0',
        signalColor: '#e0e0e0',
        signalTextColor: '#e0e0e0',
        labelBoxBkgColor: '#f9a825',
        labelBoxBorderColor: '#f9a825',
        labelTextColor: '#1a1a2e',
        loopTextColor: '#e0e0e0',
        noteBkgColor: '#f9a825',
        noteTextColor: '#1a1a2e',
        noteBorderColor: '#f9a825',
        activationBkgColor: '#3d3d5a',
        activationBorderColor: '#f9a825',
        sequenceNumberColor: '#1a1a2e'
      }
    });

    if (typeof mermaid.run === 'function') {
      await mermaid.run({ nodes: mermaidNodes });
    }
  } catch (error) {
    console.warn('Failed to render Mermaid diagrams:', error);
  }
}

if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', renderMermaidDiagrams);
} else {
  renderMermaidDiagrams();
}
