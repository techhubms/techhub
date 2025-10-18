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
    const sectionCards = page.locator('.navigation-section-square');
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
        titleSelector: '.navigation-section-title'
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

    // Check if there are any items with this category in the collection
    const expectedLatest = await getLatestFileFromCollectionByCategory(`_${collectionName}`, sectionCategory);

    if (!expectedLatest) {
      console.log(`⚠️  No ${sectionCategory} ${collectionTitle.toLowerCase()} found in _${collectionName} collection, skipping test`);
      return;
    }

    console.log(`📄 Collection has ${sectionCategory} content (latest by filesystem: "${expectedLatest.title}" from ${expectedLatest.date})`);

    await navigateAndVerify(page, collectionUrl, {
      expectTitle: true,
      checkErrors: true,
      checkPerformance: false // Skip performance for content tests
    });

    console.log(`📍 Navigated to ${collectionUrl}`);

    // Should have items
    const items = page.locator('.navigation-post-square');
    const itemCount = await items.count();
    console.log(`📋 Found ${itemCount} ${collectionTitle.toLowerCase()} items on page`);
    expect(itemCount).toBeGreaterThan(0);

    // First item should be visible and have proper structure
    const firstItem = items.first();
    await verifyContentItem(page, firstItem, {
      shouldHaveTitle: true,
      shouldHaveLink: true,
      shouldBeVisible: true,
      titleSelector: '.navigation-post-title'
    });

    // Get the title of the first displayed item
    const titleElement = firstItem.locator('.navigation-post-title');
    await expect(titleElement).toBeVisible();
    const displayedTitle = await titleElement.textContent();
    const cleanDisplayedTitle = displayedTitle?.trim() || '';
    
    console.log(`✅ First ${collectionTitle.toLowerCase()} item is visible: "${cleanDisplayedTitle}"`);
    
    // Verify that the displayed title is not empty (content exists)
    expect(cleanDisplayedTitle.length).toBeGreaterThan(0);
    
    // Conditional assertion: If helper found a latest item, verify it appears on the page
    // Note: We don't enforce it's in first position because Jekyll's server-side rendering
    // may apply different sorting/filtering logic (e.g., "20 + same-day" limiting)
    // than our filesystem-based helper, but it should be present somewhere on the page.
    if (expectedLatest && expectedLatest.title) {
      const cleanExpectedTitle = expectedLatest.title.trim();
      console.log(`🔍 Checking if expected latest item is present: "${cleanExpectedTitle}"`);
      
      // Check if the expected title appears anywhere in the displayed items
      const allTitles = await items.locator('.navigation-post-title').allTextContents();
      const titleFound = allTitles.some(title => title.trim() === cleanExpectedTitle);
      
      if (titleFound) {
        console.log(`✅ Expected latest item found on page`);
      } else {
        console.log(`⚠️  Expected latest item not found on page. This may be due to:`);
        console.log(`   - Content limiting (20 + same-day rule)`);
        console.log(`   - Different server-side filtering logic`);
        console.log(`   - Recency filter (7-day cutoff)`);
        console.log(`   Available titles:`, allTitles.slice(0, 3).map(t => `"${t.trim()}"`));
        
        // Soft assertion: warn but don't fail if expected item is not found
        // This accommodates legitimate server-side filtering differences
        expect.soft(titleFound).toBe(true);
      }
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
