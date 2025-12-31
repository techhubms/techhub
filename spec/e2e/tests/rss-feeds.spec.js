const { test, expect } = require('@playwright/test');
const {
  TEST_CONFIG,
  navigateAndVerify
} = require('./helpers.js');

test.describe('RSS Feeds', () => {

  const rssFeeds = [
    { name: 'Everything', url: '/feed.xml', section: 'all' },
    { name: 'AI', url: '/ai/feed.xml', section: 'ai' },
    { name: 'GitHub Copilot', url: '/github-copilot/feed.xml', section: 'github-copilot' },
    { name: 'ML', url: '/ml/feed.xml', section: 'ml' },
    { name: 'Azure', url: '/azure/feed.xml', section: 'azure' },
    { name: 'Coding', url: '/coding/feed.xml', section: 'coding' },
    { name: 'DevOps', url: '/devops/feed.xml', section: 'devops' },
    { name: 'Security', url: '/security/feed.xml', section: 'security' },
    { name: 'Roundups', url: '/roundups/feed.xml', section: 'roundups' }
  ];

  for (const feed of rssFeeds) {
    test(`should have valid ${feed.name} RSS feed at ${feed.url}`, async ({ page }) => {
      // Navigate to the RSS feed URL
      const response = await page.goto(feed.url, {
        waitUntil: 'networkidle',
        timeout: TEST_CONFIG.pageLoadTimeout
      });

      // Verify the response is successful
      expect(response.status()).toBe(200);

      // Verify the content type is XML
      const contentType = response.headers()['content-type'];
      expect(contentType).toMatch(/xml/);

      // Get the raw response body (not page.content() which returns DOM)
      const content = await response.text();

      // Verify it's valid Atom XML format
      expect(content).toContain('<?xml version="1.0" encoding="utf-8"?>');
      expect(content).toContain('<feed xmlns="http://www.w3.org/2005/Atom">');
      expect(content).toContain('</feed>');

      // Verify required Atom feed elements
      expect(content).toContain('<title>');
      expect(content).toContain('<link');
      expect(content).toContain('<updated>');
      expect(content).toContain('<id>');
      expect(content).toContain('<author>');
      expect(content).toContain('<generator');

      // Verify the feed has entries (except possibly roundups which might be empty)
      if (feed.name !== 'Roundups') {
        expect(content).toContain('<entry>');
        expect(content).toContain('</entry>');
      }

      console.log(`✓ ${feed.name} RSS feed is valid and accessible at ${feed.url}`);
    });

    // Test RSS icon link on section pages (skip for roundups which doesn't have a section page)
    if (feed.section !== 'roundups') {
      test(`should have RSS icon link on ${feed.name} section page`, async ({ page }) => {
        // Navigate to the section page
        const sectionUrl = feed.section === 'all' ? '/all' : `/${feed.section}`;
        await navigateAndVerify(page, sectionUrl, {
          expectTitle: true,
          checkErrors: true
        });

        // Verify RSS icon link is present
        const rssLink = page.locator('a.rss-icon-link');
        await expect(rssLink).toBeVisible();

        // Verify the link points to the correct RSS feed
        const href = await rssLink.getAttribute('href');
        expect(href).toBe(feed.url);

        // Verify the link has proper accessibility attributes
        const title = await rssLink.getAttribute('title');
        expect(title).toBe('Subscribe to RSS feed');

        const ariaLabel = await rssLink.getAttribute('aria-label');
        expect(ariaLabel).toBe('Subscribe to RSS feed');

        // Verify RSS icon SVG is present
        const rssIcon = page.locator('a.rss-icon-link svg.rss-icon');
        await expect(rssIcon).toBeVisible();

        // Verify the icon has proper role and aria-label
        const svgRole = await rssIcon.getAttribute('role');
        expect(svgRole).toBe('img');

        const svgAriaLabel = await rssIcon.getAttribute('aria-label');
        expect(svgAriaLabel).toBe('RSS Feed');

        console.log(`✓ ${feed.name} section has RSS icon link`);
      });

      test(`should navigate to RSS feed when clicking icon on ${feed.name} section page`, async ({ page }) => {
        // Navigate to the section page
        const sectionUrl = feed.section === 'all' ? '/all' : `/${feed.section}`;
        await navigateAndVerify(page, sectionUrl, {
          expectTitle: true,
          checkErrors: true
        });

        // Click the RSS icon link
        const rssLink = page.locator('a.rss-icon-link');
        await expect(rssLink).toBeVisible();

        // Get the href to verify navigation
        const href = await rssLink.getAttribute('href');
        expect(href).toBe(feed.url);

        // Click and verify navigation
        const [response] = await Promise.all([
          page.waitForResponse(response => response.url().includes(feed.url)),
          rssLink.click()
        ]);

        // Verify the response is successful
        expect(response.status()).toBe(200);

        // Verify we're on the RSS feed page
        expect(page.url()).toContain(feed.url);

        console.log(`✓ RSS icon on ${feed.name} section navigates to feed`);
      });
    }
  }

  test('should have RSS feed links in footer', async ({ page }) => {
    // Navigate to the home page
    await navigateAndVerify(page, '/', {
      expectTitle: true,
      checkErrors: true
    });

    // Verify footer RSS link is present
    const footerRssLink = page.locator('footer a[href="/all/feed.xml"]');
    await expect(footerRssLink).toBeVisible();

    // Verify the link text
    const linkText = await footerRssLink.textContent();
    expect(linkText).toContain('RSS');

    console.log('✓ Footer has RSS feed link');
  });
});
