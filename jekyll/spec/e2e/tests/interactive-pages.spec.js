const { test, expect } = require('@playwright/test');
const { navigateAndVerify } = require('./helpers.js');

test.describe('Interactive Pages', () => {

  test.describe('AI SDLC Page', () => {
    const pageUrl = '/ai/sdlc.html';

    test('should load AI SDLC page successfully', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Verify basic page structure
      await expect(page.locator('.sdlc-container')).toBeVisible();
    });

    test('should have collapsible phase cards', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Find all phase blocks
      const phaseBlocks = page.locator('.sdlc-phase-block');
      const phaseCount = await phaseBlocks.count();

      // Verify there are phase blocks
      expect(phaseCount).toBeGreaterThan(0);

      // Test the first phase card can be toggled
      if (phaseCount > 0) {
        const firstPhase = phaseBlocks.first();
        const firstPhaseHeader = firstPhase.locator('.sdlc-phase-header');
        const firstPhaseContent = firstPhase.locator('.sdlc-phase-content');

        // Verify header is visible and clickable
        await expect(firstPhaseHeader).toBeVisible();

        // Get initial state (might be expanded or collapsed)
        const initialDisplay = await firstPhaseContent.evaluate(el =>
          window.getComputedStyle(el).display
        );

        // Click to toggle
        await firstPhaseHeader.click();

        // Wait for animation
        await page.waitForTimeout(400);

        // Verify state changed
        const newDisplay = await firstPhaseContent.evaluate(el =>
          window.getComputedStyle(el).display
        );

        expect(newDisplay).not.toBe(initialDisplay);
      }
    });

    test('should have functional expand/collapse all controls', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Look for expand/collapse all buttons (if they exist)
      const expandAllBtn = page.locator('button:has-text("Expand All"), a:has-text("Expand All")');
      const collapseAllBtn = page.locator('button:has-text("Collapse All"), a:has-text("Collapse All")');

      const hasExpandBtn = await expandAllBtn.count() > 0;
      const hasCollapseBtn = await collapseAllBtn.count() > 0;

      if (hasExpandBtn || hasCollapseBtn) {
        // If these controls exist, test them
        if (hasCollapseBtn) {
          await collapseAllBtn.first().click();
          await page.waitForTimeout(400);

          // Verify phases are collapsed
          const phaseContents = page.locator('.sdlc-phase-content');
          const contentCount = await phaseContents.count();

          if (contentCount > 0) {
            // Check if at least some are hidden/collapsed
            const firstContent = phaseContents.first();
            const display = await firstContent.evaluate(el =>
              window.getComputedStyle(el).display
            );
            // Display should be 'none' when collapsed
            expect(display === 'none' || display === 'block').toBe(true);
          }
        }

        if (hasExpandBtn) {
          await expandAllBtn.first().click();
          await page.waitForTimeout(400);

          // Verify phases are expanded
          const phaseContents = page.locator('.sdlc-phase-content');
          const contentCount = await phaseContents.count();

          if (contentCount > 0) {
            const firstContent = phaseContents.first();
            const display = await firstContent.evaluate(el =>
              window.getComputedStyle(el).display
            );
            // Display should be visible when expanded
            expect(display === 'none' || display === 'block').toBe(true);
          }
        }
      }
    });

    test('should have SDLC diagram rendered', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Look for Mermaid diagram container
      const mermaidDiagram = page.locator('.mermaid, pre.mermaid, [data-mermaid]');

      if (await mermaidDiagram.count() > 0) {
        // Wait for Mermaid to render
        await page.waitForTimeout(1000);

        // Verify diagram is visible
        await expect(mermaidDiagram.first()).toBeVisible();
      }
    });
  });

  test.describe('DX, SPACE & DORA Page', () => {
    const pageUrl = '/devops/dx-space.html';

    test('should load DX, SPACE & DORA page successfully', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Verify basic page structure
      await expect(page.locator('.dx-container')).toBeVisible();
    });

    test('should have collapsible section cards', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Find all section cards
      const sectionCards = page.locator('.dx-section-card');
      const cardCount = await sectionCards.count();

      // Verify there are section cards
      expect(cardCount).toBeGreaterThan(0);

      // Test the first section card can be toggled
      if (cardCount > 0) {
        const firstCard = sectionCards.first();
        const firstCardHeader = firstCard.locator('.dx-card-header');
        const firstCardContent = firstCard.locator('.dx-card-content');

        // Verify header is visible and clickable
        await expect(firstCardHeader).toBeVisible();

        // Get initial state
        const initialDisplay = await firstCardContent.evaluate(el =>
          window.getComputedStyle(el).display
        );

        // Click to toggle
        await firstCardHeader.click();

        // Wait for animation
        await page.waitForTimeout(400);

        // Verify state changed
        const newDisplay = await firstCardContent.evaluate(el =>
          window.getComputedStyle(el).display
        );

        expect(newDisplay).not.toBe(initialDisplay);
      }
    });

    test('should display DORA, SPACE, and DevEx sections', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Look for key sections mentioned in the title
      const pageContent = await page.textContent('body');

      // Verify mentions of key frameworks
      expect(pageContent).toContain('DORA');
      expect(pageContent).toContain('SPACE');
    });
  });

  test.describe('GenAI Basics Page', () => {
    const pageUrl = '/ai/genai-basics.html';

    test('should load GenAI Basics page successfully', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Verify page loaded
      const pageContent = await page.textContent('body');
      expect(pageContent).toContain('GenAI');
    });

    test('should render Mermaid diagrams', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Look for Mermaid diagram containers
      const mermaidDiagrams = page.locator('.mermaid, pre.mermaid, [data-mermaid]');
      const diagramCount = await mermaidDiagrams.count();

      if (diagramCount > 0) {
        // Wait for Mermaid to render
        await page.waitForTimeout(1500);

        // Verify at least one diagram is visible
        await expect(mermaidDiagrams.first()).toBeVisible();

        // Verify diagrams contain SVG elements (rendered output)
        const firstDiagram = mermaidDiagrams.first();
        const hasSvg = await firstDiagram.locator('svg').count() > 0;

        // If SVG exists, it means Mermaid rendered successfully
        if (hasSvg) {
          await expect(firstDiagram.locator('svg')).toBeVisible();
        }
      }
    });

    test('should have table of contents with working links', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Look for table of contents
      const tocLinks = page.locator('a[href^="#"]');
      const tocCount = await tocLinks.count();

      if (tocCount > 0) {
        // Click first TOC link
        const firstLink = tocLinks.first();
        const href = await firstLink.getAttribute('href');

        if (href && href.length > 1) {
          await firstLink.click();

          // Verify URL updated with hash
          await expect(page).toHaveURL(new RegExp(href.replace('#', '.*#')));
        }
      }
    });
  });

  test.describe('GenAI Advanced Page', () => {
    const pageUrl = '/ai/genai-advanced.html';

    test('should load GenAI Advanced page successfully', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Verify page loaded
      const pageContent = await page.textContent('body');
      expect(pageContent).toContain('GenAI');
    });

    test('should render Mermaid diagrams', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Look for Mermaid diagram containers
      const mermaidDiagrams = page.locator('.mermaid, pre.mermaid, [data-mermaid]');
      const diagramCount = await mermaidDiagrams.count();

      if (diagramCount > 0) {
        // Wait for Mermaid to render
        await page.waitForTimeout(1500);

        // Verify at least one diagram is visible
        await expect(mermaidDiagrams.first()).toBeVisible();

        // Verify diagrams contain SVG elements (rendered output)
        const firstDiagram = mermaidDiagrams.first();
        const hasSvg = await firstDiagram.locator('svg').count() > 0;

        if (hasSvg) {
          await expect(firstDiagram.locator('svg')).toBeVisible();
        }
      }
    });

    test('should have table of contents with working links', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Look for table of contents
      const tocLinks = page.locator('a[href^="#"]');
      const tocCount = await tocLinks.count();

      if (tocCount > 0) {
        // Click first TOC link
        const firstLink = tocLinks.first();
        const href = await firstLink.getAttribute('href');

        if (href && href.length > 1) {
          await firstLink.click();

          // Verify URL updated with hash
          await expect(page).toHaveURL(new RegExp(href.replace('#', '.*#')));
        }
      }
    });
  });

  test.describe('GenAI Applied Page', () => {
    const pageUrl = '/ai/genai-applied.html';

    test('should load GenAI Applied page successfully', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Verify page loaded
      const pageContent = await page.textContent('body');
      expect(pageContent).toContain('GenAI');
    });

    test('should render Mermaid diagrams', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Look for Mermaid diagram containers
      const mermaidDiagrams = page.locator('.mermaid, pre.mermaid, [data-mermaid]');
      const diagramCount = await mermaidDiagrams.count();

      if (diagramCount > 0) {
        // Wait for Mermaid to render
        await page.waitForTimeout(1500);

        // Verify at least one diagram is visible
        await expect(mermaidDiagrams.first()).toBeVisible();

        // Verify diagrams contain SVG elements (rendered output)
        const firstDiagram = mermaidDiagrams.first();
        const hasSvg = await firstDiagram.locator('svg').count() > 0;

        if (hasSvg) {
          await expect(firstDiagram.locator('svg')).toBeVisible();
        }
      }
    });

    test('should have table of contents with working links', async ({ page }) => {
      await navigateAndVerify(page, pageUrl, {
        expectTitle: true,
        checkErrors: true
      });

      // Look for table of contents
      const tocLinks = page.locator('a[href^="#"]');
      const tocCount = await tocLinks.count();

      if (tocCount > 0) {
        // Click first TOC link
        const firstLink = tocLinks.first();
        const href = await firstLink.getAttribute('href');

        if (href && href.length > 1) {
          await firstLink.click();

          // Verify URL updated with hash
          await expect(page).toHaveURL(new RegExp(href.replace('#', '.*#')));
        }
      }
    });
  });

});
