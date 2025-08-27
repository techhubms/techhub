/**
 * Jest test setup for Tech Hub JavaScript tests
 * This file runs before each test to set up the testing environment
 */

// Mock browser globals that the filtering system uses
global.window = global.window || {
  activeFilters: new Set(),
  collapsedTagsVisible: false,
  isUpdating: false,
  dateFilterMappings: {},
  dateFilterConfig: {},
  tagRelationships: {},
  cachedCountSpans: new Map(),
  dateFilters: [],
  currentFilterData: [],
  location: {
    href: 'http://localhost/',
    search: '',
    pathname: '/'
  },
  history: {
    pushState: jest.fn(),
    replaceState: jest.fn()
  }
};

// Mock document object with basic DOM functionality
global.document = global.document || {
  addEventListener: jest.fn(),
  querySelectorAll: jest.fn(() => []),
  querySelector: jest.fn(() => null),
  getElementById: jest.fn(() => null),
  createElement: jest.fn(() => ({
    setAttribute: jest.fn(),
    getAttribute: jest.fn(),
    classList: {
      add: jest.fn(),
      remove: jest.fn(),
      contains: jest.fn(() => false),
      toggle: jest.fn()
    },
    addEventListener: jest.fn(),
    style: {},
    dataset: {},
    textContent: '',
    innerHTML: ''
  }))
};

// Mock console for clean test output
global.console = {
  ...console,
  log: jest.fn(),
  warn: jest.fn(),
  error: jest.fn()
};

// Helper function to create mock DOM elements
global.createMockElement = (tag = 'div', attributes = {}) => {
  const element = {
    tagName: tag.toUpperCase(),
    setAttribute: jest.fn(),
    getAttribute: jest.fn((attr) => attributes[attr] || null),
    classList: {
      add: jest.fn(),
      remove: jest.fn(),
      contains: jest.fn((className) => (attributes.class || '').includes(className)),
      toggle: jest.fn()
    },
    addEventListener: jest.fn(),
    removeEventListener: jest.fn(),
    click: jest.fn(),
    style: {},
    dataset: {},
    textContent: attributes.textContent || '',
    innerHTML: attributes.innerHTML || '',
    children: [],
    parentNode: null,
    isEnabled: jest.fn(() => !attributes.disabled),
    isVisible: jest.fn(() => attributes.style?.display !== 'none')
  };

  // Set attributes
  Object.keys(attributes).forEach(attr => {
    if (attr === 'data-tag' || attr.startsWith('data-')) {
      const dataKey = attr.replace('data-', '').replace(/-([a-z])/g, (_, letter) => letter.toUpperCase());
      element.dataset[dataKey] = attributes[attr];
    }
  });

  return element;
};

// Helper function to create mock filter data
global.createMockFilterData = () => [
  {
    epoch: Math.floor(Date.now() / 1000),
    tags: ['ai', 'github copilot', 'visual studio code'],
    categories: ['ai'],
    collection: 'news'
  },
  {
    epoch: Math.floor(Date.now() / 1000) - 86400, // Yesterday
    tags: ['azure', 'dotnet', 'development'],
    categories: ['ai'],
    collection: 'posts'
  },
  {
    epoch: Math.floor(Date.now() / 1000) - 604800, // Week ago
    tags: ['openai', 'machine learning'],
    categories: ['ai'],
    collection: 'videos'
  }
];

// Helper function to create mock tag relationships
global.createMockTagRelationships = () => ({
  'ai': [0, 1, 2],
  'github copilot': [0],
  'azure': [1],
  'openai': [2],
  'visual studio code': [0],
  'dotnet': [1],
  'machine learning': [2]
});

// Helper function to create mock date configuration
global.createMockDateConfig = () => [
  { label: 'Last 3 days', days: 3, count: 2 },
  { label: 'Last 7 days', days: 7, count: 3 },
  { label: 'Last 30 days', days: 30, count: 3 }
];

// Reset mocks before each test
beforeEach(() => {
  jest.clearAllMocks();

  // Reset global state
  global.window.activeFilters = new Set();
  global.window.collapsedTagsVisible = false;
  global.window.isUpdating = false;
  global.window.dateFilterMappings = {};
  global.window.dateFilterConfig = {};
  global.window.tagRelationships = {};
  global.window.cachedCountSpans = new Map();
  global.window.dateFilters = [];
  global.window.currentFilterData = [];
});

// Clean up after each test
afterEach(() => {
  jest.restoreAllMocks();
});
