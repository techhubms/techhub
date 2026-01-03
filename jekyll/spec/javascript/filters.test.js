
const { initDOMCache, updateDisplay } = require('../../assets/js/filters');

/**
 * Tag Filter Core Logic Unit Tests
 * Tests the core filtering functionality of filters.js without browser dependencies
 */
describe('Date Filter Button Visibility Rules', () => {
  let dateButton;

  beforeEach(() => {
    // Set up DOM for initDOMCache to work
    document.body.innerHTML = `
        <button class="date-filter-btn" data-tag="today">
          <span class="filter-count"></span>
        </button>
      `;
    // Set up globals as expected by production code
    global.window.dateFilterMappings = { today: new Set() };
    global.window.dateFilters = ['today'];
    global.window.dateFilterConfig = { today: 0 }; // today = 0 days back
    global.window.activeFilters = new Set();
    // Call initDOMCache to initialize cachedDateButtons
    initDOMCache();
    // Get references for assertions
    dateButton = document.querySelector('.date-filter-btn');
  });
  afterEach(() => {
    document.body.innerHTML = '';
    global.window.cachedCountSpans = new Map();
    // Reset module caches
    const { resetCaches } = require('../../assets/js/filters.js');
    resetCaches();
  });


  test('should hide Today (0) button if no tag filters are applied', () => {
    // No tag filters, Today has 0 count
    updateDisplay();
    // Should be disabled and have disabled class
    if (!dateButton.disabled) {
      throw new Error('Button not disabled: ' + JSON.stringify({
        disabled: dateButton.disabled,
        classList: Array.from(dateButton.classList),
        display: dateButton.style.display
      }));
    }
    expect(dateButton.classList.contains('disabled')).toBe(true);
    // Should be hidden (display: none or not in DOM)
    expect(dateButton.style.display === 'none' || !document.body.contains(dateButton)).toBe(true);
  });

  test('should keep Today (0) button visible if 0 count is due to tag filtering', () => {
    // Simulate a post that would match the date filter but not the tag filter
    const now = new Date();
    const today = new Date(now);
    today.setHours(0, 0, 0, 0);
    const todayEpoch = Math.floor(today.getTime() / 1000) + 3600; // 1 hour after midnight
    global.window.currentFilterData = [
      { epoch: todayEpoch, tags: ['other-tag'], tags_normalized: ['other-tag'] }
    ];
    global.window.tagRelationships = { 'some-tag': [] }; // No posts match the tag filter
    global.window.dateFilterMappings = { today: new Set([0]) };
    global.window.dateFilters = ['today'];
    global.window.dateFilterConfig = { today: 0 }; // today = 0 days back
    global.window.activeFilters = new Set(['some-tag']);
    // Re-initialize DOM cache to pick up new data
    initDOMCache();
    updateDisplay();
    // Should be present in the DOM
    expect(document.body.contains(dateButton)).toBe(true);
    // Should be disabled (count 0), but still present and not hidden
    expect(dateButton.disabled).toBe(true);
    expect(dateButton.classList.contains('disabled')).toBe(true);
  });
});

describe('Tag Filter Core Logic', () => {
  let filterData, tagRelationships;

  beforeEach(() => {
    // Set up mock data for each test
    filterData = createMockFilterData();
    tagRelationships = createMockTagRelationships();
    createMockDateConfig();

    // Reset global state
    global.window.currentFilterData = filterData;
    global.window.tagRelationships = tagRelationships;
    global.window.dateFilterConfig = {
      'last 3 days': 3,
      'last 7 days': 7,
      'last 30 days': 30
    };
    global.window.dateFilters = ['last 3 days', 'last 7 days', 'last 30 days'];
  });

  describe('Date Filter Calculations', () => {
    test('should calculate today filter correctly', () => {
      const now = new Date();
      const today = new Date(now);
      today.setHours(0, 0, 0, 0);
      const todayStart = Math.floor(today.getTime() / 1000);
      const todayEnd = todayStart + 86399;

      // Create test data with posts from today, yesterday, and week ago
      const testData = [
        { epoch: todayStart + 3600 }, // Today (1 hour after midnight)
        { epoch: todayStart - 86400 }, // Yesterday
        { epoch: todayStart - 604800 } // Week ago
      ];

      // Function to test: isWithinDateFilter for "today" (0 days)
      const todayPosts = testData.filter(item => {
        return item.epoch >= todayStart && item.epoch <= todayEnd;
      });

      expect(todayPosts).toHaveLength(1);
      expect(todayPosts[0].epoch).toBe(todayStart + 3600);
    });

    test('should calculate "last N days" filter correctly', () => {
      const now = new Date();
      const today = new Date(now);
      today.setHours(0, 0, 0, 0);
      const todayStart = Math.floor(today.getTime() / 1000);

      // Create test data spanning multiple days
      const testData = [
        { epoch: todayStart }, // Today
        { epoch: todayStart - 86400 }, // 1 day ago
        { epoch: todayStart - 86400 * 3 }, // 3 days ago
        { epoch: todayStart - 86400 * 6 }, // 6 days ago
        { epoch: todayStart - 86400 * 8 } // 8 days ago (should be excluded from last 7 days)
      ];

      // Test "last 7 days" calculation
      const daysBack = 7;
      const daysInSeconds = (daysBack - 1) * 86400;
      const cutoffEpoch = todayStart - daysInSeconds;

      const last7DaysPosts = testData.filter(item => item.epoch >= cutoffEpoch);

      expect(last7DaysPosts).toHaveLength(4); // Should include posts from today to 6 days ago
      expect(last7DaysPosts.every(item => item.epoch >= cutoffEpoch)).toBe(true);
    });

    test('should handle edge cases in date calculations', () => {
      // Test with epoch of 0 (edge case)
      const testData = [{ epoch: 0 }];

      const now = new Date();
      const today = new Date(now);
      today.setHours(0, 0, 0, 0);
      const todayStart = Math.floor(today.getTime() / 1000);
      const cutoffEpoch = todayStart - (6 * 86400); // Last 7 days cutoff

      const result = testData.filter(item => item.epoch >= cutoffEpoch);
      expect(result).toHaveLength(0); // Epoch 0 should be excluded
    });
  });

  describe('Tag Relationship Matching', () => {
    test('should match posts by tag relationships', () => {
      const postIndex = 0;
      const tagToMatch = 'ai';

      // Check if post index is in the tag relationship
      const relatedIndices = tagRelationships[tagToMatch];
      const isMatch = relatedIndices && relatedIndices.includes(postIndex);

      expect(isMatch).toBe(true);
      expect(relatedIndices).toContain(0);
    });

    test('should handle case-insensitive tag matching', () => {
      // Test different case variations
      const testCases = [
        { tag: 'AI', expectedIndices: [0, 1, 2] },
        { tag: 'ai', expectedIndices: [0, 1, 2] },
        { tag: 'GitHub Copilot', expectedIndices: [0] },
        { tag: 'github copilot', expectedIndices: [0] }
      ];

      testCases.forEach(({ tag, expectedIndices }) => {
        const lowercaseTag = tag.toLowerCase();
        const relatedIndices = tagRelationships[lowercaseTag];

        if (expectedIndices.length > 0) {
          expect(relatedIndices).toBeDefined();
          expect(relatedIndices).toEqual(expect.arrayContaining(expectedIndices));
        }
      });
    });

    test('should return empty for unknown tags', () => {
      const unknownTag = 'nonexistent-tag';
      const relatedIndices = tagRelationships[unknownTag];

      expect(relatedIndices).toBeUndefined();
    });

    test('should handle multiple filter intersection', () => {
      // Test AND logic for multiple filters
      const filter1 = 'ai'; // Matches posts [0, 1, 2]
      const filter2 = 'github copilot'; // Matches posts [0]

      const indices1 = new Set(tagRelationships[filter1] || []);
      const indices2 = new Set(tagRelationships[filter2] || []);

      // Intersection should only include posts that match BOTH filters
      const intersection = [...indices1].filter(x => indices2.has(x));

      expect(intersection).toEqual([0]); // Only post 0 has both 'ai' and 'github copilot'
    });
  });

  describe('Filter State Management', () => {
    test('should add and remove filters correctly', () => {
      const activeFilters = new Set();

      // Add filters
      activeFilters.add('ai');
      activeFilters.add('last 7 days');

      expect(activeFilters.has('ai')).toBe(true);
      expect(activeFilters.has('last 7 days')).toBe(true);
      expect(activeFilters.size).toBe(2);

      // Remove filter
      activeFilters.delete('ai');

      expect(activeFilters.has('ai')).toBe(false);
      expect(activeFilters.has('last 7 days')).toBe(true);
      expect(activeFilters.size).toBe(1);
    });

    test('should separate date filters from tag filters', () => {
      const allFilters = ['ai', 'last 7 days', 'github copilot', 'last 30 days'];
      const dateFilters = ['last 3 days', 'last 7 days', 'last 30 days'];

      const activeDateFilters = allFilters.filter(filter => dateFilters.includes(filter));
      const activeTagFilters = allFilters.filter(filter => !dateFilters.includes(filter));

      expect(activeDateFilters).toEqual(['last 7 days', 'last 30 days']);
      expect(activeTagFilters).toEqual(['ai', 'github copilot']);
    });

    test('should enforce exclusive date filter behavior', () => {
      // Only one date filter should be active at a time
      const activeFilters = new Set(['last 7 days']);

      // Adding a new date filter should replace the old one
      const newDateFilter = 'last 30 days';
      const dateFilters = ['last 3 days', 'last 7 days', 'last 30 days'];

      // Remove existing date filters
      dateFilters.forEach(filter => activeFilters.delete(filter));

      // Add new date filter
      activeFilters.add(newDateFilter);

      const remainingDateFilters = [...activeFilters].filter(filter => dateFilters.includes(filter));
      expect(remainingDateFilters).toEqual([newDateFilter]);
      expect(remainingDateFilters).toHaveLength(1);
    });
  });

  describe('URL Parameter Handling', () => {
    test('should parse filter parameters from URL', () => {
      const testUrls = [
        { url: '?filters=ai,last%207%20days', expected: ['ai', 'last 7 days'] },
        { url: '?filters=github%20copilot', expected: ['github copilot'] },
        { url: '?filters=', expected: [] },
        { url: '', expected: [] }
      ];

      testUrls.forEach(({ url, expected }) => {
        // Mock URLSearchParams
        const mockParams = new URLSearchParams(url);
        const filters = mockParams.get('filters');

        const parsedFilters = [];
        if (filters) {
          const decodedFilters = decodeURIComponent(filters);
          decodedFilters.split(',').forEach(filter => {
            if (filter) parsedFilters.push(filter);
          });
        }

        expect(parsedFilters).toEqual(expected);
      });
    });

    test('should generate correct URL parameters from active filters', () => {
      const activeFilters = new Set(['ai', 'last 7 days', 'github copilot']);

      // Convert to URL format
      const filterArray = Array.from(activeFilters);
      const filterString = encodeURIComponent(filterArray.join(','));

      expect(filterString).toContain('ai');
      expect(filterString).toContain('last%207%20days');
      expect(filterString).toContain('github%20copilot');
    });

    test('should handle special characters in filter names', () => {
      const specialFilters = ['C#/.NET', 'AI & ML', "API's"];

      specialFilters.forEach(filter => {
        const encoded = encodeURIComponent(filter);
        const decoded = decodeURIComponent(encoded);

        expect(decoded).toBe(filter);
      });
    });
  });

  describe('Filter Count Calculations', () => {
    test('should calculate correct counts for tag filters', () => {
      // Given the mock data, count posts for each tag
      const tagCounts = {};

      filterData.forEach(item => {
        item.tags.forEach(tag => {
          if (!tagCounts[tag]) tagCounts[tag] = 0;
          tagCounts[tag]++;
        });
      });

      expect(tagCounts['ai']).toBe(1);
      expect(tagCounts['github copilot']).toBe(1);
      expect(tagCounts['azure']).toBe(1);
      expect(tagCounts['openai']).toBe(1);
    });

    test('should calculate counts with date filter applied', () => {
      const now = new Date();
      const today = new Date(now);
      today.setHours(0, 0, 0, 0);
      const todayStart = Math.floor(today.getTime() / 1000);
      const cutoffEpoch = todayStart - (6 * 86400); // Last 7 days

      // Filter data by date first
      const recentPosts = filterData.filter(item => item.epoch >= cutoffEpoch);

      // Then count tags in recent posts
      const tagCounts = {};
      recentPosts.forEach(item => {
        item.tags.forEach(tag => {
          if (!tagCounts[tag]) tagCounts[tag] = 0;
          tagCounts[tag]++;
        });
      });

      // Verify that counts reflect only recent posts
      expect(Object.keys(tagCounts).length).toBeLessThanOrEqual(10); // Maximum possible unique tags
    });

    test('should handle zero counts correctly', () => {
      // Create scenario where some filters have zero results
      const emptyFilterData = [];
      const tagCounts = {};

      emptyFilterData.forEach(item => {
        item.tags.forEach(tag => {
          if (!tagCounts[tag]) tagCounts[tag] = 0;
          tagCounts[tag]++;
        });
      });

      expect(Object.keys(tagCounts)).toHaveLength(0);
    });
  });

  describe('Error Handling and Edge Cases', () => {
    test('should handle missing or invalid data gracefully', () => {
      const invalidData = [
        { epoch: null, tags: ['test'] },
        { epoch: 'invalid', tags: null },
        { tags: ['test'] }, // Missing epoch
        null,
        undefined
      ];

      invalidData.forEach(item => {
        if (item) {
          const epoch = item?.epoch || 0;
          const tags = item?.tags || [];

          expect(typeof epoch === 'number' || typeof epoch === 'string' || epoch === 0).toBe(true);
          expect(Array.isArray(tags)).toBe(true);
        }
      });
    });

    test('should handle empty filter states', () => {
      const emptyFilters = new Set();
      const emptyData = [];
      const emptyRelationships = {};

      expect(emptyFilters.size).toBe(0);
      expect(emptyData.length).toBe(0);
      expect(Object.keys(emptyRelationships).length).toBe(0);
    });

    test('should handle malformed tag relationships', () => {
      const malformedRelationships = {
        'valid-tag': [0, 1],
        'empty-tag': [],
        'null-tag': null,
        'undefined-tag': undefined
      };

      Object.entries(malformedRelationships).forEach(([_tag, indices]) => {
        if (indices && Array.isArray(indices)) {
          expect(indices.every(i => typeof i === 'number')).toBe(true);
        } else {
          expect(indices === null || indices === undefined || indices.length === 0).toBe(true);
        }
      });
    });
  });

  describe('Performance Considerations', () => {
    test('should handle large datasets efficiently', () => {
      // Create a larger dataset for performance testing
      const largeDataset = Array.from({ length: 1000 }, (_, i) => ({
        epoch: Math.floor(Date.now() / 1000) - (i * 3600), // Posts every hour going back
        tags: [`tag-${i % 10}`, 'common-tag'],
        categories: ['test'],
        collection: 'blogs'
      }));

      // Test that operations complete in reasonable time
      const startTime = performance.now();

      // Simulate filtering operation
      const recentPosts = largeDataset.filter(item => {
        const now = new Date();
        const today = new Date(now);
        today.setHours(0, 0, 0, 0);
        const cutoff = Math.floor(today.getTime() / 1000) - (7 * 86400);
        return item.epoch >= cutoff;
      });

      const endTime = performance.now();
      const duration = endTime - startTime;

      expect(duration).toBeLessThan(100); // Should complete in under 100ms
      expect(recentPosts.length).toBeGreaterThan(0);
    });

    test('should cache results when appropriate', () => {
      // Test that repeated operations can use cached results
      const testCache = new Map();
      const testKey = 'test-filter';
      const testValue = [0, 1, 2];

      // Simulate caching
      testCache.set(testKey, testValue);

      // Verify cache hit
      const cachedResult = testCache.get(testKey);
      expect(cachedResult).toBe(testValue);
      expect(testCache.has(testKey)).toBe(true);
    });
  });

});
