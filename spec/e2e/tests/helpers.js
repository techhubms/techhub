// Basic test helper functions and utilities
const { expect } = require('@playwright/test');
const fs = require('fs');
const path = require('path');

const TEST_CONFIG = {
  baseURL: 'http://localhost:4000',
  maxLoadTime: 3500, // 3500ms load time requirement (relaxed for devcontainer environment)
  mobileBreakpoint: 768,
  tabletBreakpoint: 1024
};

/**
 * Get the project root directory by searching for repository indicators
 * @returns {string} - The absolute path to project root
 */
function getProjectRoot() {
  // Check if we're in a GitHub Actions environment
  if (process.env.GITHUB_WORKSPACE) {
    return process.env.GITHUB_WORKSPACE;
  }

  // Otherwise, search upward from current directory for repository indicators
  let currentPath = __dirname;

  while (currentPath && currentPath !== path.parse(currentPath).root) {
    // Check for .git directory first (primary indicator)
    if (fs.existsSync(path.join(currentPath, '.git'))) {
      return currentPath;
    }

    // Check for _config.yml as secondary indicator
    if (fs.existsSync(path.join(currentPath, '_config.yml'))) {
      return currentPath;
    }

    // Move up one directory
    currentPath = path.dirname(currentPath);
  }

  // Fallback: use a reasonable default relative to the helpers.js file location
  // This file is in spec/e2e/tests/, so project root is 3 levels up
  return path.resolve(__dirname, '..', '..', '..');
}

/**
 * Load sections configuration from _data/sections.json
 * @returns {Object} - The sections configuration
 */
function loadSectionsConfig() {
  try {
    const projectRoot = getProjectRoot();
    const sectionsPath = path.join(projectRoot, '_data', 'sections.json');

    console.log(`Loading sections.json from: ${sectionsPath}`);

    if (!fs.existsSync(sectionsPath)) {
      console.warn(`sections.json not found at ${sectionsPath}`);
      return {};
    }

    const sectionsContent = fs.readFileSync(sectionsPath, 'utf8');
    return JSON.parse(sectionsContent);
  } catch (error) {
    console.error('Failed to load sections.json:', error.message);
    console.error('Stack trace:', error.stack);
    return {};
  }
}

// Load sections dynamically from the data file
const SECTIONS = loadSectionsConfig();

/**
 * Get the latest file from a Jekyll collection directory
 * Applies 7-day recency filter to match server-side rendering logic
 * @param {string} collectionPath - Path to the collection directory (e.g., '_posts', '_news')
 * @returns {Object} - Object with filename, date, and parsed front matter
 */
async function getLatestFileFromCollection(collectionPath) {
  const projectRoot = getProjectRoot();
  const fullPath = path.join(projectRoot, collectionPath);

  try {
    if (!fs.existsSync(fullPath)) {
      console.warn(`Collection directory not found: ${fullPath}`);
      return null;
    }

    const files = fs.readdirSync(fullPath);
    const markdownFiles = files.filter(file => file.endsWith('.md'));

    if (markdownFiles.length === 0) {
      console.warn(`No markdown files found in: ${fullPath}`);
      return null;
    }

    // Calculate 7-day cutoff date and current date (matching server-side logic)
    const now = new Date();
    now.setHours(0, 0, 0, 0); // Normalize to midnight for comparison
    const cutoffDate = new Date(now.getTime() - (7 * 24 * 60 * 60 * 1000));
    cutoffDate.setHours(0, 0, 0, 0); // Normalize to midnight

    // Parse all files to get dates and filter by recency
    const recentFiles = [];

    for (const file of markdownFiles) {
      const filePath = path.join(fullPath, file);
      const content = fs.readFileSync(filePath, 'utf8');

      // Parse front matter
      const frontMatterMatch = content.match(/^---\r?\n([\s\S]*?)\r?\n---/);
      const frontMatter = {};

      if (frontMatterMatch) {
        const frontMatterText = frontMatterMatch[1];

        // Parse title - handle both quoted and unquoted titles properly
        let titleMatch = frontMatterText.match(/^title:\s*"([^"]*)"$/m);
        if (!titleMatch) {
          titleMatch = frontMatterText.match(/^title:\s*'([^']*)'$/m);
        }
        if (!titleMatch) {
          titleMatch = frontMatterText.match(/^title:\s*(.+?)$/m);
        }
        if (titleMatch) {
          frontMatter.title = titleMatch[1].trim();
        }

        // Parse date from front matter
        const dateMatch = frontMatterText.match(/^date:\s*(.+?)$/m);
        if (dateMatch) {
          frontMatter.date = new Date(dateMatch[1].trim());
        } else {
          // Fallback to filename date
          frontMatter.date = new Date(file.substring(0, 10));
        }

        // Parse tags if they exist
        const tagsMatch = frontMatterText.match(/^tags:\s*\[(.*)\]$/m);
        if (tagsMatch) {
          frontMatter.tags = tagsMatch[1].split(',').map(tag => tag.trim().replace(/["']/g, ''));
        }
      } else {
        // No front matter, use filename date
        frontMatter.date = new Date(file.substring(0, 10));
      }

      // Normalize date to midnight for accurate comparison (matching server-side logic)
      const normalizedDate = new Date(frontMatter.date);
      normalizedDate.setHours(0, 0, 0, 0);

      // Apply 7-day recency filter and exclude future dates (matching server-side logic)
      if (normalizedDate >= cutoffDate && normalizedDate <= now) {
        // Generate title from filename if not in front matter
        if (!frontMatter.title) {
          frontMatter.title = file
            .substring(11) // Remove date prefix
            .replace('.md', '')
            .replace(/-/g, ' ')
            .replace(/\b\w/g, l => l.toUpperCase());
        }

        frontMatter.filename = file;
        frontMatter.path = filePath;
        recentFiles.push(frontMatter);
      }
    }

    // Sort by actual date (newest first) and return the latest
    recentFiles.sort((a, b) => b.date - a.date);

    if (recentFiles.length > 0) {
      const latest = recentFiles[0];
      return {
        filename: latest.filename,
        date: latest.date.toISOString().split('T')[0], // Format as YYYY-MM-DD
        title: latest.title,
        tags: latest.tags || [],
        path: latest.path
      };
    }

    console.log(`No recent files (within 7 days) found in: ${fullPath}`);
    return null;
  } catch (error) {
    console.warn(`Could not read collection ${collectionPath}:`, error.message);
    return null;
  }
}

/**
 * Get the latest file from a Jekyll collection directory filtered by category
 * @param {string} collectionPath - Path to the collection directory (e.g., '_posts', '_news')
 * @param {string} category - Category to filter by (e.g., 'AI', 'GitHub Copilot')
 * @returns {Object} - Object with filename, date, and parsed front matter
 */
async function getLatestFileFromCollectionByCategory(collectionPath, category) {
  const projectRoot = getProjectRoot();
  const fullPath = path.join(projectRoot, collectionPath);

  console.log(`Searching for ${category} files in: ${fullPath}`);

  try {
    if (!fs.existsSync(fullPath)) {
      console.warn(`Collection directory not found: ${fullPath}`);
      return null;
    }

    const files = fs.readdirSync(fullPath);
    const markdownFiles = files.filter(file => file.endsWith('.md'));

    if (markdownFiles.length === 0) {
      console.warn(`No markdown files found in: ${fullPath}`);
      return null;
    }

    // Sort files by date (filename format: YYYY-MM-DD-title.md)
    const sortedFiles = markdownFiles.sort((a, b) => {
      const dateA = a.substring(0, 10); // Extract YYYY-MM-DD
      const dateB = b.substring(0, 10);
      return dateB.localeCompare(dateA); // Newest first
    });

    // Calculate current date for future date filtering (matching server-side logic)
    const now = new Date();
    now.setHours(0, 0, 0, 0); // Normalize to midnight for comparison
    const cutoffDate = new Date(now.getTime() - (7 * 24 * 60 * 60 * 1000));
    cutoffDate.setHours(0, 0, 0, 0); // Normalize to midnight

    // Parse all files to get actual dates and filter by category
    const categoryFiles = [];

    for (const file of sortedFiles) {
      const filePath = path.join(fullPath, file);
      const content = fs.readFileSync(filePath, 'utf8');

      // Parse front matter
      const frontMatterMatch = content.match(/^---\r?\n([\s\S]*?)\r?\n---/);
      const frontMatter = {};

      if (frontMatterMatch) {
        const frontMatterText = frontMatterMatch[1];

        // Parse title - handle both quoted and unquoted titles properly
        let titleMatch = frontMatterText.match(/^title:\s*"([^"]*)"/m);
        if (!titleMatch) {
          titleMatch = frontMatterText.match(/^title:\s*'([^']*)'/m);
        }
        if (!titleMatch) {
          titleMatch = frontMatterText.match(/^title:\s*(.+?)$/m);
        }
        if (titleMatch) {
          frontMatter.title = titleMatch[1].trim();
        }

        // Parse date from front matter
        const dateMatch = frontMatterText.match(/^date:\s*(.+?)$/m);
        if (dateMatch) {
          frontMatter.date = new Date(dateMatch[1].trim());
        } else {
          // Fallback to filename date
          frontMatter.date = new Date(file.substring(0, 10));
        }

        // Parse categories
        const categoriesMatch = frontMatterText.match(/^categories:\s*\[(.*?)\]/m);
        if (categoriesMatch) {
          frontMatter.categories = categoriesMatch[1].split(',').map(cat => cat.trim().replace(/["']/g, ''));
        }

        // Parse tags if they exist
        const tagsMatch = frontMatterText.match(/^tags:\s*\[(.*?)\]/m);
        if (tagsMatch) {
          frontMatter.tags = tagsMatch[1].split(',').map(tag => tag.trim().replace(/["']/g, ''));
        }
      }

      // Normalize date to midnight for accurate comparison (matching server-side logic)
      const normalizedDate = new Date(frontMatter.date);
      normalizedDate.setHours(0, 0, 0, 0);

      // Check if this file matches the category and is within date range (7 days back, no future dates)
      if (frontMatter.categories && frontMatter.categories.includes(category) &&
          normalizedDate >= cutoffDate && normalizedDate <= now) {
        // Generate title from filename if not in front matter
        if (!frontMatter.title) {
          frontMatter.title = file
            .substring(11) // Remove date prefix
            .replace('.md', '')
            .replace(/-/g, ' ')
            .replace(/\b\w/g, l => l.toUpperCase());
        }

        frontMatter.filename = file;
        frontMatter.path = filePath;
        categoryFiles.push(frontMatter);
      }
    }

    // Sort by actual date (newest first) and return the latest
    categoryFiles.sort((a, b) => b.date - a.date);

    if (categoryFiles.length > 0) {
      const latest = categoryFiles[0];
      return {
        filename: latest.filename,
        date: latest.date.toISOString().split('T')[0], // Format as YYYY-MM-DD
        title: latest.title,
        categories: latest.categories || [],
        tags: latest.tags || [],
        path: latest.path
      };
    }

    console.warn(`No files found in ${collectionPath} with category "${category}"`);
    return null;
  } catch (error) {
    console.warn(`Could not read collection ${collectionPath}:`, error.message);
    return null;
  }
}

/**
 * Get expected latest content for each collection
 * Uses 7-day recency filter to match server-side rendering logic
 * @returns {Object} - Object with collection names as keys and latest file info as values
 */
async function getExpectedLatestContent() {
  const collections = {
    posts: await getLatestFileFromCollection('_posts'),
    news: await getLatestFileFromCollection('_news'),
    community: await getLatestFileFromCollection('_community'),
    videos: await getLatestFileFromCollection('_videos'),
    events: await getLatestFileFromCollection('_events'),
    roundups: await getLatestFileFromCollection('_roundups')
  };

  // Log the expected content for debugging
  console.log('📂 Expected Latest Content (with 7-day recency filter):');
  Object.entries(collections).forEach(([collection, info]) => {
    if (info) {
      console.log(`  ${collection}: ${info.title} (${info.date})`);
    } else {
      console.log(`  ${collection}: No recent content found (within 7 days)`);
    }
  });

  return collections;
}

// Helper functions for tests
async function checkPageLoadTime(page, url) {
  const startTime = Date.now();
  await page.goto(url);
  const loadTime = Date.now() - startTime;

  return {
    loadTime,
    passesRequirement: loadTime <= TEST_CONFIG.maxLoadTime
  };
}

async function checkForJavaScriptErrors(page) {
  const errors = [];
  page.on('pageerror', error => {
    errors.push(error.message);
  });

  return errors;
}

async function checkFor404Errors(page) {
  const failedRequests = [];
  page.on('response', response => {
    if (response.status() === 404) {
      failedRequests.push(response.url());
    }
  });

  return failedRequests;
}

async function checkNoHorizontalScrollbar(page) {
  const hasHorizontalScrollbar = await page.evaluate(() => {
    return document.documentElement.scrollWidth > document.documentElement.clientWidth;
  });

  return !hasHorizontalScrollbar;
}

async function getFilterCounts(page) {
  return await page.evaluate(() => {
    const counts = {};
    const buttons = document.querySelectorAll('.tag-filter-btn[data-tag]');
    buttons.forEach(btn => {
      const tag = btn.getAttribute('data-tag');
      const countElement = btn.querySelector('.filter-count');
      if (countElement) {
        const countText = countElement.textContent.trim();
        const count = parseInt(countText.replace(/[()]/g, ''), 10);
        if (!isNaN(count)) {
          counts[tag] = count;
        }
      }
    });
    return counts;
  });
}

async function getVisiblePostCount(page) {
  return await page.evaluate(() => {
    const posts = document.querySelectorAll('.navigation-post-square');
    let visibleCount = 0;
    posts.forEach(post => {
      const style = window.getComputedStyle(post);
      if (style.display !== 'none') {
        visibleCount++;
      }
    });
    return visibleCount;
  });
}

async function getFilterButtonCount(page, filterSelector) {
  return await page.evaluate((selector) => {
    const button = document.querySelector(selector);
    if (!button) return 0;

    const countElement = button.querySelector('.filter-count');
    if (!countElement) return 0;

    const countText = countElement.textContent.trim();
    const count = parseInt(countText.replace(/[()]/g, ''), 10);
    return isNaN(count) ? 0 : count;
  }, filterSelector);
}

async function waitForFilteringComplete(page, _timeoutMs = 1000) {
  // Since filtering is very fast (under 100ms), use a much shorter timeout
  try {
    // For E2E tests, wait a fixed short time since filtering is synchronous
    await page.waitForTimeout(150); // Just 150ms - plenty for filtering
  } catch (error) {
    console.warn(`⚠️ waitForFilteringComplete timeout: ${error.message}`);
    // Don't throw - filtering is fast, likely already complete
  }
}

/**
 * Setup error tracking for JavaScript errors and 404 responses
 * @param {Page} page - Playwright page object
 * @returns {Object} - Object with arrays to collect errors and method to get them
 */
function setupErrorTracking(page) {
  const jsErrors = [];
  const failed404Requests = [];

  page.on('pageerror', error => {
    jsErrors.push(error.message);
  });

  page.on('response', response => {
    if (response.status() === 404) {
      failed404Requests.push(response.url());
    }
  });

  return {
    jsErrors,
    failed404Requests,
    getErrors: () => ({ jsErrors, failed404Requests }),
    hasErrors: () => jsErrors.length > 0 || failed404Requests.length > 0
  };
}

/**
 * Navigate to a page and verify basic loading requirements
 * @param {Page} page - Playwright page object
 * @param {string} url - URL to navigate to
 * @param {Object} options - Options for verification
 * @returns {Object} - Navigation result with timing and error info
 */
async function navigateAndVerify(page, url, options = {}) {
  const {
    expectTitle = true,
    checkErrors = true,
    checkPerformance = true,
    titlePattern = /Tech Hub/,
    maxLoadTime = TEST_CONFIG.maxLoadTime
  } = options;

  const errorTracker = checkErrors ? setupErrorTracking(page) : null;
  const startTime = Date.now();

  await page.goto(url);
  const loadTime = Date.now() - startTime;

  // Verify navigation worked (handle trailing slash variations flexibly)
  const currentUrl = await page.url();
  const normalizedExpected = url.replace(/\/$/, ''); // Remove trailing slash
  const normalizedCurrent = currentUrl.replace(/https?:\/\/[^\/]+/, '').replace(/\/$/, ''); // Remove domain and trailing slash

  if (normalizedCurrent !== normalizedExpected) {
    throw new Error(`Navigation failed. Expected: ${normalizedExpected}, Got: ${normalizedCurrent}`);
  }

  if (expectTitle) {
    await expect(page).toHaveTitle(titlePattern);
  }

  if (checkPerformance) {
    expect(loadTime).toBeLessThan(maxLoadTime);
  }

  if (checkErrors && errorTracker) {
    expect(errorTracker.jsErrors).toHaveLength(0);
    expect(errorTracker.failed404Requests).toHaveLength(0);
  }

  return {
    loadTime,
    errors: errorTracker ? errorTracker.getErrors() : null,
    passesRequirement: loadTime <= maxLoadTime
  };
}

/**
 * Set viewport to common mobile breakpoint
 * @param {Page} page - Playwright page object
 * @param {string} size - Preset size name or 'custom'
 * @param {Object} customSize - Custom size object for 'custom' preset
 */
async function setViewportSize(page, size = 'mobile', customSize = null) {
  const presets = {
    mobile: { width: 375, height: 667 },
    tablet: { width: 768, height: 1024 },
    desktop: { width: 1200, height: 800 },
    large: { width: 1920, height: 1080 }
  };

  const targetSize = size === 'custom' && customSize ? customSize : presets[size];
  if (!targetSize) {
    throw new Error(`Unknown viewport preset: ${size}`);
  }

  await page.setViewportSize(targetSize);
}

/**
 * Find and interact with a filter button, handling various selection strategies
 * @param {Page} page - Playwright page object
 * @param {Object} criteria - Filter selection criteria
 * @returns {Object} - Selected filter info and interaction methods
 */
async function findAndSelectFilter(page, criteria = {}) {
  const {
    filterType = 'tag', // 'tag', 'date', 'content'
    excludeText = [], // Text patterns to exclude (e.g., ['Last', 'day'])
    mustHaveCount = true,
    maxCount = null, // Maximum count to consider
    minCount = 1
  } = criteria;

  let selector = '.tag-filter-btn[data-tag]';

  if (filterType === 'date') {
    selector += '[data-tag*="day"]';
  } else if (filterType === 'content') {
    selector += ':not([data-tag*="last"]):not([data-tag*="day"])';
  }

  if (mustHaveCount) {
    selector += ':has(.filter-count)';
  }

  const filterButtons = page.locator(selector);
  const buttonCount = await filterButtons.count();

  let selectedFilter = null;
  let selectedCount = 0;
  let selectedTag = '';

  for (let i = 0; i < buttonCount; i++) {
    const button = filterButtons.nth(i);
    const tagName = await button.getAttribute('data-tag');
    const buttonText = await button.textContent();

    // Check exclusion criteria
    if (excludeText.some(pattern => buttonText.toLowerCase().includes(pattern.toLowerCase()))) {
      continue;
    }

    let count = 0;
    if (mustHaveCount) {
      const countElement = button.locator('.filter-count');
      const countText = await countElement.textContent();
      count = parseInt(countText.replace(/[()]/g, ''), 10);

      if (isNaN(count) || count < minCount) continue;
      if (maxCount && count > maxCount) continue;
    }

    selectedFilter = button;
    selectedCount = count;
    selectedTag = tagName;
    break;
  }

  return {
    button: selectedFilter,
    count: selectedCount,
    tag: selectedTag,
    click: async () => {
      if (selectedFilter) {
        await selectedFilter.click();
        await waitForFilteringComplete(page);
      }
    },
    isActive: async () => {
      if (selectedFilter) {
        return await selectedFilter.evaluate(el => el.classList.contains('active'));
      }
      return false;
    }
  };
}

/**
 * Test filter application and removal with comprehensive verification
 * @param {Page} page - Playwright page object
 * @param {Object} filterInfo - Filter information from findAndSelectFilter
 * @param {Object} options - Test options
 */
async function testFilterToggle(page, filterInfo, options = {}) {
  const {
    verifyUrl = true,
    verifyCount = true,
    verifyContent = true,
    expectedCount = filterInfo.count
  } = options;

  if (!filterInfo.button) {
    throw new Error('No filter button available for testing');
  }

  // Get initial state
  const initialVisiblePosts = await getVisiblePostCount(page);
  const initialPosts = verifyContent ? await page.locator('[data-post-title]').allTextContents() : [];

  // Apply filter
  await filterInfo.click();

  // Verify filter is active
  expect(await filterInfo.isActive()).toBe(true);

  // Verify URL contains filter parameter
  if (verifyUrl) {
    const filteredUrl = page.url();
    expect(filteredUrl).toContain('filters=');
    // Accept both URL encoding (%20) and form encoding (+) for spaces
    const urlEncodedTag = encodeURIComponent(filterInfo.tag);
    const formEncodedTag = filterInfo.tag.replace(/ /g, '+');
    expect(filteredUrl.includes(urlEncodedTag) || filteredUrl.includes(formEncodedTag)).toBe(true);
  }

  // Verify count changed
  if (verifyCount) {
    const filteredVisiblePosts = await getVisiblePostCount(page);
    expect(filteredVisiblePosts).toBe(expectedCount);
    expect(filteredVisiblePosts).toBeLessThan(initialVisiblePosts);
  }

  // Verify content actually changed
  if (verifyContent) {
    const filteredPosts = await page.locator('[data-post-title]').allTextContents();
    expect(filteredPosts.length).toBe(expectedCount);
    expect(filteredPosts.length).toBeLessThan(initialPosts.length);
  }

  // Clear filters and verify restoration
  await page.click('button:has-text("Clear All")');
  await waitForFilteringComplete(page, 500);

  // Verify filter is no longer active
  expect(await filterInfo.isActive()).toBe(false);

  // Verify restoration
  if (verifyCount) {
    const finalVisiblePosts = await getVisiblePostCount(page);
    expect(finalVisiblePosts).toBe(initialVisiblePosts);
  }

  if (verifyUrl) {
    const finalUrl = page.url();
    expect(finalUrl).not.toContain('filters=');
  }

  if (verifyContent) {
    const restoredPosts = await page.locator('[data-post-title]').allTextContents();
    expect(restoredPosts.length).toBe(initialPosts.length);
  }
}

/**
 * Verify navigation link properties and functionality
 * @param {Page} page - Playwright page object
 * @param {string} linkSelector - CSS selector for the link
 * @param {string} expectedHref - Expected href attribute content
 * @param {Object} options - Verification options
 */
async function verifyNavigationLink(page, linkSelector, expectedHref, options = {}) {
  const {
    shouldBeVisible = true,
    shouldBeEnabled = true,
    clickAndVerify = false,
    expectedUrl = expectedHref
  } = options;

  const link = page.locator(linkSelector).first();

  if (shouldBeVisible) {
    await expect(link).toBeVisible();
  }

  if (shouldBeEnabled) {
    await expect(link).toBeEnabled();
  }

  const href = await link.getAttribute('href');
  expect(href).toContain(expectedHref);

  if (clickAndVerify) {
    await link.click();

    // Verify navigation occurred
    await expect(page).toHaveURL(new RegExp(expectedUrl.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')));

    // Verify page has content
    const pageContent = await page.locator('main').textContent();
    expect(pageContent.trim().length).toBeGreaterThan(50);
  }
}

/**
 * Generate tests for multiple pages with the same test logic
 * @param {Array} pages - Array of page objects with url and name properties
 * @param {Function} testFn - Test function to run for each page
 * @param {string} testName - Base name for the test
 * @param {Object} testContext - Test context object containing 'test' function
 */
function generateTestsForMultiplePages(pages, testFn, testName, testContext) {
  pages.forEach(({ url, name }) => {
    testContext.test(`${testName} - ${name}`, async ({ page }) => {
      await testFn(page, url, name);
    });
  });
}

/**
 * Run the same test function on multiple pages (non-test generating version)
 * @param {Array} pages - Array of page objects with url and name properties
 * @param {Function} testFn - Test function to run for each page
 * @param {Page} page - Playwright page object
 */
async function runTestOnMultiplePages(pages, testFn, page) {
  for (const { url, name } of pages) {
    await testFn(page, url, name);
  }
}

/**
 * Verify content item structure and properties
 * @param {Page} page - Playwright page object
 * @param {Locator} item - Content item locator
 * @param {Object} options - Verification options
 */
async function verifyContentItem(page, item, options = {}) {
  const {
    shouldHaveTitle = true,
    shouldHaveLink = true,
    shouldBeVisible = true,
    titleSelector = 'a, h1, h2, h3'
  } = options;

  if (shouldBeVisible) {
    await expect(item).toBeVisible();
  }

  if (shouldHaveTitle) {
    const titleElement = item.locator(titleSelector).first();
    await expect(titleElement).toBeVisible();

    const titleText = await titleElement.textContent();
    expect(titleText?.trim()).toBeTruthy();
  }

  if (shouldHaveLink) {
    const link = item.locator('a').first();
    if (await link.count() > 0) {
      await expect(link).toBeEnabled();
      const href = await link.getAttribute('href');
      expect(href).toBeTruthy();
    }
  }
}

/**
 * Test responsive layout basics for a page at a specific viewport
 * @param {Page} page - Playwright page object
 * @param {string} url - URL to test
 * @param {Object} viewport - Viewport dimensions {width, height}
 * @param {string} viewportName - Name for logging
 */
async function testResponsiveLayout(page, url, viewport, _viewportName) {
  await page.setViewportSize(viewport);
  await page.goto(url);

  // Verify no horizontal scrollbar exists
  const noHorizontalScrollbar = await checkNoHorizontalScrollbar(page);
  expect(noHorizontalScrollbar).toBe(true);

  // Verify basic responsive layout elements are visible and functional
  await expect(page.locator('header[role="banner"]')).toBeVisible();
  await expect(page.locator('main')).toBeVisible();
  await expect(page.locator('footer')).toBeVisible();

  // Verify content adapts to viewport
  await verifyContentFitsViewport(page, viewport);

  // Verify touch targets on mobile/tablet
  if (viewport.width <= 768) {
    await verifyTouchTargets(page);
  }
}

/**
 * Verify content elements fit within the viewport
 * @param {Page} page - Playwright page object
 * @param {Object} viewport - Viewport dimensions
 */
async function verifyContentFitsViewport(page, viewport) {
  // Verify logo scales appropriately
  const logo = page.locator('img[alt*="Logo"]').first();
  if (await logo.count() > 0) {
    await expect(logo).toBeVisible();
    const logoBox = await logo.boundingBox();
    if (logoBox) {
      expect(logoBox.width).toBeLessThanOrEqual(viewport.width);
    }
  }

  // Verify content area adapts to viewport
  const mainElement = page.locator('main');
  const mainBox = await mainElement.boundingBox();
  if (mainBox) {
    expect(mainBox.width).toBeLessThanOrEqual(viewport.width);
  }

  // Verify images don't overflow
  const images = page.locator('img');
  const imageCount = await images.count();

  for (let i = 0; i < Math.min(imageCount, 5); i++) {
    const img = images.nth(i);
    if (await img.isVisible()) {
      const boundingBox = await img.boundingBox();
      if (boundingBox) {
        expect(boundingBox.width).toBeLessThanOrEqual(viewport.width);
      }
    }
  }
}

/**
 * Verify touch targets are appropriately sized for mobile/tablet
 * @param {Page} page - Playwright page object
 */
async function verifyTouchTargets(page) {
  const clickableElements = page.locator('button, a, .clickable');
  const elementCount = await clickableElements.count();

  for (let i = 0; i < Math.min(elementCount, 3); i++) {
    const element = clickableElements.nth(i);
    if (await element.isVisible()) {
      const elementBox = await element.boundingBox();
      if (elementBox) {
        // Touch targets should be at least 32px high (minimum accessibility standard)
        expect(elementBox.height).toBeGreaterThanOrEqual(32);
      }
    }
  }
}

/**
 * Test hamburger menu functionality at mobile viewport
 * @param {Page} page - Playwright page object
 * @param {string} url - URL to test on
 */
async function testHamburgerMenu(page, url) {
  await page.setViewportSize({ width: 375, height: 667 });
  await page.goto(url);

  // Find hamburger menu
  const hamburgerMenu = page.locator('img[alt="Menu"]').or(
    page.locator('button[aria-label*="menu"]')
  ).or(
    page.locator('.menu-toggle')
  ).or(
    page.locator('[data-testid="hamburger"]')
  );

  if (await hamburgerMenu.count() > 0) {
    // Hamburger should be visible and clickable on mobile
    await expect(hamburgerMenu.first()).toBeVisible();
    await expect(hamburgerMenu.first()).toBeEnabled();

    // Verify touch target size
    const hamburgerBox = await hamburgerMenu.first().boundingBox();
    if (hamburgerBox) {
      expect(hamburgerBox.height).toBeGreaterThanOrEqual(32);
      expect(hamburgerBox.width).toBeGreaterThanOrEqual(32);
    }

    // Test menu toggle
    await hamburgerMenu.first().click();
    await page.waitForTimeout(300); // Wait for animation

    // Verify navigation appears or is accessible
    const navLinks = page.locator('a[href*="/ai"]').or(
      page.locator('a[href*="/github-copilot"]')
    );
    const linkCount = await navLinks.count();
    expect(linkCount).toBeGreaterThan(0);
  }
}

/**
 * Test desktop navigation visibility and functionality
 * @param {Page} page - Playwright page object
 * @param {string} url - URL to test on
 */
async function testDesktopNavigation(page, url) {
  await page.setViewportSize({ width: 1200, height: 800 });
  await page.goto(url);

  // Regular navigation should be visible and functional
  const navigation = page.locator('nav').or(page.locator('.navigation'));
  await expect(navigation.first()).toBeVisible();

  // Verify navigation links
  const navLinks = page.locator('nav a, .navigation a');
  const linkCount = await navLinks.count();
  if (linkCount > 0) {
    for (let i = 0; i < Math.min(linkCount, 3); i++) {
      const link = navLinks.nth(i);
      await expect(link).toBeVisible();
      await expect(link).toBeEnabled();

      const href = await link.getAttribute('href');
      expect(href).toBeTruthy();
    }
  }

  // Hamburger menu should be hidden on desktop
  const hamburgerMenu = page.locator('img[alt="Menu"]').or(
    page.locator('button[aria-label*="menu"]')
  ).or(
    page.locator('.menu-toggle')
  );

  if (await hamburgerMenu.count() > 0) {
    await expect(hamburgerMenu.first()).toBeHidden();
  }
}

/**
 * Test touch interactions on mobile devices
 * @param {Page} page - Playwright page object
 * @param {string} url - URL to test on
 */
async function testTouchInteractions(page, url) {
  await page.setViewportSize({ width: 375, height: 667 });
  await page.goto(url);

  // Wait for interactive elements to load
  if (url.includes('news.html')) {
    await page.waitForSelector('.tag-filter-btn', { timeout: 1000 });

    const filterButtons = page.locator('.tag-filter-btn[data-tag]:not(:has-text("Last")):not(.disabled):not(.hidden-tag-btn)');
    const buttonCount = await filterButtons.count();

    if (buttonCount > 0) {
      const firstButton = filterButtons.first();

      // Verify button is properly sized for touch (minimum 30px for usability)
      const buttonBox = await firstButton.boundingBox();
      if (buttonBox) {
        expect(buttonBox.height).toBeGreaterThanOrEqual(30);
      }

      // Test tap interaction (use click for compatibility)
      const initiallyActive = await firstButton.evaluate(btn => btn.classList.contains('active'));
      await firstButton.click();
      await page.waitForTimeout(500);

      const afterTapActive = await firstButton.evaluate(btn => btn.classList.contains('active'));
      expect(afterTapActive).toBe(!initiallyActive);

      // Test Clear All if activated
      if (afterTapActive) {
        const clearButton = page.locator('button:has-text("Clear All")');
        if (await clearButton.isVisible()) {
          await clearButton.click();
          await page.waitForTimeout(500);

          const afterClearActive = await firstButton.evaluate(btn => btn.classList.contains('active'));
          expect(afterClearActive).toBe(false);
        }
      }
    }
  }
}

/**
 * Safely click a button only if it's enabled and visible
 * @param {Locator} buttonLocator - Playwright locator for the button
 * @param {string} buttonDescription - Description for logging (optional)
 * @returns {Promise<boolean>} - true if clicked, false if not clickable
 */
async function safeClickButton(buttonLocator, buttonDescription = 'button') {
  try {
    // Check if button exists
    const count = await buttonLocator.count();
    if (count === 0) {
      console.log(`ℹ️ ${buttonDescription} not found, skipping`);
      return false;
    }

    // Check if button is visible
    const isVisible = await buttonLocator.isVisible({ timeout: 1000 });
    if (!isVisible) {
      console.log(`ℹ️ ${buttonDescription} not visible, skipping`);
      return false;
    }

    // Check if button is enabled
    const isEnabled = await buttonLocator.isEnabled();
    if (!isEnabled) {
      console.log(`ℹ️ ${buttonDescription} is disabled, skipping`);
      return false;
    }

    // All checks passed - safe to click
    await buttonLocator.click();
    console.log(`✅ Successfully clicked ${buttonDescription}`);
    return true;
  } catch (error) {
    console.warn(`⚠️ Error clicking ${buttonDescription}: ${error.message}`);
    return false;
  }
}

/**
 * Clear all active filters on the page by clicking the "Clear All" button
 * @param {Page} page - Playwright page object
 */
async function clearAllFilters(page) {
  try {
    const clearButton = page.locator('button:has-text("Clear All")');

    // Check if Clear All button is visible with a timeout
    const isVisible = await clearButton.isVisible({ timeout: 1000 });
    if (isVisible) {
      await clearButton.click({ timeout: 2000 });
      // Wait longer for all filter count recalculations to complete
      await page.waitForTimeout(500); // Increased from 200ms to 500ms
      console.log('🧹 Cleared all filters via Clear All button');
    } else {
      console.log('ℹ️ No Clear All button visible - no active filters to clear');
    }
  } catch (error) {
    console.warn(`⚠️ Error in clearAllFilters: ${error.message}`);
    // Don't fail the test, just continue
  }
}

// Export all functions
/**
 * Generate test URLs from sections config to reduce hardcoded values
 * @returns {Object} - Object with arrays of test URLs by type
 */
function generateTestUrls() {
  const urls = {
    sectionIndexes: [],
    collectionPages: [],
    allPages: ['/'] // Include root
  };

  Object.entries(SECTIONS).forEach(([sectionKey, sectionData]) => {
    // Add section index page
    urls.sectionIndexes.push({
      url: sectionData.url,
      name: `${sectionData.title} Index`,
      section: sectionKey
    });
    urls.allPages.push(sectionData.url);

    // Add collection pages
    if (sectionData.collections) {
      sectionData.collections.forEach(collection => {
        if (collection.url) {
          urls.collectionPages.push({
            url: collection.url,
            name: `${sectionData.title} - ${collection.title}`,
            section: sectionKey,
            collection: collection.collection
          });
          urls.allPages.push(collection.url);
        }
      });
    }
  });

  return urls;
}

// Generate test URLs from config
const TEST_URLS = generateTestUrls();

module.exports = {
  TEST_CONFIG,
  SECTIONS,
  TEST_URLS,
  loadSectionsConfig,
  getLatestFileFromCollection,
  getLatestFileFromCollectionByCategory,
  getExpectedLatestContent,
  checkPageLoadTime,
  checkForJavaScriptErrors,
  checkFor404Errors,
  checkNoHorizontalScrollbar,
  getFilterCounts,
  getVisiblePostCount,
  getFilterButtonCount,
  waitForFilteringComplete,
  setupErrorTracking,
  navigateAndVerify,
  setViewportSize,
  findAndSelectFilter,
  testFilterToggle,
  verifyNavigationLink,
  generateTestsForMultiplePages,
  runTestOnMultiplePages,
  verifyContentItem,
  testResponsiveLayout,
  verifyContentFitsViewport,
  verifyTouchTargets,
  testHamburgerMenu,
  testDesktopNavigation,
  testTouchInteractions,
  safeClickButton,
  clearAllFilters
};
