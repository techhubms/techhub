const { test, expect } = require('@playwright/test');
const {
  navigateAndVerify,
  setViewportSize,
  checkNoHorizontalScrollbar,
  verifyNavigationLink
} = require('./helpers.js');

test.describe('Basic Site Functionality', () => {

  test('should load index page successfully', async ({ page }) => {
    // Navigate and verify with comprehensive checks
    const result = await navigateAndVerify(page, '/', {
      expectTitle: true,
      checkErrors: true,
      checkPerformance: true
    });

    console.log(`Index page loaded in ${result.loadTime}ms`);

    // Verify page has actual content (not just template)
    const bodyText = await page.innerText('body');
    expect(bodyText.trim().length).toBeGreaterThan(100);

    // Verify no raw HTML is showing (proper templating)
    // Check for unescaped HTML tags, but allow legitimate use of < and > in content
    expect(bodyText).not.toMatch(/<[^>]+>/); // No HTML tags in plain text
    expect(bodyText).not.toContain('{{');
    expect(bodyText).not.toContain('}}');

    // Verify essential page elements are present and functional
    await expect(page.locator('header')).toBeVisible();
    await expect(page.locator('main')).toBeVisible();
    await expect(page.locator('footer')).toBeVisible();

    // Verify main navigation links are present and functional
    await verifyNavigationLink(page, 'a[href*="/ai"]', '/ai', {
      shouldBeVisible: true,
      shouldBeEnabled: true
    });

    await verifyNavigationLink(page, 'a[href*="/github-copilot"]', '/github-copilot', {
      shouldBeVisible: true,
      shouldBeEnabled: true
    });

    // Verify the page has meaningful content sections
    const mainContent = page.locator('main');
    const mainText = await mainContent.textContent();
    expect(mainText.trim().length).toBeGreaterThan(50);
  });

  test('should render without horizontal scrollbar on desktop', async ({ page }) => {
    await setViewportSize(page, 'desktop');
    await page.goto('/');

    const noHorizontalScrollbar = await checkNoHorizontalScrollbar(page);
    expect(noHorizontalScrollbar).toBe(true);
  });

  test('should render without horizontal scrollbar on mobile', async ({ page }) => {
    await setViewportSize(page, 'mobile');
    await page.goto('/');

    const noHorizontalScrollbar = await checkNoHorizontalScrollbar(page);
    expect(noHorizontalScrollbar).toBe(true);
  });

  test('should have working RSS feed link', async ({ page }) => {
    await page.goto('/');

    // Find RSS link and verify it exists
    const rssLink = page.locator('a[href="/feed.xml"]');
    await expect(rssLink).toBeVisible();
    await expect(rssLink).toBeEnabled();

    const href = await rssLink.getAttribute('href');
    expect(href).toBe('/feed.xml');

    // Test that RSS feed actually exists and is valid
    const response = await page.request.get('/feed.xml');
    expect(response.status()).toBe(200);

    const content = await response.text();
    expect(content.length).toBeGreaterThan(100);
    expect(content).toContain('<?xml');

    // Feed can be either RSS or Atom format
    const isRssFeed = content.includes('<rss');
    const isAtomFeed = content.includes('<feed xmlns="http://www.w3.org/2005/Atom"');
    expect(isRssFeed || isAtomFeed).toBe(true);

    if (isRssFeed) {
      expect(content).toContain('<channel>');
    } else if (isAtomFeed) {
      expect(content).toContain('<feed');
    }

    // Verify feed has actual content (works for both RSS and Atom)
    // RSS feeds use <title>, Atom feeds use <title type="html">
    expect(content).toMatch(/<title[^>]*>/i);

    // Atom feeds use different tags than RSS
    if (isAtomFeed) {
      expect(content).toContain('<entry>');
      expect(content).toContain('<updated>');
    } else {
      expect(content).toContain('<description>');
    }

    // Test clicking the RSS link works
    await rssLink.click();

    // Should navigate to the RSS feed
    await expect(page).toHaveURL('/feed.xml');

    // Verify the RSS content is displayed (check for actual content since XML declarations are stripped from textContent)
    const pageContent = await page.textContent('body');
    expect(pageContent).toContain('Tech Hub'); // Feed title should be present
  });

  test('should have consistent Tech Hub title across all main pages', async ({ page }) => {
    const pagesToTest = [
      { url: '/', name: 'Homepage' },
      { url: '/ai/', name: 'AI Section' },
      { url: '/github-copilot/', name: 'GitHub Copilot Section' },
      { url: '/ai/news.html', name: 'AI News' },
      { url: '/ai/posts.html', name: 'AI Posts' },
      { url: '/github-copilot/news.html', name: 'Copilot News' },
      { url: '/github-copilot/posts.html', name: 'Copilot Posts' }
    ];

    for (const { url, name } of pagesToTest) {
      await navigateAndVerify(page, url, {
        expectTitle: true,
        checkErrors: true,
        checkPerformance: false, // Skip performance checks for this bulk test
        titlePattern: /Tech Hub/
      });

      // Verify the page actually loaded with content
      const bodyText = await page.textContent('body');
      expect(bodyText.trim().length).toBeGreaterThan(100);

      console.log(`✓ ${name} (${url}) has correct title: Tech Hub`);
    }
  });
});
