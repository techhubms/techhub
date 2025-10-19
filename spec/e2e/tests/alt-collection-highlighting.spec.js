const { test, expect } = require('@playwright/test');
const {
  navigateAndVerify,
  setupErrorTracking
} = require('./helpers.js');

test.describe('Alt-Collection Tab Highlighting', () => {
  test.describe('VS Code Updates Videos', () => {
    test('should highlight Visual Studio Code Updates tab for vscode-updates video', async ({ page }) => {
      // Navigate to a VS Code Updates video
      await navigateAndVerify(page, '/2025-08-21-Visual-Studio-Code-and-GitHub-Copilot-Whats-new-in-1103.html', {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Wait for any JavaScript navigation highlighting to complete
      await page.waitForTimeout(500);

      // Check if GitHub Copilot section is visible (should be active section)
      const githubCopilotSection = page.locator('#section-collections-github-copilot');
      await expect(githubCopilotSection).toBeVisible();
      await expect(githubCopilotSection).not.toHaveClass(/hidden/);

      // Check if Visual Studio Code Updates tab has the active class
      const vscodeUpdatesTab = page.locator('a[href="/github-copilot/vscode-updates.html"]');
      await expect(vscodeUpdatesTab).toBeVisible();
      await expect(vscodeUpdatesTab).toHaveClass(/active/);

      // Verify Videos tab is NOT active
      const videosTab = page.locator('a[href="/github-copilot/videos.html"]');
      await expect(videosTab).toBeVisible();
      await expect(videosTab).not.toHaveClass(/active/);

      // Verify the page content is actually a VS Code Updates video
      const pageContent = await page.textContent('main');
      expect(pageContent.toLowerCase()).toContain('visual studio code');

      console.log('✅ VS Code Updates video correctly highlights Visual Studio Code Updates tab');
    });

    test('should highlight Visual Studio Code Updates tab for another vscode-updates video', async ({ page }) => {
      // Navigate to another VS Code Updates video
      await navigateAndVerify(page, '/2025-10-16-Visual-Studio-Code-and-GitHub-Copilot-Whats-new-in-1105.html', {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Wait for any JavaScript navigation highlighting to complete
      await page.waitForTimeout(500);

      // Check if Visual Studio Code Updates tab has the active class
      const vscodeUpdatesTab = page.locator('a[href="/github-copilot/vscode-updates.html"]');
      await expect(vscodeUpdatesTab).toBeVisible();
      await expect(vscodeUpdatesTab).toHaveClass(/active/);

      // Verify Videos tab is NOT active
      const videosTab = page.locator('a[href="/github-copilot/videos.html"]');
      await expect(videosTab).toBeVisible();
      await expect(videosTab).not.toHaveClass(/active/);

      console.log('✅ Another VS Code Updates video correctly highlights Visual Studio Code Updates tab');
    });
  });

  test.describe('GitHub Copilot Features Videos', () => {
    test('should highlight Features tab for ghc-features video', async ({ page }) => {
      // Navigate to a GitHub Copilot Features video
      await navigateAndVerify(page, '/2025-04-09-Next-Edit-Suggestions.html', {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Wait for any JavaScript navigation highlighting to complete
      await page.waitForTimeout(500);

      // Check if GitHub Copilot section is visible (should be active section)
      const githubCopilotSection = page.locator('#section-collections-github-copilot');
      await expect(githubCopilotSection).toBeVisible();
      await expect(githubCopilotSection).not.toHaveClass(/hidden/);

      // Check if Features tab has the active class
      const featuresTab = page.locator('a[href="/github-copilot/features.html"]');
      await expect(featuresTab).toBeVisible();
      await expect(featuresTab).toHaveClass(/active/);

      // Verify Videos tab is NOT active
      const videosTab = page.locator('a[href="/github-copilot/videos.html"]');
      await expect(videosTab).toBeVisible();
      await expect(videosTab).not.toHaveClass(/active/);

      // Verify the page content is actually a GitHub Copilot feature
      const pageContent = await page.textContent('main');
      expect(pageContent.toLowerCase()).toContain('github copilot');

      console.log('✅ GitHub Copilot Features video correctly highlights Features tab');
    });

    test('should highlight Features tab for another ghc-features video', async ({ page }) => {
      // Navigate to another GitHub Copilot Features video
      await navigateAndVerify(page, '/2025-04-15-Agent-Mode.html', {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Wait for any JavaScript navigation highlighting to complete
      await page.waitForTimeout(500);

      // Check if Features tab has the active class
      const featuresTab = page.locator('a[href="/github-copilot/features.html"]');
      await expect(featuresTab).toBeVisible();
      await expect(featuresTab).toHaveClass(/active/);

      // Verify Videos tab is NOT active
      const videosTab = page.locator('a[href="/github-copilot/videos.html"]');
      await expect(videosTab).toBeVisible();
      await expect(videosTab).not.toHaveClass(/active/);

      console.log('✅ Another GitHub Copilot Features video correctly highlights Features tab');
    });

    test('should highlight Features tab for third ghc-features video', async ({ page }) => {
      // Navigate to a third GitHub Copilot Features video to ensure consistency
      await navigateAndVerify(page, '/2025-05-02-Bring-Your-Own-LLM.html', {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Wait for any JavaScript navigation highlighting to complete
      await page.waitForTimeout(500);

      // Check if Features tab has the active class
      const featuresTab = page.locator('a[href="/github-copilot/features.html"]');
      await expect(featuresTab).toBeVisible();
      await expect(featuresTab).toHaveClass(/active/);

      // Verify Videos tab is NOT active
      const videosTab = page.locator('a[href="/github-copilot/videos.html"]');
      await expect(videosTab).toBeVisible();
      await expect(videosTab).not.toHaveClass(/active/);

      console.log('✅ Third GitHub Copilot Features video correctly highlights Features tab');
    });
  });

  test.describe('Regular Videos (Control)', () => {
    test('should highlight Videos tab for regular GitHub Copilot video', async ({ page }) => {
      // Navigate to a regular GitHub Copilot video (not in ghc-features or vscode-updates)
      await navigateAndVerify(page, '/2025-08-16-GPT-5-Now-Available-in-GitHub-Copilot-Advanced-Features-and-How-to-Enable.html', {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Wait for any JavaScript navigation highlighting to complete
      await page.waitForTimeout(500);

      // Check if GitHub Copilot section is visible
      const githubCopilotSection = page.locator('#section-collections-github-copilot');
      await expect(githubCopilotSection).toBeVisible();
      await expect(githubCopilotSection).not.toHaveClass(/hidden/);

      // Check if Videos tab has the active class
      const videosTab = page.locator('a[href="/github-copilot/videos.html"]');
      await expect(videosTab).toBeVisible();
      await expect(videosTab).toHaveClass(/active/);

      // Verify Features tab is NOT active
      const featuresTab = page.locator('a[href="/github-copilot/features.html"]');
      await expect(featuresTab).toBeVisible();
      await expect(featuresTab).not.toHaveClass(/active/);

      // Verify Visual Studio Code Updates tab is NOT active
      const vscodeUpdatesTab = page.locator('a[href="/github-copilot/vscode-updates.html"]');
      await expect(vscodeUpdatesTab).toBeVisible();
      await expect(vscodeUpdatesTab).not.toHaveClass(/active/);

      console.log('✅ Regular GitHub Copilot video correctly highlights Videos tab');
    });
  });

  test.describe('Collection Pages (Control)', () => {
    test('should highlight Features tab when on Features collection page', async ({ page }) => {
      // Navigate to the Features collection page itself
      await navigateAndVerify(page, '/github-copilot/features.html', {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Wait for any JavaScript navigation highlighting to complete
      await page.waitForTimeout(500);

      // Check if Features tab has the active class
      const featuresTab = page.locator('a[href="/github-copilot/features.html"]');
      await expect(featuresTab).toBeVisible();
      await expect(featuresTab).toHaveClass(/active/);

      // Verify Videos tab is NOT active
      const videosTab = page.locator('a[href="/github-copilot/videos.html"]');
      await expect(videosTab).toBeVisible();
      await expect(videosTab).not.toHaveClass(/active/);

      console.log('✅ Features collection page correctly highlights Features tab');
    });

    test('should highlight Visual Studio Code Updates tab when on VS Code Updates collection page', async ({ page }) => {
      // Navigate to the VS Code Updates collection page itself
      await navigateAndVerify(page, '/github-copilot/vscode-updates.html', {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Wait for any JavaScript navigation highlighting to complete
      await page.waitForTimeout(500);

      // Check if Visual Studio Code Updates tab has the active class
      const vscodeUpdatesTab = page.locator('a[href="/github-copilot/vscode-updates.html"]');
      await expect(vscodeUpdatesTab).toBeVisible();
      await expect(vscodeUpdatesTab).toHaveClass(/active/);

      // Verify Videos tab is NOT active
      const videosTab = page.locator('a[href="/github-copilot/videos.html"]');
      await expect(videosTab).toBeVisible();
      await expect(videosTab).not.toHaveClass(/active/);

      console.log('✅ VS Code Updates collection page correctly highlights Visual Studio Code Updates tab');
    });

    test('should highlight Videos tab when on Videos collection page', async ({ page }) => {
      // Navigate to the Videos collection page
      await navigateAndVerify(page, '/github-copilot/videos.html', {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: true
      });

      // Wait for any JavaScript navigation highlighting to complete
      await page.waitForTimeout(500);

      // Check if Videos tab has the active class
      const videosTab = page.locator('a[href="/github-copilot/videos.html"]');
      await expect(videosTab).toBeVisible();
      await expect(videosTab).toHaveClass(/active/);

      // Verify Features tab is NOT active
      const featuresTab = page.locator('a[href="/github-copilot/features.html"]');
      await expect(featuresTab).toBeVisible();
      await expect(featuresTab).not.toHaveClass(/active/);

      // Verify Visual Studio Code Updates tab is NOT active
      const vscodeUpdatesTab = page.locator('a[href="/github-copilot/vscode-updates.html"]');
      await expect(vscodeUpdatesTab).toBeVisible();
      await expect(vscodeUpdatesTab).not.toHaveClass(/active/);

      console.log('✅ Videos collection page correctly highlights Videos tab');
    });
  });
});
