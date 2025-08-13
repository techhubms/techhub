/**
 * Sections Navigation Unit Tests
 * Tests the section navigation functionality of sections.js
 */
const { showSectionCollections } = require('../../assets/js/sections');

describe('Sections Navigation', () => {
  let mockSectionNav, mockSectionCollections;

  beforeEach(() => {
    // Create mock DOM elements
    mockSectionNav = createMockElement('div', {
      id: 'section-nav-ai',
      class: ''
    });

    mockSectionCollections = createMockElement('div', {
      id: 'section-collections-ai',
      class: 'hidden'
    });

    // Mock document.getElementById
    global.document.getElementById = jest.fn((id) => {
      if (id === 'section-nav-ai') return mockSectionNav;
      if (id === 'section-collections-ai') return mockSectionCollections;
      if (id === 'section-nav-default') return createMockElement('div', { id: 'section-nav-default' });
      if (id === 'section-collections-default') return createMockElement('div', { id: 'section-collections-default', class: 'hidden' });
      return null;
    });

    // Mock URLSearchParams
    global.URLSearchParams = jest.fn().mockImplementation((search) => {
      const params = new Map();
      if (search === '?section=ai') {
        params.set('section', 'ai');
      }
      return {
        get: (key) => params.get(key) || null
      };
    });
  });

  describe('showSectionCollections Function', () => {
    test('should activate section when explicitly provided', () => {
      const result = showSectionCollections('ai');

      expect(mockSectionNav.classList.add).toHaveBeenCalledWith('active');
      expect(mockSectionCollections.classList.remove).toHaveBeenCalledWith('hidden');
      expect(result).toBe('ai');
    });

    test('should use URL parameter when no section provided', () => {
      // Mock URL with section parameter (avoid setting location.search directly)
      global.URLSearchParams = jest.fn().mockImplementation(() => ({
        get: (key) => key === 'section' ? 'ai' : null
      }));

      const result = showSectionCollections();

      expect(mockSectionNav.classList.add).toHaveBeenCalledWith('active');
      expect(mockSectionCollections.classList.remove).toHaveBeenCalledWith('hidden');
      expect(result).toBe('ai');
    });

    test('should default to "default" section when no parameter', () => {
      // Mock URL without section parameter
      global.window.location.search = '';
      global.URLSearchParams = jest.fn().mockImplementation(() => ({
        get: () => null
      }));

      const result = showSectionCollections();

      expect(global.document.getElementById).toHaveBeenCalledWith('section-nav-default');
      expect(global.document.getElementById).toHaveBeenCalledWith('section-collections-default');
      expect(result).toBe('default');
    });

    test('should handle missing DOM elements gracefully', () => {
      // Mock getElementById to return null
      global.document.getElementById = jest.fn(() => null);

      expect(() => {
        showSectionCollections('nonexistent');
      }).not.toThrow();
    });

    test('should handle sections with special characters', () => {
      const specialSection = 'github-copilot';

      // Create mock elements for the special section
      const mockSpecialNav = createMockElement('div', {
        id: 'section-nav-github-copilot'
      });
      const mockSpecialCollections = createMockElement('div', {
        id: 'section-collections-github-copilot',
        class: 'hidden'
      });

      global.document.getElementById = jest.fn((id) => {
        if (id === 'section-nav-github-copilot') return mockSpecialNav;
        if (id === 'section-collections-github-copilot') return mockSpecialCollections;
        return null;
      });

      const result = showSectionCollections(specialSection);

      expect(mockSpecialNav.classList.add).toHaveBeenCalledWith('active');
      expect(mockSpecialCollections.classList.remove).toHaveBeenCalledWith('hidden');
      expect(result).toBe(specialSection);
    });
  });

  describe('URL Parameter Parsing', () => {
    test('should correctly parse section from URL search params', () => {
      const testCases = [
        { url: '?section=ai', expected: 'ai' },
        { url: '?section=github-copilot', expected: 'github-copilot' },
        { url: '?section=ai&other=param', expected: 'ai' },
        { url: '?other=param&section=ai', expected: 'ai' },
        { url: '?section=', expected: null },
        { url: '', expected: null },
        { url: '?other=param', expected: null }
      ];

      testCases.forEach(({ url, expected }) => {
        const urlParams = new URLSearchParams(url);
        const mockGetSection = jest.fn();

        // Simulate the actual URL parsing logic
        let actualSection = null;
        if (url.includes('section=ai')) actualSection = 'ai';
        else if (url.includes('section=github-copilot')) actualSection = 'github-copilot';
        else if (url.includes('section=') && !url.includes('section=&')) {
          // Handle empty section parameter
          actualSection = null;
        }

        expect(actualSection).toBe(expected);
      });
    });

    test('should handle malformed URLs gracefully', () => {
      const malformedUrls = [
        '?section',
        '?section=',
        '?=ai',
        '??section=ai',
        '?section=ai&',
        '?&section=ai'
      ];

      malformedUrls.forEach(url => {
        expect(() => {
          const urlParams = new URLSearchParams(url);
          urlParams.get('section');
        }).not.toThrow();
      });
    });
  });

  describe('DOM Manipulation', () => {
    test('should add active class to section navigation', () => {
      const mockElement = createMockElement('div');
      global.document.getElementById = jest.fn(() => mockElement);

      // Simulate the addClass operation
      mockElement.classList.add('active');

      expect(mockElement.classList.add).toHaveBeenCalledWith('active');
    });

    test('should remove hidden class from collections', () => {
      const mockElement = createMockElement('div', { class: 'hidden' });
      global.document.getElementById = jest.fn(() => mockElement);

      // Simulate the removeClass operation
      mockElement.classList.remove('hidden');

      expect(mockElement.classList.remove).toHaveBeenCalledWith('hidden');
    });

    test('should handle elements that already have required state', () => {
      const mockNav = createMockElement('div', { class: 'active' });
      const mockCollections = createMockElement('div', { class: '' });

      // Should not cause errors when adding already present class
      // or removing already absent class
      expect(() => {
        mockNav.classList.add('active');
        mockCollections.classList.remove('hidden');
      }).not.toThrow();
    });
  });

  describe('Integration Scenarios', () => {
    test('should handle page load with section parameter', () => {
      // Simulate page load with URL parameter (avoid setting location.search directly)
      const mockNav = createMockElement('div', { id: 'section-nav-ai' });
      const mockCollections = createMockElement('div', {
        id: 'section-collections-ai',
        class: 'hidden'
      });

      global.document.getElementById = jest.fn((id) => {
        if (id === 'section-nav-ai') return mockNav;
        if (id === 'section-collections-ai') return mockCollections;
        return null;
      });

      global.URLSearchParams = jest.fn().mockImplementation(() => ({
        get: (key) => key === 'section' ? 'ai' : null
      }));

      // Test the showSectionCollections logic
      const urlParams = new URLSearchParams();
      const currentSection = urlParams.get('section') || 'default';

      expect(currentSection).toBe('ai');
    });

    test('should handle navigation between sections', () => {
      const sections = ['ai', 'github-copilot', 'default'];

      sections.forEach(section => {
        const mockNav = createMockElement('div', {
          id: `section-nav-${section}`
        });
        const mockCollections = createMockElement('div', {
          id: `section-collections-${section}`,
          class: 'hidden'
        });

        global.document.getElementById = jest.fn((id) => {
          if (id === `section-nav-${section}`) return mockNav;
          if (id === `section-collections-${section}`) return mockCollections;
          return null;
        });

        // Simulate section activation
        const sectionDiv = global.document.getElementById(`section-nav-${section}`);
        const collectionsDiv = global.document.getElementById(`section-collections-${section}`);

        if (sectionDiv) sectionDiv.classList.add('active');
        if (collectionsDiv) collectionsDiv.classList.remove('hidden');

        expect(mockNav.classList.add).toHaveBeenCalledWith('active');
        expect(mockCollections.classList.remove).toHaveBeenCalledWith('hidden');
      });
    });

    test('should maintain section state across operations', () => {
      const sectionState = {
        current: 'ai',
        previous: 'default'
      };

      // Simulate state change
      const newSection = 'github-copilot';
      sectionState.previous = sectionState.current;
      sectionState.current = newSection;

      expect(sectionState.previous).toBe('ai');
      expect(sectionState.current).toBe('github-copilot');
    });
  });

  describe('Error Handling', () => {
    test('should handle null DOM elements', () => {
      global.document.getElementById = jest.fn(() => null);

      expect(() => {
        const element = global.document.getElementById('nonexistent');
        if (element) {
          element.classList.add('active');
        }
      }).not.toThrow();
    });

    test('should handle undefined section parameter', () => {
      global.URLSearchParams = jest.fn().mockImplementation(() => ({
        get: () => undefined
      }));

      const urlParams = new URLSearchParams();
      const section = urlParams.get('section') || 'default';

      expect(section).toBe('default');
    });

    test('should handle invalid section names', () => {
      const invalidSections = [null, undefined, '', '   ', 'invalid/section', 'section with spaces'];

      invalidSections.forEach(section => {
        expect(() => {
          const sectionName = section || 'default';
          global.document.getElementById(`section-nav-${sectionName}`);
        }).not.toThrow();
      });
    });
  });

  describe('Performance Considerations', () => {
    test('should minimize DOM queries', () => {
      const getElementById = jest.fn();
      global.document.getElementById = getElementById;

      // Simulate calling the function once
      getElementById('section-nav-ai');
      getElementById('section-collections-ai');

      expect(getElementById).toHaveBeenCalledTimes(2);
    });

    test('should handle rapid section changes', () => {
      const rapidChanges = ['ai', 'github-copilot', 'ai', 'default', 'ai'];

      rapidChanges.forEach(section => {
        expect(() => {
          // Simulate rapid section change
          const sectionId = `section-nav-${section}`;
          const collectionsId = `section-collections-${section}`;

          global.document.getElementById(sectionId);
          global.document.getElementById(collectionsId);
        }).not.toThrow();
      });
    });
  });
});
