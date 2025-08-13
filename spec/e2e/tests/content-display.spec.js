const { test, expect } = require('@playwright/test');
const {
  getExpectedLatestContent,
  getLatestFileFromCollection,
  getLatestFileFromCollectionByCategory,
  SECTIONS,
  navigateAndVerify,
  verifyContentItem
} = require('./helpers.js');

test.describe('Latest Content Display', () => {

  test('should display latest content on index page', async ({ page }) => {
    await navigateAndVerify(page, '/', {
      expectTitle: true,
      checkErrors: true,
      checkPerformance: true
    });

    // Check that there are section cards (the main navigation sections)
    const sectionCards = page.locator('.section-square');
    const sectionCardCount = await sectionCards.count();
    expect(sectionCardCount).toBeGreaterThan(0);

    // Check for roundups section instead of "Latest of everything" (which doesn't exist on homepage)
    const roundupsSection = page.locator('h2:text("Last 4 Roundups")');
    await expect(roundupsSection).toBeVisible();
    
    // Check that roundups are displayed
    const roundupItems = page.locator('.site-roundups li');
    const roundupCount = await roundupItems.count();
    console.log(`📋 Found ${roundupCount} roundup items on homepage`);
    // Roundups may be 0 if no roundups exist, which is okay

    // Check that section cards have proper structure
    for (let i = 0; i < sectionCardCount; i++) {
      const sectionCard = sectionCards.nth(i);

      await verifyContentItem(page, sectionCard, {
        shouldHaveTitle: true,
        shouldHaveLink: true,
        shouldBeVisible: true,
        titleSelector: '.section-title'
      });
    }

    // Roundups section check is already done above with "Last 4 Roundups"
  });

  /**
   * Test latest content display for a specific collection
   * @param {Page} page - Playwright page object
   * @param {string} collectionName - Name of the collection
   * @param {string} collectionTitle - Display title of the collection
   * @param {string} collectionUrl - URL of the collection page
   * @param {string} sectionCategory - Category to filter by
   */
  async function testLatestCollectionContent(page, collectionName, collectionTitle, collectionUrl, sectionCategory) {
    console.log(`\n🧪 Testing: Latest ${collectionTitle.toLowerCase()} from _${collectionName} collection (${sectionCategory} category)`);

    // Get the expected latest content from the file system filtered by category
    const expectedLatest = await getLatestFileFromCollectionByCategory(`_${collectionName}`, sectionCategory);

    if (!expectedLatest) {
      console.log(`⚠️  No ${sectionCategory} ${collectionTitle.toLowerCase()} found in _${collectionName} collection, skipping test`);
      return;
    }

    console.log(`📄 Expected latest ${sectionCategory} ${collectionTitle.toLowerCase()}: "${expectedLatest.title}" (${expectedLatest.date})`);
    console.log(`📂 Categories: ${expectedLatest.categories?.join(', ') || 'none'}`);

    await navigateAndVerify(page, collectionUrl, {
      expectTitle: true,
      checkErrors: true,
      checkPerformance: false // Skip performance for content tests
    });

    console.log(`📍 Navigated to ${collectionUrl}`);

    // Should have items
    const items = page.locator('.post-item');
    const itemCount = await items.count();
    console.log(`📋 Found ${itemCount} ${collectionTitle.toLowerCase()} items on page`);
    expect(itemCount).toBeGreaterThan(0);

    // First item should be the latest
    const firstItem = items.first();
    await verifyContentItem(page, firstItem, {
      shouldHaveTitle: true,
      shouldHaveLink: true,
      shouldBeVisible: true
    });

    console.log(`✅ First ${collectionTitle.toLowerCase()} item is visible`);

    // Get the title of the first item on the page using the proper selector
    const titleElement = firstItem.locator('.post-title');
    await expect(titleElement).toBeVisible();
    const displayedTitle = await titleElement.textContent();
    console.log(`🔍 Displayed title: "${displayedTitle?.trim()}"`);

    // Verify the displayed title exactly matches the expected title
    // Text should never be truncated on the website and all text should always be shown
    const cleanDisplayedTitle = displayedTitle?.trim() || '';
    const cleanExpectedTitle = expectedLatest.title.trim();

    if (cleanDisplayedTitle !== cleanExpectedTitle) {
      console.log('❌ Title mismatch:');
      console.log(`   Expected: "${cleanExpectedTitle}"`);
      console.log(`   Got: "${cleanDisplayedTitle}"`);
      expect(cleanDisplayedTitle).toBe(cleanExpectedTitle);
    } else {
      console.log(`✅ Title matches exactly: "${cleanExpectedTitle}"`);
    }
  }

  // Dynamic tests for all sections and collections based on sections.json
  Object.entries(SECTIONS).forEach(([sectionKey, sectionConfig]) => {
    const sectionTitle = sectionConfig.title;
    const sectionCategory = sectionConfig.category;

    test.describe(`${sectionTitle} Section Tests`, () => {

      // Test each collection in this section
      sectionConfig.collections.forEach((collection) => {
        // Skip custom collections as they don't correspond to Jekyll collections
        if (collection.custom) {
          return;
        }

        const collectionName = collection.collection;
        const collectionTitle = collection.title;
        const collectionUrl = collection.url;

        test(`should show latest ${collectionTitle.toLowerCase()} from _${collectionName} collection (${sectionCategory} category)`, async ({ page }) => {
          await testLatestCollectionContent(page, collectionName, collectionTitle, collectionUrl, sectionCategory);
        });
      });
    });
  });
});
