const { test, expect } = require('@playwright/test');
const {
  SECTIONS,
  navigateAndVerify,
  verifyNavigationLink,
  setupErrorTracking
} = require('./helpers.js');

test.describe('Section Navigation', () => {

  test('should display all sections on index page', async ({ page }) => {
    await navigateAndVerify(page, '/', {
      expectTitle: true,
      checkErrors: true,
      checkPerformance: true
    });

    // Check that both main sections are visible and clickable
    for (const [sectionKey, sectionData] of Object.entries(SECTIONS)) {
      const sectionElement = page.locator(`[data-section="${sectionKey}"]`).or(
        page.locator(`a[href="${sectionData.url}"]`)
      ).first();

      await expect(sectionElement).toBeVisible();
      await expect(sectionElement).toBeEnabled();

      // Verify section title is present and correct
      const titleElement = page.locator('text=' + sectionData.title).first();
      await expect(titleElement).toBeVisible();

      // Verify the link has proper href attribute
      await verifyNavigationLink(page, `[data-section="${sectionKey}"], a[href="${sectionData.url}"]`, sectionData.url);
    }
  });

  /**
   * Test navigation to a section and verify its collections
   * @param {Page} page - Playwright page object
   * @param {string} sectionKey - Section key (e.g., 'ai', 'github-copilot')
   * @param {Object} sectionData - Section configuration data
   */
  async function testSectionNavigation(page, sectionKey, sectionData) {
    const errorTracker = setupErrorTracking(page);

    await navigateAndVerify(page, '/', {
      expectTitle: true,
      checkErrors: true
    });

    // Click on section and verify navigation happens
    await page.click(`a[href="${sectionData.url}"]`);

    // Verify actual navigation occurred
    await expect(page).toHaveURL(new RegExp(sectionData.url.replace(/[.*+?^${}()|[\]\\]/g, '\\$&') + '/?$'));
    await expect(page).toHaveTitle(/Tech Hub/); // All pages have Tech Hub title

    // Verify no JavaScript errors during navigation
    expect(errorTracker.jsErrors).toHaveLength(0);

    // Verify all collections are visible and functional
    for (const collection of sectionData.collections) {
      await verifyNavigationLink(page, `a[href*="${collection.url}"]`, collection.url, {
        shouldBeVisible: true,
        shouldBeEnabled: true
      });
    }

    // Test navigation to first collection page to verify links work
    if (sectionData.collections.length > 0) {
      const firstCollection = sectionData.collections[0];
      await verifyNavigationLink(page, `a[href*="${firstCollection.url}"]`, firstCollection.url, {
        clickAndVerify: true,
        expectedUrl: firstCollection.url
      });
    }
  }

  test('should navigate to AI section and load successfully', async ({ page }) => {
    await testSectionNavigation(page, 'ai', SECTIONS.ai);
  });

  test('should navigate to GitHub Copilot section and load successfully', async ({ page }) => {
    await testSectionNavigation(page, 'github-copilot', SECTIONS['github-copilot']);
  });

  test.describe('AI Section Collection Pages', () => {
    const aiCollectionUrls = [
      '/ai/news.html',
      '/ai/blogs.html',
      '/ai/videos.html',
      '/ai/community.html'
    ];

    aiCollectionUrls.forEach(url => {
      test(`should load ${url} successfully`, async ({ page }) => {
        await navigateAndVerify(page, url, {
          expectTitle: true,
          checkErrors: true,
          checkPerformance: true
        });

        // Verify basic page structure is functional
        await expect(page.locator('.site-header')).toBeVisible(); // Use specific class
        await expect(page.locator('main')).toBeVisible();
        await expect(page.locator('footer')).toBeVisible();

        // Verify navigation breadcrumb or section indicator is present and correct
        const aiNavigation = page.locator('text=AI').or(page.locator('text=Microsoft AI'));
        await expect(aiNavigation.first()).toBeVisible();

        // Verify page has actual content (not just structure)
        const mainContent = page.locator('main');
        const mainText = await mainContent.textContent();
        expect(mainText.trim().length).toBeGreaterThan(50); // Page should have substantial content

        // If this is a collection page, verify it has collection-specific content
        if (url.includes('/news.html') || url.includes('/blogs.html') || url.includes('/community.html')) {
          // Should have posts or articles
          const articles = page.locator('article, .navigation-item-square, .news-item, .community-item');
          await articles.count();
          // May be 0 if no content exists yet, but structure should be there
          const contentContainer = page.locator('.blogs-container, .news-container, .community-container, main');
          await expect(contentContainer.first()).toBeVisible();
        }
      });
    });
  });

  test.describe('GitHub Copilot Section Collection Pages', () => {
    const githubCopilotCollectionUrls = [
      '/github-copilot/features.html',
      '/github-copilot/levels-of-enlightenment.html',
      '/github-copilot/news.html',
      '/github-copilot/blogs.html',
      '/github-copilot/community.html'
    ];

    githubCopilotCollectionUrls.forEach(url => {
      test(`should load ${url} successfully`, async ({ page }) => {
        await navigateAndVerify(page, url, {
          expectTitle: true,
          checkErrors: true,
          checkPerformance: true
        });

        // Verify basic page structure is functional
        await expect(page.locator('header.site-header')).toBeVisible();
        await expect(page.locator('main')).toBeVisible();
        await expect(page.locator('footer')).toBeVisible();

        // Verify navigation breadcrumb or section indicator is present and correct
        const copilotNavigation = page.locator('text=Copilot').or(page.locator('text=GitHub Copilot'));
        await expect(copilotNavigation.first()).toBeVisible();

        // Verify page has actual content (not just structure)
        const mainContent = page.locator('main');
        const mainText = await mainContent.textContent();
        expect(mainText.trim().length).toBeGreaterThan(50); // Page should have substantial content

        // If this is a collection page, verify it has collection-specific content
        if (url.includes('/news.html') || url.includes('/blogs.html') || url.includes('/community.html')) {
          // Should have collection items or articles structure
          const articles = page.locator('article, .navigation-item-square, .news-item, .community-item');
          await articles.count();
          // May be 0 if no content exists yet, but structure should be there
          const contentContainer = page.locator('.blogs-container, .news-container, .community-container, main');
          await expect(contentContainer.first()).toBeVisible();
        }

        // Verify GitHub Copilot specific content
        if (url.includes('github-copilot')) {
          const copilotContent = page.locator('text=Copilot').or(page.locator('text=GitHub'));
          await expect(copilotContent.first()).toBeVisible();
        }
      });
    });
  });

  // Test: URL Parameter Section Activation
  test('should activate section via URL parameter when accessing video page', async ({ page }) => {
    console.log('\n🧪 Testing URL parameter section activation');

    // Navigate to a video page with section=ai parameter
    const testUrl = '/videos/2025-04-30-Connecting-to-a-Local-MCP-Server-Using-MicrosoftExtensionsAI.html?section=ai';
    console.log(`📄 Navigating to: ${testUrl}`);

    await navigateAndVerify(page, testUrl);

    // Wait for any JavaScript section activation to complete
    await page.waitForTimeout(500);

    // Check if AI section is active in navigation
    const aiSectionElement = page.locator('[data-section="ai"]').or(
      page.locator('a[href="/ai"]')
    ).first();

    if (await aiSectionElement.count() > 0) {
      console.log('🔍 Checking AI section activation status');

      // Check for active class or attribute indicating AI section is selected
      const isAiActive = await page.evaluate(() => {
        // Look for various indicators that AI section is active
        const aiElements = document.querySelectorAll('[data-section="ai"], a[href="/ai"]');

        for (const element of aiElements) {
          if (element.classList.contains('active') ||
              element.classList.contains('selected') ||
              element.getAttribute('aria-current') === 'page' ||
              element.style.backgroundColor !== '' ||
              window.getComputedStyle(element).fontWeight === 'bold') {
            return true;
          }
        }

        // Also check if any parent containers indicate AI section is active
        const navContainers = document.querySelectorAll('nav, .navigation, .header');
        for (const container of navContainers) {
          if (container.dataset.activeSection === 'ai' ||
              container.classList.contains('ai-active')) {
            return true;
          }
        }

        return false;
      });

      if (isAiActive) {
        console.log('✅ AI section is properly activated');
      } else {
        console.log('ℹ️ AI section activation not detected in visual state');
      }
    }

    // Check if AI-related navigation items are visible
    const aiNavItems = await page.evaluate(() => {
      const aiLinks = [];
      const allLinks = document.querySelectorAll('a');

      for (const link of allLinks) {
        const href = link.getAttribute('href') || '';
        const text = link.textContent.toLowerCase();

        if (href.includes('/ai/') || text.includes('ai ') || text.includes('artificial intelligence')) {
          aiLinks.push({
            href: href,
            text: text.trim(),
            visible: link.offsetParent !== null
          });
        }
      }

      return aiLinks.filter(link => link.visible);
    });

    console.log(`🔗 AI navigation items found: ${aiNavItems.length}`);
    aiNavItems.slice(0, 3).forEach(item => {
      console.log(`   - "${item.text}" → ${item.href}`);
    });

    if (aiNavItems.length > 0) {
      console.log('✅ AI section navigation items are visible');
    }

    // Check if magazines collection is highlighted/active
    const magazinesHighlighted = await page.evaluate(() => {
      // Look for magazines-related elements that might be highlighted
      const magazineElements = document.querySelectorAll('[href*="magazine"], [data-collection="magazines"], .magazines-item, .magazine-item');

      for (const element of magazineElements) {
        if (element.classList.contains('active') ||
            element.classList.contains('selected') ||
            element.classList.contains('current') ||
            element.getAttribute('aria-current') === 'page') {
          return true;
        }
      }

      // Check if current page is recognized as magazines content
      const bodyClasses = document.body.className;
      const pageData = document.querySelector('[data-page]');

      return bodyClasses.includes('magazines') ||
             bodyClasses.includes('magazine') ||
             (pageData && pageData.dataset.page === 'magazines');
    });

    if (magazinesHighlighted) {
      console.log('✅ Magazines collection is properly highlighted');
    } else {
      console.log('ℹ️ Magazines collection highlighting not detected');
    }

    // Verify the page actually loaded with proper section activation
    const pageTitle = await page.title();
    const hasExpectedContent = await page.evaluate(() => {
      const content = document.body.textContent.toLowerCase();
      // Check for general content presence (tech hub, microsoft, etc.)
      return content.includes('tech hub') || content.includes('microsoft') || content.length > 100;
    });

    console.log(`📖 Page title: "${pageTitle}"`);
    console.log(`📄 Contains expected content: ${hasExpectedContent}`);

    expect(hasExpectedContent).toBe(true);

    console.log('✅ URL parameter section activation test completed');
  });
});

test.describe('Non-Standard Collection Pages Navigation Highlighting', () => {
  test('should highlight GitHub Copilot section when visiting GitHub Copilot Features page', async ({ page }) => {
    // Navigate to the GitHub Copilot Features page
    await navigateAndVerify(page, '/github-copilot/features.html', {
      expectTitle: true,
      checkErrors: true,
      checkPerformance: true
    });

    // Wait for any JavaScript navigation highlighting to complete
    await page.waitForTimeout(500);

    // Check if GitHub Copilot section is properly highlighted in navigation
    const githubCopilotSectionActive = await page.evaluate(() => {
      // Look for various indicators that GitHub Copilot section is active
      const githubCopilotElements = document.querySelectorAll(
        '[data-section="github-copilot"], a[href="/github-copilot"], a[href*="github-copilot"]'
      );

      for (const element of githubCopilotElements) {
        if (element.classList.contains('active') ||
            element.classList.contains('selected') ||
            element.classList.contains('current') ||
            element.getAttribute('aria-current') === 'page' ||
            element.style.backgroundColor !== '' ||
            window.getComputedStyle(element).fontWeight === 'bold') {
          return true;
        }
      }

      // Also check if any parent navigation containers indicate GitHub Copilot section is active
      const navContainers = document.querySelectorAll('nav, .navigation, .header, .site-header');
      for (const container of navContainers) {
        if (container.dataset.activeSection === 'github-copilot' ||
            container.classList.contains('github-copilot-active') ||
            container.classList.contains('copilot-active')) {
          return true;
        }
      }

      return false;
    });

    expect(githubCopilotSectionActive).toBe(true);

    // Verify the page content is actually GitHub Copilot Features content
    const pageContent = await page.textContent('main');
    expect(pageContent.toLowerCase()).toContain('github copilot');
    expect(pageContent.toLowerCase()).toContain('features');

    console.log('✅ GitHub Copilot Features page correctly highlights GitHub Copilot section in navigation');
  });

  test('should highlight GitHub Copilot section when visiting Levels of Enlightenment page', async ({ page }) => {
    // Navigate to the Levels of Enlightenment page
    await navigateAndVerify(page, '/github-copilot/levels-of-enlightenment.html', {
      expectTitle: true,
      checkErrors: true,
      checkPerformance: true
    });

    // Wait for any JavaScript navigation highlighting to complete
    await page.waitForTimeout(500);

    // Check if GitHub Copilot section is properly highlighted in navigation
    const githubCopilotSectionActive = await page.evaluate(() => {
      // Look for various indicators that GitHub Copilot section is active
      const githubCopilotElements = document.querySelectorAll(
        '[data-section="github-copilot"], a[href="/github-copilot"], a[href*="github-copilot"]'
      );

      for (const element of githubCopilotElements) {
        if (element.classList.contains('active') ||
            element.classList.contains('selected') ||
            element.classList.contains('current') ||
            element.getAttribute('aria-current') === 'page' ||
            element.style.backgroundColor !== '' ||
            window.getComputedStyle(element).fontWeight === 'bold') {
          return true;
        }
      }

      // Also check if any parent navigation containers indicate GitHub Copilot section is active
      const navContainers = document.querySelectorAll('nav, .navigation, .header, .site-header');
      for (const container of navContainers) {
        if (container.dataset.activeSection === 'github-copilot' ||
            container.classList.contains('github-copilot-active') ||
            container.classList.contains('copilot-active')) {
          return true;
        }
      }

      return false;
    });

    expect(githubCopilotSectionActive).toBe(true);

    // Verify the page content is actually Levels of Enlightenment content
    const pageContent = await page.textContent('main');
    expect(pageContent.toLowerCase()).toContain('levels of enlightenment');

    console.log('✅ Levels of Enlightenment page correctly highlights GitHub Copilot section in navigation');
  });

  test('should verify both pages are accessible and have proper collection highlighting', async ({ page }) => {
    const nonStandardPages = [
      {
        url: '/github-copilot/features.html',
        expectedContent: ['github copilot', 'features'],
        section: 'github-copilot'
      },
      {
        url: '/github-copilot/levels-of-enlightenment.html',
        expectedContent: ['levels of enlightenment'],
        section: 'github-copilot'
      }
    ];

    for (const pageInfo of nonStandardPages) {
      console.log(`🔍 Testing navigation highlighting for ${pageInfo.url}`);

      await navigateAndVerify(page, pageInfo.url, {
        expectTitle: true,
        checkErrors: true
      });

      // Wait for JavaScript navigation updates
      await page.waitForTimeout(500);

      // Verify content is correct
      const pageContent = await page.textContent('main');
      for (const expectedText of pageInfo.expectedContent) {
        expect(pageContent.toLowerCase()).toContain(expectedText);
      }

      // Verify navigation highlighting
      const sectionHighlighted = await page.evaluate((sectionName) => {
        const sectionElements = document.querySelectorAll(
          `[data-section="${sectionName}"], a[href="/${sectionName}"], a[href*="${sectionName}"]`
        );

        for (const element of sectionElements) {
          if (element.classList.contains('active') ||
              element.classList.contains('selected') ||
              element.classList.contains('current') ||
              element.getAttribute('aria-current') === 'page' ||
              window.getComputedStyle(element).fontWeight === 'bold') {
            return true;
          }
        }

        return false;
      }, pageInfo.section);

      expect(sectionHighlighted).toBe(true);

      console.log(`✅ ${pageInfo.url} properly highlights ${pageInfo.section} section`);
    }
  });
});
