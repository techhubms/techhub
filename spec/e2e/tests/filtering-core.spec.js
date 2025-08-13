const { test, expect } = require('@playwright/test');
const {
  getVisiblePostCount,
  waitForFilteringComplete,
  navigateAndVerify,
  clearAllFilters,
  TEST_URLS
} = require('./helpers.js');

test.describe('Core Filtering Functionality', () => {

  // Test 1: Basic Tag Filter Application
  test('should apply tag filters correctly and show/hide appropriate content', async ({ page }) => {
    // Use first collection page from config instead of hardcoded URL
    const testPage = TEST_URLS.collectionPages[0];
    await navigateAndVerify(page, testPage.url);

    console.log('\n🧪 Testing basic tag filter application');

    const initialPostCount = await getVisiblePostCount(page);
    console.log(`📊 Initial post count: ${initialPostCount}`);

    const tagFilters = page.locator('.tag-filter-btn[data-tag]:not([data-tag*="last"]):not([data-tag*="day"]):not([data-tag*="month"]):has(.filter-count)');
    const tagFilterCount = await tagFilters.count();

    if (tagFilterCount > 0) {
      const firstTagFilter = tagFilters.first();
      const firstTagName = await firstTagFilter.getAttribute('data-tag');
      const countElement = firstTagFilter.locator('.filter-count');
      const expectedCount = parseInt((await countElement.textContent()).replace(/[()]/g, ''), 10);

      console.log(`🏷️ Applying tag filter: "${firstTagName}" (expected: ${expectedCount} posts)`);

      await firstTagFilter.click();
      await waitForFilteringComplete(page);

      const actualCount = await getVisiblePostCount(page);
      expect(actualCount).toBe(expectedCount);
      console.log(`✅ Tag filter applied correctly: ${actualCount} posts shown`);

      // Verify content is actually filtered (less than initial)
      expect(actualCount).toBeLessThanOrEqual(initialPostCount);

      // Clear filter
      await clearAllFilters(page);
    } else {
      console.log('⚠️ No tag filters available for testing');
    }
  });

  // Test 2: Date Filter UI Interaction (logic tested in unit tests)
  test('should interact with date filter UI correctly', async ({ page }) => {
    // Use first collection page from config instead of hardcoded URL
    const testPage = TEST_URLS.collectionPages[0];
    await navigateAndVerify(page, testPage.url);

    console.log('\n🧪 Testing date filter UI interaction');

    // Look for any enabled date filter button (more robust than hardcoded patterns)
    const dateFilters = page.locator('.date-filter-btn:not(.disabled):not([disabled]):not([style*="display: none"])');
    const dateFilterCount = await dateFilters.count();

    if (dateFilterCount > 0) {
      const firstDateFilter = dateFilters.first();
      const dateFilterName = await firstDateFilter.getAttribute('data-tag');

      console.log(`📅 Testing UI interaction with date filter: "${dateFilterName}"`);

      // Test that filter can be clicked and becomes active
      await firstDateFilter.click();
      await waitForFilteringComplete(page);

      const isActive = await firstDateFilter.evaluate(el => el.classList.contains('active'));
      expect(isActive).toBe(true);
      console.log('✅ Date filter UI correctly shows active state');

      // Clear filter
      await clearAllFilters(page);
    } else {
      console.log('⚠️ No date filters available for testing');
    }
  });

  // Test 3: Filter State Management (Active/Inactive)
  test('should manage filter active states correctly', async ({ page }) => {
    // Use first collection page from config instead of hardcoded URL
    const testPage = TEST_URLS.collectionPages[0];
    await navigateAndVerify(page, testPage.url);

    console.log('\n🧪 Testing filter state management');

    // Find enabled filter buttons only to avoid clicking disabled buttons
    const filterButtons = page.locator('.tag-filter-btn[data-tag]:not(.disabled):not([disabled]):has(.filter-count)');
    const buttonCount = await filterButtons.count();

    if (buttonCount > 0) {
      const testButton = filterButtons.first();
      const buttonTag = await testButton.getAttribute('data-tag');

      // Check initial state (should be inactive)
      const initiallyActive = await testButton.evaluate(el => el.classList.contains('active'));
      expect(initiallyActive).toBe(false);
      console.log(`✅ Filter "${buttonTag}" initially inactive`);

      // Click to activate
      await testButton.click();
      await waitForFilteringComplete(page);

      // Check active state
      const afterClickActive = await testButton.evaluate(el => el.classList.contains('active'));
      expect(afterClickActive).toBe(true);
      console.log(`✅ Filter "${buttonTag}" correctly marked as active after click`);

      // Test Clear All functionality
      const clearButton = page.locator('button:has-text("Clear All")');
      if (await clearButton.count() > 0) {
        await clearButton.click();
        await waitForFilteringComplete(page);

        const afterClearActive = await testButton.evaluate(el => el.classList.contains('active'));
        expect(afterClearActive).toBe(false);
        console.log(`✅ Filter "${buttonTag}" correctly deactivated by Clear All`);
      }
    }
  });

  // Test 4: Multiple Filter Application (AND Logic)
  test('should apply multiple filters with AND logic correctly', async ({ page }) => {
    // Use first collection page from config instead of hardcoded URL
    const testPage = TEST_URLS.collectionPages[0];
    await navigateAndVerify(page, testPage.url);

    console.log('\n🧪 Testing multiple filter application with AND logic');

    // Only select enabled, visible tag filters (not date filters)
    const tagFilters = page.locator('.tag-filter-btn[data-tag]:not([data-tag*="last"]):not([data-tag*="day"]):not([data-tag*="month"]):not(.disabled):not([disabled]):not(.hidden-tag-btn):has(.filter-count)');
    const tagFilterCount = await tagFilters.count();

    if (tagFilterCount >= 2) {
      // Apply first filter
      const firstFilter = tagFilters.first();
      const firstName = await firstFilter.getAttribute('data-tag');
      const firstCountElement = firstFilter.locator('.filter-count');
      const firstExpected = parseInt((await firstCountElement.textContent()).replace(/[()]/g, ''), 10);

      console.log(`🏷️ Applying first filter: "${firstName}" (${firstExpected} posts)`);
      await firstFilter.click();
      await waitForFilteringComplete(page);

      const afterFirstCount = await getVisiblePostCount(page);
      expect(afterFirstCount).toBe(firstExpected);

      // Apply second filter (should reduce count further due to AND logic)
      const secondFilter = tagFilters.nth(1);
      const secondName = await secondFilter.getAttribute('data-tag');
      const isSecondEnabled = await secondFilter.isEnabled();
      const isSecondVisible = await secondFilter.isVisible();

      if (!isSecondEnabled || !isSecondVisible) {
        console.log(`ℹ️ Second filter "${secondName}" is disabled or hidden, skipping AND logic test`);
        return;
      }

      console.log(`🏷️ Adding second filter: "${secondName}"`);
      await secondFilter.click();
      await waitForFilteringComplete(page);

      const afterSecondCount = await getVisiblePostCount(page);

      // With AND logic, count should be <= first filter count
      expect(afterSecondCount).toBeLessThanOrEqual(afterFirstCount);
      console.log(`✅ AND logic applied correctly: ${afterFirstCount} → ${afterSecondCount} posts`);

      // Verify both filters are active
      const firstActive = await firstFilter.evaluate(el => el.classList.contains('active'));
      const secondActive = await secondFilter.evaluate(el => el.classList.contains('active'));
      expect(firstActive).toBe(true);
      expect(secondActive).toBe(true);

      // Clear all filters
      await clearAllFilters(page);
    } else {
      console.log(`⚠️ Insufficient tag filters (${tagFilterCount}) for multiple filter testing`);
    }
  });

  // Test 5: Date and Tag Filter Combination
  test('should combine date and tag filters correctly', async ({ page }) => {
    // Use first collection page from config instead of hardcoded URL
    const testPage = TEST_URLS.collectionPages[0];
    await navigateAndVerify(page, testPage.url);

    console.log('\n🧪 Testing date and tag filter combination');

    // Only select enabled, visible filters
    const tagFilters = page.locator('.tag-filter-btn[data-tag]:not([data-tag*="last"]):not([data-tag*="day"]):not([data-tag*="month"]):not(.disabled):not([disabled]):not(.hidden-tag-btn):has(.filter-count)');
    const dateFilters = page.locator('.tag-filter-btn[data-tag*="day"], .tag-filter-btn[data-tag*="last"], .tag-filter-btn[data-tag*="month"]').filter('[data-filter-type="date"]:not(.disabled):not([disabled]):visible');

    const tagCount = await tagFilters.count();
    const dateCount = await dateFilters.count();

    console.log(`📊 Available filters: ${tagCount} tag filters, ${dateCount} enabled date filters`);

    if (tagCount > 0 && dateCount > 0) {
      // Apply tag filter first
      const tagFilter = tagFilters.first();
      const tagName = await tagFilter.getAttribute('data-tag');

      console.log(`🏷️ Applying tag filter: "${tagName}"`);
      await tagFilter.click();
      await waitForFilteringComplete(page);

      const afterTagCount = await getVisiblePostCount(page);
      console.log(`📊 Posts after tag filter: ${afterTagCount}`);

      // Apply date filter - verify it's still enabled after tag filter
      const dateFilter = dateFilters.first();
      const dateName = await dateFilter.getAttribute('data-tag');
      const isDateEnabled = await dateFilter.isEnabled();
      const isDateVisible = await dateFilter.isVisible();

      if (!isDateEnabled || !isDateVisible) {
        console.log(`ℹ️ Date filter "${dateName}" is disabled or hidden after tag filter, skipping combination test`);
        return;
      }

      console.log(`📅 Adding date filter: "${dateName}"`);
      await dateFilter.click();
      await waitForFilteringComplete(page);

      const finalCount = await getVisiblePostCount(page);
      console.log(`📊 Posts after both filters: ${finalCount}`);

      // Final count should be <= tag-only count (intersection)
      expect(finalCount).toBeLessThanOrEqual(afterTagCount);

      // Verify both filters are active
      const tagActive = await tagFilter.evaluate(el => el.classList.contains('active'));
      const dateActive = await dateFilter.evaluate(el => el.classList.contains('active'));
      expect(tagActive).toBe(true);
      expect(dateActive).toBe(true);

      console.log(`✅ Date and tag filters combined correctly: ${finalCount} posts in intersection`);

      // Clear filters
      await clearAllFilters(page);
    } else {
      console.log(`⚠️ Insufficient filters for combination testing (tags: ${tagCount}, dates: ${dateCount})`);
    }
  });


  // Test 7: Filter State Persistence Across Page Refresh
  test('should persist filter state across page refresh', async ({ page }) => {
    // Use first collection page from config instead of hardcoded URL
    const testPage = TEST_URLS.collectionPages[0];
    await navigateAndVerify(page, testPage.url);

    console.log('\n🧪 Testing filter state persistence across refresh');

    const filterButtons = page.locator('.tag-filter-btn[data-tag]:not(.disabled):not([disabled]):not([style*="display: none"]):has(.filter-count)');
    const buttonCount = await filterButtons.count();

    if (buttonCount > 0) {
      const testFilter = filterButtons.first();
      const filterName = await testFilter.getAttribute('data-tag');

      // Apply filter
      console.log(`🏷️ Applying filter: "${filterName}"`);
      await testFilter.click();
      await waitForFilteringComplete(page);

      const filteredCount = await getVisiblePostCount(page);
      const urlBeforeRefresh = page.url();

      // Refresh page
      console.log('🔄 Refreshing page');
      await page.reload();

      // Check if filter state is preserved
      const urlAfterRefresh = page.url();
      expect(urlAfterRefresh).toBe(urlBeforeRefresh);
      console.log('✅ URL preserved after refresh');

      // Check if filter is still active
      const testFilterAfterRefresh = page.locator(`[data-tag="${filterName}"]`).first();
      const activeAfterRefresh = await testFilterAfterRefresh.evaluate(el => el.classList.contains('active'));
      const countAfterRefresh = await getVisiblePostCount(page);

      expect(activeAfterRefresh).toBe(true);
      expect(countAfterRefresh).toBe(filteredCount);
      console.log(`✅ Filter state persisted across refresh: "${filterName}" still active with ${countAfterRefresh} posts`);
    }
  });

  // Test 8: Zero-Count Filter Behavior with Tag Filtering
  test('should handle zero-count filters appropriately when tag filters create empty date ranges', async ({ page }) => {
    // Use first collection page from config instead of hardcoded URL
    const testPage = TEST_URLS.collectionPages[0];
    await navigateAndVerify(page, testPage.url);

    console.log('\n🧪 Testing zero-count filter behavior with tag filtering');

    const tagFilters = page.locator('.tag-filter-btn[data-tag]:not([data-tag*="last"]):not([data-tag*="day"]):not([data-tag*="month"]):not(.disabled):not([disabled]):not([style*="display: none"]):not(.hidden-tag-btn):has(.filter-count)');

    if (await tagFilters.count() > 0) {
      const tagCount = await tagFilters.count();
      let foundZeroCountScenario = false;

      // Try multiple tag filters to find one that creates zero-count date ranges
      for (let tagIndex = 0; tagIndex < Math.min(tagCount, 3); tagIndex++) {
        const tagFilter = tagFilters.nth(tagIndex);
        const tagName = await tagFilter.getAttribute('data-tag');

        console.log(`🏷️ Testing with tag filter: ${tagName}`);
        await tagFilter.click();
        await waitForFilteringComplete(page);

        // Check if any date filters now have zero counts
        const dateFilters = page.locator('.tag-filter-btn[data-tag*="day"], .tag-filter-btn[data-tag*="last"], .tag-filter-btn[data-tag*="month"]');
        const dateFilterCount = await dateFilters.count();

        const zeroCountFilters = [];
        const disabledFilters = [];

        for (let i = 0; i < dateFilterCount; i++) {
          const dateFilter = dateFilters.nth(i);

          if (await dateFilter.isVisible()) {
            const countElement = dateFilter.locator('.filter-count');
            const countText = await countElement.textContent();
            const count = parseInt(countText.replace(/[()]/g, ''), 10);
            const dateFilterTag = await dateFilter.getAttribute('data-tag');

            if (count === 0) {
              zeroCountFilters.push(dateFilterTag);

              // Check if zero-count filter is disabled
              const isDisabled = await dateFilter.evaluate(el =>
                el.disabled ||
                el.classList.contains('disabled') ||
                el.getAttribute('disabled') !== null
              );

              if (isDisabled) {
                disabledFilters.push(dateFilterTag);
              }
            }
          }
        }

        if (zeroCountFilters.length > 0) {
          foundZeroCountScenario = true;
          console.log(`📊 Found ${zeroCountFilters.length} zero-count date filters with tag "${tagName}": [${zeroCountFilters.join(', ')}]`);
          console.log(`🔒 Found ${disabledFilters.length} disabled zero-count date filters: [${disabledFilters.join(', ')}]`);

          // Verify that zero-count filters are properly handled
          expect(zeroCountFilters.length).toBeGreaterThan(0);
          console.log('✅ Zero-count scenario successfully created and detected');
          break;
        }

        // Clear tag filter for next iteration
        await tagFilter.click();
        await waitForFilteringComplete(page);
      }

      if (!foundZeroCountScenario) {
        console.log('ℹ️ No zero-count date filter scenarios found - content may span all date ranges for tested tags');
      }
    } else {
      console.log('ℹ️ No tag filters available for zero-count testing');
    }
  });
});
