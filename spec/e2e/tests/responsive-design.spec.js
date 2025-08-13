const { test, expect } = require('@playwright/test');
const {
  testResponsiveLayout,
  testHamburgerMenu,
  testDesktopNavigation,
  testTouchInteractions,
  checkNoHorizontalScrollbar,
  verifyContentFitsViewport
} = require('./helpers.js');

test.describe('Responsive Design', () => {

  const viewports = [
    { name: 'Mobile Portrait', width: 375, height: 667 },
    { name: 'Mobile Landscape', width: 667, height: 375 },
    { name: 'Tablet Portrait', width: 768, height: 1024 },
    { name: 'Tablet Landscape', width: 1024, height: 768 },
    { name: 'Desktop Small', width: 1200, height: 800 },
    { name: 'Desktop Large', width: 1920, height: 1080 },
    { name: 'Ultra Wide', width: 2560, height: 1440 }
  ];

  const pagesToTest = [
    '/',
    '/ai',
    '/ai/news.html',
    '/github-copilot',
    '/github-copilot/news.html'
  ];

  // Generate responsive layout tests for all viewport/page combinations
  viewports.forEach(({ name, width, height }) => {
    test.describe(`${name} (${width}x${height})`, () => {
      pagesToTest.forEach(url => {
        test(`should render ${url} correctly on ${name}`, async ({ page }) => {
          await testResponsiveLayout(page, url, { width, height }, name);
        });
      });
    });
  });

  test.describe('Navigation Menu Responsiveness', () => {

    test('should show hamburger menu on mobile devices', async ({ page }) => {
      await testHamburgerMenu(page, '/');
    });

    test('should hide hamburger menu on desktop', async ({ page }) => {
      await testDesktopNavigation(page, '/');
    });

    test('should toggle hamburger menu correctly', async ({ page }) => {
      await testHamburgerMenu(page, '/');
    });

  });

  test.describe('Content Layout Responsiveness', () => {

    test('should stack content appropriately on mobile', async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
      await page.goto('/');

      const noHorizontalScrollbar = await checkNoHorizontalScrollbar(page);
      expect(noHorizontalScrollbar).toBe(true);

      await expect(page.locator('main')).toBeVisible();
    });

    test('should handle filter buttons responsively', async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
      await page.goto('/ai/news.html');

      // Wait for filters to load
      await page.waitForSelector('.tag-filter-btn', { timeout: 1000 });

      // Check that filter buttons are accessible
      const filterButtons = page.locator('.tag-filter-btn');
      const buttonCount = await filterButtons.count();

      if (buttonCount > 0) {
        await expect(filterButtons.first()).toBeVisible();

        // Check if there's a "More" toggle for mobile
        const moreButton = page.locator('button:has-text("More")');
        if (await moreButton.count() > 0) {
          await moreButton.click();
          await page.waitForTimeout(300);
        }
      }

      const noHorizontalScrollbar = await checkNoHorizontalScrollbar(page);
      expect(noHorizontalScrollbar).toBe(true);
    });

    test('should handle long content without horizontal scroll', async ({ page }) => {
      await page.setViewportSize({ width: 320, height: 568 }); // Very narrow viewport
      await page.goto('/ai/news.html');

      const noHorizontalScrollbar = await checkNoHorizontalScrollbar(page);
      expect(noHorizontalScrollbar).toBe(true);

      await expect(page.locator('main')).toBeVisible();
    });

  });

  test.describe('Image and Media Responsiveness', () => {

    test('should scale images appropriately', async ({ page }) => {
      const viewport = { width: 375, height: 667 };
      await page.setViewportSize(viewport);
      await page.goto('/');

      await verifyContentFitsViewport(page, viewport);
    });

  });

  test.describe('Touch Interaction', () => {

    test('should handle touch interactions on mobile', async ({ page }) => {
      await testTouchInteractions(page, '/ai/news.html');
    });

  });

});
