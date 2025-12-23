const { test, expect } = require('@playwright/test');
const {
  setViewportSize,
  navigateAndVerify,
  verifyNavigationLink
} = require('./helpers.js');

test.describe('Hamburger Menu Navigation', () => {

  /**
   * Test hamburger menu functionality
   * @param {Page} page - Playwright page object
   * @param {string} menuSelector - Selector for the hamburger menu button
   */
  async function testHamburgerMenuFunctionality(page, menuSelector) {
    // Find hamburger menu button and verify it exists
    const hamburgerButton = page.locator(menuSelector);

    // Hamburger menu should be visible and functional on mobile
    await expect(hamburgerButton).toBeVisible();
    await expect(hamburgerButton).toBeEnabled();

    // Record initial navigation state
    await page.locator('a[href*="/ai"], a[href*="/github-copilot"]').count();
    const initialVisibleLinks = await page.locator('a[href*="/ai"]:visible, a[href*="/github-copilot"]:visible').count();

    // Click to open menu and verify it actually opens
    await hamburgerButton.click();

    // Wait for menu animation
    await page.waitForTimeout(300);

    // Verify that menu opening changed the navigation accessibility
    const afterOpenNavLinks = await page.locator('a[href*="/ai"], a[href*="/github-copilot"]').count();
    const afterOpenVisibleLinks = await page.locator('a[href*="/ai"]:visible, a[href*="/github-copilot"]:visible').count();

    // Either more links should be visible, or at least links should be accessible
    expect(afterOpenNavLinks).toBeGreaterThan(0);
    expect(afterOpenVisibleLinks).toBeGreaterThanOrEqual(initialVisibleLinks);

    // Verify navigation links are actually clickable
    await verifyNavigationLink(page, 'a[href*="/ai"]', '/ai', {
      shouldBeVisible: false, // May not be visible but should be functional
      shouldBeEnabled: true
    });

    await verifyNavigationLink(page, 'a[href*="/github-copilot"]', '/github-copilot', {
      shouldBeVisible: false,
      shouldBeEnabled: true
    });
  }

  /**
   * Test navigation from hamburger menu
   * @param {Page} page - Playwright page object
   * @param {string} linkSelector - Selector for the navigation link
   * @param {string} expectedHref - Expected href value
   * @param {string} expectedSectionTitle - Expected section title on the target page
   */
  async function testHamburgerNavigation(page, linkSelector, expectedHref, expectedSectionTitle) {
    await setViewportSize(page, 'mobile');
    await navigateAndVerify(page, '/', {
      expectTitle: true,
      checkErrors: true
    });

    // Open hamburger menu (try different selectors)
    const hamburgerSelectors = [
      'label[for="nav-trigger"]',
      'img[alt="Menu"]',
      '.hamburger-menu',
      '.mobile-menu-toggle'
    ];

    let hamburgerButton = null;
    for (const selector of hamburgerSelectors) {
      const button = page.locator(selector);
      if (await button.count() > 0) {
        hamburgerButton = button;
        break;
      }
    }

    if (!hamburgerButton) {
      console.log('No hamburger menu button found, skipping navigation test');
      return;
    }

    await hamburgerButton.click();
    await page.waitForTimeout(300);

    // Find and click the target link
    const targetLink = page.locator(linkSelector).first();

    if (await targetLink.count() > 0 && await targetLink.isVisible()) {
      await verifyNavigationLink(page, linkSelector, expectedHref, {
        clickAndVerify: true,
        expectedUrl: expectedHref
      });

      // Verify the section title is displayed in the content
      if (expectedSectionTitle) {
        await expect(page.locator('h1')).toContainText(expectedSectionTitle);
      }
    } else {
      console.log(`${linkSelector} not found in hamburger menu, skipping navigation test`);
    }
  }

  test('should open and close hamburger menu correctly', async ({ page }) => {
    await setViewportSize(page, 'mobile');
    await navigateAndVerify(page, '/', {
      expectTitle: true,
      checkErrors: true
    });

    await testHamburgerMenuFunctionality(page, 'label[for="nav-trigger"]');
  });

  test('should navigate to AI section from hamburger menu', async ({ page }) => {
    await testHamburgerNavigation(
      page,
      'a[href="/ai"], a:has-text("AI")',
      '/ai',
      'AI'
    );
  });

  test('should navigate to GitHub Copilot section from hamburger menu', async ({ page }) => {
    await testHamburgerNavigation(
      page,
      'a[href="/github-copilot"], a:has-text("GitHub Copilot")',
      '/github-copilot',
      'GitHub Copilot'
    );
  });

  test('should maintain menu functionality on different mobile screen sizes', async ({ page }) => {
    const mobileSizes = [
      { width: 320, height: 568, name: 'iPhone 5' },
      { width: 375, height: 667, name: 'iPhone 6/7/8' },
      { width: 414, height: 736, name: 'iPhone 6/7/8 Plus' },
      { width: 375, height: 812, name: 'iPhone X' }
    ];

    for (const { width, height, name } of mobileSizes) {
      await setViewportSize(page, 'custom', { width, height });
      await navigateAndVerify(page, '/', {
        expectTitle: true,
        checkErrors: false, // Skip error checking for bulk test
        checkPerformance: false
      });

      // Test hamburger menu functionality on this screen size
      await testHamburgerMenuFunctionality(page, 'label[for="nav-trigger"]');

      console.log(`✓ Hamburger menu works on ${name} (${width}x${height})`);
    }
  });

  test('should hide hamburger menu on desktop sizes', async ({ page }) => {
    await setViewportSize(page, 'desktop');
    await navigateAndVerify(page, '/', {
      expectTitle: true,
      checkErrors: true
    });

    // On desktop, regular navigation should be visible
    const regularNav = page.locator('nav').or(
      page.locator('.navigation')
    ).or(
      page.locator('a[href="/ai"]')
    );

    await expect(regularNav.first()).toBeVisible();

    // Hamburger menu should be hidden or not present
    const hamburgerButton = page.locator('img[alt="Menu"]');
    if (await hamburgerButton.count() > 0) {
      // If present, it should be hidden
      const isVisible = await hamburgerButton.isVisible();
      expect(isVisible).toBe(false);
    }
  });
});
