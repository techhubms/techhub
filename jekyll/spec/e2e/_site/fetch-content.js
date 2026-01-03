const { chromium } = require('playwright');

// Get timeout and URLs from command line arguments
const timeoutSeconds = parseInt(process.argv[2]) || 30; // Increased default timeout to 30 seconds
const urls = process.argv.slice(3); // All remaining arguments are URLs

if (!urls.length) {
  console.error('Error: At least one URL is required');
  process.exit(1);
}

(async () => {
  let browser;
  try {
    browser = await chromium.launch({
      headless: true,
      args: [
        '--no-sandbox',
        '--disable-setuid-sandbox',
        '--disable-dev-shm-usage',
        '--disable-accelerated-2d-canvas',
        '--no-first-run',
        '--no-zygote',
        '--disable-gpu'
      ]
    });

    const results = [];

    const page = await browser.newPage();

    // Set realistic viewport
    await page.setViewportSize({ width: 1920, height: 1080 });

    // Set realistic user agent - use context method instead
    await page.route('**/*', (route) => {
      const headers = {
        ...route.request().headers(),
        'user-agent': 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36'
      };
      route.continue({ headers });
    });

    // Set additional headers to mimic real browser
    await page.setExtraHTTPHeaders({
      'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8',
      'Accept-Language': 'en-US,en;q=0.9',
      'Accept-Encoding': 'gzip, deflate, br',
      'DNT': '1',
      'Connection': 'keep-alive',
      'Upgrade-Insecure-Requests': '1',
      'Sec-Fetch-Dest': 'document',
      'Sec-Fetch-Mode': 'navigate',
      'Sec-Fetch-Site': 'none',
      'Sec-Fetch-User': '?1',
      'Cache-Control': 'max-age=0'
    });

    // Set timeout
    page.setDefaultTimeout(timeoutSeconds * 1000);

    // Process each URL with the same browser instance
    for (const url of urls) {
      try {
        // Navigate to the page with additional options - use more lenient wait condition
        await page.goto(url, {
          waitUntil: 'domcontentloaded', // Changed from 'networkidle' to be less strict
          timeout: timeoutSeconds * 1000
        });

        // Wait a bit more for dynamic content to load
        try {
          await page.waitForTimeout(2000); // Fixed 2 second wait for content
        } catch {
          // Continue even if this times out
        }

        // Check if we hit a blocking page and try to handle it
        const pageTitle = await page.title();
        if (pageTitle.toLowerCase().includes('blocked') || pageTitle.toLowerCase().includes('access denied')) {
          results.push({
            url: url,
            content: null,
            error: 'Access blocked'
          });
          continue;
        }

        // Get the full HTML content
        const content = await page.content();
        results.push({
          url: url,
          content: content,
          error: null
        });

      } catch (error) {
        results.push({
          url: url,
          content: null,
          error: error.message
        });
      }

      // Add delay between requests to prevent rate limiting
      if (urls.indexOf(url) < urls.length - 1) { // Don't delay after the last URL
        await new Promise(resolve => setTimeout(resolve, 10000)); // 10 second delay
      }
    }

    try {
      await page.close();
    }
    catch {
      //Do nothing
    }

    // Output results as JSON
    console.log(JSON.stringify(results));

  } catch (error) {
    console.error('Error:', error.message);
    process.exit(1);
  } finally {
    if (browser) {
      await browser.close();
    }
  }
})();
